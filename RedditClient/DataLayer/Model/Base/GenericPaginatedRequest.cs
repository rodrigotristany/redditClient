using System;
using Newtonsoft.Json;

namespace RedditClient.DataLayer.Base.Models.Base
{
    public class GenericPaginatedRequest
    {
        public GenericPaginatedRequest()
        {
            Desc = true;
            Page = 0;
            RowsPage = 100;
        }

        [JsonProperty("desc")]
        public bool Desc { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("rowsPage")]
        public int RowsPage { get; set; }
    }
}
