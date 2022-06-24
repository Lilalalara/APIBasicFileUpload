using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiFileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok("File Upload API running...");
        }

        string uploads = "/app/uploads";

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file, string id)
        {
            //making sure the directory exists without files in it
            string dirPath = Path.Combine(uploads, id);
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                foreach (FileInfo info in di.GetFiles())
                {
                    info.Delete();
                }
                DirectoryInfo dir = Directory.CreateDirectory(dirPath);
            }
            else 
            {
                DirectoryInfo di = Directory.CreateDirectory(dirPath);
            }

            string filePath = Path.Combine(dirPath, file.FileName + ".jpeg");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return Ok();
        }

        [HttpGet]
        [Route("download/{id}")]
        public IActionResult Get(string id)
        {
            string dirPath = Path.Combine(uploads, id);
            if (Directory.Exists(dirPath))
            {
                string[] files = Directory.GetFiles(dirPath);
                Byte[] b = System.IO.File.ReadAllBytes(Path.Combine(dirPath, files[0]));
                return File(b, "image/jpeg");
            }
            else 
            {
                return NotFound();
            }
        }
    }
}