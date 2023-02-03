using Microsoft.AspNetCore.Mvc;

namespace Thesis.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult save(IFormFile file)
        {
            if (file != null)
            {
                //TODO
                //get list of allowed extensions from db
                List<string> extensions = new List<string>();
                extensions.Add(".pdf");
                extensions.Add(".docx");
                extensions.Add(".jpg");
                if (extensions.Contains(Path.GetExtension(file.FileName)) && file.Length < 4194304)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            return View("index");
        }
    }
}
