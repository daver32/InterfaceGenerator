using System.Reflection;
using FluentAssertions;
using Xunit;

namespace InterfaceGenerator.Tests
{
    public class VisibilityModifierTests
    {
        [Fact]
        public void IExplicitlyPublicService_IsPublic()
        {
            var type = typeof(IExplicitlyPublicService);
            type.Attributes.Should().HaveFlag(TypeAttributes.Public);
        }
        
        [Fact]
        public void IExplicitlyInternalService_IsInternal()
        {
            var type = typeof(IExplicitlyInternalService);
            type.Attributes.Should().HaveFlag(TypeAttributes.NotPublic);
        }

        [Fact]
        public void IImplicitlyPublicService_IsPublic()
        {
            var type = typeof(IImplicitlyPublicService);
            type.Attributes.Should().HaveFlag(TypeAttributes.Public);
        }
        
        [Fact]
        public void IImplicitlyInternalService_IsInternal()
        {
            var type = typeof(IImplicitlyInternalService);
            type.Attributes.Should().HaveFlag(TypeAttributes.NotPublic);
        }
    }

    [GenerateAutoInterface(VisibilityModifier = "public")]
    internal class ExplicitlyPublicService : IExplicitlyPublicService
    {
    }
    
    [GenerateAutoInterface(VisibilityModifier = "internal")]
    public class ExplicitlyInternalService : IExplicitlyInternalService
    {
    }
    
    [GenerateAutoInterface]
    public class ImplicitlyPublicService : IImplicitlyPublicService
    {
    }
    
    [GenerateAutoInterface]
    internal class ImplicitlyInternalService : IImplicitlyInternalService
    {
    }
}