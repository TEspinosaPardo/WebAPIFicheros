using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models;
using WebAPIFicheros.Models.Dtos;
using WebAPIFicheros.Services;
using WebAPIFicheros.Services.Interfaces;

namespace WebAPIFicheros.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService = new FileService();

        public FileController()
        {
            
        }  
        
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadAsync([FromBody] FileDTO file)
        {
            await fileService.Upload(file);

            return Ok(file);
        }

        [HttpPost]
        [Route("Download")]
        public async Task<IActionResult> Download([FromBody] FileDTO file)
        {
            await fileService.Download(file);

            return Ok(file);
        }

        [HttpPost]
        [Route("ModifyName")]
        public async Task<IActionResult> ModifyNameAsync([FromBody] FileDTO file)
        {
            await fileService.ModifyFilename(file);

            return Ok(file);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] FileDTO file)
        {
            await fileService.Delete(file);

            return Ok(file);
        }

        [HttpPost]
        [Route("GetURL")]
        public IActionResult GetURL([FromBody] FileDTO file)
        {
            fileService.GetURL(file);

            return Ok(file);
        }
    }
}
