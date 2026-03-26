using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 10000.00, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        // Phần mở rộng upload ảnh theo PDF
        public string? ImageUrl { get; set; }

        // Quan hệ
        public Category? Category { get; set; }

// Ảnh phụ
        public List<ProductImage>? Images { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}