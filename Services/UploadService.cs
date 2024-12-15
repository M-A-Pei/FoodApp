using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FoodApp.Data;
using Microsoft.Extensions.Options;

namespace FoodApp.Services{
    public interface IUploadService{
        Task<string?> UploadFileAsync(IFormFile file);
    }

    public class UploadService: IUploadService{
        private readonly Cloudinary _cloudinary;
        public UploadService(IOptions<CloudinarySettings> x)
        {
            _cloudinary = new Cloudinary(
                new Account(x.Value.cloudName, x.Value.apiKey, x.Value.apiSecret)
            );
        }

         public async Task<string?> UploadFileAsync(IFormFile file){
            if (file.Length <= 0)
                return null; // Early exit for invalid files.

            using var stream = file.OpenReadStream(); // Convert the file to a stream.
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream), // Describe the file for upload.
                Folder = "FoodApp"
            };

            // Upload the file to Cloudinary and get the result.
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Return the secure URL for public access.
            return uploadResult?.SecureUrl?.ToString();
        }

    }
}