using Dmm.Core;
using Dmm.Core.Models.Api;
using Xunit;

namespace Dmm.Tests;

public class DmmApiTests
{
    [Fact]
    public void GetLoginUrl_ReturnsResponse()
    {
        DmmApiResponse<LoginUrl> url = DmmApi.GetLoginUrl();
        
        Assert.Equal(100, url.ResultCode);
        Assert.NotNull(url.Data?.Url);
        Assert.Null(url.Error);
    }
}