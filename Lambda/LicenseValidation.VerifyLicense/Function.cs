using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using LicenseValidation.Business.Abstract;
using LicenseValidation.Business.Dto;
using LicenseValidation.Business.Implementation;
using LicenseValidation.Exceptions.App;
using LicenseValidation.Exceptions.Token;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LicenseValidation.VerifyLicense
{
    public class Function
    {

        private ILicenseManager _licenseManager;

        public Function()
        {
            _licenseManager = new LicenseManager();
        }

        public Function(ILicenseManager licenseManager)
        {
            _licenseManager = licenseManager;
        }

        public async Task<VerifyLicenseResponse> FunctionHandler(VerifyLicenseRequest request, ILambdaContext context)
        {
            if (request == null)
            {
                return new VerifyLicenseResponse()
                {
                    ErrorMessage = "Please add application data"
                };
            }

            try
            {
                var license = await _licenseManager.GetLicense(new LicenseDto()
                {
                    AppId = request.AppId,
                    AppToken = request.AppToken,
                    MachineKey = request.MachineKey,
                    UserName = request.Name
                });

                return new VerifyLicenseResponse(license);
            }
            catch (AppDoesNotExistException ex)
            {
                return new VerifyLicenseResponse("Application does not exist");
            }
            catch (AppDoesNotSupportAutoLicenceException ex)
            {
                return new VerifyLicenseResponse("Application does not support trial license");
            }
            catch (TokesDoesNotMatchToAppException ex)
            {
                return new VerifyLicenseResponse("App token mismatch");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new VerifyLicenseResponse("Something went wrong. Please contact CS-IT support");
            }
        }
    }
}
