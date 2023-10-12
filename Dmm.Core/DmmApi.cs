using System.Collections.ObjectModel;
using System.Text.Json;
using Dmm.Core.Models.Api;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Cookie = System.Net.Cookie;
using Serilog;

namespace Dmm.Core;

public static class DmmApi
{
    private const string BaseApiDomain = "https://apidgp-gameplayer.games.dmm.com";
    private const string SignedUrl = "https://cdn-gameplayer.games.dmm.com/product/*";

    private const string LoginUrlEndpoint = "/v5/loginurl";
    private const string GameInfoEndpoint = "/v5/gameinfo";
    private const string LaunchClEndpoint = "/v5/launch/cl";
    private const string LaunchPkgEndpoint = "/v5/launch/pkg";
    private const string ReportEndpoint = "/v5/report";
    private const string HardwareCodeEndpoint = "/v5/hardwarecode";
    private const string HardwareConfEndpoint = "/v5/hardwareconf";
    private const string HardwareListEndpoint = "/v5/hardwarelist";
    private const string HardwareRejectEndpoint = "/v5/hardwarereject";
    private const string GetCookieEndpoint = "/getCookie";

    private const string MacAddress = "";
    private const string HddSerial = "";
    private const string Motherboard = "";

    private static readonly Dictionary<string, string> LoginHeaders = new()
    {
        { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" },
        { "Upgrade-Insecure-Requests", "1" },
        { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.84 Safari/537.36" }
    };

    private static readonly Dictionary<string, string> LauncherHeaders = new()
    {
        { "Accept-Encoding", "gzip, deflate, br" },
        { "User-Agent", "DMMGamePlayer5-Win/5.2.22 Electron/22.0.0" },
        { "Client-App", "DMMGamePlayer5" },
        { "Host", "apidgp-gameplayer.games.dmm.com" },
        { "Connection", "keep-alive" },
        { "Client-version", "5.2.22" },
        { "Sec-Fetch-Dest", "empty" },
        { "Sec-Fetch-Mode", "no-cors" },
        { "Sec-Fetch-Site", "none" },
    };

    public static async Task LaunchGame(string username, string password)
    {
        await LoginAsync(username, password);
        await GetGameInfoAsync();
        await GetLaunchCommandAsync();
        await ReportAsync();
    }

    public static DmmApiResponse<LoginUrl> GetLoginUrl()
    {
        return RestUtility.Get<DmmApiResponse<LoginUrl>>($"{BaseApiDomain}{LoginUrlEndpoint}", LoginHeaders);
    }

    public static async Task<DmmApiResponse<LoginUrl>> GetLoginUrlAsync()
    {
        return await RestUtility.GetAsync<DmmApiResponse<LoginUrl>>($"{BaseApiDomain}{LoginUrlEndpoint}", LoginHeaders);
    }

    public static async Task LoginAsync(string username, string password)
    {
        if (CheckCookies()) return;
        
        Log.Information("No cookies, logging in");
        
        DmmApiResponse<LoginUrl> loginUrl = await GetLoginUrlAsync();
        
        Log.Information("Launching headless Chrome driver");

        ChromeOptions chromeOptions = new();
        chromeOptions.AddArguments(new List<string> { "no-sandbox", "headless", "disable-gpu", "disable-dev-shm-usage", "disable-extensions", "blink-settings=imagesEnabled=false" });
        IWebDriver driver = new ChromeDriver(chromeOptions);
        
        driver.Navigate().GoToUrl(loginUrl.Data!.Url!);

        driver.FindElement(By.Id("login_id")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.XPath("//*[@id=\"loginbutton_script_on\"]/span/input")).Click();
        
        Log.Information("Logging into DMM");

        while (true)
        {
            OpenQA.Selenium.Cookie cookie = driver.Manage().Cookies.GetCookieNamed("login_session_id");
            if (cookie != null) break;
        }

        List<Cookie> cookies = driver.Manage().Cookies.AllCookies
            .Select(cookie => new Cookie()
            {
                Name = cookie.Name,
                Value = cookie.Value,
                Domain = cookie.Domain,
                Path = cookie.Path,
                Secure = cookie.Secure,
                HttpOnly = cookie.IsHttpOnly,
                Expires = cookie.Expiry ?? DateTime.MaxValue,
            })
            .ToList();

        string cookiesJson = JsonSerializer.Serialize(cookies);
        await File.WriteAllTextAsync("cookies.json", cookiesJson);

        LoadCookies(cookies);
        
        Log.Information("Successfully logged into DMM");
        
        driver.Quit();
    }

    public static async Task GetGameInfoAsync()
    {
        Dictionary<string, string> body = new()
        {
            { "product_id", "umamusume" },
            { "game_type", "GCL" },
            { "launch_type", "LIB" },
            { "game_os", "win" },
            { "mac_address", MacAddress },
            { "hdd_serial", HddSerial },
            { "motherboard", Motherboard },
            { "user_os", "win" },
        };

        string bodyJson = JsonSerializer.Serialize(body);
        DmmApiResponse<GameInfo> response = await RestUtility.PostAsync<DmmApiResponse<GameInfo>>($"{BaseApiDomain}{GameInfoEndpoint}", LauncherHeaders, bodyJson);
        
        Log.Verbose("GameInfo response = {Response}", JsonSerializer.Serialize(response));
    }

    public static async Task GetLaunchCommandAsync()
    {
        Dictionary<string, string> body = new()
        {
            { "product_id", "umamusume" },
            { "game_type", "GCL" },
            { "launch_type", "LIB" },
            { "game_os", "win" },
            { "mac_address", MacAddress },
            { "hdd_serial", HddSerial },
            { "motherboard", Motherboard },
            { "user_os", "win" },
        };
        
        string bodyJson = JsonSerializer.Serialize(body);
        DmmApiResponse<LaunchCommand> response = await RestUtility.PostAsync<DmmApiResponse<LaunchCommand>>($"{BaseApiDomain}{LaunchClEndpoint}", LauncherHeaders, bodyJson);
        
        Log.Verbose("LaunchCommand response = {Response}", JsonSerializer.Serialize(response));
    }

    public static async Task ReportAsync()
    {
        Dictionary<string, string> body = new()
        {
            { "type", "start" },
            { "product_id", "umamusume" },
            { "game_type", "GCL" }
        };
        
        string bodyJson = JsonSerializer.Serialize(body);
        DmmApiResponse<Report> response = await RestUtility.PostAsync<DmmApiResponse<Report>>($"{BaseApiDomain}{ReportEndpoint}", LauncherHeaders, bodyJson);
        
        Log.Verbose("Report response = {Response}", JsonSerializer.Serialize(response));
    }

    private static bool CheckCookies()
    {
        try
        {
            Log.Information("Checking cookies");

            if (!File.Exists("cookies.json")) return false;
        
            string cookiesJson = File.ReadAllText("cookies.json");
            Cookie[]? cookies = JsonSerializer.Deserialize<Cookie[]>(cookiesJson);

            if (cookies == null || cookies.Length == 0 || cookies.Any(x => x.Expired)) return false;

            LoadCookies(cookies);
        
            Log.Information("Found cookies");
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception was thrown while checking cookies");
            return false;
        }
    }

    private static void LoadCookies(IEnumerable<Cookie> cookies)
    {
        foreach (Cookie cookie in cookies)
        {
            Log.Verbose("Adding cookie to HttpClient: {Name}", cookie.Name);
            RestUtility.AddCookie(cookie);
        }
    }
}