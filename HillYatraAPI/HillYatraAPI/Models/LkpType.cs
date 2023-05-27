using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class LkpType
    {
        public LkpType()
        {
            Places = new HashSet<Places>();
            Shedule = new HashSet<Shedule>();
            Transport = new HashSet<Transport>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int Categoy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedOn { get; set; }

        public virtual LkpCategory CategoyNavigation { get; set; }
        public virtual ICollection<Places> Places { get; set; }
        public virtual ICollection<Shedule> Shedule { get; set; }
        public virtual ICollection<Transport> Transport { get; set; }
    }
}
