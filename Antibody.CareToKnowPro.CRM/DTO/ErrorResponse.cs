using Microsoft.AspNetCore.Http;

namespace Antibody.CareToKnowPro.CRM.DTO
{
    public class ErrorResponse
    {
        public Error Error { get; set; }

        public static ErrorResponse Create(Error error)
        {
            return Create(error.Code, error.Message);
        }

        public static ErrorResponse Create(int? code, string message)
        {
            return new ErrorResponse
            {
                Error = new Error
                {
                    Code = code ?? StatusCodes.Status500InternalServerError,
                    Message = message
                }
            };
        }
    }

    public class Error
    {
        public int? Code { get; set; }
        public string Message { get; set; }
    }
}