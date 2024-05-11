using CloudinaryDotNet.Actions;
using FluentResults;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<ImageUploadResult>> UploadPhotoAsync(IFormFile file);
        Task<Result<DeletionResult>> DeletePhotoAsync(string publicId);
    }
}
