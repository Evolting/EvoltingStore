using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EvoltingStore.Pages
{
    public class CategoriesModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
            List<Game> games = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).OrderBy(g => g.Name).ToList();
            List<Genre> genres = context.Genres.ToList();
            List<Boolean> selected = new List<Boolean>();
            for (int i = 1; i <= genres.Count; i++)
            {
                selected.Add(false);
            }

            ViewData["games"] = games;
            ViewData["genres"] = genres;
            ViewData["selected"] = selected;
        }

        public void OnPostFilter(List<int> genre, string searchName, string orderBy)
        {
            List<Game> games = new List<Game>();
            List<Boolean> selected = new List<Boolean>();
            List<Genre> genres = context.Genres.ToList();

            if(genre.Count > 0)
            {
                List<Genre> selectedGenre = new List<Genre>();

                foreach (var genreId in genre)
                {
                    selectedGenre.Add(genres[genreId - 1]);
                }

                foreach(var game in context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).ToList())
                {
                    HashSet<Genre> common = new HashSet<Genre>(game.Genres);
                    common.IntersectWith(selectedGenre);
                    if (common.Count == selectedGenre.Count)
                    {
                        games.Add(game);
                    }
                }

                for (int i = 1; i <= genres.Count; i++)
                {
                    if (genre.Contains(i)) selected.Add(true);
                    else selected.Add(false);
                }
            }
            else
            {
                games = context.Games.Include(g => g.Genres).Include(g => g.Comments).Include(g => g.Users).ToList();

                for (int i = 1; i <= genres.Count; i++)
                {
                    selected.Add(false);
                }
            }

            ViewData["games"] = games;
            ViewData["genres"] = genres;
            ViewData["selected"] = selected;
        }
    }
}
