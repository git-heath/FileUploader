using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileUploader
{
    public class MultipleFileUploadViewModel
    {
        public ICollection<IFormFile> Files { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
