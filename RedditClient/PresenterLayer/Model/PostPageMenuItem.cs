using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditClient
{
    public class PostPageMenuItem
    {
        public PostPageMenuItem()
        {
            TargetType = typeof(PostPageDetail);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Comments { get; set; }
        public string Username { get; set; }
        public Type TargetType { get; set; }
    }
}
