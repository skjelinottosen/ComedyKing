using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComedyKing.DataAccess;
using ComedyKing.Model;

namespace ComdeyKing.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CelebrityInCelebrityJokesController : ControllerBase
    {
        private readonly CelebrityInCelebrityJokeContext _context;

        public CelebrityInCelebrityJokesController(CelebrityInCelebrityJokeContext context)
        {
            _context = context;
        }

        // GET: api/CelebrityInCelebrityJokes
        [HttpGet]
        public IEnumerable<CelebrityInCelebrityJoke> GetCelebrityInCelebrityJokes()
        {
            return _context.CelebrityInCelebrityJokes;
        }

        // GET: api/CelebrityInCelebrityJokes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCelebrityInCelebrityJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celebrityInCelebrityJoke = await _context.CelebrityInCelebrityJokes.FindAsync(id);

            if (celebrityInCelebrityJoke == null)
            {
                return NotFound();
            }

            return Ok(celebrityInCelebrityJoke);
        }

        // PUT: api/CelebrityInCelebrityJokes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCelebrityInCelebrityJoke([FromRoute] int id, [FromBody] CelebrityInCelebrityJoke celebrityInCelebrityJoke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != celebrityInCelebrityJoke.CelebrityID)
            {
                return BadRequest();
            }

            _context.Entry(celebrityInCelebrityJoke).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CelebrityInCelebrityJokeExists(id))
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

        // POST: api/CelebrityInCelebrityJokes
        [HttpPost]
        public async Task<IActionResult> PostCelebrityInCelebrityJoke([FromBody] CelebrityInCelebrityJoke celebrityInCelebrityJoke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CelebrityInCelebrityJokes.Add(celebrityInCelebrityJoke);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CelebrityInCelebrityJokeExists(celebrityInCelebrityJoke.CelebrityID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCelebrityInCelebrityJoke", new { id = celebrityInCelebrityJoke.CelebrityID }, celebrityInCelebrityJoke);
        }

        // DELETE: api/CelebrityInCelebrityJokes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelebrityInCelebrityJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celebrityInCelebrityJoke = await _context.CelebrityInCelebrityJokes.FindAsync(id);
            if (celebrityInCelebrityJoke == null)
            {
                return NotFound();
            }

            _context.CelebrityInCelebrityJokes.Remove(celebrityInCelebrityJoke);
            await _context.SaveChangesAsync();

            return Ok(celebrityInCelebrityJoke);
        }

        private bool CelebrityInCelebrityJokeExists(int id)
        {
            return _context.CelebrityInCelebrityJokes.Any(e => e.CelebrityID == id);
        }
    }
}