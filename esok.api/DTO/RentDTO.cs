using esok.api.Data;

namespace esok.api.DTO
{
    public class RentDTO : Entity
    {
        public int Id { get; set; }
        public int NumberRentOfDay { get; set; }
        public string Comments { get; set; }
        public int CustomerId { get; set; }
        public string ElapsedTime { get; set; } = string.Empty;
    }
}
