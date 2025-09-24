using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Models;

namespace PhotoAlbum.Controllers
{
    // The Home Controller class
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        List<Photo> photos;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            photos = new List<Photo>();

            Photo photo1 = new Photo();
            photo1.PhotoId = 1;
            photo1.Title = "Penny";
            photo1.Description = "My cat Penny who lives in Toronto";
            photo1.Filename = "penny.jpg";
            photo1.CreateDate = new DateTime(2024, 12, 15, 18, 30, 0);

            Photo photo2 = new Photo();
            photo2.PhotoId = 2;
            photo2.Title = "Audrey";
            photo2.Description = "Audrey is an old pup.";
            photo2.Filename = "audrey.jpg";
            photo2.CreateDate = new DateTime(2025, 08, 1);

            photos.Add(photo1);
            photos.Add(photo2);
        }

        public IActionResult Index()
        {  
            // Pass the photos List<> into the View
            return View(photos);
        }

        public IActionResult Details(int id)
        {
            Photo? photo = photos.Where(m => m.PhotoId == id).FirstOrDefault();

            if (photo == null)
            {
                return NotFound();   
            }
            else
            {
                return View(photo);
            }               
        }


        public IActionResult Privacy()
        {
            return View();
        }

        // My first action method
        public IActionResult Hello(int id)
        {

            int num = id;

            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
