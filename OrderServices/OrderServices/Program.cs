using OrderServices.DAL;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;
using Microsoft.OpenApi.Models;

using System;
using Microsoft.OpenApi.Models;
using OrderServices.DTO.Customer;
using OrderServices.DTO.OrderHeader;
using OrderServices.Services;
using OrderServices.DTO.OrderDetail;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICustomer, CustomerDapper>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDapper>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDapper>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICostumerService, CostumerService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/orderheaders", (IOrderHeader orderHeader) =>
{
    try
    {
        List<OrderHeaderGetDTO> orderHeaderDTO = new List<OrderHeaderGetDTO>();
        var orderHeadersFromDb = orderHeader.GetAll();
        foreach (var oh in orderHeadersFromDb)
        {
            orderHeaderDTO.Add(new OrderHeaderGetDTO
            {
                OrderHeaderId = oh.OrderHeaderId,
                CustomerId = oh.CustomerId,
                OrderDate = oh.OrderDate
            });
        }

        return Results.Ok(orderHeaderDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/orderheaders/{id}", (IOrderHeader orderHeader, int id) =>
{
    try
    {
        OrderHeaderGetDTO orderHeaderDTO = new OrderHeaderGetDTO();

        var orderHeaderFromDb = orderHeader.GetByOrderHeaderId(id);
        if (orderHeaderFromDb == null)
        {
            return Results.NotFound();
        }

        orderHeaderDTO.OrderHeaderId = orderHeaderFromDb.OrderHeaderId;
        orderHeaderDTO.CustomerId = orderHeaderFromDb.CustomerId;
        orderHeaderDTO.OrderDate = orderHeaderFromDb.OrderDate;

        return Results.Ok(orderHeaderDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.MapPost("/orderheaders/insert", async (IOrderHeader orderHeader, OrderHeaderInsertDTO obj, ICostumerService costumerService) =>
{
    try
    {


        ICustomer customer = new CustomerDapper();
        if (customer.GetByCustomerId(obj.CustomerId) == null)
        {
            return Results.BadRequest("Customer not found");
        }

        OrderHeader orderHeaders = new OrderHeader
        {
            OrderHeaderId = obj.OrderHeaderId,
            CustomerId = obj.CustomerId,
            OrderDate = obj.OrderDate
        };

        orderHeader.Insert(orderHeaders);

        return Results.Created($"/orderheaders/{orderHeaders.OrderHeaderId}", orderHeaders);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/update/{id}", (IOrderHeader orderHeader, OrderHeaderUpdateDTO orderHeaderDTO) =>
{
    try
    {
        var orderHeaderFromDb = orderHeader.GetByOrderHeaderId(orderHeaderDTO.OrderHeaderId);
        if (orderHeaderFromDb == null)
        {
            return Results.NotFound();
        }

        orderHeaderFromDb.OrderHeaderId = orderHeaderDTO.OrderHeaderId;
        orderHeaderFromDb.CustomerId = orderHeaderDTO.CustomerId;
        orderHeaderFromDb.OrderDate = orderHeaderDTO.OrderDate;

        orderHeader.Update(orderHeaderFromDb);

        return Results.Ok(orderHeaderFromDb);

    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/orderdetails/", (IOrderDetail orderDetail) =>
{
    try
    {
        List<OrderDetailGetDTO> orderDetailDTO = new List<OrderDetailGetDTO>();
        var orderDetailsFromDb = orderDetail.GetAll();
        foreach (var od in orderDetailsFromDb)
        {
            orderDetailDTO.Add(new OrderDetailGetDTO
            {
                OrderDetailId = od.OrderDetailId,
                OrderHeaderId = od.OrderHeaderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price
            });
        }

        return Results.Ok(orderDetailDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/orderdetails/{id}", (IOrderDetail orderDetail, int id) =>
{
    try
    {
        OrderDetailGetDTO orderDetailDTO = new OrderDetailGetDTO();

        var orderDetailFromDb = orderDetail.GetByOrderDetailId(id);
        if (orderDetailFromDb == null)
        {
            return Results.NotFound();
        }

        orderDetailDTO.OrderDetailId = orderDetailFromDb.OrderDetailId;
        orderDetailDTO.OrderHeaderId = orderDetailFromDb.OrderHeaderId;
        orderDetailDTO.ProductId = orderDetailFromDb.ProductId;
        orderDetailDTO.Quantity = orderDetailFromDb.Quantity;
        orderDetailDTO.Price = orderDetailFromDb.Price;

        return Results.Ok(orderDetailDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/orderdetails/insert", async (IOrderDetail orderDetailService, IProductService productService, OrderDetailInsertDTO obj) =>
{
    try
    {
        var product = await productService.GetByProductId(obj.ProductId);


        if (product == null)
        {
            return Results.BadRequest("Invalid Product");
        }

        var orderDetail = new OrderDetail
        {
            OrderHeaderId = obj.OrderHeaderId,
            ProductId = obj.ProductId,
            Quantity = obj.Quantity,
            Price = product.Price
        };

        var addedOrderDetail = orderDetailService.Insert(orderDetail);

        return Results.Created($"/orderdetails/{addedOrderDetail.OrderDetailId}", obj);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/orderdetails/update/{id}", async (IOrderDetail orderDetail, IOrderHeader orderHeader, IProductService productService, OrderDetailUpdateDTO obj) =>
{
    try
    {
        var orderDetailFromDb = orderDetail.GetByOrderDetailId(obj.OrderDetailId);
        if (orderDetailFromDb == null)
        {
            return Results.NotFound();
        }

        var product = await productService.GetByProductId(obj.ProductId);
        if (product == null)
        {
            return Results.BadRequest("Invalid Product");
        }

        if (obj.Quantity > product.Quantity || obj.Quantity <= 0)
        {
            return Results.BadRequest("Invalid Quantity Products");
        }

        orderDetailFromDb.OrderHeaderId = obj.OrderHeaderId;
        orderDetailFromDb.ProductId = obj.ProductId;
        orderDetailFromDb.Quantity = obj.Quantity;
        orderDetailFromDb.Price = product.Price;

        orderDetail.Update(orderDetailFromDb);

        return Results.Ok(orderDetailFromDb);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.Run();
