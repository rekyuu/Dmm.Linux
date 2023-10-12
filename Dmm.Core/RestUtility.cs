using System.Net;
using System.Text.Json;

namespace Dmm.Core;

public static class RestUtility
{
    private static readonly CookieContainer Cookies;
    private static readonly HttpClient Client;

    static RestUtility()
    {
        Cookies = new CookieContainer();
        HttpClientHandler handler = new() { CookieContainer = Cookies };
        
        Client = new HttpClient(handler);
    }

    public static void AddCookie(Cookie cookie)
    {
        Cookies.Add(cookie);
    }

    public static T Get<T>(string url, Dictionary<string, string> headers)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        
        string response = Send(request, headers);

        return JsonSerializer.Deserialize<T>(response)!;
    }

    public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, url);
        
        string response = await SendAsync(request, headers);

        return JsonSerializer.Deserialize<T>(response)!;
    }

    public static T Post<T>(string url, Dictionary<string, string> headers, string body)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, url);
        request.Content = new StringContent(body);
        
        string response = Send(request, headers);

        return JsonSerializer.Deserialize<T>(response)!;
    }

    public static async Task<T> PostAsync<T>(string url, Dictionary<string, string> headers, string body)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, url);
        request.Content = new StringContent(body);
        
        string response = await SendAsync(request, headers);

        return JsonSerializer.Deserialize<T>(response)!;
    }

    private static string Send(HttpRequestMessage request, Dictionary<string, string> headers)
    {
        foreach (KeyValuePair<string, string> header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        
        string cookieHeader = "";
        foreach (Cookie cookie in Cookies.GetAllCookies())
        {
            cookieHeader += $"{cookie.Name}={cookie.Value};";
        }
        request.Headers.Add("Cookie", cookieHeader);
        
        using HttpResponseMessage response = Client.Send(request);
        response.EnsureSuccessStatusCode();
        string responseBody = response.Content.ReadAsStringAsync().Result;

        return responseBody;
    }

    private static async Task<string> SendAsync(HttpRequestMessage request, Dictionary<string, string> headers)
    {
        foreach (KeyValuePair<string, string> header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        string cookieHeader = "";
        foreach (Cookie cookie in Cookies.GetAllCookies())
        {
            cookieHeader += $"{cookie.Name}={cookie.Value};";
        }
        request.Headers.Add("Cookie", cookieHeader);
        
        using HttpResponseMessage response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}