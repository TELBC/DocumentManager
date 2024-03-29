﻿using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class Tag : IEntity<int>
{
#pragma warning disable CS8618
    protected Tag()
    {
    }
#pragma warning restore CS8618

    public Tag(string name, Category category)
    {
        Name = name;
        Category = category;
    }

    [Required] [MaxLength(255)] public string Name { get; protected set; }

    public Category Category { get; protected set; }

    [Key] public int Id { get; protected set; }
}