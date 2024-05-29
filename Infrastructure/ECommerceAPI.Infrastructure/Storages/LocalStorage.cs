using ECommerceAPI.Application.Storages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Storages
{
    public class LocalStorage : Storage,IStorage
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool ContainsFile(string filePath, string fileName)
        
            =>File.Exists(Path.Combine(filePath, fileName));

        public async Task DeleteAsync(string filePath, string fileName)
        
            => File.Delete(Path.Combine(filePath,fileName));
        

        public List<string> GetFiles(string filePath)
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            return directory.GetFiles().Select(x => x.Name).ToList();
        }

        private async Task<bool> SaveFileAsync(string filePath, IFormFile file)
        {
            try
            {
                using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }
        public async Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }


            List<(string fileName, string filePath)> data = new();
            List<bool> results = new();

            foreach (IFormFile file in files)
            {
                string fileChangedName = await FileRenameAsync(file.FileName, uploadPath);

                bool res = await SaveFileAsync(Path.Combine(uploadPath, fileChangedName), file);

                data.Add((fileChangedName, Path.Combine(filePath, fileChangedName)));
                results.Add(res);


            }
            if (results.TrueForAll(x => x.Equals(true)))
            {
                return data;
            }
            return null;
            //todo exception
        }
    }
}
