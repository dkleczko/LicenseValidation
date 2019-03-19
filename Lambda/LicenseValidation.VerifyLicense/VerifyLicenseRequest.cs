using System;
using System.Collections.Generic;
using System.Text;

namespace LicenseValidation.VerifyLicense
{
    public class VerifyLicenseRequest
    {
        public string AppId { get; set; }
        public string AppToken { get; set; }
        public string Name { get; set; }
        public string MachineKey { get; set; }
    }
}
