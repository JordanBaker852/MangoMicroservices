namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CartHeaderDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
    }
}