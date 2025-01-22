using FooBar.Services;
using NUnit.Framework;

namespace FooBar.Tests;

public class MathServiceTest
{
    [Test]
    public void TestAdd()
    {
        // Arrange
        var mathService = new MathService();

        // Act
        var result = mathService.Add(1, 1);

        // Assert
         Assert.That(result, Is.EqualTo(2));
    }


}
