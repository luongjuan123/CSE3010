using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Entities
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên tác giả tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }

        // Navigation property (1 tác giả có thể có nhiều sách)
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
