using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Salmpled.Models;
using FirebaseAdmin;
using Newtonsoft.Json;
namespace Salmpled.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        //this controller is necessary when deploying on aws as a health check
        [HttpGet("/")]
        public IEnumerable<string> Index()
        {
            return new List<string>()
            {
                "Welcome Home",
            };
        }

    }
}