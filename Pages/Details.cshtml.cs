using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages
{
    public class DetailsModel : PageModel
    {
        EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet(int gameId)
        {
            Game game = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).FirstOrDefault(g => g.GameId == gameId);

            GameRequirement minimum = context.GameRequirements.FirstOrDefault(gr => gr.GameId == gameId && gr.Type.Equals("minimum"));
            GameRequirement recommend = context.GameRequirements.FirstOrDefault(gr => gr.GameId == gameId && gr.Type.Equals("recommend"));

            ViewData["game"] = game;
            ViewData["minimum"] = minimum;
            ViewData["recommend"] = recommend;
        }
    }
}
