using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public interface IEntity<T>
{
    [Key] T Id { get; }
}