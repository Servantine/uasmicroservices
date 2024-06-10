using ShippingServices.DAL;
using ShippingServices.DAL.Interfaces;
using ShippingServices.Models;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IShipping, ShippingDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/shippings", (IShipping shipping) =>
{
    try
    {
        List<ShippingGetDTO> shippingDTO = new List<ShippingGetDTO>();
        var shippingDTOFromDb = shipping.GetAll();
        foreach (var sh in shippingDTOFromDb)
        {
            shippingDTO.Add(new ShippingGetDTO
            {
                ShippingId = sh.ShippingId,
                ShippingVendor = sh.ShippingVendor,
                ShippingDate = sh.ShippingDate,
                ShippingStatus = sh.ShippingStatus,
                OrderHeaderId = sh.OrderHeaderId,
                BeratBarang = sh.BeratBarang,
                BiayaShipping = sh.BiayaShipping
            });
        }

        return Results.Ok(shippingDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/shpping/insert", async (IShipping shipping, ShippingInsertDTO obj) =>
{
    try
    {


        IOrderHeader orderHeader = new OrderHeaderDapper();
        if (orderHeader.GetByOrderHeaderId(obj.OrderHeaderId) == null)
        {
            return Results.BadRequest("OrderHeader not found");
        }

        Shipping shpping = new Shipping
        {
            OrderHeaderId = obj.OrderHeaderId,
            ShippingId = obj.ShippingId,
            ShippingVendor = obj.ShippingVendor,
            ShippingDate = obj.ShippingDate,
            BeratBarang = obj.BeratBarang,
            BiayaShipping = obj.BiayaShipping
        };

        shipping.Insert(shpping);

        return Results.Created($"/shipping/{shpping.ShippingId}", shipping);
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
