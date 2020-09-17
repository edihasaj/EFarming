

using System;
using System.Data;
using System.Data.SqlClient;

namespace EFarming.Repository
{
    public class IrrigationState
    {
        private static string connectionString = "";

        public static void Insert(State state)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
             //   SqlCommand cmd = new SqlCommand()
            }
        }
    }

    public class State
    {
        public int Id { get; set; }
        public int ActuatorId { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsOpen { get; set; }
    }
}