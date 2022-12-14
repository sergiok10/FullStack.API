using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;
        public GamesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _fullStackDbContext.Games.ToListAsync();
            return Ok(games);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] Game gameRequest)
        {
            gameRequest.Id = Guid.NewGuid();
            await _fullStackDbContext.Games.AddAsync(gameRequest);
            await _fullStackDbContext.SaveChangesAsync();

            return Ok(gameRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetGame([FromRoute] Guid id)
        {
            var game = await _fullStackDbContext.Games.FirstOrDefaultAsync(x => x.Id == id);

            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateGame([FromRoute] Guid id, Game updateGameRequest)
        {
            var game = await _fullStackDbContext.Games.FindAsync(id);
            if(game == null)
            {
                return NotFound();
            }
            game.Name = updateGameRequest.Name;
            game.Description = updateGameRequest.Description;
            game.Platform = updateGameRequest.Platform;
            game.Imgurl = updateGameRequest.Imgurl;

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(game);


        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid id)
        {
            var game = await _fullStackDbContext.Games.FindAsync(id);
            if(game == null){
                return NotFound();
            }
            _fullStackDbContext.Games.Remove(game);
            await _fullStackDbContext.SaveChangesAsync();

            return Ok(game);


        }
    }
}
