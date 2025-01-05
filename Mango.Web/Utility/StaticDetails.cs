namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIBase { get; set; } = string.Empty;
        public static string AuthAPIBase { get; set; } = string.Empty;

        public const string ROLE_ADMIN = "ADMIN"; 
        public const string ROLE_CUSTOMER = "CUSTOMER"; 

        public enum HttpRequestType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
