# Welcome to your CDK C# project!

You should explore the contents of this project. It demonstrates a CDK app with an instance of a stack (`CdkCloudwatchAlarmTaggingStack`)
which contains an Amazon SQS queue that is subscribed to an Amazon SNS topic.

It also create a CloudWatch Alarm for the ApproximateNumberOfMessagesVisible metric of the SQS queue. The CloudWatch Alarm is tagged using the [AwsCustomResource](https://docs.aws.amazon.com/cdk/api/latest/dotnet/api/Amazon.CDK.CustomResources.AwsCustomResource.html).

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk ls`           list all stacks in the app
* `cdk synth`       emits the synthesized CloudFormation template
* `cdk deploy`      deploy this stack to your default AWS account/region
* `cdk diff`        compare deployed stack with current state
* `cdk docs`        open CDK documentation

Enjoy!
