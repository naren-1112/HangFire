using Hangfire.Logging;
using HangFire.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HangFire.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IConfiguration _configuration;
        public CustomerServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            var query = "SELECT * FROM DETAILS";
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(ds);

            Console.WriteLine($"SyncData :sync is going on...");
            return ds.Tables[0];
        }

         void ICustomerServices.AddCustReview(Details detail)
        {
            try
            {
                string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                var query = "INSERT INTO DETAILS (ID,Name,Review) VALUES(@ID,@Name,@Review)";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("ID", detail.ID);
                    cmd.Parameters.AddWithValue("Name", detail.Name);
                    cmd.Parameters.AddWithValue("Review", detail.Review);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process...");
        }
        public List<Details> GetReviews()
        {
            DataTable details = SyncData();
            return (from DataRow dr in details.Rows
                    select new Details()
                    {
                        ID = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Review = dr["Review"].ToString()

                    }).ToList();


        }


    }
}

