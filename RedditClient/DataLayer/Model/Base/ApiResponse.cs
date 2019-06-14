using System;

namespace RedditClient.DataLayer.Base.Models.Base
{
    public class ApiResponse<T>
    {
        public T Response { get; set; }
        public ApiOutput Output { get; set; }

        public ApiResponse()
        {
            this.Response = Activator.CreateInstance<T>();
            this.Output = new ApiOutput() { HasError = false, Message = ""};
        }

        public bool HasError
        {
            get
            {
                return Output.HasError;
            }
        }
	}
}
