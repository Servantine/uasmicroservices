using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;
using System.Data.SqlClient;
using Dapper;
using CatalogServices.DTO;

namespace CatalogServices.DAL
{
    public class ProductDAL : IProducts
    {
        private string GetConnectionString()
        {
            return "Data Source=.\\SQLEXPRESS; Initial Catalog=CatalogDB;Integrated Security=true;TrustServerCertificate=false;";
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Categories 
                        WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryID", id);
                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal dihapus");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> Products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Products ORDER BY Name";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Product product = new Product();
                        product.ProductID = Convert.ToInt32(dr["ProductID"]);
                        product.Name = dr["Name"].ToString();
                        Products.Add(product);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return Products;
            }
        }

        public Product GetById(int id)
        {
            Product product = new Product();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Products
                            WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@ProductId", id);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    product.ProductID = Convert.ToInt32(dr["ProductID"]);
                    product.Name = dr["Name"].ToString();
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return product;
            }
        }

        public IEnumerable<Product> GetByName(string name)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                List<Product> Products = new List<Product>();
                var strSql = @"SELECT * FROM Products
                            WHERE Name LIKE @Name";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Product product = new Product();
                        product.ProductID = Convert.ToInt32(dr["ProductID"]);
                        product.Name = dr["Name"].ToString();
                        Products.Add(product);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return Products;
            }
        }

        public void Insert(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Product (Name, Description, Price, Quantity) VALUES (@Name, @Description, @Price, @Quantity)";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@Description", obj.Description);
                cmd.Parameters.AddWithValue("@Price", obj.Price);
                cmd.Parameters.AddWithValue("@Quantity", obj.Quantity);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal disimpan");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void Update(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Product SET Name = @Name 
                            WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@ProductId", obj.ProductID);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal diupdate");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void UpdateStockAfterOrder(ProductsUpdateStockDto productsUpdateStockDto)
        {
            var strSql = @"UPDATE Products SET Quantity = Quantity - @Quantity WHERE ProductID = @ProductId";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var param = new { ProductId = productsUpdateStockDto.ProductID, Quantity = productsUpdateStockDto.Quantity };
                try
                {
                    conn.Execute(strSql,param);
                }catch(SqlException sqlEx)
                {
                    throw new ArgumentException($"Error : {sqlEx.Message}");
                }
                
            }
        }

    }
}
