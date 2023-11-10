using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hh_clone.ViewModels;
using System.Security.Cryptography;
using hh_clone.MiddleWares;

namespace hh_clone.Controllers
{
	[SiteAuthorize()]
	public class ProfileController : Controller
	{
		[HttpGet]
		[Route("/profile")]
		public IActionResult Index()
		{
			return View(new ProfileViewModel());
		}

		[HttpPost]
		[Route("/profile")]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> IndexSave()
		{
			string fileName = "";
			IFormFile imageData = Request.Form.Files[0];

			MD5 md5hash = MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(imageData.FileName);
			byte[] hashBytes = md5hash.ComputeHash(inputBytes);

			string hash = Convert.ToHexString(hashBytes);

			var dir = "./wwwroot/images/" + hash.Substring(0, 2) + "/" + hash.Substring(0, 4);

			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			fileName = dir + "/" + imageData.FileName;

			using (var stream = System.IO.File.Create(fileName))
			{
				await imageData.CopyToAsync(stream);
			}

				return View("Index", new ProfileViewModel());
		}
	}
}
