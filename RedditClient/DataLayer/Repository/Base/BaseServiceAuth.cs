using System;
using System.Net.Http;
using System.Threading.Tasks;
using Commons;
using RedditClient.DataLayer.Base.Models.Base;

namespace RedditClient.DataLayer.Repository.Base
{
	public abstract class BaseServiceAuth : BaseService
    {
		public BaseServiceAuth(string token = null) : base(token)
        {
            base.http.BaseAddress = new Uri(Environments.Current.AuthEndpoint);
        }

        protected override async Task<ApiResponse<T>> Get<T>(string relativeUri = "")
        {
            ApiResponse<T> resultObject = new ApiResponse<T>();
            try
            {
                await SetHeaderToken();
                if(!AuthenticationService.Instance.Token.HasError)
                {
                    HttpResponseMessage response = await http.GetAsync(relativeUri);
                    resultObject = await CreateApiResponseByStatusCode<T>(response);
                }
                else
                {
                    resultObject.Output.HasError = true;
                    resultObject.Output.Message = Constants.SERVICE_MESSAGE_LOGIN_ERROR;
                }

            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

    }
}
