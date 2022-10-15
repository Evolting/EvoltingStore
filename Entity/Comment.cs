using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Comment1 { get; set; } = null!;
        public byte[] Time { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
