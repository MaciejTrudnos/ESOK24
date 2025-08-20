namespace esok.api.DTO.PageAccessTokens
{
    public class DatumDTO
    {
        public string Access_token { get; set; }
        public string Category { get; set; }
        public List<CategoryListDTO> Category_list { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public List<string> Tasks { get; set; }
    }
}
