using System;
using Big12MemoryApp.Domain.Entities.Base;

namespace Big12MemoryApp.Domain.Entities.Lookup;

public class MemoryType : Entity
{
    protected MemoryType(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }


    private MemoryType(int id, string code, string name, DateTime createdAt, DateTime modifiedAt)
    {
        Id = id;
        Code = code;
        Name = name;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public static MemoryType Create(int id, string code, string name, DateTime createdAt, DateTime modifiedAt)
    {
        return new(id, code, name, createdAt, modifiedAt);
    }
}