using Exam.Services;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers;

public class FightController 
{
    private readonly ILogger<FightController> _logger;
    private readonly IGameService _gameService;

    public FightController(ILogger<FightController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

 
    [HttpPost]
    [Route("Game")]
    public List<Round> Game(Fight fight)
    {
        return _gameService.Game(fight.Player, fight.Monster);
    }
}