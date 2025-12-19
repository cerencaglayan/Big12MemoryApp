
using System;

namespace Big12MemoryApp.Application.DTO.Responses
{
    public class AttachmentResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; } 
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string FileUrl { get; set; } 
        public int DisplayOrder { get; set; }
        public string? Caption { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}