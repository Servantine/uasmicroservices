using System.Data.SqlClient;
using CatalogServices.DAL.Interfaces;
using CatalogServices.DTO;
using CatalogServices.Models;
using Dapper;

namespace CatalogServices;

public class ProductDapper : IProducts
{
    private string GetConnectionString()
    {
        return "Data Source=.\\SQLEXPRESS; Initial Catalog=CatalogDB;Integrated Security=true;TrustServerCertificate=false;";
    }
    public void Delete(int id)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"DELETE FROM Products 
                            WHERE ProductId = @ProductId";
            var param = new { ProductId = id };
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }

    public IEnumerable<Product> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Products order by Name";
            var Products = conn.Query<Product>(strSql);
            return Products;
        }
    }

    public Product GetById(int id)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Products
                            WHERE ProductId = @ProductId";
            var param = new { ProductId = id };
            var product = conn.QueryFirstOrDefault<Product>(strSql, param);
            if (product == null)
            {
                throw new ArgumentException("Data tidak ditemukan");
            }
            return product;
        }
    }

    public IEnumerable<Product> GetByName(string name)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Products
                            WHERE Name LIKE @Name";
            var param = new { Name = $"%{name}%" };
            var Products = conn.Query<Product>(strSql, param);
            return Products;
        }
    }

    public void Insert(Product obj)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"INSERT INTO Products (Name, Description, Price, Quantity, CategoryID) VALUES (@Name, @Description, @Price, @Quantity, @CategoryID)";
            var param = new { Name = obj.Name, Description = obj.Description, Price = obj.Price, Quantity = obj.Quantity, CategoryID = obj.CategoryID};
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }

    public void Update(Product obj)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"UPDATE Products SET Name = @Name 
                            WHERE ProductId = @ProductId";
            var param = new { Name = obj.Name, ProductId = obj.ProductID };
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }

    public void UpdateStockAfterOrder(int ProductId, int quantity)
    {
        throw new NotImplementedException();
    }

    public void UpdateStockAfterOrder(ProductsUpdateStockDto productsUpdateStockDto)
    {
        throw new NotImplementedException();
    }
}
