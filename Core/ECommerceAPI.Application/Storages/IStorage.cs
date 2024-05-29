using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Storages
{
    public interface IStorage
    {
        List<string> GetFiles(string filePath);
        Task DeleteAsync(string filePath, string fileName);
        bool ContainsFile(string filePath, string fileName);
        Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files);

    }
}
