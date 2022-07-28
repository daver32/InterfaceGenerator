using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace InterfaceGenerator
{
    internal static class SymbolExtensions
    {
        public static bool TryGetAttribute(
            this ISymbol symbol,
            INamedTypeSymbol attributeType,
            out IEnumerable<AttributeData> attributes)
        {
            attributes = symbol.GetAttributes()
                               .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeType));
            return attributes.Any();
        }

        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeType)
        {
            return symbol.GetAttributes()
                         .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeType));
        }

        //Ref: https://stackoverflow.com/questions/27105909/get-fully-qualified-metadata-name-in-roslyn
        public static string GetFullMetadataName(this ISymbol s, bool useNameWhenNotFound = false)
        {
            if (s == null || IsRootNamespace(s))
            {
                if (useNameWhenNotFound)
                {
                    return s.Name;
                }
                return string.Empty;
            }

            var sb = new StringBuilder(s.MetadataName);
            var last = s;

            s = s.ContainingSymbol;

            while (!IsRootNamespace(s))
            {
                if (s is ITypeSymbol && last is ITypeSymbol)
                {
                    sb.Insert(0, '+');
                }
                else
                {
                    sb.Insert(0, '.');
                }

                sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                //sb.Insert(0, s.MetadataName);
                s = s.ContainingSymbol;
            }

            var retval = sb.ToString();
            return string.IsNullOrEmpty(retval) && useNameWhenNotFound ? s.Name : retval;
        }

        private static bool IsRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol s = null;
            return ((s = symbol as INamespaceSymbol) != null) && s.IsGlobalNamespace;
        }
    }
}