using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;

public class AwsSqsService : BackgroundService
{
    private string _queueUrl;

    public AwsSqsService()
    {
        _queueUrl = "https://sqs.us-west-2.amazonaws.com/22021*****12/SVTPWebQueue";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var amazonSqsClient = new AmazonSQSClient();
        do
        {
            var receiveMessageResponse = await amazonSqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                WaitTimeSeconds = 20
            });
            if (receiveMessageResponse.Messages.Count != 0)
            {
                var message = receiveMessageResponse.Messages.FirstOrDefault();
                Console.WriteLine(message.Body);
                await amazonSqsClient.DeleteMessageAsync(new DeleteMessageRequest
                {
                    QueueUrl = _queueUrl,
                    ReceiptHandle = message.ReceiptHandle
                });
            }
        } while (true);
    }
}
