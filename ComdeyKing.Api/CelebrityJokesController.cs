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
    public class CelebrityJokesController : ControllerBase
    {
        private readonly CelebrityInCelebrityJokeContext _context;

        public CelebrityJokesController(CelebrityInCelebrityJokeContext context)
        {
            _context = context;
        }

        // GET: api/CelebrityJokes
        [HttpGet]
        public IEnumerable<CelebrityJoke> GetCelebrityJokes()
        {
            return _context.CelebrityJokes;
        }

        // GET: api/CelebrityJokes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCelebrityJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celebrityJoke = await _context.CelebrityJokes.FindAsync(id);

            if (celebrityJoke == null)
            {
                return NotFound();
            }

            return Ok(celebrityJoke);
        }

        // PUT: api/CelebrityJokes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCelebrityJoke([FromRoute] int id, [FromBody] CelebrityJoke celebrityJoke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != celebrityJoke.JokeID)
            {
                return BadRequest();
            }

            _context.Entry(celebrityJoke).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CelebrityJokeExists(id))
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

        // POST: api/CelebrityJokes
        [HttpPost]
        public async Task<IActionResult> PostCelebrityJoke([FromBody] CelebrityJoke celebrityJoke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CelebrityJokes.Add(celebrityJoke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCelebrityJoke", new { id = celebrityJoke.JokeID }, celebrityJoke);
        }

        // DELETE: api/CelebrityJokes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelebrityJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celebrityJoke = await _context.CelebrityJokes.FindAsync(id);
            if (celebrityJoke == null)
            {
                return NotFound();
            }

            _context.CelebrityJokes.Remove(celebrityJoke);
            await _context.SaveChangesAsync();

            return Ok(celebrityJoke);
        }

        private bool CelebrityJokeExists(int id)
        {
            return _context.CelebrityJokes.Any(e => e.JokeID == id);
        }
    }
}