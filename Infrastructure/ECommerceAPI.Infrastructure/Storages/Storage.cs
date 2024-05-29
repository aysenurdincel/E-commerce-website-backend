using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Storages
{
    public class Storage
    {
        protected async Task<string> FileRenameAsync(string fileName, string filePath)
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
    }
}
