using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Controllers
{
    [ApiController]
    //[Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public string _connectionString = string.Empty;
        public WeatherForecastController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionStrings:DataBase").Value.ToString();
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //[Route("WeatherForecast")]
        [HttpGet("WeatherForecast")]
        public IEnumerable<WeatherForecast> WeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //[Route("GetAllRoles")]
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            List<Roles> role = new List<Roles>();
            try
            {
                role = GetAllRoleDetails();
            }
            catch (Exception ex) {
                throw ex;
            }
            return Ok(role);
        }

        private List<Roles> GetAllRoleDetails()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_connectionString);

            conn.Open();

            // Passing PostGre SQL Function Name
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM PUBLIC.\"Role_Mstr_tbl\"", conn);

            // Execute the query and obtain a result set
            NpgsqlDataReader reader = command.ExecuteReader();
            List<Roles> rolesList = new List<Roles>();
            while (reader.Read())
            {
                Roles role = new Roles();
                // Map the reader columns to the role properties
                role.Role_Mstr_ID = reader.GetInt32(reader.GetOrdinal("Role_Mstr_ID"));
                role.Role_Mstr_Code = reader.GetInt32(reader.GetOrdinal("Role_Mstr_Code"));
                role.Role_Mstr_Name = reader.IsDBNull(reader.GetOrdinal("Role_Mstr_Name")) ? null : reader.GetString(reader.GetOrdinal("Role_Mstr_Name"));
                role.Role_Mstr_LandingPage = reader.IsDBNull(reader.GetOrdinal("Role_Mstr_LandingPage")) ? null : reader.GetString(reader.GetOrdinal("Role_Mstr_LandingPage"));
                role.Role_Mstr_CreatedDate = reader.GetDateTime(reader.GetOrdinal("Role_Mstr_CreatedDate"));
                role.Role_Mstr_CreatedBy = reader.IsDBNull(reader.GetOrdinal("Role_Mstr_CreatedBy")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Role_Mstr_CreatedBy"));
                role.Role_Mstr_ModDate = reader.GetDateTime(reader.GetOrdinal("Role_Mstr_ModDate"));
                role.Role_Mstr_ModBy = reader.IsDBNull(reader.GetOrdinal("Role_Mstr_ModBy")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Role_Mstr_ModBy"));
                role.Role_Mstr_IsActive = reader.GetBoolean(reader.GetOrdinal("Role_Mstr_IsActive"));
                role.Role_Mstr_Description = reader.IsDBNull(reader.GetOrdinal("Role_Mstr_Description")) ? null : reader.GetString(reader.GetOrdinal("Role_Mstr_Description"));

                // Add the role to the list
                rolesList.Add(role);
            }
            reader.Close();

            command.Dispose();
            conn.Close();
            return rolesList;
        }

        private class Roles
        {
            [Key]
            public int Role_Mstr_ID { get; set; }
            public int Role_Mstr_Code { get; set; }
            public string Role_Mstr_Name { get; set; }
            public string Role_Mstr_LandingPage { get; set; }
            public DateTime Role_Mstr_CreatedDate { get; set; }
            public int? Role_Mstr_CreatedBy { get; set; }
            public DateTime Role_Mstr_ModDate { get; set; }
            public int? Role_Mstr_ModBy { get; set; }
            public bool Role_Mstr_IsActive { get; set; }
            public string Role_Mstr_Description { get; set; }
        }
    }
}
