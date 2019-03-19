using System;
using System.Collections.Generic;
using LicenseValidation.Business.Model;
using LicenseValidation.Shared;

namespace LicenseValidation.GetLicenses
{
    public class GetLicensesResponse : Response
    {

        public List<License> Licenses { get; set; }

        public GetLicensesResponse() { }

        public GetLicensesResponse(string errorMessage)
            : base(errorMessage) { }
    }
}
