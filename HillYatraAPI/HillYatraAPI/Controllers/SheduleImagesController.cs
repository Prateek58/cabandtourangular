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
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace HillYatraAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SheduleImagesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public SheduleImagesController(RepositoryContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: api/Transports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SheduleImages>>> GetAllSheduleImages()
        {
            return await _context.SheduleImages.ToListAsync();
        }

        [HttpGet("GetAllBySheduleId/{id}")]
        public async Task<ActionResult<IEnumerable<SheduleImages>>> GetAllBySheduleId(int id)
        {
            return await _context.SheduleImages.Where(a=>a.SheduleId==id). ToListAsync();
        }
        [HttpDelete("DeleteBySheduleId/{id}")]
        public async Task<ActionResult<IEnumerable<SheduleImages>>> DeleteBySheduleId(int id)
        {
            
            var sheduleImages = await _context.SheduleImages.Where(a => a.SheduleId == id).ToListAsync();
           foreach(var item in sheduleImages)
            {
                _context.SheduleImages.Remove(item);
               
            }

            await _context.SaveChangesAsync();
            return Ok("1");


        }


        // DELETE: api/SheduleImages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SheduleImages >> DeleteSheduleImage(int id)
        {
            var sheduleImage = await _context.SheduleImages.FindAsync(id);
            if (sheduleImage == null)
            {
                return NotFound();
            }

            _context.SheduleImages.Remove(sheduleImage);
            await _context.SaveChangesAsync();

            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string fileName = sheduleImage.ImageSrc.Trim('"');
            string fullPath = Path.Combine(newPath, fileName);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return sheduleImage;
        }


    }
}
