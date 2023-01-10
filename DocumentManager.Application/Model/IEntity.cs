using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public interface IEntity<T>
{
    [Key]
    T Id { get; }
}
//updated to what is required in the project