using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedditClient.DataLayer.Base.Models.Base;
using Xamarin.Forms;

namespace RedditClient.DataLayer.Repository.Base
{
    public abstract class UtilHandlersHttpClient
    {
        private const string SERVICE_MESSAGE_NETWORK_FAILED = "Check your internet connection.";
        private const string SERVICE_MESSAGE_CANCELLED_TASK = "The operation was cancelled.";
        private const string SERVICE_MESSAGE_TIMEOUT = "The server is busy, please try again later";
        private const string SERVICE_MESSAGE_SERVER_ERROR = "An error occur trying to access the server";
        private const string SERVICE_MESSAGE_REQUEST_ERROR = "Check the request data";

        public async Task<ApiResponse<T>> CreateApiResponseByStatusCode<T>(HttpResponseMessage responseMessage)
        {
            ApiResponse<T> resultObject = new ApiResponse<T>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var stringResult = await responseMessage.Content.ReadAsStringAsync();
                resultObject.Response = JsonConvert.DeserializeObject<T>(stringResult);
                resultObject.Output.OutputType = ApiOutputType.RESPONSE_OK;
            }
            else
            {
                ErrorHandler(resultObject, responseMessage.StatusCode);
            }
            return resultObject;
        }

		public ApiResponse<T> ExceptionHandler<T>(Exception ex)
        {
            ApiResponse<T> apiResponse = new ApiResponse<T>();
            if (ex is OperationCanceledException)
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.CANCELLED_TASK;
                if(((OperationCanceledException)ex).CancellationToken.IsCancellationRequested)
                    apiResponse.Output.Message = SERVICE_MESSAGE_CANCELLED_TASK;
                else
                    apiResponse.Output.Message = SERVICE_MESSAGE_TIMEOUT;
            }
            else if (ex is HttpRequestException)
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.NETWORK_FAILED;
                apiResponse.Output.Message = SERVICE_MESSAGE_NETWORK_FAILED;
            }
            else
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.GENERIC_ERROR;
                apiResponse.Output.Message = SERVICE_MESSAGE_SERVER_ERROR;
            }

            return apiResponse;
        }

		public void ErrorHandler<T>(ApiResponse<T> apiResponse, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode is HttpStatusCode.BadRequest)
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.REQUEST_ERROR;
                apiResponse.Output.Message = SERVICE_MESSAGE_REQUEST_ERROR;
            }
            else if (httpStatusCode is HttpStatusCode.InternalServerError)
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.SERVER_ERROR;
                apiResponse.Output.Message = SERVICE_MESSAGE_SERVER_ERROR;
            }
            else
            {
                apiResponse.Output.HasError = true;
                apiResponse.Output.OutputType = ApiOutputType.GENERIC_ERROR;
                apiResponse.Output.Message = SERVICE_MESSAGE_SERVER_ERROR;
            }
        }
    }
}
