namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIBase { get; set; } = string.Empty;

        public enum HttpRequestType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
