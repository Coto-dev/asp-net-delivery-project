using Common.Enums;

namespace Common.DTO {
	public class DishDetailsDTO {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsVagetarian { get; set; }
        public string PhotoUrl { get; set; }
        public Categories Category { get; set; }
        public double Rating { get; set; }
    }
}