using System;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Xunit;

namespace InterfaceGenerator.Tests
{
    public class MethodGenerationTests
    {
        private readonly IMethodsTestService _sut;

        public MethodGenerationTests()
        {
            _sut = new MethodsTestService();
        }

        [Fact]
        public void VoidMethod_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.VoidMethod));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Should().BeEmpty();

            _sut.VoidMethod();
        }

        [Fact]
        public void VoidMethodWithParams_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.VoidMethodWithParams),
                new[] { typeof(string), typeof(string) });

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Select(x => x.ParameterType).Should().AllBeEquivalentTo(typeof(string));
            parameters.Should().HaveCount(2);

            _sut.VoidMethodWithParams(string.Empty, string.Empty);
        }

        [Fact]
        public void VoidMethodWithOutParam_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.VoidMethodWithOutParam),
                new[] { typeof(string).MakeByRefType() });

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Select(x => x.ParameterType).Should().AllBeEquivalentTo(typeof(string).MakeByRefType());
            parameters.Should().HaveCount(1);
            parameters[0].IsOut.Should().BeTrue();

            _sut.VoidMethodWithOutParam(out var _);
        }

        [Fact]
        public void VoidMethodWithInParam_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.VoidMethodWithInParam),
                new[] { typeof(string).MakeByRefType() });

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Select(x => x.ParameterType).Should().AllBeEquivalentTo(typeof(string).MakeByRefType());
            parameters.Should().HaveCount(1);
            parameters[0].IsIn.Should().BeTrue();

            var stub = string.Empty;
            _sut.VoidMethodWithInParam(in stub);
        }

        [Fact]
        public void VoidMethodWithRefParam_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.VoidMethodWithRefParam),
                new[] { typeof(string).MakeByRefType() });

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Select(x => x.ParameterType).Should().AllBeEquivalentTo(typeof(string).MakeByRefType());
            parameters.Should().HaveCount(1);
            parameters[0].IsIn.Should().BeFalse();
            parameters[0].IsOut.Should().BeFalse();

            var stub = string.Empty;
            _sut.VoidMethodWithRefParam(ref stub);
        }

        [Fact]
        public void StringMethod_IsImplemented()
        {
            var method = typeof(IMethodsTestService).GetMethod(
                nameof(MethodsTestService.StringMethod));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(string));

            var parameters = method.GetParameters();
            parameters.Should().BeEmpty();

            var _ = _sut.StringMethod();
        }

        [Fact]
        public void GenericVoidMethod_IsImplemented()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.GenericVoidMethod));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            method.GetParameters().Should().BeEmpty();

            var genericArgs = method.GetGenericArguments();
            genericArgs.Should().HaveCount(2);

            _sut.GenericVoidMethod<string, int>();
        }

        [Fact]
        public void GenericVoidMethodWithGenericParam_IsImplemented()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.GenericVoidMethodWithGenericParam));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var genericArgs = method.GetGenericArguments();
            genericArgs.Should().HaveCount(2);

            var parameters = method.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(genericArgs[0]);

            _sut.GenericVoidMethodWithGenericParam<string, int>(string.Empty);
        }

        [Fact]
        public void GenericVoidMethodWithConstraints_IsImplemented()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.GenericVoidMethodWithConstraints));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var genericArgs = method.GetGenericArguments();
            genericArgs.Should().HaveCount(2);

            genericArgs[0].IsClass.Should().BeTrue();
            genericArgs[0].GetGenericParameterConstraints().Should().HaveCount(0);

            genericArgs[1].IsClass.Should().BeTrue();
            genericArgs[1].GetGenericParameterConstraints().Should().HaveCount(1);
            genericArgs[1].GetGenericParameterConstraints()[0].Should().Be(genericArgs[0]);
            genericArgs[1].GenericParameterAttributes.Should()
                          .HaveFlag(GenericParameterAttributes.DefaultConstructorConstraint);

            _sut.GenericVoidMethodWithConstraints<object, StringBuilder>();
        }

        [Fact]
        public void VoidMethodWithOptionalParams_IsImplemented()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.VoidMethodWithOptionalParams));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Should().HaveCount(5);
            parameters.Select(x => x.IsOptional).Should().AllBeEquivalentTo(true);

            parameters[0].DefaultValue.Should().Be("cGFyYW0=");
            parameters[1].DefaultValue.Should().Be(MethodsTestService.StringConstant);
            parameters[2].DefaultValue.Should().Be(0.1f);
            parameters[3].DefaultValue.Should().Be(0.2d);
            parameters[4].DefaultValue.Should().Be(0.3d);

            _sut.VoidMethodWithOptionalParams();
        }

        [Fact]
        public void VoidMethodWithExpandingParam_IsImplemented()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.VoidMethodWithExpandingParam));

            method.Should().NotBeNull();
            method.ReturnType.Should().Be(typeof(void));

            var parameters = method.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(string[]));
            parameters[0].GetCustomAttribute<ParamArrayAttribute>().Should().NotBeNull();
        }

        [Fact]
        public void IgnoreMethod_IsOmitted()
        {
            var method = typeof(IMethodsTestService)
                         .GetMethods()
                         .FirstOrDefault(x => x.Name == nameof(MethodsTestService.IgnoredMethod));

            method.Should().BeNull();
        }
    }

    [GenerateAutoInterface]
    internal class MethodsTestService : IMethodsTestService
    {
        public const string StringConstant = "Const";

        public void VoidMethod()
        {
        }

        public void VoidMethodWithParams(string a, string b)
        {
        }

        public void VoidMethodWithOutParam(out string a)
        {
            a = default;
        }

        public void VoidMethodWithRefParam(ref string a)
        {
        }

        public void VoidMethodWithInParam(in string a)
        {
        }

        public string StringMethod()
        {
            return string.Empty;
        }

        public void GenericVoidMethod<TX, TY>()
        {
        }

        public void GenericVoidMethodWithGenericParam<TX, TY>(TX a)
        {
        }

        public void GenericVoidMethodWithConstraints<TX, TY>()
            where TX : class
            where TY : class, TX, new()
        {
        }

        public void VoidMethodWithOptionalParams(
            string stringLiteral = "cGFyYW0=",
            string stringConstant = StringConstant,
            float floatLiteral = 0.1f,
            double doubleLiteral = 0.2,
            decimal decimalLiteral = 0.3m)
        {
        }

        public void VoidMethodWithExpandingParam(params string[] strings)
        {
        }

        [AutoInterfaceIgnore]
        public void IgnoredMethod()
        {
            
        }
    }

    [GenerateAutoInterface]
    internal class MethodsTestServiceGeneric<T> : IMethodsTestServiceGeneric<T> where T : class
    {
    }
}