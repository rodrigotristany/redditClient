using Newtonsoft.Json;
using RedditClient.DataLayer.Base.Models.Base;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditClient.DataLayer.Repository.Base
{
	public abstract class BaseService : UtilHandlersHttpClient
    {
        protected HttpClient http;

        private readonly int SERVICE_TIMEOUT = 30000;
        private readonly string redditBaseURL = "http://www.reddit.com/dev/api";

        public BaseService(string token = null)
        {
            http = new HttpClient(new RequestHanlder(token));
            http.Timeout = new TimeSpan(0, 0, 0, 0, SERVICE_TIMEOUT);
            http.BaseAddress = new Uri(redditBaseURL);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected virtual async Task<ApiResponse<T>> Get<T>(string relativeUri = "")
        {
            ApiResponse<T> resultObject = new ApiResponse<T>();
            try
            {
                HttpResponseMessage response = await http.GetAsync(relativeUri);
                resultObject = await CreateApiResponseByStatusCode<T>(response);
			
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

        protected async Task<ApiResponse<T>> GetWithCancellation<T>(CancellationToken ct, string relativeUri = "")
        {
            ApiResponse<T> resultObject;
            try
            {
                HttpResponseMessage response = await http.GetAsync(relativeUri, ct);
                resultObject = await CreateApiResponseByStatusCode<T>(response);
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

		protected async Task<ApiResponse<T>> Post<T,TEntity>(ApiRequest<TEntity> request)
        {
            ApiResponse<T> resultObject;
            try
            {
                var json = JsonConvert.SerializeObject(request.Request);
                HttpContent content = new StringContent(json, Encoding.UTF8, request.EncodedMediaType);
                HttpResponseMessage result = await http.PostAsync(request.RelativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

        protected async Task<ApiResponse<T>> Post<T>(string relativeUrl = "")
        {
            ApiResponse<T> resultObject;
            try
            {
                HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await http.PostAsync(relativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

        protected async Task<ApiResponse<T>> Put<T>(ApiRequest<T> request)
        {
            ApiResponse<T> resultObject;
            try
            {
                string json = JsonConvert.SerializeObject(request.Request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await http.PutAsync(request.RelativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }
            return resultObject;
        }

        protected async Task<ApiResponse<T>> Delete<T>(string relativeUrl = "")
        {
            ApiResponse<T> resultObject;
            try
            {
                HttpResponseMessage result = await http.DeleteAsync(relativeUrl);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }
            return resultObject;
        }
    }

    class RequestHanlder : HttpClientHandler
    {
        private string Token;

        public RequestHanlder(string token) : base()
        {
            this.Token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Token != null)
            {
                request.Headers.Add("Authorization", $"Bearer {Token}");
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
