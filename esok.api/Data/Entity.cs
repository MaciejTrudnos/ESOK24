using System.ComponentModel;

namespace esok.api.Data
{
    public abstract class Entity
    {
        public DateTime CreateDate { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Active { get; set; }
    }
}
