
namespace Kyrsach.Models;

public class ApiRequest
{
    public string ApiType { get; set; } = "GET";
    public string ApiUrl { get; set; }
    public object ApiData { get; set; }
    public string AccessToken { get; set; }
}
