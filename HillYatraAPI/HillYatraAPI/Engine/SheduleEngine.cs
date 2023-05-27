using HillYatraAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HillYatraAPI.Engine
{
    public class SheduleEngine
    {
        private readonly RepositoryContext _context;
         public SheduleEngine(RepositoryContext _context) {
             this._context = _context;
         }
        public IQueryable<Shedule> GetShedulesQueryByParam(int vendorId = 0, int sheduleId = 0, int type = 0, bool isFeatured = false, string fromPlace = null, string toPlace = null, DateTime? pickupTime = null, DateTime? returnTime = null,int? totalPeople=null )
        {
            if (fromPlace == "null")
                fromPlace = null;
            if (toPlace == "null")
                toPlace = null;

            IQueryable<Shedule> query;
            var user = _context.User.Where(a => a.Id == vendorId).FirstOrDefault();
            query = _context.Shedule.Include(a => a.Transport);
            if (vendorId != 0 && user.UserTypeId != 1)
            {
                query = query.Where(a => a.VendorId == vendorId);
            }
            if (sheduleId != 0)
            {
                query = query.Where(a => a.Id == sheduleId);
            }
            if (isFeatured)
            {
                query = query.Where(a => a.IsFeatured == 1);
            }
            if (type!=0)
            {
                query = query.Where(a => a.Type == type);
            }
             if (totalPeople!=null && totalPeople!=0)
             {
                 query = query.Where(a => a.MaxPeople <= totalPeople);
             }
            if (fromPlace != null && toPlace != null)
            {
                query = query.Where(a => a.FromPlaceNavigation.Place == fromPlace && a.ToPlaceNavigation.Place == toPlace);
            }
            else if(fromPlace != null && toPlace == null)
            {
                query = query.Where(a => a.FromPlaceNavigation.Place == fromPlace);
            }

            //pickupTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)pickupTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //returnTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)returnTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
             

            if (pickupTime!=null && pickupTime != DateTime.MinValue)
            {
                query = query.Where(a => a.DateTimePickup > pickupTime);
            }
            if (returnTime != null && returnTime != DateTime.MinValue)
            {
                query = query.Where(a => a.DateTimeReturn < returnTime);
            }
             
            query = query.Select(x => new Shedule()
            {
                DateTimePickup = x.DateTimePickup,
                DateTimeReturn = x.DateTimeReturn,
                FromPlace = x.FromPlace,
                ToPlace = x.ToPlace,
                Booking = x.Booking.Select(a => new Booking { Id = a.Id, CreatedOn = a.CreatedOn, Passenger = a.Passenger, PassengerId = a.PassengerId, SheduleId = a.SheduleId, UpdatedOn = a.UpdatedOn, VenderId = a.VenderId }).ToList(),
                VendorId = x.VendorId,
                AdditionalCharges = x.AdditionalCharges,
                DistanceTotal = x.DistanceTotal,
                CreatedOn = x.CreatedOn,
                DriverAllowancePerDay = x.DriverAllowancePerDay,
                FarePerKm = x.FarePerKm,
                PricePerPerson=x.PricePerPerson,
                FuelCharge=x.FuelCharge,
                TripType=x.TripType,
                MaxPeople=x.MaxPeople,
                Id = x.Id,
                NightCharge = x.NightCharge,
                WaitingCharge=x.WaitingCharge,
                TollTax=x.TollTax,
                Discount=x.Discount,
                FarePerKmExtra = x.FarePerKmExtra,
                TotalFare = x.TotalFare,
                FuelType = x.FuelType,
                Transport = new Transport() { Title = x.Transport.Title, Image = x.Transport.Image, Id = x.Transport.Id, Description = x.Transport.Description, VenderId = x.Transport.VenderId, CreatedOn = x.Transport.CreatedOn, UpdatedOn = x.Transport.UpdatedOn, TransportTypeNavigation = x.Transport.TransportTypeNavigation },
                TransportId = x.TransportId,
                UpdatedOn = x.UpdatedOn,
                DescriptionTitle = x.DescriptionTitle,
                Description = x.Description,
                IsFeatured = x.IsFeatured,
                Type = x.Type,
                FromPlaceNavigation = x.FromPlaceNavigation,
                ToPlaceNavigation = x.ToPlaceNavigation,
                TypeNavigation =new LkpType() { Id=x.TypeNavigation.Id,CreatedOn=x.TypeNavigation.CreatedOn,Type=x.TypeNavigation.Type},
                // SheduleImages = query.Include(a => a.SheduleImages).Select(i => i.SheduleImages.Select(a => new { a.Id,a.ImageSrc,a.SheduleId,a.CreatedOn}))
                SheduleImages = x.SheduleImages.Select(a => new SheduleImages { Id = a.Id, ImageSrc = a.ImageSrc, CreatedOn = a.CreatedOn, SheduleId = a.SheduleId }).ToList()
            }); ;
            return query;
        }

    }
}
