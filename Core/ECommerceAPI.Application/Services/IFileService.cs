
using Microsoft.AspNetCore.Http;

namespace ECommerceAPI.Application.Services
{
    public interface IFileService
    {
        Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files);
        Task<bool> SaveFileAsync(string filePath, IFormFile file);
    }
}
