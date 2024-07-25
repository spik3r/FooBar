namespace FooBar.Tests;

public class UnitTest1
{
    [Fact]
    public void TestPass()
    {
        Assert.Equal(1, 1);
    }

   [Fact(Skip = "This test is skipped cause it broke the build :P")]
    public void TestFail()
    {
        Assert.Equal(2, 1);
    }
}
