namespace esok.api.DTO
{
    public class RentDetiailsDTO
    {
        public CustomerDTO Customer { get; set; }
        public List<ProductDTO> Product { get; set; }
        public decimal NetPrice { get; set; }
    }
}
