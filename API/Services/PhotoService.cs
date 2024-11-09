using API.Interfaces;
using API.Utility;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentResults;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly CloudinaryOptions _cloudinaryOptions;
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinaryOptions> options)
        {
            _cloudinaryOptions = options.Value;
            _cloudinary = new Cloudinary(new Account
            {
                Cloud = _cloudinaryOptions.CloudName,
                ApiKey = _cloudinaryOptions.ApiKey,
                ApiSecret = _cloudinaryOptions.ApiSecret
            });
            _cloudinary.Api.Secure = true;
        }

        public async Task<List<ImageUploadResult>> UploadPhotosAsync(IFormFileCollection files)
        {

            if (files is null || files.Count == 0)
            {
                return null;
            }

            List<Task<ImageUploadResult>> tasks = new();
            List<Stream> streams = new();
            try 
            {
                foreach(IFormFile file in files) { 

                var stream = file.OpenReadStream();
                streams.Add(stream);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "product"
                };

                tasks.Add(_cloudinary.UploadAsync(uploadParams));
            }

            var uploadResults = await Task.WhenAll(tasks);
            return uploadResults.ToList();
            }
            finally 
            {
                foreach (var stream in streams) 
                {
                    stream.Dispose();
                }
            }
        }

        public async Task<Result<DeletionResult>> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return Result.Ok(await _cloudinary.DestroyAsync(deleteParams));
        }

        public async Task DeletePhotosAsync(List<string> publicIds)
        {
            DelResParams delResParams = new DelResParams()
            {
                PublicIds = publicIds
            };

           await _cloudinary.DeleteResourcesAsync(delResParams);
        }

        public Task<Result<List<ImageUploadResult>>> UpdatePhotosAsync(IFormFileCollection files)
        {
            throw new NotImplementedException();
        }
    }
}
