using Amazon.CDK;

namespace CdkCloudwatchAlarmTagging
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new CdkCloudwatchAlarmTaggingStack(app, "CdkCloudwatchAlarmTaggingStack");

            app.Synth();
        }
    }
}
