using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models.Dtos;
using WebAPIFicheros.Services.Interfaces;

namespace WebAPIFicheros.Services
{
    public class FileService: IFileService
    {
        private readonly string key = "AKIAY74DF3JTGVARYGEZ";
        private readonly string secret = "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";
        private readonly string bucketName = "aluxion.bucket/tests";
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;

        private static IAmazonS3 S3Client;

        public FileService()
        {
            S3Client = new AmazonS3Client(key, secret, bucketRegion);
        }

        public async Task Upload(FileDTO file)
        {
            try
            {
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.Upload.Request.FileName,
                    ContentBody = file.Upload.Request.Data                    
                };

                await S3Client.PutObjectAsync(uploadRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task Delete(FileDTO file)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.Delete.Request.FileName                    
                };

                Console.WriteLine("Deleting an object");

                await S3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }

        public async Task Download(FileDTO file)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.Download.Request.FileName
                };

                using (GetObjectResponse response = await S3Client.GetObjectAsync(request))
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    file.Download.Response.Data = Convert.FromBase64String(reader.ReadToEnd());
                }

            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }
        }

        public async Task ModifyFilename(FileDTO file)
        {
            try
            {
                CopyObjectRequest request = new CopyObjectRequest
                {
                    SourceBucket = bucketName,
                    SourceKey = file.ModifyName.Request.OldFileName,
                    DestinationBucket = bucketName,
                    DestinationKey = file.ModifyName.Request.NewFileName
                };
                await S3Client.CopyObjectAsync(request);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.ModifyName.Request.OldFileName,
                };

                await S3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public void GetURL(FileDTO file)
        {
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = file.GetURL.Request.FileName,
                    Expires = DateTime.Now.AddMinutes(10)
            };

                file.GetURL.Response.FileURL = S3Client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }
        }
    }
}
