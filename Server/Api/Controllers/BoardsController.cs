using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers
{
    [ApiController]  
    [Route("api/[controller]")]  
    public class BoardsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBoards()
        {
            var boards = new List<string> { "Board 1", "Board 2" };
            return Ok(boards);  
        }

        [HttpPost]
        public IActionResult CreateBoard([FromBody] Board board)
        {
            if (board == null)
            {
                return BadRequest("Board is null");
            }

            return CreatedAtAction(nameof(GetBoards), new { id = board.Id }, board);  // Devuelve el tablero creado
        }
    }

    public class Board
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
