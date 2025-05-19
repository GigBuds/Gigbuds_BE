namespace Gigbuds_BE.Domain.Entities
{
    /// <summary>
    /// A base entity class, all other entities should inherit form this class, except Identity entities.
    /// </summary>
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
