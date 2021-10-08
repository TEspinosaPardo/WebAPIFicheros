using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIFicheros.Models.Dtos
{
    public abstract class BaseDTO
    {
        public List<string> Errors { get; set; }
        public Guid? RequestId { get; set; }
        public BaseDTO()
        {
            Errors = new List<string>();

        }
    }
}
