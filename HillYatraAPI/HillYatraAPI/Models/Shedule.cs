using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class Shedule
    {
        public Shedule()
        {
            Booking = new HashSet<Booking>();
            SheduleImages = new HashSet<SheduleImages>();
        }

        public int Id { get; set; }
        public int? FromPlace { get; set; }
        public int? ToPlace { get; set; }
        public DateTime? DateTimePickup { get; set; }
        public DateTime? DateTimeReturn { get; set; }
        public int? TransportId { get; set; }
        public int? MaxPeople { get; set; }
        public int? DistanceTotal { get; set; }
        public int? FarePerKm { get; set; }
        public int? FarePerKmExtra { get; set; }
        public int? FuelCharge { get; set; }
        public string FuelType { get; set; }
        public int? DriverAllowancePerDay { get; set; }
        public int? NightCharge { get; set; }
        public int? WaitingCharge { get; set; }
        public int? TollTax { get; set; }
        public int? AdditionalCharges { get; set; }
        public int? PricePerPerson { get; set; }
        public string DescriptionTitle { get; set; }
        public string Description { get; set; }
        public int? TotalFare { get; set; }
        public int? Discount { get; set; }
        public int VendorId { get; set; }
        public int Type { get; set; }
        public byte? IsFeatured { get; set; }
        public string TripType { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public virtual Places FromPlaceNavigation { get; set; }
        public virtual Places ToPlaceNavigation { get; set; }
        public virtual Transport Transport { get; set; }
        public virtual LkpType TypeNavigation { get; set; }
        public virtual User Vendor { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<SheduleImages> SheduleImages { get; set; }
    }
}
