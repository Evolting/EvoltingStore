using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EvoltingStore.Entity;

namespace EvoltingStore.Pages.Management
{
    public class AddGameModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public AddGameModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(IFormFile gameImg, Game game)
        {
            if (gameImg != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", gameImg.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await gameImg.CopyToAsync(fileStream);
                }
            }

            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(_environment.ContentRootPath, "uploads", gameImg.FileName)),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var myJsonString = uploadResult.JsonObj.ToString();
            var data = JObject.Parse(myJsonString);

            var url = data["url"].ToString();

            game.Image = url;
            game.UpdateDate = DateTime.Now;

            context.Games.Add(game);
            context.SaveChanges();

            return Redirect("/Management/Games");
        }
    }
}
