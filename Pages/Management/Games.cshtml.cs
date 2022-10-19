using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages.Management
{
    public class GamesModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
            List<Game> games = context.Games.ToList();

            ViewData["games"] = games;
        }

        public void OnPostRemove(int gameId)
        {

        }
    }
}
