using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string HelloWorld()=> "Hello world!";        

      
        [HttpPost("/rawfile")]         
        public string PostFile(IFormFile file)
        {
            string ret = $"Filename {file.FileName}, ContentType {file.ContentType}, ContentDisposition {file.ContentDisposition}";
            _logger.LogInformation(ret);            
            return ret;
        }

       
        [HttpPost("/viewmodel")]
        public ActionResult<string> PostModel([FromForm] FileUploadViewModel model)
        {
            string ret = $"Filename {model.File.FileName}, ContentType {model.File.ContentType}, ContentDisposition {model.File.ContentDisposition}, Name {model.FirstName} { model.LastName}";
            _logger.LogInformation(ret);
            return ret;
        }

        [HttpPost("/multiplefilesviewmodel")]
        public ActionResult<string> PostMultipleFilesModel([FromForm] MultipleFileUploadViewModel model)
        {
            var ret = new StringBuilder();
            ret.AppendLine($"Name {model.FirstName} { model.LastName}");

            foreach (var file in model.Files)
            {
                ret.AppendLine($"Filename {file.FileName}, ContentType {file.ContentType}, ContentDisposition {file.ContentDisposition}");
            }
            
            _logger.LogInformation(ret.ToString());
            return ret.ToString();
        }

        [HttpPost("/fileasbytearray")]
        public ActionResult<string> PostByteArrayModel([FromBody] FileAsByteArrayViewModel model)
        {
            string ret = $"Filename {model.FileName}, MimeType {model.MimeType}, Name {model.FirstName} { model.LastName}";
            _logger.LogInformation(ret);
            return ret;
        }
    }
}
