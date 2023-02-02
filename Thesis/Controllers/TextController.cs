using Microsoft.AspNetCore.Mvc;

namespace Thesis.Controllers
{
    public class TextController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult TextEditor()
        {
            return View();
        }

        [HttpPost]
        public int save([FromBody] string content)
        {
            return 0;
        }
    }
}
