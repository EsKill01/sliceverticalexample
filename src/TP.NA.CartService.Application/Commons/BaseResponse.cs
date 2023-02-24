namespace TP.NA.CartService.Application.Commons
{
    /// <summary>
    /// Base response
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Gets or sets a value whatever is error
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// Gets or sets error validation messages list
        /// </summary>
        public List<ValidationMessage> Errors { get; set; }

        /// <summary>
        ///  Gets or sets status code response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Set failure response
        /// </summary>
        /// <param name="property">Property name</param>
        /// <param name="message">Message property</param>
        public void SetFailureResponse(string property, string message)
        {
            IsError = true;
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
        /// Set failure response
        /// </summary>
        /// <param name="errors">Error message parameter</param>
        public void SetFailureResponse(List<ValidationMessage> errors)
        {
            IsError = true;
            Errors.AddRange(errors);
        }
    }
}