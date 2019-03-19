using System;

namespace LicenseValidation.Shared
{
    public class Response
    {
        public string ErrorMessage { get; set; }

        public Response()
        {

        }

        public Response(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
 
    }
}
