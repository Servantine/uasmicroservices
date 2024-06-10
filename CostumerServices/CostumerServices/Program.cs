using OrderServices.DAL;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomer, CustomerDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/costumers", (ICustomer customer) =>
{
    try
    {
        List<CustomerGetDTO> customerDTO = new List<CustomerGetDTO>();
        var customersFromDb = customer.GetAll();
        foreach (var c in customersFromDb)
        {
            customerDTO.Add(new CustomerGetDTO
            {
                CustomerId = c.CustomerId,
                Username = c.Username,
                Password = c.Password,
                FullName = c.FullName,
            });
        }
        return Results.Ok(customerDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/costumers/{id}", (ICustomer customer, int id) =>
{
    try
    {
        CustomerGetDTO customerDTO = new CustomerGetDTO();

        var customerFromDb = customer.GetByCustomerId(id);
        if (customerFromDb == null)
        {
            return Results.NotFound();
        }
        customerDTO.CustomerId = customerFromDb.CustomerId;
        customerDTO.Username = customerFromDb.Username;
        customerDTO.Password = customerFromDb.Password;
        customerDTO.FullName = customerFromDb.FullName;

        return Results.Ok(customerDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/costumers/insert", (ICustomer customer, CustomerInsertDTO customerDTO) =>
{
    try
    {
        if (customerDTO.Username != null)
        {
            Customer customers = new Customer
            {
                CustomerId = customerDTO.CustomerId,
                Username = customerDTO.Username,
                Password = customerDTO.Password,
                FullName = customerDTO.FullName,
            };

            customer.Insert(customers);

            return Results.Created($"/costumers/{customers.CustomerId}", customers);
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

app.MapPut("/costumers/update/{id}", (ICustomer customer, CustomerUpdateDTO customerDTO) =>
{
    try
    {
        var customerFromDb = customer.GetByCustomerId(customerDTO.CustomerId);
        if (customerFromDb == null)
        {
            return Results.NotFound();
        }

        if (customerDTO.Username != null)
        {
            customerFromDb.Username = customerDTO.Username;
            customerFromDb.Password = customerDTO.Password;
            customerFromDb.FullName = customerDTO.FullName;

            customer.Update(customerFromDb);

            return Results.Ok(customerFromDb);
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

app.MapDelete("/costumers/delete/{id}", (ICustomer customer, int id) =>
{
    try
    {
        var customerFromDb = customer.GetByCustomerId(id);
        if (customerFromDb == null)
        {
            return Results.NotFound();
        }

        customer.Delete(id);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
