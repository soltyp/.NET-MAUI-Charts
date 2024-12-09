using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MauiApp4
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {

            _connectionString = "null"; //Provide your connection string
        }

        public Dictionary<string, int> GetTemperatureStatistics()
        {
            var temperatureCounts = new Dictionary<string, int>();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();

            //    var query = "SELECT CurrentTemperature, COUNT(*) AS Count FROM Patients GROUP BY CurrentTemperature";
            //    using (var command = new SqlCommand(query, connection))
            //    {
            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var temperature = reader["CurrentTemperature"].ToString();
            //                var count = Convert.ToInt32(reader["Count"]);
            //                temperatureCounts[temperature] = count;
            //            }
            //        }
            //    }
            //}

            return temperatureCounts;
        }
    }
}
