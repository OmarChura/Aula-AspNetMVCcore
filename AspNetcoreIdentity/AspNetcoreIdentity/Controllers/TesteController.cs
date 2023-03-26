using Microsoft.AspNetCore.Mvc;


namespace AspNetcoreIdentity.Controllers
{
    public class TesteController : Controller
    {
        private readonly ILogger<TesteController> _logger;

        public TesteController(ILogger<TesteController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogError("Esse erro aconteceu!");

            return View();
        }
    }
}
