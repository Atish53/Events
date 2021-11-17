using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class EventBooking
    {
        [Key]
        public int EventBookingId { get; set; }
        public int EventId { get; set; }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string DateBooked { get; set; }        

        public string TicketNumber { get; set; }

        public virtual Event Event { get; set; }
    }
}