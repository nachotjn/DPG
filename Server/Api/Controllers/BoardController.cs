using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.Generic;

[ApiController]  
[Route("api/[controller]")]  
public class BoardController(IAppService appService) : ControllerBase{
        [HttpPost]
        [Route("")]
        public ActionResult<Board> CreateBoard(CreateBoardDto createBoardDto){
            var board = appService.CreateBoard(createBoardDto);
            return Ok(board);
        }

        [HttpGet]
        [Route("")]
        public ActionResult<List<Board>> GetAllBoards(){
            var boards = appService.GetAllBoards();
            return Ok(boards);
        }
}


