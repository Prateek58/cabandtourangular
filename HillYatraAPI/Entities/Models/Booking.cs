using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public int? SheduleId { get; set; }
        public int? PassengerId { get; set; }
        public int? VenderId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public virtual User Passenger { get; set; }
        public virtual Shedule Shedule { get; set; }
        public virtual User Vender { get; set; }
    }
}
