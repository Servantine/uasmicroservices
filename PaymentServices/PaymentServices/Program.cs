using PaymentServices.DAL;
using PaymentServices.DAL.Interfaces;
using PaymentServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomer, CustomerDapper>();
builder.Services.AddScoped<IPayment, PaymentDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/payments", (IPayment payment) =>
{
    try
    {
        List<PaymentGetDTO> customerDTO = new List<PaymentGetDTO>();
        var paymentsFromDb = payment.GetAll();
        foreach (var p in paymentsFromDb)
        {
            customerDTO.Add(new PaymentGetDTO
            {
                PaymentId = p.PaymentId,
                CustomerId = p.CustomerId,
                PaymentWallet = p.PaymentWallet,
                Saldo = p.Saldo,
            });
        }
        return Results.Ok(customerDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/payments/{id}", (IPayment payment, int id) =>
{
    try
    {
        PaymentGetDTO paymentDTO = new PaymentGetDTO();

        var paymentsFromDb = payment.GetByPaymentId(id);
        if (paymentsFromDb == null)
        {
            return Results.NotFound();
        }
        paymentDTO.PaymentId = paymentsFromDb.PaymentId;
        paymentDTO.CustomerId = paymentsFromDb.CustomerId;
        paymentDTO.PaymentWallet = paymentsFromDb.PaymentWallet;
        paymentDTO.Saldo = paymentsFromDb.Saldo;

        return Results.Ok(paymentDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/payments/insert", (IPayment payment, PaymentInsertDTO paymentDTO) =>
{
    try
    {
        ICustomer customer = new CustomerDapper();
        if (customer.GetByCustomerId(paymentDTO.CustomerId) == null)
        {
            return Results.BadRequest("Customer not found");
        }
        if (paymentDTO.PaymentId != null)
        {
            Payment payments = new Payment
            {
                PaymentId = paymentDTO.PaymentId,
                CustomerId = paymentDTO.CustomerId,
                PaymentWallet = paymentDTO.PaymentWallet,
                Saldo = paymentDTO.Saldo,
            };

            payment.Insert(payments);

            return Results.Created($"/costumers/{payments.PaymentId}", payments);
        }
        else
        {
            return Results.BadRequest("Invalid Data");
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
