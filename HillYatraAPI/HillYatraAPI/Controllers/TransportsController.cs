using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using HillYatraAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HillYatraAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransportsController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public TransportsController(RepositoryContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Transports/5
        [HttpGet("{id}")]
        public   ActionResult<Transport> GetTransport(int id)
        {
            var transport =  getTransport(id);
            if (transport == null)
            {
                return NotFound();
            }
            return  Ok( transport);
        }
        private Transport getTransport(int id)
        {
            var transport =  _context.Transport.Find(id);
            return transport;
        }

        // GET: api/Transports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transport>>> GetAllTransport()
        {
            return await _context.Transport.ToListAsync();
        }
       

        [HttpGet("GetAllTransportByVendorId/{id}")]
        public async Task<ActionResult<IEnumerable<Transport>>> GetAllTransportByVendorId(int id)
        {
            try
            {
                var user = _context.User.Where(a => a.Id == id).FirstOrDefault();
                return await _context.Transport.Where(a => a.VenderId == id || user.UserTypeId == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                return Problem("0");
             
            }
            //TODO change hardcoded usertype id to enum
           
        }
        [HttpPost(), DisableRequestSizeLimit]
        public async Task<ActionResult<Shedule>> PostTransport()
        {
            string fileName = string.Empty;
            try
            {
                var file = Request?.Form?.Files?.Count > 0 ? Request.Form.Files[0] : null;
                var transportJson = Request.Form["transport"];
                Transport transport = JsonConvert.DeserializeObject<Transport>(transportJson);
                if (file != null)  
                {
                    string folderName = "Upload/Transport";
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
                            //await DeleteTransportImage(transport.Id);

                            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.Ticks + Path.GetExtension(fileName);
                            string fullPath = Path.Combine(newPath, fileName);

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                }

                transport.Image = fileName;
                _context.Transport.Add(transport);
                await _context.SaveChangesAsync();

                return Ok("1");
            }
            catch (Exception ex)
            {
                return Problem("0");
            }


        }


        // DELETE: api/Transports/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transport>> DeleteTransports(int id)
        {
            try
            {
                var transport = await _context.Transport.FindAsync(id);
                if (transport == null)
                {
                    return NotFound();
                }

                _context.Transport.Remove(transport);
                await _context.SaveChangesAsync();

                return transport;
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }

        }


        [HttpDelete("DeleteTransportImage/{id}")]
        public async Task<ActionResult> DeleteTransportImage(int id)
        {
            try
            {
                //var transport =   _context.Transport.Where(a => a.Id == id).SingleOrDefault();
                var transport = _context.Transport.Find(id);

                string folderName = "Upload\\Transport";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
               
                string fullPath = transport.Image!=null?  Path.Combine(newPath, transport.Image.Trim('/')) : string.Empty;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                transport.Image = null;
                _context.Entry(transport).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(transport);


            }
            catch (Exception ex)
            {
                return Problem("0");
            }
        }


        [HttpPut]
        public async Task<IActionResult> PutTransport()
        {
            try
            {
                var file = Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null;
                var transportJson = Request.Form["transport"];
                string fileName = string.Empty;
                Transport transport = JsonConvert.DeserializeObject<Transport>(transportJson);
                Transport transportInDb =  getTransport(transport.Id);
                transportInDb.Title = transport.Title;
                transportInDb.VenderId = transport.VenderId;
                transportInDb.Description = transport.Description;
                //image 
                if (file != null)
                {
                    string folderName = "Upload/Transport";
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

                            string fullPath = string.Empty;
                             
                             
                              fullPath = transportInDb.Image != null ? Path.Combine(newPath, transportInDb.Image.Trim('/')) : string.Empty;

                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }

                            //await DeleteTransportImage(transport.Id);
                            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            fileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.Ticks + Path.GetExtension(fileName);
                              fullPath = Path.Combine(newPath, fileName);

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            transportInDb.Image = "/" + fileName;
                        }
                    }
                }

                
                //_context.Add(transport);
               _context.Entry(transportInDb).State = EntityState.Modified;
                await _context.SaveChangesAsync();
               return Ok(transportInDb);
                //return CreatedAtAction("GetTransport", new { id = transport.Id }, transport);

            }
            catch (Exception ex)
            {
                return Problem("0");
            }

        }

    }
}
