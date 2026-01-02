using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Test_App.Data;
using Software_Test_App.Models;

namespace Software_Test_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchLog>>> GetSearchLogs()
        {
            return await _context.SearchLogs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SearchLog>> GetSearchLog(int id)
        {
            var searchLog = await _context.SearchLogs.FindAsync(id);

            if (searchLog == null)
            {
                return NotFound();
            }

            return searchLog;
        }

        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<Entry>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest("Query cannot be empty.");
            }

            var results = await _context.Entries
                .Where(e => e.Title.Contains(q) || e.Content.Contains(q))
                .ToListAsync();

            _context.SearchLogs.Add(new SearchLog { Query = q });
            await _context.SaveChangesAsync();

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<SearchLog>> PostSearchLog(SearchLog searchLog)
        {
            _context.SearchLogs.Add(searchLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSearchLog), new { id = searchLog.Id }, searchLog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSearchLog(int id, SearchLog searchLog)
        {
            if (id != searchLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(searchLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SearchLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchLog(int id)
        {
            var searchLog = await _context.SearchLogs.FindAsync(id);
            if (searchLog == null)
            {
                return NotFound();
            }

            _context.SearchLogs.Remove(searchLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SearchLogExists(int id)
        {
            return _context.SearchLogs.Any(e => e.Id == id);
        }
    }
}
