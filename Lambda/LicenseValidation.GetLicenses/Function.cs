using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;
using LicenseValidation.Business.Abstract;
using LicenseValidation.Business.Implementation;
using LicenseValidation.Exceptions.App;
using LicenseValidation.Exceptions.Token;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LicenseValidation.GetLicenses
{
    public class Function
    {
        private ILicenseManager _licenseManager;
        public Function(ILicenseManager licenseManager)
        {
            _licenseManager = licenseManager;
        }

        public Function()
        {
            _licenseManager = new LicenseManager();
        }
        

        public async Task<GetLicensesResponse> FunctionHandler(GetLicensesRequest request, ILambdaContext context)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.AppId) ||
                string.IsNullOrWhiteSpace(request.AppToken))
            {
                return new GetLicensesResponse("Please add application data");
            }

            try
            {

                var licenses = await _licenseManager.GetAllLicensesForApp(request.AppToken, request.AppId);
                return new GetLicensesResponse()
                {
                    Licenses = licenses
                };

            }
            catch (AppDoesNotExistException ex)
            {
                return new GetLicensesResponse("Application does not exist");
            }
            catch (TokesDoesNotMatchToAppException ex)
            {
                return new GetLicensesResponse("App token mismatch");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new GetLicensesResponse(ex.Message);
            }
        }
    }
}
