using System.Data.SqlClient;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;
using Dapper;

namespace CatalogServices;

public class JoinDapper : IJoin
{
    public IEnumerable<Join> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT prod.* , cat.CategoryName FROM Products prod INNER JOIN Categories cat ON prod.CategoryID = cat.CategoryID";
            var result = conn.Query<Join>(strSql);
            return result;
        }
    }

    public Join GetById(int id)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT prod.* , cat.CategoryName FROM Products prod INNER JOIN Categories cat ON prod.CategoryID = cat.CategoryID
                            WHERE prod.CategoryID = @CategoryID";
            var param = new { CategoryID = id };
            var product = conn.QueryFirstOrDefault<Join>(strSql, param);
            if (product == null)
            {
                throw new ArgumentException("Data tidak ditemukan");
            }
            return product;
        }
    }

    public IEnumerable<Join> GetByName(string name)
    {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT prod.* , cat.CategoryName FROM Products prod INNER JOIN Categories cat ON prod.CategoryID = cat.CategoryID
                            WHERE Name LIKE @Name";
            var param = new { Name = $"%{name}%" };
            var categories = conn.Query<Join>(strSql, param);
            return categories;
        }
    }

    private string GetConnectionString()
    {
        return "Data Source=.\\SQLEXPRESS; Initial Catalog=CatalogDB;Integrated Security=true;TrustServerCertificate=false;";
    }
    
}
