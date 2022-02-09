using AppointmentREST.DBUtil;
using AppointmentREST.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppointmentREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        /// <summary>
        /// Skaber forbindelsen til klassen der har forbindelse til databasen
        /// </summary>
        private string connectionString = ConnectionString.connectionString;

        /// <summary>
        /// En liste med tidsbestillinger, som kun blev brugt til at afprøve Rest.
        /// </summary>
        private static readonly List<Appointment> Appointments = new List<Appointment>()
        {
            new Appointment(1, "Anne Glaubig", "CEO of Inuatek", "Job meeting", new DateTime(2019, 3, 1, 8, 0, 0), 30),
            new Appointment(2, "Lene Kirkegaard", "Pizzaria", "Ordering", new DateTime(2019, 5, 2, 14, 0, 0), 20),
            new Appointment(3, "Izuma Suzuki", "Hair cut", "Hair cut", new DateTime(2021, 1, 1, 15, 0, 0), 45),
            new Appointment(4, "Josef Stalin", "Army", "Speech", new DateTime(2019, 10, 10, 8, 0, 0), 40),
            new Appointment(5, "Robin Holder", "robi@holder.com", "Checking mail", new DateTime(2029, 10, 1, 13, 0, 0), 30)
        };

        /// <summary>
        /// Henter alle tidsbestillinger fra listen, eller fra databasen ved hjælp af metoden fra ManageAppointment.
        /// </summary>
        /// <returns>Liste med tidsbestillinger.</returns>
        // GET: api/<AppointmentController>
        [HttpGet]
        public IEnumerable<Appointment> GetAll()
        {
            ManageAppointment ma = new ManageAppointment();
            return ma.Get();
            //return Appointments;
        }

        // GET api/<AppointmentController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// Henter alle tidsbestillinger fra listen der indeholder det bestemte topic, eller fra databasen ved hjælp af metoden fra ManageAppointment.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("topic/{topic}")]
        public IEnumerable<Appointment> GetAppointmentByTopic(string topic)
        {
            ManageAppointment ma = new ManageAppointment();
            return ma.GetByTopic(topic);
            //List<Appointment> lAppointments = Appointments.FindAll(a => a.Topic.Contains(topic));
            //return lAppointments;
        }

        /// <summary>
        /// Denne metode tilføjer en ny tidsbestilling til databasen ved hjælp af metoden fra ManageAppointment. For at kunne bruge POST skulle der gives adgang til Cors, hvilket er gjort før metoden.
        /// </summary>
        /// <param name="value"></param>
        // POST api/<AppointmentController>
        [HttpPost]
        [EnableCors("AllowAnyOriginGetPostPutDelete")]
        public void Post([FromBody] Appointment value)
        {
            ManageAppointment ma = new ManageAppointment();
            ma.Add(value);
        }

        /// <summary>
        /// Denne metode laver opdateringer til en tidsbestilling i databasen, hvor den gøre brug af metoderne fra ManageAppointment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        /// <returns>En opdateret tidsbestilling.</returns>
        // PUT api/<AppointmentController>/5
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] Appointment a)
        {
            ManageAppointment ma = new ManageAppointment();
            try
            {
                ma.UpdateAppointment(id, a);
                return Ok();
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(knfe.Message);
            }
        }

        /// <summary>
        /// Sletter en tidsbestilling i databasen ved hjælp af metoderne fra ManageAppointment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<AppointmentController>/5
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            ManageAppointment ma = new ManageAppointment();
            try
            {
                return Ok(ma.DeleteAppointment(id));
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(knfe.Message);
            }
        }
    }
}
