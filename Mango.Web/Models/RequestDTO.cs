using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Models
{
    public class RequestDTO
    {
        public HttpRequestType HttpRequest { get; set; } = HttpRequestType.GET;
        public string Url { get; set; }
        public string Data { get; set; }
        public string AccessToken { get; set; }
    }
}
