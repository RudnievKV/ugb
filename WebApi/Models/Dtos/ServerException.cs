using System.Net;

namespace WebApi.Models.Dtos
{
    public class ServerException : Exception
    {
        public ServerException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ServerException()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public ServerException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public ServerException(string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
