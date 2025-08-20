namespace esok.api.DTO
{
    public class RentProductDTO
    {
        public string Comments { get; set; }
        public CustomerDTO Customer { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
