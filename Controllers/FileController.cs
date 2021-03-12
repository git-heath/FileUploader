using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".pdf", new List<byte[]>
                {                    
                    new byte[] { 0x25, 0x50, 0x44, 0x46}
                }
            },
            { ".docx", new List<byte[]>
                {                
                    new byte[] { 0x50, 0x4B, 0x03, 0x04},
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00}
                }
            },
            { ".doc", new List<byte[]>
                {                
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}
                }
            },
        };

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string HelloWorld() => "Hello world!";


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
            var valid = ValidateFile(model.File.OpenReadStream(), model.File.FileName);
            if (!valid)
            {
                return BadRequest($"File type must be { string.Join(",", _fileSignature.Keys)}");
            }
            string ret = $"Filename {model.File.FileName}, size {model.File.Length}, ContentType {model.File.ContentType}, ContentDisposition {model.File.ContentDisposition}, Name {model.FirstName} { model.LastName}";
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

        private bool ValidateFile(Stream stream, string filename)
        {
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            if (_fileSignature.TryGetValue(ext, out List<byte[]> signatures))
            {
                using (var reader = new BinaryReader(stream))
                {

                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    return signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));
                }
            }
            return false;
        }
    }
}
