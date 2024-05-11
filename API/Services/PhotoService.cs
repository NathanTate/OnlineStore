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

        public async Task<Result<ImageUploadResult>> UploadPhotoAsync(IFormFile file)
        {

            if (file.Length < 0)
            {
                return Result.Fail("No image to upload");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Gravity("face").Height(500).Width(500)
                .Crop("fill"),
                Folder = "product"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return Result.Ok(uploadResult);
        }

        public async Task<Result<DeletionResult>> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return Result.Ok(await _cloudinary.DestroyAsync(deleteParams));
        }
    }
}
