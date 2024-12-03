using Microsoft.AspNetCore.Mvc;
using Service;
using System.Collections.Generic;

[ApiController]  
[Route("api/[controller]")]  
public class BoardController(IAppService appService) : ControllerBase{
    [HttpPost]
    [Route("")]
    public ActionResult<BoardDto> CreateBoard(CreateBoardDto createBoardDto){
        var board = appService.CreateBoard(createBoardDto);
        return Ok(board);
    }

    [HttpGet]
    [Route("")]
    public ActionResult<List<BoardDto>> GetAllBoards(){
        var boards = appService.GetAllBoards();
        return Ok(boards);
    }

    [HttpGet]
    [Route("players/{playerId}")]
    public ActionResult<List<BoardDto>> GetBoardsForPlayer(Guid playerId){
        var boards = appService.GetBoardsForPlayer(playerId);
        return Ok(boards);
    }

    [HttpGet]
    [Route("games/{gameId}")]
    public ActionResult<List<BoardDto>> GetBoardsForGame(Guid gameId){
        var boards = appService.GetBoardsForGame(gameId);
        return Ok(boards);
    }


}


