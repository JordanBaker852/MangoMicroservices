using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Models.DTO
{
    public class RequestDTO
    {
        public HttpRequestType HttpRequest { get; set; } = HttpRequestType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
