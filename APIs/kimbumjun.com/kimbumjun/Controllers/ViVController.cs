using kimbumjun.Data;
using kimbumjun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kimbumjun.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViVController : ControllerBase
    {
        private readonly CSharpDbContext Db;

        public ViVController(CSharpDbContext db)
        {
            Db = db;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await Db.ViVs.ToListAsync();

            if (!data.Any()) return NotFound("data list not found.");

            return Ok(data);
        }

        [HttpGet]
        [Route("GetCategoryDataList/{category:int}")]
        public async Task<IActionResult> GetCategoryDataList([FromRoute] int category)
        {
            var data = await Db.ViVs.Where(x => x.Category == category).ToListAsync();

            if (!data.Any()) return NotFound($"{category} data not found");

            return Ok(data);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetData([FromRoute] int id)
        {
            var data = await Db.ViVs.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null) return NotFound($"data number ({id}) is not found.");

            return Ok(data);
        }

        [HttpPost("PostData")]
        public async Task<IActionResult> PostData([FromBody] ViV data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Db.ViVs.Add(data);
            await Db.SaveChangesAsync();
            return Ok(data);
        }

        [HttpPost("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody] ViV data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            ViV viv = await Db.ViVs.FirstOrDefaultAsync(x => x.Id == data.Id);

            if (viv == null) return NotFound($"데이터 번호: {data.Id} 의 데이터가 없습니다.");

            viv.Title = data.Title;
            viv.Contents = data.Contents;
            viv.Note = data.Note;
            viv.Category = data.Category;

            Db.Entry(viv).State = EntityState.Modified;
            await Db.SaveChangesAsync();
            return Ok(viv);
        }

        [HttpPut("PutData/{id}")]
        public async Task<IActionResult> PutData(int id, ViV data)
        {
            if (id != data.Id)
            {
                return BadRequest("Data Id 가 잘못되었습니다.");
            }

            Db.Entry(data).State = EntityState.Modified;
            try
            {
                await Db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound($"자료번호 ({id})에 해당하는 자료를 찾을수 없습니다.");
                }
            }

            return Ok(data);
        }

        [HttpDelete]
        [Route("DeleteData/{id:int}")]
        public async Task<IActionResult> DeleteData([FromRoute] int id)
        {
            var data = await Db.ViVs.FindAsync(id);
            if (data == null) return NotFound($"데이터 번호: {id} 의 데이터가 없습니다.");

            Db.ViVs.Remove(data);
            await Db.SaveChangesAsync();
            return Ok(data);
        }

        private bool WorkoutExists(int id)
        {
            return Db.ViVs.Any(e => e.Id == id);
        }
    }
}
