using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Category
    {
        public int Id { get; set; }
        public List<Product>? Products { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(50, ErrorMessage = "Tên danh mục tối đa 50 ký tự")]
        public string Name { get; set; } = string.Empty;
    }
}