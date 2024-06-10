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
    public class CustomerDapper : ICustomer
    {
        private string GetConnectionString()
        {
            return "Data Source=.\\SQLEXPRESS; Initial Catalog=CostumerDB;Integrated Security=true;TrustServerCertificate=false;";
        }

        public Customer Insert(Customer obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO Customers (CustomerId, Username, Password, FullName) 
                         VALUES (@CustomerId, @Username, @Password, @FullName);
                         SELECT CAST(SCOPE_IDENTITY() as int)";
                var param = new { CustomerId = obj.CustomerId, Username = obj.Username, Password = obj.Password , FullName = obj.FullName};
                try
                {
                    int newCustomerId = conn.ExecuteScalar<int>(query, param);

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
                string query = @"DELETE FROM Customers WHERE CustomerId = @CustomerId";
                var param = new { CustomerId = id };
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

        public IEnumerable<Customer> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Customers";
                try
                {
                    return conn.Query<Customer>(query);
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

        public Customer GetByCustomerId(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                var param = new { CustomerId = id };
                try
                {
                    var result = conn.QueryFirstOrDefault<Customer>(query, param);
                    if (result == null)
                    {
                        throw new ArgumentException("Customer not found");
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

        public IEnumerable<Customer> GetByCustomerName(string name)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Customers WHERE Username LIKE @Username ORDER BY CustomerId";
                var param = new { Username = '%' + name + '%' };
                try
                {
                    return conn.Query<Customer>(query, param);
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

        public void Update(Customer obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"UPDATE Customers SET Username = @Username WHERE CustomerId = @CustomerId";
                var param = new { CustomerName = obj.Username, CustomerId = obj.CustomerId };
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