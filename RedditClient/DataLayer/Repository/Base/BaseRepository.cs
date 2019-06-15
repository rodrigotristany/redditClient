using Commons;
using Commons.Cache;
using Commons.Environment;
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
        private static AuthenticationService TokenService;

        public BaseService(string token = null)
        {
            http = new HttpClient(new RequestHanlder(token));
            http.Timeout = new TimeSpan(0, 0, 0, 0, Constants.SERVICE_TIMEOUT);
            http.BaseAddress = new Uri(Environments.Current.ApiEndpoint);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Add("Origin", Environments.Current.OriginUrl);

            if (TokenService == null)
                TokenService = AuthenticationService.Instance;
        }

        protected async Task SetHeaderToken()
        {
            if (CacheUserData.Token?.AccessToken == null)
                await TokenService.GetToken();
            else if (AuthenticationService.Instance.CheckTokenExpirated())
                await TokenService.RefreshToken();
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + CacheUserData.Token?.AccessToken);
        }

        protected virtual async Task<ApiResponse<T>> Get<T>(string relativeUri = "")
        {
            ApiResponse<T> resultObject = new ApiResponse<T>();
            try
            {
                await SetHeaderToken();
                HttpResponseMessage response = await http.GetAsync(relativeUri);
                resultObject = await CreateApiResponseByStatusCode<T>(response);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await Get<T>(relativeUri);
                }
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
                await SetHeaderToken();
                HttpResponseMessage response = await http.GetAsync(relativeUri, ct);
                resultObject = await CreateApiResponseByStatusCode<T>(response);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await GetWithCancellation<T>(ct, relativeUri);
                }
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }

            return resultObject;
        }

        protected async Task<ApiResponse<T>> Post<T, TEntity>(ApiRequest<TEntity> request)
        {
            ApiResponse<T> resultObject;
            try
            {
                await SetHeaderToken();
                var json = JsonConvert.SerializeObject(request.Request);
                HttpContent content = new StringContent(json, Encoding.UTF8, request.EncodedMediaType);
                HttpResponseMessage result = await http.PostAsync(request.RelativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await Post<T, TEntity>(request);
                }
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
                await SetHeaderToken();
                HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await http.PostAsync(relativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await Post<T>(relativeUrl);
                }
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
                await SetHeaderToken();
                string json = JsonConvert.SerializeObject(request.Request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await http.PutAsync(request.RelativeUrl, content);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await Put<T>(request);
                }
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
                await SetHeaderToken();
                HttpResponseMessage result = await http.DeleteAsync(relativeUrl);
                resultObject = await CreateApiResponseByStatusCode<T>(result);
                if (resultObject.Output.OutputType == ApiOutputType.AUTHENTICATION_ERROR)
                {
                    if (CacheUserData.Token?.AccessToken != null)
                        await TokenService.RefreshToken();
                    else
                        await TokenService.GetToken();
                    return await Delete<T>(relativeUrl);
                }
            }
            catch (Exception ex)
            {
                resultObject = ExceptionHandler<T>(ex);
            }
            return resultObject;
        }

        protected bool CacheExpirated(DateTime lastPetitionDate)
        {
            bool result = false;
            double differenceTime = (DateTime.Now - lastPetitionDate).TotalHours;
            result = differenceTime - CacheUserData.LimitHoursToReuseData > 0.00001;
            return result;
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
