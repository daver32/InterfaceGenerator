using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace InterfaceGenerator.Tests
{
    public class RecordInterfaceGenerationTests
    {
        private readonly ITestRecord _sut;

        public RecordInterfaceGenerationTests()
        {
            _sut = new TestRecord(420);
        }

        [Fact]
        public void RecordProperty_IsGenerated()
        {
            var prop = typeof(ITestRecord)
                .GetProperty(nameof(TestRecord.RecordProperty))!;

            prop.Should().NotBeNull();

            prop.GetMethod.Should().NotBeNull();
            prop.SetMethod.Should().NotBeNull();
            prop.SetMethod!.ReturnParameter!.GetRequiredCustomModifiers().Should().Contain(typeof(IsExternalInit));

            _sut.RecordProperty.Should().Be(420);
        }

        [Fact]
        public void RecordMethod_IsGenerated()
        {
            var method = typeof(ITestRecord).GetMethod(
                nameof(TestRecord.RecordMethod));

            method.Should().NotBeNull();
            method!.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Should().BeEmpty();

            _sut.RecordMethod();
        }

        [Fact]  
        public void Deconstruct_IsGenerated()
        {
            var method = typeof(ITestRecord).GetMethod(
                nameof(TestRecord.Deconstruct));

            method.Should().NotBeNull();
            method!.ReturnType.Should().Be(typeof(void));
            
            var parameters = method.GetParameters();
            parameters.Length.Should().Be(1);

            var parameter = parameters[0];
            parameter.ParameterType.Should().Be(typeof(int).MakeByRefType());
            parameter.IsOut.Should().BeTrue();
        }
    }

    [GenerateAutoInterface]
    internal record TestRecord(int RecordProperty) : ITestRecord
    {
        public void RecordMethod()
        {
        }
    }
}