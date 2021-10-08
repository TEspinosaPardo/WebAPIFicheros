using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models.Dtos;

namespace WebAPIFicheros.Services.Interfaces
{
    public interface IFileService
    {
        public Task Upload(FileDTO file);
        public Task Download(FileDTO file);
        public Task ModifyFilename(FileDTO file);
        public Task Delete(FileDTO file);
        public void GetURL(FileDTO file);
    }
}
