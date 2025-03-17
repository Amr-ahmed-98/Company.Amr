using System.Diagnostics;
using System.Text;
using Company.Amr.PL.Models;
using Company.Amr.PL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedServices scopedServices1;
        private readonly IScopedServices scopedServices2;
        private readonly ITransentServices transentServices1;
        private readonly ITransentServices transentServices2;
        private readonly ISingletonServices singletonServices1;
        private readonly ISingletonServices singletonServices2;

        public HomeController(
            ILogger<HomeController> logger,
            IScopedServices scopedServices1,
            IScopedServices scopedServices2,
            ITransentServices transentServices1,
            ITransentServices transentServices2,
            ISingletonServices singletonServices1,
            ISingletonServices singletonServices2
            )
        {
            _logger = logger;
            this.scopedServices1 = scopedServices1;
            this.scopedServices2 = scopedServices2;
            this.transentServices1 = transentServices1;
            this.transentServices2 = transentServices2;
            this.singletonServices1 = singletonServices1;
            this.singletonServices2 = singletonServices2;
        }

        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"scopedServices1 :: {scopedServices1.GetGuid()}\n");
            builder.Append($"scopedServices2 :: {scopedServices2.GetGuid()}\n\n");
            builder.Append($"transentServices1 :: {transentServices1.GetGuid()}\n");
            builder.Append($"transentServices2 :: {transentServices2.GetGuid()}\n\n");
            builder.Append($"singletonServices1 :: {singletonServices1.GetGuid()}\n");
            builder.Append($"singletonServices2 :: {singletonServices2.GetGuid()}\n\n");

            return builder.ToString();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
