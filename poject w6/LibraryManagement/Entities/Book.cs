using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Entities
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề sách không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề sách tối đa 200 ký tự")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? ISBN { get; set; }

        public int PublishedYear { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000000, ErrorMessage = "Giá sách phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        // Khóa ngoại đến Author
        [Required]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        // Navigation property đến Author
        public virtual Author Author { get; set; } = null!;

        // Khóa ngoại đến Category
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        // Navigation property đến Category
        public virtual Category Category { get; set; } = null!;
    }
}
