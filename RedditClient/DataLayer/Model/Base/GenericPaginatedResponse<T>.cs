using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RedditClient.DataLayer.Base.Models.Base
{
    public class GenericPaginatedResponse<T>
    {
        [JsonProperty("resultList")]
        public List<T> ResultList { get; set; }

        [JsonProperty("totalRows")]
        public int TotalRows { get; set; }
    }
}
