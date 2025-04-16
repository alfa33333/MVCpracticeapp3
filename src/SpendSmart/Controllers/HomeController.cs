using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Data;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SpendSmartDbContext _context;

    public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpenses = allExpenses.Sum(expense => expense.Value);
        ViewBag.TotalExpenses = totalExpenses;
        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? Id)
    {
        if (Id != null)
        {
            var expense = _context.Expenses.SingleOrDefault(expense => expense.Id == Id);
            return View(expense);
        }

        return View();
    }

    [HttpPost]
    public IActionResult CreateEditExpense(Expense model)
    {
        if (model.Id == 0)
        {
            _context.Expenses.Add(model);
        }
        else
        {
            _context.Expenses.Update(model);
        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }

    public IActionResult DeleteExpense(int Id)
    {
        var expense = _context.Expenses.SingleOrDefault(expense => expense.Id == Id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }
        return RedirectToAction("Expenses");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
