# InterfaceGenerator [![NuGet Version](http://img.shields.io/nuget/v/InterfaceGenerator.svg?style=flat)](https://www.nuget.org/packages/InterfaceGenerator/)

A simple source generator that creates interfaces by implementation. 

Example user implementation:
```cs
[GenerateAutoInterface]
public class SampleService : ISampleService
{
    public double Multiply(double x, double y)
    {
        return x * y;            
    }

    public int NiceNumber => 69;
}
```

Auto generated interface:
```cs
public partial interface ISampleService
{
    double Multiply(double x, double y);
    int NiceNumber { get; }
}
```

<br>

Supports:
 - Methods, properties, indexers.
 - Default arguments, `params` arguments.
 - Generic types and methods.
 - XML docs.
 - Explicit interface names (using the `Name` property on `GenerateAutoInterface`.
 - Explicit interface visibility (using the `VisibilityModifier` property on `GenerateAutoInterface`.
 - Explicitly excluding a member from the interface (using `[AutoInterfaceIgnore]`).
 
Missing:
 - Events.
 - Nested types.
 - Probably a bunch of edge cases I forgot about.
 
