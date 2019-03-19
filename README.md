# License Validation

License validation is application based on AWS cloud services. App can be used to verify current user licenses, automatic accept trail periods, verifying current licenses.

## Getting Started

To run LicenseValidation aws account is necessary.

One file to configure main services via CloudFormation "configuraiton.yaml":

- DynamoDb
- S3
- IAM roles and polices

File apps.json should be deployed on S3 Bucket.

Lambdas have to be publish manually via VS AWS Toolkit or AWS Console.

Current Lambdas could be easy set behind API Gateway so it will be accept REST calls or can be called directly from application .Net code.

### Prerequisites

```
- AWS Account
- AWS Console / AWS Toolkit for Visual Studio
```

### AWS Services used
- AWS Lambda
- DynamoDb
- X-Ray
- IAM
- S3
- CloudFormation

## Authors

* **Damian Kleczko** - *AWS cloud License Validation* - [Damian Kleczko](https://github.com/dkleczko)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details


