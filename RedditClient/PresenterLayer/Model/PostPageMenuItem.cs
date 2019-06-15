using System;

namespace RedditClient
{
    public class PostPageMenuItem
    {
        public PostPageMenuItem()
        {
            TargetType = typeof(PostPageDetail);
            Title = "Placeholder title";
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Comments { get; set; }
        public string Username { get; set; }
        public Type TargetType { get; set; }
    }
}
