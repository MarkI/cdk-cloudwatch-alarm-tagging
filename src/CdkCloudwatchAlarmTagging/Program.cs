using Amazon.CDK;

namespace CdkCloudwatchAlarmTagging
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            var alarmStack = new CdkCloudwatchAlarmTaggingStack(app, "CdkCloudwatchAlarmTaggingStack");

            Tags.Of(alarmStack).Add("TestTag", "TagEverything");

            app.Synth();
        }
    }
}
