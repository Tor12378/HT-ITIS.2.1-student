using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace UI.Pages;

public class IndexModel : PageModel
{
    private const string Db = "http://localhost:5220/GetMonster";
    private const string Bl = "http://localhost:5220/Game";

    public List<Round> FightLog { get; set; } = null!;
    public Monster? Monster { get; set; }
    
    [BindProperty]
    public Player? Player { get; set; } = new Player();
    
    public async Task OnPost()
    {
        try
        {
            var client = new HttpClient();
            var monster = await client.GetFromJsonAsync<Monster>(Db);

            if (monster != null)
            {
                Monster = monster;
                var fight = new Fight { Player = Player, Monster = monster };
                
                HttpResponseMessage fightResponse = await client.PostAsJsonAsync(Bl, fight);
               fightResponse.EnsureSuccessStatusCode(); 
               
                var responseContent = await fightResponse.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<Round>>(responseContent);

                FightLog = list!;
                foreach (var item in list!)
                {
                    foreach (var itemRound in item.Rounds!)
                    {
                        Console.WriteLine(itemRound.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ошибка: Получен null при запросе монстра.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выполнении OnPost: {ex.Message}");
        }
    }
}