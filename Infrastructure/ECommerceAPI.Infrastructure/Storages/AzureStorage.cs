using ECommerceAPI.Application.Storages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Storages
{
    public class AzureStorage : IStorage
    {
        public bool ContainsFile(string filePath, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string filePath, string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFiles(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files)
        {
            throw new NotImplementedException();
        }
    }
}
