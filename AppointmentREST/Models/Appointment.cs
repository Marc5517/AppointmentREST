using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentREST.Models
{
    public class Appointment
    {
        private int _id;
        private string _firstParty;
        private string _secondParty;
        private string _topic;
        private DateTime _date;
        private int _duration;

        public Appointment()
        {

        }

        public Appointment(int id, string firstParty, string secondParty, string topic, DateTime date, int duration)
        {
            _id = id;
            _firstParty = firstParty;
            _secondParty = secondParty;
            _topic = topic;
            _date = date;
            _duration = duration;
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public string FirstParty
        {
            get => _firstParty;
            set
            {
                _firstParty = value;
            }
        }

        public string SecondParty
        {
            get => _secondParty;
            set
            {
                _secondParty = value;
            }
        }

        public string Topic
        {
            get => _topic;
            set
            {
                _topic = value;
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
            }
        }

        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
            }
        }

        /// <summary>
        /// Laver præsentationen af objektet, tidsbestilling, om til en string, og vises på denne måde når vi køre restapi.
        /// </summary>
        /// <returns>Data fra objektet i string.</returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {_id}, {nameof(FirstParty)}: {_firstParty}, {nameof(SecondParty)}: {_secondParty}, " +
                   $"{nameof(Topic)}: {_topic}, {nameof(Date)}: {_date}, {nameof(Duration)}: {_duration}";
        }
    }
}
