using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using LicenseValidation.Business.Statics;

namespace LicenseValidation.Business.Model
{
    [DynamoDBTable(LicenseStatic.LicenseTableName)]
    public class License
    {
        [DynamoDBHashKey]
        public string MachineKey { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey("ApplicationId-index")]
        public string ApplicationId { get; set; }
        public string ApplicationToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserName { get; set; }
    }
}
