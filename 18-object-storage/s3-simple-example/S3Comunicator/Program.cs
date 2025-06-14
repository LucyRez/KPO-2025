using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Access Key: ");
        string accessKey = Console.ReadLine() ?? throw new InvalidOperationException("Access key is required");

        Console.Write("Secret Key: ");
        string secretKey = Console.ReadLine() ?? throw new InvalidOperationException("Secret key is required");

        Console.Write("Endpoint: ");
        string endpoint = Console.ReadLine() ?? throw new InvalidOperationException("Endpoint is required");

        Console.Write("Bucket Name: ");
        string bucketName = Console.ReadLine() ?? throw new InvalidOperationException("Bucket name is required");

        string filePath = Console.ReadLine() ?? throw new ArgumentException();


        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonS3Config
        {
            ForcePathStyle = true,
            ServiceURL = endpoint,
            UseHttp = endpoint.StartsWith("http://")
        };

        using var s3Client = new AmazonS3Client(credentials, config);

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. List files in bucket");
            Console.WriteLine("2. Download file");
            Console.WriteLine("3. Upload");
            Console.Write("Enter your choice (1-3): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListFiles(s3Client, bucketName);
                    break;
                case "2":
                    await DownloadFile(s3Client, bucketName);
                    break;
                case "3":
                    await UploadFile(s3Client, filePath, bucketName);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    } 

    static async Task UploadFile(IAmazonS3 s3Client, string fileName, string bucketName) {
        using (var fileStream = File.OpenRead(fileName)){

            var name = Path.GetFileName(fileName);
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = name,
                InputStream = fileStream
            };

            await s3Client.PutObjectAsync(putRequest, CancellationToken.None);
        }
    }

    static async Task ListFiles(IAmazonS3 s3Client, string bucketName)
    {
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };

            var response = await s3Client.ListObjectsV2Async(request);

            if (response.S3Objects.Count == 0)
            {
                Console.WriteLine("Bucket is empty.");
                return;
            }

            Console.WriteLine("\nFiles in bucket:");
            foreach (var s3Object in response.S3Objects)
            {
                Console.WriteLine($"- {s3Object.Key} (Size: {s3Object.Size} bytes, Last Modified: {s3Object.LastModified})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing files: {ex.Message}");
        }
    }

    static async Task DownloadFile(IAmazonS3 s3Client, string bucketName)
    {
        Console.Write("\nEnter file name to download: ");
        string? fileName = Console.ReadLine();

        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("File name cannot be empty.");
            return;
        }

        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            using var response = await s3Client.GetObjectAsync(request);
            using var responseStream = response.ResponseStream;
            using var fileStream = File.Create(fileName);
            
            await responseStream.CopyToAsync(fileStream);
            Console.WriteLine($"File '{fileName}' downloaded successfully.");
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"File '{fileName}' not found in bucket.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
        }
    }
}
