using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class Transport
    {
        public Transport()
        {
            Shedule = new HashSet<Shedule>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? VenderId { get; set; }
        public int? Seats { get; set; }
        public int TransportType { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Model { get; set; }
        public int? MakeYear { get; set; }

        public virtual LkpType TransportTypeNavigation { get; set; }
        public virtual User Vender { get; set; }
        public virtual ICollection<Shedule> Shedule { get; set; }
    }
}
