using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FeyenZylstra.PhotoKiosk.Models;
using FeyenZylstra.PhotoKiosk.Services;

namespace FeyenZylstra.PhotoKiosk.Controllers
{
    [Produces("application/xml")]
    public class PhotosController: Controller
    {
        private readonly PhotoService _photoService;

        public PhotosController(PhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        [Route("api/photos/view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/photos/download/{id}")]
        public async Task<IActionResult> DownloadAsync([FromRoute] string id)
        {
            var stream = await _photoService.GetPhotoAsync(id);
            return File(stream, "image/png", id + ".png");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/photos/random")]
        public IActionResult Random()
        {
            var (stream, filename) = _photoService.GetRandomPhoto();
            return File(stream, "image/png", filename);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/photos/upload")]
        public async Task<IActionResult> UploadAsync([FromForm] Photo photo)
        {
            var file = Request.Form.Files["media"];

            if (file != null)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _photoService.SavePhotoAsync(stream, default);
                }
            }

            var response = new Response {Status = "ok"};

            await Task.CompletedTask;

            return Ok(response);
        }

    }
}
