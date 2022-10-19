using EvoltingStore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EvoltingStore.Pages.Management
{
    public class UsersModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
            var user = (String)HttpContext.Session.GetString("user");
            var userData = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(user);
            if(userData.RoleId != 1)
            {
                return;
            }

            List<User> users = context.Users.Include(u => u.Role).Include(u => u.UserDetail).ToList();

            ViewData["users"] = users;
        }

        public void OnPostStatus(int userId)
        {
            User user = context.Users.FirstOrDefault(u => u.UserId == userId);
            user.IsActive = !user.IsActive;

            context.SaveChanges();

            List<User> users = context.Users.Include(u => u.Role).Include(u => u.UserDetail).ToList();

            ViewData["users"] = users;
        }
    }
}
