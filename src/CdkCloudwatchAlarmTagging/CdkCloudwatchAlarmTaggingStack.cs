using Amazon.CDK;
using Amazon.CDK.AWS.CloudWatch;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SQS;
using Amazon.CDK.CustomResources;
using Constructs;
using System.Collections.Generic;

namespace CdkCloudwatchAlarmTagging
{
    public class CdkCloudwatchAlarmTaggingStack : Stack
    {
        internal CdkCloudwatchAlarmTaggingStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
             // The CDK includes built-in constructs for most resource types, such as Queues and Topics.
            var queue = new Queue(this, "CdkCloudwatchAlarmTaggingQueue", new QueueProps
            {
                VisibilityTimeout = Duration.Seconds(300)
            });

            var topic = new Topic(this, "CdkCloudwatchAlarmTaggingTopic");

            topic.AddSubscription(new SqsSubscription(queue));

            var metric = queue.Metric("ApproximateNumberOfMessagesVisible");

            var alarm = metric.CreateAlarm(this, "Alarm", new CreateAlarmOptions
            {
                Threshold = 100,
                EvaluationPeriods = 3,
                DatapointsToAlarm = 2
            });

            #region Another Alarm create option
            //var alarm = new Alarm(this, "Alarm", new AlarmProps
            //{
            //    Metric = metric,
            //    Threshold = 100,
            //    EvaluationPeriods = 3,
            //    DatapointsToAlarm = 2
            //});
            #endregion

            //Role role; 
            AwsCustomResource awsCustomResource = new AwsCustomResource(this, "MyAwsCustomResource", new AwsCustomResourceProps
            {
                //Policy = AwsCustomResourcePolicy.FromSdkCalls(new SdkCallsPolicyOptions
                //{
                //    Resources = AwsCustomResourcePolicy.ANY_RESOURCE
                //}),
                Policy = AwsCustomResourcePolicy.FromStatements(new PolicyStatement[] {
                    new PolicyStatement(new PolicyStatementProps{
                        Effect = Effect.ALLOW,
                        Actions = new string[] { "cloudwatch:TagResource", "cloudwatch:UntagResource" },
                        Resources = new string[] { alarm.AlarmArn }
                    })
                }),
                // the properties below are optional
                FunctionName = "TagCloudWatchAlarm",
                InstallLatestAwsSdk = false,
                LogRetention = RetentionDays.ONE_DAY,
                OnCreate = new AwsSdkCall
                {
                    Action = "tagResource",
                    Service = "CloudWatch",
                    PhysicalResourceId = PhysicalResourceId.Of($"{alarm.AlarmName}_Tag"),
                    Parameters = new Dictionary<string, object> {
                        { "ResourceARN", alarm.AlarmArn },
                        { "Tags", new object[] { new Dictionary<string, string>(){ { "Key", "TestTag" }, { "Value", "TagEverything" }} } }
                    }
                    // the properties below are optional
                    //ApiVersion = "apiVersion",
                    //AssumedRoleArn = "assumedRoleArn",
                    //IgnoreErrorCodesMatching = "ignoreErrorCodesMatching",
                    //OutputPath = "outputPath",
                    //OutputPaths = new[] { "outputPaths" },
                    //Parameters = parameters,
                    //PhysicalResourceId = physicalResourceId,
                    //Region = "region"
                },
                OnUpdate = new AwsSdkCall
                {
                    Action = "tagResource",
                    Service = "CloudWatch",
                    PhysicalResourceId = PhysicalResourceId.Of($"{alarm.AlarmName}_Tag"),
                    Parameters = new Dictionary<string, object> {
                        { "ResourceARN", alarm.AlarmArn },
                        { "Tags", new object[] { new Dictionary<string, string>(){ { "Key", "TestTag" }, { "Value", "TagEverything" }} } }
                    }
                },
                OnDelete = new AwsSdkCall
                {
                    Action = "untagResource",
                    Service = "CloudWatch",
                    Parameters = new Dictionary<string, object> {
                        { "ResourceARN", alarm.AlarmArn },
                        { "TagKeys", new string[] { "TestTag" }},
                    }
                },
                //ResourceType = "resourceType",
                //Role = role,
                Timeout = Duration.Minutes(3)
            });
        }
    }
}
