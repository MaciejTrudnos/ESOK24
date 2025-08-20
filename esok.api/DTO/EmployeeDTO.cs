namespace esok.api.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Confirmed { get; set; }
    }
}
