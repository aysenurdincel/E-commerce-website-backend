using ECommerceAPI.Application.Storages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Storages
{
    //kullanılacak depolama alanına uygun işlem yapmak için(azure, aws, local vb.)
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public bool ContainsFile(string filePath, string fileName)
        
            => _storage.ContainsFile(filePath, fileName);
       

        public async Task DeleteAsync(string filePath, string fileName)
         
            => await _storage.DeleteAsync(filePath, fileName);
    

        public List<string> GetFiles(string filePath)
         
            => _storage.GetFiles(filePath);

        public async Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files)

            => await _storage.UploadAsync(filePath, files);
    }
}
