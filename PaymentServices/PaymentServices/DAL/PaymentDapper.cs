using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentServices.DAL.Interfaces;
using PaymentServices.Models;
using System.Data.SqlClient;
using Dapper;

namespace PaymentServices.DAL
{
    public class PaymentDapper : IPayment
    {
        private string GetConnectionString()
        {
            return "Data Source=.\\SQLEXPRESS; Initial Catalog=PaymentDB;Integrated Security=true;TrustServerCertificate=false;";
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Payment> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Payments";
                try
                {
                    return conn.Query<Payment>(query);
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



        public Payment GetByPaymentId(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Payments WHERE PaymentId = @PaymentId";
                var param = new { PaymentId = id };
                try
                {
                    var result = conn.QueryFirstOrDefault<Payment>(query, param);
                    if (result == null)
                    {
                        throw new ArgumentException("Payments not found");
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

        public Payment Insert(Payment obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO Payments (PaymentId, CustomerId, PaymentWallet, Saldo) 
                         VALUES (@PaymentId, @CustomerId, @PaymentWallet, @Saldo);
                         SELECT CAST(SCOPE_IDENTITY() as int)";
                var param = new { PaymentId = obj.PaymentId, CustomerId = obj.CustomerId, PaymentWallet = obj.PaymentWallet , Saldo = obj.Saldo};
                try
                {
                    int newPaymentId = conn.ExecuteScalar<int>(query, param);

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

        public void Update(Payment obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Payment> GetByPaymentName(string name)
        {
            throw new NotImplementedException();
        }
    }
}