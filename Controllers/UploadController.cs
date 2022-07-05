using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolProject.Controllers
{
    public class UploadController : Controller
    {
        // GET: UploadController

        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(
            IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
              return View();
        }

        // GET: UploadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UploadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UploadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var x = collection;
                if (collection.Files != null)
                {
                    foreach(var file in collection.Files)
                    {
                        string folder = "books/pdf/";
                        var y = UploadImage(folder, file);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }

        // GET: UploadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UploadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UploadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UploadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
