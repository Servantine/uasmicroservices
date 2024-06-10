using CatalogServices;
using CatalogServices.DAL;
using CatalogServices.DAL.Interfaces;
using CatalogServices.DTO;
using CatalogServices.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "UTS",
        Title = "UTS MICROSERVICES",
        Description = "Oleh 72210465 - Stefanus Audy Advent Kristy",
    });
});

//meregister service menggunakan DI
builder.Services.AddScoped<ICategory, CategoryDapper>();
builder.Services.AddScoped<IProducts, ProductDapper>();
builder.Services.AddScoped<IJoin, JoinDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/categories", (ICategory categoryDal) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetAll();
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    CategoryDTO categoryDto = new CategoryDTO();
    var category = categoryDal.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    categoryDto.CategoryName = category.CategoryName;
    return Results.Ok(categoryDto);
});

app.MapGet("/api/categories/search/{name}", (ICategory categoryDal, string name) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetByName(name);
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapPost("/api/categories", (ICategory categoryDal, CategoryCreateDto categoryCreateDto) =>
{
    try
    {
        Category category = new Category
        {
            CategoryName = categoryCreateDto.CategoryName
        };
        categoryDal.Insert(category);

        //return 201 Created
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/categories", (ICategory categoryDal, CategoryUpdateDto categoryUpdateDto) =>
{
    try
    {
        var category = new Category
        {
            CategoryID = categoryUpdateDto.CategoryID,
            CategoryName = categoryUpdateDto.CategoryName
        };
        categoryDal.Update(category);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    try
    {
        categoryDal.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/products", (IProducts productDal) =>
{
    List<ProductsDTO> productsDto = new List<ProductsDTO>();
    var products = productDal.GetAll();
    foreach (var product in products)
    {
        productsDto.Add(new ProductsDTO
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryID = product.CategoryID,
            Quantity = product.Quantity
        });
    }
    return Results.Ok(productsDto);
});

app.MapGet("/api/products/{id}", (IProducts productDal, int id) =>
{
    ProductsDTO ProductsDTO = new ProductsDTO();
    var product = productDal.GetById(id);
    if (product == null)
    {
        return Results.NotFound();
    }
    ProductsDTO.Name = product.Name;
    return Results.Ok(ProductsDTO);
});

app.MapGet("/api/products/search/{name}", (IProducts productDal, string name) =>
{
    List<ProductsDTO> productsDto = new List<ProductsDTO>();
    var products = productDal.GetByName(name);
    foreach (var product in products)
    {
        productsDto.Add(new ProductsDTO
        {
            Name = product.Name,
        });
    }
    return Results.Ok(productsDto);
});

app.MapPost("/api/products", (IProducts productDal, ProductsCreateDto ProductsCreateDto) =>
{
    try
    {
        Product product = new Product
        {
            Name = ProductsCreateDto.Name,
            Description = ProductsCreateDto.Description,
            Price = ProductsCreateDto.Price,
            CategoryID = ProductsCreateDto.CategoryID,
            Quantity = ProductsCreateDto.Quantity,
        };
        productDal.Insert(product);

        return Results.Created($"/api/products/{product.ProductID}", product);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/products", (IProducts productDal, ProductsUpdateDto productUpdateDto) =>
{
    try
    {
        var product = new Product
        {
            ProductID = productUpdateDto.ProductId,
            Name = productUpdateDto.Name,
        };
        productDal.Update(product);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/products/{id}", (IProducts productDal, int id) =>
{
    try
    {
        productDal.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/api/join", (IJoin joinDapper) =>
{
    List<JoinDTO> joinDto = new List<JoinDTO>();
    var joins = joinDapper.GetAll();
    foreach (var join in joins)
    {
        joinDto.Add(new JoinDTO
        {
            ProductID = join.ProductID,
            Name = join.Name,
            Description = join.Description,
            Price = join.Price,
            CategoryID = join.CategoryID,
            Quantity = join.Quantity,
            CategoryName = join.CategoryName
        });
    }
    return Results.Ok(joinDto);
});

app.MapGet("/api/join/{id}", (IJoin joinDapper, int id) =>
{
    JoinDTO joinDto = new JoinDTO();
    var join = joinDapper.GetById(id);
    if (join == null)
    {
        return Results.NotFound();
    }
    joinDto.ProductID = join.ProductID;
    joinDto.Name = join.Name;
    joinDto.Description = join.Description;
    joinDto.Price = join.Price;
    joinDto.CategoryID = join.CategoryID;
    joinDto.Quantity = join.Quantity;
    joinDto.CategoryName = join.CategoryName;
    return Results.Ok(joinDto);
});

app.MapGet("/api/join/search/{name}", (IJoin joinDapper, string name) =>
{
    List<JoinDTO> joinDto = new List<JoinDTO>();
    var joins = joinDapper.GetByName(name);
    foreach (var join in joins)
    {
        joinDto.Add(new JoinDTO
        {
            ProductID = join.ProductID,
            Name = join.Name,
            Description = join.Description,
            Price = join.Price,
            CategoryID = join.CategoryID,
            Quantity = join.Quantity,
            CategoryName = join.CategoryName
        });
    }
    return Results.Ok(joinDto);
});

//update product 
app.MapPut("/api/products/updatestock", (IProducts productDal, ProductsUpdateStockDto productUpdateStockDto) =>
{
    try
    {
        var product = new Product
        {
            ProductID = productUpdateStockDto.ProductID,
            Quantity = productUpdateStockDto.Quantity,
        };
        productDal.Update(product);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.Run();
