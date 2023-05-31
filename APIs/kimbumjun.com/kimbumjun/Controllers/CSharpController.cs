using kimbumjun.Data;
using kimbumjun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kimbumjun.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CSharpController : ControllerBase
    {
        private readonly CSharpDbContext Db;

        public CSharpController(CSharpDbContext db)
        {
            Db = db;
        }

        [HttpGet]
        [Route("getaddr")]
        public async Task<string> GetAddr()
        {
            await Task.Delay(1000);

            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        [HttpGet]
        [Route("/api/alldata")]
        public async Task<IActionResult> GetAllData()
        {
            var csharps = await Db.CSharps.ToListAsync();

            if (!csharps.Any())
            {
                return NotFound();
            }

            return Ok(csharps);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetCsharp([FromRoute] int id)
        {
            var csharp = await Db.CSharps.FirstOrDefaultAsync(x => x.Id == id);

            if (csharp == null)
            {
                return NotFound();
            }

            return Ok(csharp);
        }

        [HttpPost]
        public async Task<IActionResult> AddCharp([FromBody] CSharp csharp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Db.CSharps.Add(csharp);
            await Db.SaveChangesAsync();
            return Ok(csharp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCsharp([FromRoute] int id, [FromBody] CSharp data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != data.Id)
                return BadRequest();

            Db.Entry(data).State = EntityState.Modified;

            try
            {
                await Db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                    return NotFound();
            }

            return Ok(data);
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteCsahrp([FromRoute] int id)
        {
            var csharp = await Db.CSharps.FindAsync(id);
            if (csharp == null) return NotFound();

            Db.CSharps.Remove(csharp);

            await Db.SaveChangesAsync();

            return Ok(csharp);
        }

        private bool WorkoutExists(int id)
        {
            return Db.CSharps.Any(e => e.Id == id);
        }
    }
}
