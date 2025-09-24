using Microsoft.AspNetCore.Mvc;

namespace PhotoAlbum.Controllers
{
    public class PhotosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
