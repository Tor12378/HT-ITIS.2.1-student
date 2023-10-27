using Hw7.Models.ForTests;

namespace Hw7.Controllers;
using Microsoft.AspNetCore.Mvc;
public class TestController: Controller
{
    [HttpPost]
    public IActionResult TestModel(TestModel account)
    {
        return View(account);
    }
    
    [HttpGet]
    public IActionResult TestModel()
    {
        return View();
    }
}