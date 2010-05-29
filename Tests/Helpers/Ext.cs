using System;
using System.Web;
using Moq;

namespace MultiTenancy.Tests.Helpers
{
    public static class Ext
    {
        public static void MapBaseUrl(this Mock<HttpRequestBase> mock, string uri)
        {
            mock.Setup(x => x.Url).Returns(new Uri(uri));
            mock.Setup(x => x.ApplicationPath).Returns("");
        }
    }
}