using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Configuration;
using System.Linq;

namespace EmailConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var accessKey = ConfigurationManager.AppSettings.Get("AccessKey");
            var secretKey = ConfigurationManager.AppSettings.Get("SecretKey");
            var queueName = "EmailQueue2";

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

            IAmazonSQS amazonSQS = new AmazonSQSClient(credentials, RegionEndpoint.EUWest2);

            var queueUrl = amazonSQS.GetQueueUrlAsync(queueName).Result.QueueUrl;

            var receveMessagerequest = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl
            };

            var receiveMessageResponse = amazonSQS.ReceiveMessageAsync(receveMessagerequest).Result;

            foreach (var message in receiveMessageResponse.Messages)
            {
                Console.WriteLine("Message \n");
                Console.WriteLine($" MessageId: {message.MessageId} \n");
                Console.WriteLine($" Receipthandle: {message.ReceiptHandle} \n");
                Console.WriteLine($" MSD5Body: {message.MD5OfBody} \n");
                Console.WriteLine($" Body: {message.Body} \n");

                foreach (var attributes in message.Attributes)
                {
                    Console.WriteLine("Attributes \n");
                    Console.WriteLine($" Name: {attributes.Key} \n");
                    Console.WriteLine($" Value: {attributes.Value} \n");
                }

                var messageHandle = receiveMessageResponse.Messages.FirstOrDefault()?.ReceiptHandle;

                var deleteRequest = new DeleteMessageRequest
                {
                    QueueUrl = queueUrl,
                    ReceiptHandle = messageHandle
                };

                Console.WriteLine($"deleting message with MessageHandle: {messageHandle}");

                amazonSQS.DeleteMessageAsync(deleteRequest);

                Console.ReadLine();
            }
        }
    }
}
