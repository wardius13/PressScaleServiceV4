using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace PressScaleWebAPI.Controllers
{
    [RoutePrefix("api/meetdata")]
    public class MeetdataController : ApiController
    {
        private readonly string connectionString = "Server=LAPTOP-MAC90EQP\\TEW_SQLEXPRESS;Database=PressScaleDB;Trusted_Connection=True;";

        [HttpGet]
        [Route("")]
        public IEnumerable<Meetdata> GetMeetdata()
        {
            var meetdataList = new List<Meetdata>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Tijdstip, Gewicht, AantalBalen, MateriaalID FROM Meetdata";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        meetdataList.Add(new Meetdata
                        {
                            Tijdstip = reader.GetDateTime(0),
                            Gewicht = (float)reader.GetDouble(1),
                            AantalBalen = reader.GetInt32(2),
                            MateriaalID = reader.GetInt32(3)
                        });
                    }
                }
            }

            return meetdataList;
        }
    }

    public class Meetdata
    {
        public DateTime Tijdstip { get; set; }
        public float Gewicht { get; set; }
        public int AantalBalen { get; set; }
        public int MateriaalID { get; set; }
    }
}
