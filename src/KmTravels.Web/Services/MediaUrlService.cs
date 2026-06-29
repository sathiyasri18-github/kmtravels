namespace SnlEngineering.Web.Services;

/// <summary>
/// Resolves uploaded file paths (/uploads/...) to the API server origin.
/// </summary>
public class MediaUrlService
{
    private readonly string _apiOrigin;

    public MediaUrlService(IConfiguration configuration)
    {
        var mediaBase = configuration["MediaBaseUrl"];
        if (string.IsNullOrWhiteSpace(mediaBase))
        {
            var apiBase = configuration["ApiBaseUrl"] ?? "http://localhost:5224/";
            mediaBase = apiBase.TrimEnd('/');
        }

        _apiOrigin = mediaBase.TrimEnd('/');
    }

    public string Resolve(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return url;

        if (url.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
            return $"{_apiOrigin}{url}";

        return url;
    }
}
