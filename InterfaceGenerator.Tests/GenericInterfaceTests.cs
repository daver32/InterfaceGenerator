using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace InterfaceGenerator.Tests;

public class GenericInterfaceTests
{
    [Fact]
    public void GenericParametersGeneratedCorrectly()
    {
        var genericArgs = typeof(IGenericInterfaceTestsService<,>).GetGenericArguments();

        genericArgs.Should().HaveCount(2);
        genericArgs[0].Name.Should().Be("TX");
        genericArgs[1].Name.Should().Be("TY");

        genericArgs[0].IsClass.Should().BeTrue();
        genericArgs[0]
            .GenericParameterAttributes
            .Should()
            .HaveFlag(GenericParameterAttributes.DefaultConstructorConstraint);

        var iEquatableOfTx = typeof(IEquatable<>).MakeGenericType(genericArgs[0]);
        genericArgs[0].GetGenericParameterConstraints().Should().HaveCount(1).And.Contain(iEquatableOfTx);

        genericArgs[1].IsValueType.Should().BeTrue();
    }

    [Fact]
    public void IsNullableEnabled()
    {
        typeof(IGenericInterfaceTestsService<,>).GetCustomAttributes()
            .Select(ca => ca.TypeId)
            .OfType<Type>()
            .Should()
            .ContainSingle(at => at.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
    }
}

[GenerateAutoInterface]
// ReSharper disable once UnusedType.Global
internal class GenericInterfaceTestsService<TX, TY> : IGenericInterfaceTestsService<TX, TY>
    where TX : class, IEquatable<TX>, new()
    where TY : struct
{
}