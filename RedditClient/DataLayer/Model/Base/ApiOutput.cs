using System;

namespace RedditClient.DataLayer.Base.Models.Base
{
    public class ApiOutput
    {
        public bool HasError { get; set; }
        public ApiOutputType OutputType { get; set; }
        public string Message { get; set; }
    }
}
