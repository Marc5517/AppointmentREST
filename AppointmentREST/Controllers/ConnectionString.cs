using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentREST.Controllers
{
    internal class ConnectionString
    {
        /// <summary>
        /// Forbindelsen til tidsbestillingsdatabasen som er i min lokale server.
        /// </summary>
        public static readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AppointmentDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}
