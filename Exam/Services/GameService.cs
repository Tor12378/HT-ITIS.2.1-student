using Models;

namespace Exam.Services
{
    public class GameService : IGameService
    {
        private const int CriticalHitRoll = 20;
        private const int CriticalMissRoll = 1;

        public List<Round> Game(Player player, Monster enemy)
        {
            var rounds = new List<Round>();
            var roundId = 1;

            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                var round = CreateRound(roundId++);
                rounds.Add(round);

                PerformPlayerTurn(player, enemy, round);
                PerformEnemyTurn(player, enemy, round);

                if (player.HitPoints <= 0 || enemy.HitPoints <= 0)
                {
                    round.Rounds.Add(new FightResult
                    {
                        Message = player.HitPoints <= 0
                            ? $"Вы проиграли. Ваш персонаж мертв. HP противника {enemy.HitPoints}"
                            : $"Вы победили! Противник повержен. Ваши HP {player.HitPoints}",
                        IsRoundEnd = true
                    });

                    Console.WriteLine(round.Rounds.Last().Message);
                }
            }

            return rounds;
        }

        private Round CreateRound(int roundId)
        {
            var round = new Round
            {
                Id = roundId,
                Rounds = new List<FightResult>
                {
                    new FightResult { Message = $"Раунд {roundId}", IsRoundEnd = false }
                }
            };

            Console.WriteLine($"Раунд {roundId}");
            return round;
        }

        private void PerformPlayerTurn(Player player, Monster enemy, Round round)
        {
            var attackRoll = RollAttack();
            Console.WriteLine($"вы выбили {attackRoll}");

            if (attackRoll == CriticalHitRoll)
            {
                PerformCriticalHit(player, enemy, round);
            }
            else if (attackRoll == CriticalMissRoll)
            {
                PerformCriticalMiss(round);
            }
            else if (attackRoll + player.AttackModifier > enemy.Ac)
            {
                PerformSuccessfulHit(player, enemy, round, attackRoll);
            }
            else
            {
                PerformMiss(round);
            }
        }

        private void PerformCriticalHit(Player player, Monster enemy, Round round)
        {
            var damage = RollDamage(player.DamageModifier, player.Damage) * 2;
            enemy.HitPoints -= damage;
            AddFightResult(round, $"Критическое попадание! Нанесено урона:({player.DamageModifier} + {damage / 2 - player.DamageModifier}) * 2. " +
                                  $"У противника осталось {enemy.HitPoints} HP.");
        }

        private void PerformCriticalMiss(Round round)
        {
            AddFightResult(round, "Критический промах!");
        }

        private void PerformSuccessfulHit(Player player, Monster enemy, Round round, int attackRoll)
        {
            var damage = RollDamage(player.DamageModifier, player.Damage);
            enemy.HitPoints -= damage;
            AddFightResult(round, $"({player.AttackModifier} + {attackRoll}) больше {enemy.Ac}, " +
                                  $"вы попали!  Нанесено урона: {damage - player.DamageModifier} + {player.DamageModifier}. " +
                                  $"У противника осталось {enemy.HitPoints} HP.");
        }

        private void PerformMiss(Round round)
        {
            AddFightResult(round, "Вы промахнулись!");
        }

        private void PerformEnemyTurn(Player player, Monster enemy, Round round)
        {
            if (enemy.HitPoints <= 0)
            {
                return;
            }

            var enemyAttackRoll = RollAttack();
            Console.WriteLine($"Противник выбил {enemyAttackRoll}");

            if (enemyAttackRoll == CriticalHitRoll)
            {
                PerformCriticalHitByEnemy(player, enemy, round);
            }
            else if (enemyAttackRoll == CriticalMissRoll)
            {
                PerformCriticalMissByEnemy(round);
            }
            else if (enemyAttackRoll + enemy.AttackModifier > player.Ac)
            {
                PerformSuccessfulHitByEnemy(player, enemy, round, enemyAttackRoll);
            }
            else
            {
                PerformMissByEnemy(round);
            }
        }

        private void PerformCriticalHitByEnemy(Player player, Monster enemy, Round round)
        {
            var damage = RollDamage(enemy.DamageModifier, enemy.Damage) * 2;
            player.HitPoints -= damage;
            AddFightResult(round, $"Противник наносит критический урон! Получено урона: ({enemy.DamageModifier} + {damage / 2 - enemy.DamageModifier}) * 2." +
                                  $" У вас осталось {player.HitPoints} HP.");
        }

        private void PerformCriticalMissByEnemy(Round round)
        {
            AddFightResult(round, "Противник критически промахивается!");
        }

        private void PerformSuccessfulHitByEnemy(Player player, Monster enemy, Round round, int enemyAttackRoll)
        {
            var damage = RollDamage(enemy.DamageModifier, enemy.Damage);
            player.HitPoints -= damage;
            AddFightResult(round, $"({enemy.AttackModifier} + {enemyAttackRoll}) больше {player.Ac}. " +
                                  $"Противник попадает! Получено урона: {enemy.DamageModifier} + {damage - enemy.DamageModifier}. " +
                                  $"У вас осталось {player.HitPoints} HP.");
        }

        private void PerformMissByEnemy(Round round)
        {
            AddFightResult(round, "Противник промахивается!");
        }

        private void AddFightResult(Round round, string message)
        {
            round.Rounds.Add(new FightResult { Message = message, IsRoundEnd = false });
            Console.WriteLine(message);
        }

        private int RollAttack()
        {
            return Dice.DiceTwenty.Roll();
        }

        private int RollDamage(int damageModifier, string? damage)
        {
            if (string.IsNullOrEmpty(damage))
            {
                return 0;
            }

            var parts = damage.Split('d');

            if (parts.Length == 2 && int.TryParse(parts[0], out var numberOfRolls) && int.TryParse(parts[1], out var diceSides))
            {
                var totalDamage = Enumerable.Range(0, numberOfRolls).Sum(_ => new Dice(diceSides).Roll());
                return totalDamage + damageModifier;
            }

            return 0;
        }
    }
}
