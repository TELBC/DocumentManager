using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public interface IEntity<out T>
{
    [Key] T Id { get; }
}