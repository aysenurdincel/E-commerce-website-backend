using ECommerceAPI.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public FileService(IWebHostEnvironment webHostEnvironment) { 
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> FileRenameAsync(string fileName, string filePath)
        {
                return await Task.Run<string>(() =>
                {
                    string extension = Path.GetExtension(fileName);
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string newFileName = $"{NameCorrectingOperation.CorrectName(fileName)}{extension}";
                    bool fileExists = false;
                    int fileIndex = 0;
                    do
                    {
                        if (File.Exists($"{filePath}\\{newFileName}"))
                        {
                            fileExists = true;
                            fileIndex++;
                            newFileName = $"{NameCorrectingOperation.CorrectName(nameWithoutExtension + "-" + fileIndex)}{extension}";
                        }
                        else
                        {
                            fileExists = false;
                        }
                    } while (fileExists);

                    return newFileName;
                });
          }
        

        public async Task<bool> SaveFileAsync(string filePath, IFormFile file)
        {
            try
            {
                using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                return true;
            }catch (Exception ex)
            {
                //todo log!
                throw ex ;
            }
        }

        public async Task<List<(string fileName, string filePath)>> UploadAsync(string filePath, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath,filePath); 

            if(!Directory.Exists(uploadPath)) { 
                Directory.CreateDirectory(uploadPath);
            }


            List<(string fileName, string filePath)> data = new();
            List<bool> results = new();

            foreach(IFormFile file in files)
            {
                string fileChangedName = await FileRenameAsync(file.FileName, uploadPath);
               
                bool res = await SaveFileAsync(Path.Combine(uploadPath,fileChangedName),file);

                data.Add((fileChangedName, Path.Combine(filePath, fileChangedName)));
                results.Add(res);


            }
            if(results.TrueForAll(x => x.Equals(true)))
            {
                return data;
            }
            return null;
            //todo exception
        }
    }
}
