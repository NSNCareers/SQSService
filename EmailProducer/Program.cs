using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Configuration;

namespace EmailProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            Console.WriteLine("*************************");
            Console.WriteLine("Amazon SQS");
            Console.WriteLine("************************\n");

            //var accessKey = JsonConfig.GetJsonValue("AccessKey");
            //var secretKey = JsonConfig.GetJsonValue("SecretKey");
            var accessKey = ConfigurationManager.AppSettings.Get("AccessKey");
            var secretKey = ConfigurationManager.AppSettings.Get("SecretKey");

            var queueName = "EmailQueue2";

            AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

            IAmazonSQS amazonSQS = new AmazonSQSClient(credentials,RegionEndpoint.EUWest2);

            Console.WriteLine("Create a queue called EmailQueue.\n");
            logger.Info("Create a queue called EmailQueue");
            logger.Info("Testing Logs");

            var sqsRequest = new CreateQueueRequest
            {
                QueueName = queueName
            };

            var createQueueResponse = amazonSQS.CreateQueueAsync(sqsRequest).Result;

            var myQueueRrl = createQueueResponse.QueueUrl;

            var listQueueRequest = new ListQueuesRequest();
            var listQueuesResponce = amazonSQS.ListQueuesAsync(listQueueRequest);

            Console.WriteLine("List of Amazon SQS queuse.\n");

            foreach (var queueurl in listQueuesResponce.Result.QueueUrls)
            {
                Console.WriteLine($"QueueUrl : {queueurl}");
            }

            Console.WriteLine("Sendin a message to our emailQueue. \n");

            var sqsMessageRequest = new SendMessageRequest
            {
                QueueUrl = myQueueRrl,
                MessageBody = "Email Information"
            };

            amazonSQS.SendMessageAsync(sqsMessageRequest);

            Console.WriteLine("Finished sending message to our sqs queue. \n");
            Console.ReadLine();
        }
    }
}
