using System;
using FluentAssertions;
using InterfaceGenerator.Tests.Partial;
using Xunit;

namespace InterfaceGenerator.Tests;

public class PartialClassTests
{
    [Fact]
    public void GeneratesMethodFromOtherParts()
    {
        var tInterface = typeof(IPartialClass);
        tInterface.GetMethods().Should().Contain(x => x.Name == nameof(PartialClass.SomeMethodThatShouldGenerate));
    }
}