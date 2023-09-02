namespace SampleWebApiAspNetCore.Entities
{
    public class CareEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Product { get; set; }
        public int Effectiveness { get; set; }
        public DateTime Created { get; set; }
    }
}
