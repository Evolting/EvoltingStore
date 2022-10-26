using EvoltingStore.Entity;
using Microsoft.AspNetCore.SignalR;

namespace EvoltingStore.Hubs
{
    public class CommentHub:Hub
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public async Task PostComment(String userId, String gameId, String comment)
        {
            int gId = Int32.Parse(gameId);
            int uId = Int32.Parse(userId);

            Comment c = new Comment();
            c.PostTime = DateTime.Now;
            c.UserId = uId;
            c.GameId = gId;
            c.Content = comment;

            context.Comments.Add(c);
            context.SaveChanges();

            await Clients.All.SendAsync("ReceivedMess", userId, comment);
        }
    }
}
