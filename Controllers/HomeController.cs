using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers;

public class HomeController : Controller
{
    private readonly ITaskService _taskService;

    public HomeController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var dashboardData = await _taskService.GetDashboardDataAsync(userId);
        return View(dashboardData);
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
}