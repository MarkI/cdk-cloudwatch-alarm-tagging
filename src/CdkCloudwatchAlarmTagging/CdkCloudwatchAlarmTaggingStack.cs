using Amazon.CDK;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.SQS;
using Constructs;

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
        }
    }
}
