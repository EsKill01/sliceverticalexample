using Microsoft.AspNetCore.Http;

namespace TP.NA.CartService.Application.Commons
{
    #region Interfaces

    /// <summary>
    /// Interface by Response
    /// </summary>
    public interface IResponse
    {
        int StatusCode { get; set; }

        /// <summary>
        /// Indicate if has error
        /// </summary>
        bool IsError { get; set; }

        /// <summary>
        /// List of errors
        /// </summary>
        List<ValidationMessage> Errors { get; set; }
    }

    #endregion Interfaces

    #region Response

    public class Response<T> : IResponse
    {
        /// <summary>
        /// Data to return
        /// </summary>
        public T Payload { get; set; }

        /// <summary>
        /// Is error
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// List of errors
        /// </summary>
        public List<ValidationMessage> Errors { get; set; } = new List<ValidationMessage>();

        /// <summary>
        /// Gets or sets status code
        /// </summary>
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        /// <summary>
        /// Set a failure response
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        public void SetFailureResponse(string property, string message, int statusCode = StatusCodes.Status400BadRequest)
        {
            IsError = true;
            StatusCode = statusCode;
            Errors = new List<ValidationMessage>
            {
                new ValidationMessage
                {
                    Message = message,
                    Property = property
                }
            };
        }

        /// <summary>
        /// Set a failure response
        /// </summary>
        /// <param name="errors"></param>
        public void SetFailureResponse(List<ValidationMessage> errors, int statusCode = StatusCodes.Status400BadRequest)
        {
            IsError = true;
            StatusCode = statusCode;
            Errors.AddRange(errors);
        }
    }

    #endregion Response

    #region Validation messages

    public class ValidationMessage
    {
        /// <summary>
        /// Property
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }

    #endregion Validation messages
}