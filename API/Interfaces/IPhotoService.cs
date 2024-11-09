using CloudinaryDotNet.Actions;
using FluentResults;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<List<ImageUploadResult>> UploadPhotosAsync(IFormFileCollection files);
        Task<Result<List<ImageUploadResult>>> UpdatePhotosAsync(IFormFileCollection files);
        Task<Result<DeletionResult>> DeletePhotoAsync(string publicId);
        Task DeletePhotosAsync(List<string> publicIds);
    }
}
