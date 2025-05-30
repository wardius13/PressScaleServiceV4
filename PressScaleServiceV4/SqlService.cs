﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PressScaleServiceV4.Services
{
    public class SqlService
    {
        private readonly string _connectionString;

        public SqlService()
        {
            _connectionString = "Server=LAPTOP-MAC90EQP\\TEW_SQLEXPRESS;Database=PressScaleDB;Trusted_Connection=True;";
        }

        public async Task SaveDataAsync(float gewicht, int aantalBalen, int materiaalID)
        {
            var tijdstip = DateTime.Now;

            string query = @"INSERT INTO Meetdata (Tijdstip, Gewicht, AantalBalen, MateriaalID)
                             VALUES (@Tijdstip, @Gewicht, @AantalBalen, @MateriaalID);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))  // Let op _connectionString
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Tijdstip", tijdstip);
                    command.Parameters.AddWithValue("@Gewicht", gewicht);
                    command.Parameters.AddWithValue("@AantalBalen", aantalBalen);
                    command.Parameters.AddWithValue("@MateriaalID", materiaalID);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                Console.WriteLine("✅ Data succesvol weggeschreven naar database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Database fout: " + ex.Message);
            }
        }

        public async Task DeleteDataAsync(int id)
        {

        }
    }
    public class Meetdata
    {
        public int Id { get; set; }
        public DateTime Tijdstip { get; set; }
        public float Gewicht { get; set; }
        public int AantalBalen { get; set; }
        public int MateriaalID { get; set; }
    }
}
