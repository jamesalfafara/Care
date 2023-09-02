using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class CareCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Product { get; set; }
        public int Effectiveness { get; set; }
        public DateTime Created { get; set; }
    }
}
