using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EvoltingStore.Entity;

namespace EvoltingStore.Pages.Management
{
    public class UserDetailsModel : PageModel
    {
        EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet(int userId)
        {
            //var user = (String)HttpContext.Session.GetString("user");
            //var userData = (User)Newtonsoft.Json.JsonConvert.DeserializeObject<User>(user);
            //if (userData.RoleId != 1)
            //{
            //    return;
            //}

            UserDetail userDetail = context.UserDetails.FirstOrDefault(ud => ud.UserId == userId);

            ViewData["profile"] = userDetail;
        }
    }
}
