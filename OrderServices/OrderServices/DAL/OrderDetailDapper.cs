using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;
using System.Data.SqlClient;
using Dapper;

namespace OrderServices.DAL
{
    public class OrderDetailDapper : IOrderDetail
    {
        private string GetConnectionString()
        {
            return "Data Source=.\\SQLEXPRESS; Initial Catalog=OrderDB2;Integrated Security=true;TrustServerCertificate=false;";
        }

        public OrderDetail Insert(OrderDetail obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO OrderDetails (OrderDetailId, OrderHeaderId, ProductId, Quantity, Price) VALUES (@OrderDetailId, @OrderHeaderId, @ProductId, @Quantity, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
                var param = new { OrderDetailId = obj.OrderDetailId, OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price };
                try
                {
                    int newOrderDetailId = conn.ExecuteScalar<int>(query, param);

                    
                    return obj;
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

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"DELETE FROM OrderDetails WHERE OrderDetailId = @OrderDetailId";
                var param = new { OrderDetailId = id };
                try
                {
                    conn.Execute(query, param);
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

        public IEnumerable<OrderDetail> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM OrderDetails";
                try
                {
                    return conn.Query<OrderDetail>(query);
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

        public OrderDetail GetByOrderDetailId(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM OrderDetails WHERE OrderDetailId = @OrderDetailId";
                var param = new { OrderDetailId = id };
                try
                {
                    var result = conn.QueryFirstOrDefault<OrderDetail>(query, param);
                    if (result == null)
                    {
                        throw new ArgumentException("Order Detail not found");
                    }
                    else
                    {
                        return result;
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
            }
        }
        public void Update(OrderDetail obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"UPDATE OrderDetails SET OrderHeaderId = @OrderHeaderId, ProductId = @ProductId, Quantity = @Quantity, Price = @Price WHERE OrderDetailId = @OrderDetailId";
                var param = new { OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price, OrderDetailId = obj.OrderDetailId };
                try
                {
                    conn.Execute(query, param);
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
    }
}