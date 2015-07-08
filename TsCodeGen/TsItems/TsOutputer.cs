using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsCodeGen.TsItems
{
    class TsOutputer
    {
        private static IDictionary<string, string> mappings = new Dictionary<string, string>(){
            { "System.DateTime", "Date"},
            { "System.String", "string"},

            { "System.UByte", "number"},
            { "System.UInt16", "number"},
            { "System.UInt32", "number"},
            { "System.UInt64", "number"},

            { "System.Byte", "number"},
            { "System.Int16", "number"},
            { "System.Int32", "number"},
            { "System.Int64", "number"},
            { "System.Double", "number"},
            { "System.Float", "number"},
            { "System.Decimal", "number"},

            { "System.Boolean", "boolean"},
            { "System.Object", "any"},
            { "System.Nullable`1", "@@T0@@"},
            { "System.Collections.Generic.IDictionary`2", "{ [index: @@T0@@]:@@T1@@}"},
            { "System.Collections.Generic.Dictionary`2", "{ [index: @@T0@@]:@@T1@@}"},

            { "System.Collections.Generic.IEnumerable`1", "@@T0@@[]"},
            { "System.Collections.Generic.IReadOnlyList`1", "@@T0@@[]"},
            { "System.Collections.Generic.List`1", "@@T0@@[]"},
            { "System.Array`1", "@@T0@@[]"},
            { "System.Array`2", "@@T0@@[][]"},
            { "System.Array`3", "@@T0@@[][][]"}
        };


        

        

        public static string GetT4Type(ITypeSymbol typeSymbol)
        {
            var typeWithNs = typeSymbol.ContainingNamespace + "." + typeSymbol.MetadataName;
            if (typeSymbol is IArrayTypeSymbol)
            {
                var art = (IArrayTypeSymbol)typeSymbol;
                typeWithNs = "System.Array`" + art.Rank.ToString();
                if (mappings.ContainsKey(typeWithNs))
                {
                    return mappings[typeWithNs].Replace("@@T0@@", GetT4Type(art.ElementType));
                }
                else
                {
                    return GetT4Type(art.ElementType) + string.Concat(Enumerable.Repeat("[]", art.Rank));
                }
            }
            if (typeSymbol is INamedTypeSymbol)
            {
                var nts = (INamedTypeSymbol)typeSymbol;
                if (nts.TypeArguments.Length > 0)
                {
                    string[] types = nts.TypeArguments.Select(s => GetT4Type(s)).ToArray();
                    string toReturn;
                    if (mappings.ContainsKey(typeWithNs))
                    {
                         toReturn = mappings[typeWithNs];
                        
                    }
                    else
                    {
                        toReturn = typeSymbol.ContainingNamespace + "." + typeSymbol.Name + "@@TypeArgs@@";
                    }

                    for (int i = 0; i < types.Length; i++)
                    {
                        toReturn = toReturn.Replace("@@T" + i.ToString() + "@@", types[i]);
                    }
                    if (toReturn.Contains("@@TypeArgs@@"))
                    {
                        var typeArgs =  "<";
                        for (int i = 0; i < types.Length; i++)
                        {
                            if (i != 0)
                                typeArgs += ",";
                            typeArgs += types[i];
                        }
                        typeArgs += ">";
                        toReturn = toReturn.Replace("@@TypeArgs@@", typeArgs);
                    }
                    return toReturn;
                }
            }
            if (mappings.ContainsKey(typeWithNs))
            {
                return mappings[typeWithNs];
            }
            if (!typeSymbol.OriginalDefinition.Locations.Any(s => s.IsInSource))
                return "any";

            return typeWithNs;
        }
    }
}
