﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InterfaceGenerator
{
    internal static class TypeParameterSymbolExtensions
    {
        public static IEnumerable<string> EnumGenericConstraints(this ITypeParameterSymbol symbol)
        {
            // the class/struct/unmanaged/notnull constraint has to be the first
            // and cannot be combined with one another
            if (symbol.HasNotNullConstraint)
            {
                yield return "notnull";
            }
            else if (symbol.HasUnmanagedTypeConstraint)
            {
                yield return "unmanaged";
            }
            else if (symbol.HasValueTypeConstraint)
            {
                yield return "struct";
            }
            else if (symbol.HasReferenceTypeConstraint)
            {
                yield return symbol.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated
                    ? "class?"
                    : "class";
            }

            
            // types go in the middle
            foreach (var constraintType in symbol.ConstraintTypes)
            {
                yield return constraintType.ToDisplayString();
            }
            
            
            // the new() constraint has to be the last
            if (symbol.HasConstructorConstraint)
            {
                yield return "new()";
            }
        }
    }
}