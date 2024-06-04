using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RssProxyDotnet.Controllers;

[EnableCors("AllowAll")]
[ApiController]
[Route("[controller]")]
public class RssController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _client;

    public RssController(IMemoryCache cache)
    {
        _memoryCache = cache;
        _client = new HttpClient();
    }

    [HttpGet]
    public async Task<SyndicationFeed> GetFeed(string url)
    {
        var decodedUrl = HttpUtility.UrlDecode(url);
        var cachedValue = _memoryCache.Get<SyndicationFeed>(decodedUrl);
        if (cachedValue is not null) return cachedValue;
        
        using var reader = XmlReader.Create(decodedUrl);
        var feed = SyndicationFeed.Load(reader);

        _memoryCache.Set(decodedUrl, feed);
        return feed;
        
    }
}