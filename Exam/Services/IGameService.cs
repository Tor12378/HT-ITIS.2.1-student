using Models;

namespace Exam.Services;

public interface IGameService
{
    public List<Round> Game(Player player, Monster enemy);

}