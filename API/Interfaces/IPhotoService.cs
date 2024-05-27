using CloudinaryDotNet.Actions;
using FluentResults;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<List<ImageUploadResult>>> UploadPhotoAsync(IFormCollection files);
        Task<Result<DeletionResult>> DeletePhotoAsync(string publicId);
    }
}
