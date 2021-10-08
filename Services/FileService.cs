using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unsplasharp;
using WebAPIFicheros.Models.Dtos;
using WebAPIFicheros.Services.Interfaces;
using static WebAPIFicheros.Models.Dtos.FileDTO;
using static WebAPIFicheros.Models.Dtos.FileDTO.GetImageFromExternalAPIDTO;

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
                file.Upload.Response.Errors.Add($"Error uploading file to AWS: {e.Message}");
            }
            catch (Exception e)
            {
                file.Upload.Response.Errors.Add($"Unknown error: {e.Message}");
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
                file.Delete.Response.Errors.Add($"Error deleting file from AWS: {e.Message}");
            }
            catch (Exception e)
            {
                file.Delete.Response.Errors.Add($"Unknown error: {e.Message}");
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
                file.Download.Response.Errors.Add($"Error downloading file from AWS: {e.Message}");
            }
            catch (Exception e)
            {
                file.Download.Response.Errors.Add($"Unknown error: {e.Message}");
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
                file.ModifyName.Response.Errors.Add($"Error modifying file from AWS: {e.Message}");
            }
            catch (Exception e)
            {
                file.ModifyName.Response.Errors.Add($"Unknown error: {e.Message}");
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
                file.GetURL.Response.Errors.Add($"Error getting file URL from AWS: {e.Message}");
            }
            catch (Exception e)
            {
                file.GetURL.Response.Errors.Add($"Unknown error: {e.Message}");
            }
        }

        public async Task GetImageFromExternalAPI(FileDTO file)
        {
            var client = new UnsplasharpClient("esEblHWlSb1TxwSBJup1VazdlvvgHhUlKdx3sGzRemk");
            var photosFound = await client.SearchPhotos(file.GetImageFromExternalAPI.Request.SearchWord);

            file.GetImageFromExternalAPI.Response.Photos = photosFound.Select(photo => new PhotoDTO
            {
                Id = photo.Id,
                Description = photo.Description,
                URL = photo.Urls.Regular
            }).ToList();
        }

        public async Task GetImageFromExternalAPIAndUpload(FileDTO file)
        {
            var unsplashClient = new UnsplasharpClient("esEblHWlSb1TxwSBJup1VazdlvvgHhUlKdx3sGzRemk");
            var photo = await unsplashClient.GetPhoto(file.GetImageFromExternalAPIAndUpload.Request.PhotoId);

            if(photo != null)
            {
                using (var client = new WebClient())
                {
                    byte[] dataBytes = client.DownloadData(new Uri(photo.Urls.Regular));
                    string encodedFileAsBase64 = Convert.ToBase64String(dataBytes);

                    file.Upload = new UploadDTO(photo.Description, encodedFileAsBase64);

                    await Upload(file);
                }
            }
            else
            {
                file.Download.Response.Errors.Add($"Photo not found by specified id");
            }
        }
    }
}
