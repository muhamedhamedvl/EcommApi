using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.InfraStructure.Repositores.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();

            var ImageDirectory = Path.Combine("wwwroot", "Images", src);

            if (!Directory.Exists(ImageDirectory))
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach (IFormFile item in files)
            {
                if (item.Length > 0)
                {
                    //get Image Name
                    var ImageName = item.FileName;
                    //create Image Path 
                    var ImageSrc = $"/Images/{src}/{ImageName}";
                    var root = Path.Combine(ImageDirectory, ImageName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    SaveImageSrc.Add(ImageSrc);
                }
            }

            return SaveImageSrc;
        }

        public Task<string> DeleteImageAsync(string src)
        {
            try
            {
                var fileInfo = fileProvider.GetFileInfo(src);
                if (fileInfo.Exists)
                {
                    File.Delete(fileInfo.PhysicalPath);
                    return Task.FromResult("Image deleted successfully.");
                }
                else
                {
                    return Task.FromResult("Image not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting image: {ex.Message}");
            }
        }
    }
}
