using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Security.Cryptography.X509Certificates;
using LoggerService;
using HillYatraAPI.Models;
using Contracts;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using HillYatraAPI.Enums;
using Microsoft.Extensions.Configuration;
using HillYatraAPI.ModelsCusom;

namespace HillYatraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; set; }
        public BookingsController(RepositoryContext context, ILoggerManager logger, IRepositoryWrapper repoWrapper, IHostingEnvironment hostingEnvironment, IConfiguration iconfig)
        {
            _context = context;
            _logger = logger;
            _repoWrapper = repoWrapper;
            _hostingEnvironment = hostingEnvironment;
            Configuration = iconfig;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost()]
        public async Task<ActionResult<Booking>> PostBookings(Booking booking)
        {
            try
            {

                User passenger;
                string vendorEmail = string.Empty;
                passenger = _context.User.Where(a => a.Mobile == booking.Passenger.Mobile).SingleOrDefault();
                if (passenger == null)
                {
                    passenger = new User();
                    passenger.FirstName = booking.Passenger.FirstName;
                    passenger.LastName = booking.Passenger.LastName;
                    passenger.Mobile = booking.Passenger.Mobile;
                    passenger.Email = booking.Passenger.Email;
                    passenger.UserTypeId = (int)UserTypeEnum.User;
                    passenger.Password = RandomString(5);
                    _context.User.Add(passenger);
                    await _context.SaveChangesAsync();
                }
                //= new User();

                vendorEmail = _context.User.Where(a => a.Id == booking.VenderId).Select(a=>a.Email).FirstOrDefault();

                booking.Passenger = passenger;

                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();

                //mail sending code
                EmailModel emailData = new EmailModel();
                emailData.EmailTo = passenger.Email;
                emailData.EmailToCC = vendorEmail;//booking.Vender.Email;
                emailData.Subject = "Enquiry hillyatra.in From | "+passenger.FirstName+" "+passenger.LastName;
                emailData.Body = "Thank you for enquiry "+booking.Id;
                EmailSend emailsend = new EmailSend(Configuration);

                emailsend.SendMail(emailData);

                var response = new {StatusCode="1", BookingId= booking.Id };
                return Ok(response);

            }
            catch (Exception ex)
            {

                return Problem("0");
            }
        }

            private bool SheduleExists(int id)
        {
            return _context.Shedule.Any(e => e.Id == id);
        }
    } 
}
