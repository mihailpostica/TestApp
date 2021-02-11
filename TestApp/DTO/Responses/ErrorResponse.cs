using System.Collections.Generic;

namespace Test
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(ValidationError error)
        {
            Errors.Add(error);
        }

        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }

}