using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        public string ShippingAddress { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}