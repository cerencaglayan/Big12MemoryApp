using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Big12MemoryApp.Application.DTO.Requests
{
    public class CreateMemoryRequest
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<string?>? Captions { get; set; }

        public int? MemoryTypeId { get; set; }
    }
}