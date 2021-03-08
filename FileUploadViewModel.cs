using Microsoft.AspNetCore.Http;

namespace FileUploader
{
    public class FileUploadViewModel
    {
      
        public IFormFile File { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
