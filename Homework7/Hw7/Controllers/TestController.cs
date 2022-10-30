﻿using Hw7.Models;
using Hw7.Models.ForTests;
using Microsoft.AspNetCore.Mvc;

namespace Hw7.Controllers;

public class TestController : Controller
{
    // GET
    [HttpGet]
    public IActionResult TestModel()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult TestModel(TestModel testModel)
    {
        return View(testModel);
    }
}