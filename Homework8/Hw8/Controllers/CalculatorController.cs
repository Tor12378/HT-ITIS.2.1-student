using System.Diagnostics.CodeAnalysis;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    private readonly ICalculator _calculator;

    public CalculatorController(ICalculator calculator)
    {
        _calculator = calculator;
    }

    public ActionResult<double> Calculate(
        string val1,
        string operation,
        string val2)
    {
        try
        {
            var args = Parser.ParseArguments(val1, operation, val2);
            return Ok(_calculator.Calculate(args.Item2, args.Item1, args.Item3));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}