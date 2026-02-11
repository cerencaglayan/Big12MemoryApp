using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Big12MemoryApp.Domain.Entities.Base;
using Big12MemoryApp.Domain.Entities.Lookup;

namespace Big12MemoryApp.Domain.Entities;

public class Memory : Entity
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Açıklama")]
    public required string Description { get; set; }

    [Required] public DateTime Date { get; set; }

    public int? MemoryTypeId { get; set; }
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public MemoryType MemoryType { get; set; } = null!;

    public ICollection<MemoryAttachment> MemoryAttachments { get; set; } = new List<MemoryAttachment>();
}