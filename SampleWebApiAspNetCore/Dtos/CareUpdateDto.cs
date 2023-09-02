
namespace SampleWebApiAspNetCore.Dtos
{
    public class CareUpdateDto
    {
        public string? Name { get; set; }
        public string? Product { get; set; }
        public int Effectiveness { get; set; }
        public DateTime Created { get; set; }
    }
}
