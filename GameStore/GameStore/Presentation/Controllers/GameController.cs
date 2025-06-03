using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Interfaces;
using GameStore.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetById(string id)
        {
            var game = await _service.GetByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Game game)
        {
            await _service.AddAsync(game);
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Game game)
        {
            if (id != game.Id) return BadRequest();
            await _service.UpdateAsync(game);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}