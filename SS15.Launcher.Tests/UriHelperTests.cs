using System;
using NUnit.Framework;

namespace SS15.Launcher.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class UriHelperTests
{
    [Test]
    [TestCase("server.traumastation.com", "http://server.traumastation.com:1212/status")]
    [TestCase("ss14s://server.traumastation.com", "https://server.traumastation.com/status")]
    [TestCase("ss14s://server.traumastation.com:1212", "https://server.traumastation.com:1212/status")]
    [TestCase("ss14s://server.traumastation.com/foo", "https://server.traumastation.com/foo/status")]
    public void GetServerStatusAddress(string input, string expected)
    {
        var uri = UriHelper.GetServerStatusAddress(input);

        Assert.That(uri, Is.EqualTo(new Uri(expected)));
    }
}
