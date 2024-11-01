using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string Name { get; set; } 

        [Required]
        [Range(10,1000)]
        public double Price { get; set; }

        [Required]
        [Range(1,100)]
        public int Count { get; set; }
    }

