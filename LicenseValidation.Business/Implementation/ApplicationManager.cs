using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using LicenseValidation.Business.Dto;
using LicenseValidation.Business.Model;
using LicenseValidation.Exceptions.App;
using Newtonsoft.Json;

namespace LicenseValidation.Business.Implementation
{
    public class ApplicationManager : IApplicationManager
    {
        private string _s3BucketName;
        private RegionEndpoint _regionEndpoint;
        private IAmazonS3 _client;

        public ApplicationManager(string s3BucketName)
        {
            _s3BucketName = s3BucketName;
            _regionEndpoint = RegionEndpoint.EUWest1;
            _client = new AmazonS3Client(_regionEndpoint);
        }

        public ApplicationManager(string s3BucketName, RegionEndpoint regionEndpoint)
        {
            _s3BucketName = s3BucketName;
            _regionEndpoint = regionEndpoint;
            _client = new AmazonS3Client(_regionEndpoint);
        }

        public async Task<Application> GetApplication(string file, string appId)
        {
            var allApplications = await GetApplications(file);
            Console.WriteLine("App Lenghts: {0} appId: {1}", allApplications.Count, appId);
            var requiredApplication = allApplications.FirstOrDefault(p => p.Id == appId);
            if (requiredApplication != null)
            {
                return requiredApplication;
            }

            throw new AppDoesNotExistException();

        }

        public async Task<List<Application>> GetApplications(string file)
        {
            try
            {
                var request = new GetObjectRequest()
                {
                    BucketName = _s3BucketName,
                    Key = file
                };
                using (var response = await _client.GetObjectAsync(request))
                {
                    using (var responseStream = response.ResponseStream)
                    using (var reader = new StreamReader(responseStream))
                    {
                        var responseBody = reader.ReadToEnd();

                        return JsonConvert.DeserializeObject<List<Application>>(responseBody);
                    }

                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error when reading from s3 bucket: {0}", e.Message);
                Console.WriteLine(e.ToString());
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
