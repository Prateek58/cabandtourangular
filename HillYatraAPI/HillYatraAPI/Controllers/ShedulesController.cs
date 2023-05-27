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
using HillYatraAPI.Engine;
using TimeZoneConverter;

namespace HillYatraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShedulesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IHostingEnvironment _hostingEnvironment;
        public ShedulesController(RepositoryContext context, ILoggerManager logger, IRepositoryWrapper repoWrapper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _repoWrapper = repoWrapper;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("GetAllShedule")]
        public  async Task<IActionResult> GetAllShedule([FromQuery] int vendorId,int type=0, bool isFeatured=false)
        {
            try
            {
                SheduleEngine engine = new SheduleEngine(_context);
                return Ok(await engine.GetShedulesQueryByParam(vendorId, type: type, isFeatured: isFeatured).OrderByDescending(a=>a.Id).ToListAsync());
            }
            catch (Exception EX)
            {

                return Problem("0");
            }
            
        }
        
        // GET: api/Shedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shedule>> GetShedule(int id)
        {
            var shedule = await _context.Shedule.FindAsync(id);

            if (shedule == null)
            {
                return NotFound();
            }

            return shedule;
        }


        [HttpGet("GetSheduleByFilter")]
        public async Task<IActionResult> GetSheduleByFilter([FromQuery] string fromPlace, [FromQuery] string toPlace, [FromQuery] DateTime? pickupTime, [FromQuery] DateTime? returnTime,[FromQuery] int type, [FromQuery]  int? totalPeople)
        {
            try
            {
                _logger.LogInfo("Here is info message from the controller.");
                _logger.LogDebug("Here is debug message from the controller.");
                _logger.LogWarn("Here is warn message from the controller.");
                _logger.LogError("Here is error message from the controller.");

                List<Shedule> shedules;
                //fromPlace = fromPlace == "null" ? null : fromPlace;
                //toPlace = toPlace == "null" ? null : toPlace;
                SheduleEngine engine = new SheduleEngine(_context);
                shedules = engine.GetShedulesQueryByParam(fromPlace:fromPlace,toPlace:toPlace,pickupTime:pickupTime,returnTime:returnTime,type:type, totalPeople:totalPeople).ToList();
                 
                return Ok(shedules);
            }
            catch (Exception ex)
            {

                return Problem("0");
            } 
        } 
        // PUT: api/Shedules/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutShedule()
        {
            string fileName = string.Empty;
     

            try
            {
                var files = Request.Form.Files;
                var shedulesJson = Request.Form["shedules"];
                Shedule shedule = JsonConvert.DeserializeObject<Shedule>(shedulesJson);
             
                var istPickupDate= TimeZoneInfo.ConvertTimeFromUtc((DateTime)shedule.DateTimePickup, TZConvert.GetTimeZoneInfo("India Standard Time")  );
                var istDropDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)shedule.DateTimeReturn, TZConvert.GetTimeZoneInfo("India Standard Time"));

                shedule.DateTimePickup = istPickupDate;
                shedule.DateTimeReturn = istDropDate;
                _context.Entry(shedule).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        return Content("file not selected");
                    string folderName = "Upload";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    else
                    {
                        if (file.Length > 0)
                        {
                            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"') ;
                            fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.Ticks + Path.GetExtension(fileName);
                            string fullPath = Path.Combine(newPath, fileName);

                            if (System.IO.File.Exists(fullPath))
                            {
                                Problem("File already exist with same name!");
                            }

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                        shedule.SheduleImages.Add(new SheduleImages { SheduleId = shedule.Id, ImageSrc = fileName });
                        await _context.SaveChangesAsync();
                    }

                }
                SheduleEngine engine = new SheduleEngine(_context);
                var returnItem = engine.GetShedulesQueryByParam(0, shedule.Id).ToList();
                
                return Ok(returnItem);
            }
           
            catch (Exception ex)
            {
                return Problem(ex.Message);
            } 
        }

        // POST: api/Shedules
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost(), DisableRequestSizeLimit]
        public async Task<ActionResult<Shedule>> PostShedule()
        {
            string fileName = string.Empty;
            try
            {
                var files = Request.Form.Files;  
                var shedulesJson = Request.Form["shedules"]; 
                Shedule shedule = JsonConvert.DeserializeObject<Shedule>(shedulesJson);
                var istPickupDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)shedule.DateTimePickup, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                var istDropDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)shedule.DateTimeReturn, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                shedule.DateTimePickup = istPickupDate;
                shedule.DateTimeReturn = istDropDate;
                _context.Shedule.Add(shedule);
                await _context.SaveChangesAsync();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        return Content("file not selected");
                    string folderName = "Upload";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName); 

                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    } 
                    else
                    { 
                        if (file.Length > 0)
                        {
                             
                            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.Ticks + Path.GetExtension(fileName);
                            string fullPath = Path.Combine(newPath, fileName);
                             
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                        shedule.SheduleImages.Add(new SheduleImages { SheduleId = shedule.Id, ImageSrc = fileName });
                        await _context.SaveChangesAsync();
                    }
                    
                } 
                await _context.SaveChangesAsync();
                SheduleEngine engine = new SheduleEngine(_context);
                var returnItem = engine.GetShedulesQueryByParam(0, shedule.Id).ToList();
                return Ok(returnItem);
                // return CreatedAtAction("GetShedulesQueryByParam", new { id = sheduele.Id }, sheduele);
            }
            catch (Exception ex)
            {
                return Problem("0");
            }

        }

        // DELETE: api/Shedules/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Shedule>> DeleteShedule(int id)
        {
            try
            {
              
                var shedule = await _context.Shedule.FindAsync(id);
                if (shedule == null)
                {
                    return NotFound();
                }
                var sheduleImages = await _context.SheduleImages.Where(a => a.SheduleId == id).ToListAsync();
                _context.SheduleImages.RemoveRange(sheduleImages);


                _context.Shedule.Remove(shedule);
                await _context.SaveChangesAsync();

                SheduleEngine engine = new SheduleEngine(_context);
                var returnItem = engine.GetShedulesQueryByParam(0, shedule.Id).ToList();
                return returnItem.SingleOrDefault();
            }
            catch (Exception ex)
             {

                return Problem(ex.Message);
            }
            
        }

        private bool SheduleExists(int id)
        {
            return _context.Shedule.Any(e => e.Id == id);
        }
    } 
}
