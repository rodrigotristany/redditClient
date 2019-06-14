using System;
namespace RedditClient.DataLayer.Base.Models.Base
{
    public enum ApiOutputType
    {
        TIMEOUT,
        REQUEST_ERROR,
        RESPONSE_ERROR,
        SERVER_ERROR,
        JSON_ERROR,
        FORBIDEN_ERROR,
        GENERIC_ERROR,
        CANCELLED_TASK,
        NETWORK_FAILED,
        RESPONSE_OK,
        AUTHENTICATION_ERROR
    }
}
