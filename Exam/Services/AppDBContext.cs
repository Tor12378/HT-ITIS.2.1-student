using Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Services;

public class AppDBContext : DbContext
{
    public DbSet<Monster>? Monsters { get; set; }
    
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var baboon = new Monster()
        {
            Id = 1, 
            Name = "Бабуин",
            HitPoints = 20,
            AttackModifier = 3,
            AttackPerRound = 1,
            Damage = "1d6",
            DamageModifier = 3,
            Ac = 12
        };
        var asharra = new Monster()
        {
            Id = 2,
            Name = "Ашара",
            HitPoints = 4,
            AttackModifier = -2,
            AttackPerRound = 3,
            Damage = "1d6",
            DamageModifier = 4,
            Ac = 13
        };
        
        var arrigal = new Monster()
        {
            Id = 3,
            Name = "Арригал",
            HitPoints = 12, 
            AttackModifier = -3, 
            AttackPerRound = 2, 
            Damage = "1d6", 
            DamageModifier = 3, 
            Ac = 5 
        };
        modelBuilder.Entity<Monster>().HasData(baboon, asharra, arrigal);
        base.OnModelCreating(modelBuilder);
    }

}