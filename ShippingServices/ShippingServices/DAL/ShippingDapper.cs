using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.DAL.Interfaces;
using ShippingServices.Models;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace ShippingServices.DAL
{
    public class ShippingDapper : IShipping
    {
        private string GetConnectionString()
        {
            return "Data Source=.\\SQLEXPRESS; Initial Catalog=ShippingDB;Integrated Security=true;TrustServerCertificate=false;";
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shipping> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"SELECT * FROM Shipping";
                try
                {
                    return conn.Query<Shipping>(query);
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

        public IEnumerable<Shipping> GetByShippingVendor(string name)
        {
            throw new NotImplementedException();
        }

        public Shipping GetByShippingId(int id)
        {
            throw new NotImplementedException();
        }

        public Shipping Insert(Shipping obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                string query = @"INSERT INTO Shipping (ShippingId, ShippingVendor, ShippingDate, ShippingStatus, OrderHeaderId, BeratBarang, BiayaShipping) VALUES (@ShippingId, @ShippingVendor, @ShippingDate, @ShippingStatus, @OrderHeaderId, @BeratBarang, @BiayaShipping ); SELECT CAST(SCOPE_IDENTITY() as int)";
                var param = new { ShippingId = obj.ShippingId, ShippingVendor = obj.ShippingVendor, ShippingDate = obj.ShippingDate, ShippingStatus = obj.ShippingStatus,  OrderHeaderId = obj.OrderHeaderId, BeratBarang = obj.BeratBarang, BiayaShipping = obj.BiayaShipping};
                try
                {
                    int newOrderHeaderId = conn.ExecuteScalar<int>(query, param);

                    
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

        public void Update(Shipping obj)
        {
            throw new NotImplementedException();
        }


    }
}