namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
