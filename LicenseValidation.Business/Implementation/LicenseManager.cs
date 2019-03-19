using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3.Model;
using LicenseValidation.Business.Abstract;
using LicenseValidation.Business.Dto;
using LicenseValidation.Business.Model;
using LicenseValidation.Business.Statics;
using LicenseValidation.Exceptions.App;
using LicenseValidation.Exceptions.Token;

namespace LicenseValidation.Business.Implementation
{
    public class LicenseManager : ILicenseManager
    {
        private string _bucketName = Environment.GetEnvironmentVariable(LicenseStatic.LicenseBucketName);
        private string _fileName = Environment.GetEnvironmentVariable(LicenseStatic.LicenseFileName);
        private string _dynamoDbTableName = LicenseStatic.LicenseTableName;


        private readonly IApplicationManager _applicationManager;

        public LicenseManager()
        {
            _applicationManager = new ApplicationManager(_bucketName);
        }


        public async Task<License> GetLicense(LicenseDto dto)
        {
            await AuthTokenMatchesToApp(dto.AppId, dto.AppToken);
            var license = await LoadLicense(dto.MachineKey, dto.AppId);
            if (license == null)
            {
                license = await CreateTrialLicense(dto);
            }

            return license;

        }

        public async Task<List<License>> GetAllLicensesForApp(string token, string appId)
        {
            await AuthTokenMatchesToApp(appId, token);
            var licenses = await LoadAllLicensesForApp(appId);

            return licenses;
        }

        public async Task<List<License>> LoadAllLicensesForApp(string appId)
        {
            var dynamoDb = GetDynamoDbContext();
            var asyncSearch = dynamoDb.ScanAsync<License>
                (new []{new ScanCondition("ApplicationId", ScanOperator.Equal, appId)});

            var result = (await asyncSearch.GetRemainingAsync()).ToList();
            return result;
        }

        public async Task<License> LoadLicense(string machineKey, string appId)
        {
            var dynamoDbContext = GetDynamoDbContext();
            return await dynamoDbContext.LoadAsync<License>(machineKey, appId);
        }

        public async Task<License> CreateTrialLicense(LicenseDto dto)
        {
            Console.WriteLine(_fileName);
            var app = await _applicationManager.GetApplication(_fileName, dto.AppId);
            if (app != null && app.IsTrial())
            {
                var expiryDate = DateTime.UtcNow.AddDays(app.TrialDays);
                var license = new License()
                {
                    MachineKey = dto.MachineKey,
                    ApplicationId = dto.AppId,
                    ApplicationToken = dto.AppToken,
                    CreateDate =  DateTime.UtcNow,
                    ExpiryDate = expiryDate,
                    UserName = dto.UserName
                };

                var dynamoDbClinet = GetDynamoDbContext();
                await dynamoDbClinet.SaveAsync(license);

                return await LoadLicense(dto.MachineKey, dto.AppId);
            }

            throw new AppDoesNotSupportAutoLicenceException();
        }



        public async Task AuthTokenMatchesToApp(string appId, string appToken)
        {
            var app = await _applicationManager.GetApplication(_fileName, appId);
            if(app == null || app.Token != appToken)
                throw new TokesDoesNotMatchToAppException();
        }

        private DynamoDBContext GetDynamoDbContext()
        {
            return new DynamoDBContext(new AmazonDynamoDBClient());
            
        }


    }
}
