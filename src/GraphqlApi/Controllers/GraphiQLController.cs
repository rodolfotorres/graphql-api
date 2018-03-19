using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_react.Controllers
{
    public class GraphiQLController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
