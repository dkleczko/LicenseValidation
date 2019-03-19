using System;
using LicenseValidation.Business.Dto;
using LicenseValidation.Business.Model;
using LicenseValidation.Shared;

namespace LicenseValidation.VerifyLicense
{
    public class VerifyLicenseResponse : Response
    {
        public string MachineKey { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserName { get; set; }

        public VerifyLicenseResponse() { }

        public VerifyLicenseResponse(string errorMessage)
            : base(errorMessage) { }

        public VerifyLicenseResponse(License license)
        {
            MachineKey = license.MachineKey;
            ApplicationId = license.ApplicationId;
            ApplicationToken = license.ApplicationToken;
            CreateDate = license.CreateDate;
            ExpiryDate = license.ExpiryDate;
            UserName = license.UserName;
        }

    }
}
