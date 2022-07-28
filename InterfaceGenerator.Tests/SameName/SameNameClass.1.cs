// ReSharper disable CheckNamespace
namespace InterfaceGenerator.Tests.SameName_1;

/// <summary>
/// A class with the same name as <see cref="SameName_2.SameNameClass"/>. It exists to test if the generated source units have fully
/// qualified names.
/// </summary>
[GenerateAutoInterface]
internal class SameNameClass : ISameNameClass
{
    
}