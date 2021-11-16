using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Events.Models
{
    public class EventCategory
    {
        [Key]
        public int EventCategoryId { get; set; }

        public string EventCategoryName { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}