using Exam.Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Exam.Controllers;


[ApiController]

public class MonsterController
{
    private readonly IMonsterService _monsterService;

    private readonly ILogger<MonsterController> _logger;

    public MonsterController(ILogger<MonsterController> logger, IMonsterService monsterService)
    {
        _logger = logger;
        _monsterService = monsterService;
    }

    [HttpGet]
    [Route("GetMonster")]
    public Monster GetRandomMonster()
    {
        var result =  _monsterService.GetRandomMonster();
        return result;
    }
}