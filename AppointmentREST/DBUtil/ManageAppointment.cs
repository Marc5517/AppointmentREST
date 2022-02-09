using AppointmentREST.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentREST.DBUtil
{
    public class ManageAppointment
    {
        /// <summary>
        /// Forbindelsen til databasen i min lokale server.
        /// </summary>
        private const String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AppointmentDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const String Get_All = "select * from Appointment";

        /// <summary>
        /// Henter alle tidsbestillinger fra databasen via SQL-kommando.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Appointment> Get()
        {
            List<Appointment> liste = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(Get_All, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Appointment app = ReadNextElement(reader);
                    liste.Add(app);
                }

                reader.Close();
            }

            return liste;
        }

        private const String Get_By_Topic = "select * from Appointment WHERE Topic LIKE @topic";

        /// <summary>
        /// Henter alle tidsbestillinger ved indtastning af emner via SQL-kommando.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>Liste af tidsbestillinger</returns>
        public IEnumerable<Appointment> GetByTopic(string topic)
        {
            List<Appointment> aPList = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(Get_By_Topic, conn))
                {
                    cmd.Parameters.AddWithValue("@topic", $"%{topic}%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Appointment app = ReadNextElement(reader);
                        aPList.Add(app);
                    }
                    reader.Close();
                }
            }

            return aPList;
        }

        private const String INSERT =
            "insert into Appointment(FirstParty, SecondParty, Topic, Date, Duration) Values(@FirstParty, @SecondParty, @Topic, @Date, @Duration)";

        /// <summary>
        /// Tilføjer en tidsbestilling til databasen ved indtastning af tidsbestillingens værdier via SQL-kommando.
        /// </summary>
        /// <param name="value"></param>
        public void Add(Appointment value)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(INSERT, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@FirstParty", value.FirstParty);
                cmd.Parameters.AddWithValue("@SecondParty", value.SecondParty);
                cmd.Parameters.AddWithValue("@Topic", value.Topic);
                cmd.Parameters.AddWithValue("@Date", value.Date);
                cmd.Parameters.AddWithValue("@Duration", value.Duration);

                int rowsAffected = cmd.ExecuteNonQuery();
                // evt. return rowsAffected == 1 => true if inserted otherwise false
            }

            // evt. hente sidste måling og sende tilbage
        }

        private const String Get_By_Id = "select * from Appointment WHERE id = @ID";

        /// <summary>
        /// Henter en tidsbestlling via ID ved hjælp af SQL-kommando. Denne her bruges primært til PUT- og DELETE-metoden nedenunder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>En tidsbestilling.</returns>
        public Appointment GetById(int id)
        {
            Appointment a = new Appointment();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Get_By_Id, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read()) a = ReadNextElement(reader);
                }
            }

            return a;
        }


        private const String UPDATE_Appointment = "UPDATE Appointment set FirstParty=@FirstParty, SecondParty=@SecondParty, Topic=@Topic, Date=@Date, Duration=@Duration where id=@ID";

        /// <summary>
        /// Kan opdatere en tidsbestilling i databasen ved at nævne tidsbestillingens ID og SQL-kommando. Hvis du ikke nævne alle værdierne i tidsbestillingen, vil de originale værdier blive fjernet.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appointment"></param>
        public void UpdateAppointment(int id, Appointment appointment)
        {
            Appointment appo = GetById(id);

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(UPDATE_Appointment, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@FirstParty", appointment.FirstParty);
                cmd.Parameters.AddWithValue("@SecondParty", appointment.SecondParty);
                cmd.Parameters.AddWithValue("@Topic", appointment.Topic);
                cmd.Parameters.AddWithValue("@Date", appointment.Date);
                cmd.Parameters.AddWithValue("@Duration", appointment.Duration);

                int rowsAffected = cmd.ExecuteNonQuery();
                // evt. return rowsAffected == 1 => true if inserted otherwise false

                if (rowsAffected != 1)
                {
                    throw new KeyNotFoundException("Id not found was " + id);
                }
            }
        }

        private const String DELETE_Appointment = "DELETE Appointment WHERE ID = @ID";

        /// <summary>
        /// Sletter en tidsbestilling ved hjælp af SQL-kommando. Man skal nævne den bestillings ID for at fjerne den specifikke kunde.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>En tidsbestilling som slettes.</returns>
        public Appointment DeleteAppointment(int id)
        {
            Appointment appo = GetById(id);

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(DELETE_Appointment, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@ID", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                // evt. return rowsAffected == 1 => true if inserted otherwise false
            }

            return appo;
        }

        private Appointment ReadNextElement(SqlDataReader reader)
        {
            Appointment appointment = new Appointment();

            appointment.Id = reader.GetInt32(0);
            appointment.FirstParty = reader.GetString(1);
            appointment.SecondParty = reader.GetString(2);
            appointment.Topic = reader.GetString(3);
            appointment.Date = reader.GetDateTime(4);
            appointment.Duration = reader.GetInt32(5);

            return appointment;
        }
    }
}
