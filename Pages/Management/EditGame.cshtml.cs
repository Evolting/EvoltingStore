using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using dotenv.net;
using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages.Management
{
    public class EditGameModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public EditGameModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet(int gameId)
        {
            Game game = context.Games.FirstOrDefault(g => g.GameId == gameId);

            GameRequirement minimum = context.GameRequirements.FirstOrDefault(gr => gr.GameId == gameId && gr.Type.Equals("minimum"));
            GameRequirement recommend = context.GameRequirements.FirstOrDefault(gr => gr.GameId == gameId && gr.Type.Equals("recommend"));

            ViewData["game"] = game;
            ViewData["minimum"] = minimum;
            ViewData["recommend"] = recommend;
        }

        public async Task<IActionResult> OnPost(IFormFile gameImg, Game game, GameRequirement minimum, GameRequirement recommend)
        {
            minimum.Type = "minimum";
            minimum.GameId = game.GameId;

            recommend.Type = "recommend";
            recommend.GameId = game.GameId;

            Game g = context.Games.FirstOrDefault(g => g.GameId == game.GameId);

            if (gameImg != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", gameImg.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await gameImg.CopyToAsync(fileStream);
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
            }
            else
            {
                game.Image = g.Image;
            }

            game.ReleaseDate = g.ReleaseDate;
            game.UpdateDate = DateTime.Now;

            context = new EvoltingStoreContext();
            context.Entry<Game>(game).State = EntityState.Modified;
            context.SaveChanges();

            GameRequirement m = context.GameRequirements.FirstOrDefault(gr => gr.GameId == game.GameId && gr.Type.Equals("minimum"));
            GameRequirement r = context.GameRequirements.FirstOrDefault(gr => gr.GameId == game.GameId && gr.Type.Equals("recommend"));

            if (m == null)
            {
                var context = new EvoltingStoreContext();
                context.GameRequirements.Add(minimum);
                context.SaveChanges();
            }
            else
            {
                var context = new EvoltingStoreContext();
                context.Entry<GameRequirement>(minimum).State = EntityState.Modified;
                context.SaveChanges();
            }

            if (r == null)
            {
                var context = new EvoltingStoreContext();
                context.GameRequirements.Add(recommend);
                context.SaveChanges();
            }
            else
            {
                var context = new EvoltingStoreContext();
                context.Entry<GameRequirement>(recommend).State = EntityState.Modified;
                context.SaveChanges();
            }


            return Redirect("/Management/Games");
        }
    }
}
