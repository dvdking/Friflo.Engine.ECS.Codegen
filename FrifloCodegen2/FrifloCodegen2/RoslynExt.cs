using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FrifloCodegen
{
  public static class RoslynExt
  {
    public static bool HasInterface(this StructDeclarationSyntax source, string interfaceName)
    {
      if(source.BaseList is null)
        return false;
      
      var baseTypes = source.BaseList.Types.Select(baseType => baseType);
      return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }
  }
}