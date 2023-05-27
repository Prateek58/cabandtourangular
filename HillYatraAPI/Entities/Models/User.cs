using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class User
    {
        public User()
        {
            BookingPassenger = new HashSet<Booking>();
            BookingVender = new HashSet<Booking>();
            Shedule = new HashSet<Shedule>();
            Transport = new HashSet<Transport>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public int? UserTypeId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Password { get; set; }
        public short IsActive { get; set; }

        public virtual LkpUsertype UserType { get; set; }
        public virtual ICollection<Booking> BookingPassenger { get; set; }
        public virtual ICollection<Booking> BookingVender { get; set; }
        public virtual ICollection<Shedule> Shedule { get; set; }
        public virtual ICollection<Transport> Transport { get; set; }
    }
}
