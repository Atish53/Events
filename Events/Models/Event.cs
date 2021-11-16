using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Display(Name = "Event Picture")]
        public byte[] EventPicture { get; set; }
        
        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Event Description")]
        public string EventDescription1 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Event Description")]
        public string EventDescription2 { get; set; }

        [Required]
        [Display(Name = "Event Location")]
        public string EventLocation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double EventPrice { get; set; }

        [Required]
        [Display(Name = "Tickets Left: ")]
        public int TicketsRemaining { get; set; }

        public int EventCategoryId { get; set; }

        public virtual EventCategory EventCategory { get; set; }

        public bool isActive { get; set; }

        public List<EventBooking> EventBookings { get; set; }
    }
}