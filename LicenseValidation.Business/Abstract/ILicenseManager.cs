using System.Collections.Generic;
using System.Threading.Tasks;
using LicenseValidation.Business.Dto;
using LicenseValidation.Business.Model;

namespace LicenseValidation.Business.Abstract
{
    public interface ILicenseManager
    {
        Task<License> GetLicense(LicenseDto dto);
        Task<License> LoadLicense(string machineKey, string appId);
        Task<License> CreateTrialLicense(LicenseDto dto);
        Task AuthTokenMatchesToApp(string appId, string appToken);
        Task<List<License>> GetAllLicensesForApp(string token, string appId);
    }
}
