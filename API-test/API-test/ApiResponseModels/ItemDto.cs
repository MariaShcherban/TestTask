
namespace API_test.ApiResponseModels
{
    public class ItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CountryDto Country { get; set; }
    }
}
