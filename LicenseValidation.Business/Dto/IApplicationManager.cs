using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LicenseValidation.Business.Model;

namespace LicenseValidation.Business.Dto
{
    public interface IApplicationManager
    {
        Task<Application> GetApplication(string file, string appId);
        Task<List<Application>> GetApplications(string file);
    }
}
