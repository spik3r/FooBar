using NUnit.Framework;

namespace FooBar.Tests;

public class UnitTest1
{
    [Test]
    public void TestPass()
    {
        Assert.That(1, Is.EqualTo(1));
    }

   [Test]
   [Ignore("This test is skipped cause it broke the build :P")]
    public void TestFail()
    {
        Assert.That(2,Is.EqualTo(1));
    }
}
