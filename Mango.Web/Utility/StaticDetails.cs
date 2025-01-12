namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIBase { get; set; } = string.Empty;
        public static string AuthAPIBase { get; set; } = string.Empty;

        public static string ProductAPIBase { get; set; } = string.Empty;

        public const string ROLE_ADMIN = "ADMIN"; 
        public const string ROLE_CUSTOMER = "CUSTOMER";

        public const string TOKEN_COOKIE_NAME = "JwtToken";
        public const string JWT_ROLE_CLAIM_KEY = "role";

        public enum HttpRequestType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
