using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using HillYatraAPI.Models;

namespace HillYatraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LkpTypesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public LkpTypesController(RepositoryContext context)
        {
            _context = context;
        }

        // GET: api/Types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LkpType>>> GetTypes()
        {
            return await _context.LkpType.ToListAsync();
        }

        // GET: api/TypesByCategory
        [HttpGet("GetTypesByCategory/{id}")]
        public async Task<ActionResult<IEnumerable<LkpType>>> GetTypesByCategory(int id)
        {
            var abc = await _context.LkpType.Where(a => a.Categoy == id).ToListAsync();
            return abc;
        }
    }
}
