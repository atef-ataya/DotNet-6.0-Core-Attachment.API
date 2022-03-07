using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ADP.Attachment.API.Controllers
{
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        public string imagesPath = "c:\\AD-Recv\\";
        const String folderName = "AD-Recv";
        readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        public FilesController()
        {
            string root = imagesPath;
            string yearFolder = root + DateTime.Now.Year.ToString() + "\\";
            if (!Directory.Exists(yearFolder))
            {
                Directory.CreateDirectory(yearFolder);
            }
            string monthFolder = yearFolder + DateTime.Now.Month.ToString() + "\\";
            if (!Directory.Exists(monthFolder))
            {
                Directory.CreateDirectory(monthFolder);
            }
            string subDirectory = monthFolder + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "\\";
            if (!Directory.Exists(subDirectory))
            {
                Directory.CreateDirectory(subDirectory);
            }
            folderPath = subDirectory;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(
            [FromForm(Name = "myFile")] IFormFile myFile)
        {
            using (var fileContentStream = new MemoryStream())
            {
                await myFile.CopyToAsync(fileContentStream);
                await System.IO.File.WriteAllBytesAsync(Path.Combine(folderPath, myFile.FileName), fileContentStream.ToArray());
            }
            return CreatedAtRoute(routeName: "myFile", routeValues: new { filename = myFile.FileName }, value: null); ;
        }

        [HttpGet("{filename}", Name = "myFile")]
        public async Task<IActionResult> Get([FromRoute] String filename)
        {
            //var filePath = Path.Combine(folderPath, filename);
            var filePath = "c:\\AD-Recv\\2022\\2\\28-13-54\\today.jpg";
            if (System.IO.File.Exists(filePath))
            {
                return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", filename);
            }
            return NotFound();
        }
    }
}

