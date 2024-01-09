using Models;

namespace Exam.Services;

public class MonsterService : IMonsterService
{
    private readonly AppDBContext _context;

    public MonsterService(AppDBContext context)
    {
        _context = context;
    }

    public Monster GetRandomMonster()
    {
        var rnd = new Random();
        var randomIndex = rnd.Next(0, _context.Monsters.Count());
        return _context.Monsters.OrderBy(m => m.Id).Skip(randomIndex).FirstOrDefault();
    }
}