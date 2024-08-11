using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FrifloCodegen
{
  public static class RoslynExt
  {
    public static bool HasInterface(this StructDeclarationSyntax source, string interfaceName)
    {
      if (source.BaseList is null)
        return false;

      var baseTypes = source.BaseList.Types.Select(baseType => baseType);
      return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }

    public static bool HasPartial(this INamedTypeSymbol typeSymbol)
    {
      return typeSymbol.DeclaringSyntaxReferences.Any(syntax =>
        syntax.GetSyntax() is BaseTypeDeclarationSyntax declaration
        && declaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword)));
    }


    public static bool HasPartial(this TypeDeclarationSyntax cds)
    {
      foreach (var modifier in cds.Modifiers)
      {
        if (modifier.IsKind(SyntaxKind.PartialKeyword))
          return true;
      }

      return false;
    }
  }
}