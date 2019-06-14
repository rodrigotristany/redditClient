using System;
namespace RedditClient.DataLayer.Base.Models.Base
{
    public class ApiRequest<T>
    {
        public ApiRequest()
        {
            EncodedMediaType = "application/json";
        }

        public T Request { get; set; }
        public string RelativeUrl { get; set; }
        public string EncodedMediaType { get; set; }

    }
}
