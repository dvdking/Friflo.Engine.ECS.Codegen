// Copyright 2022 Flavien Charlon
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace QuickConstructor.Generator;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ClassMembersAnalyzer
{
  private static readonly Regex IdentifierTrim = new(
    @"^[^\p{L}]+",
    RegexOptions.Compiled | RegexOptions.CultureInvariant);

  private readonly INamedTypeSymbol _classSymbol;
  private readonly TypeDeclarationSyntax _declarationSyntax;

  public ClassMembersAnalyzer(
    INamedTypeSymbol classSymbol,
    TypeDeclarationSyntax declarationSyntax)
  {
    _classSymbol = classSymbol;
    _declarationSyntax = declarationSyntax;
  }

  public ImmutableArray<ConstructorParameter> GetConstructorParameters()
  {
    return ImmutableArray.CreateRange(GetFields());
  }

  private IEnumerable<ConstructorParameter> GetFields()
  {
    foreach (var field in _classSymbol.GetMembers().OfType<IFieldSymbol>())
    {
      if (ExcludeMember(field))
        continue;

      if (HasFieldInitializer(field))
        continue;

      yield return CreateParameter(field, field.Type);
    }
  }

  private static bool ExcludeMember(ISymbol member) => !member.CanBeReferencedByName || member.IsStatic;

  private static bool HasFieldInitializer(IFieldSymbol symbol)
  {
    var syntaxNode = symbol.DeclaringSyntaxReferences.ElementAtOrDefault(0)?.GetSyntax();
    var field = syntaxNode as VariableDeclaratorSyntax;

    return field?.Initializer != null;
  }

  private ConstructorParameter CreateParameter(
    ISymbol member,
    ITypeSymbol type)
  {
    var parameterName = GetParameterName(member.Name);

    if (!SyntaxFacts.IsValidIdentifier(parameterName))
    {
      throw new DiagnosticException(Diagnostic.Create(
        DiagnosticDescriptors.InvalidParameterName,
        _declarationSyntax.Identifier.GetLocation(),
        parameterName,
        _classSymbol.Name));
    }

    return new(
      member,
      type,
      parameterName);
  }

  private static string GetParameterName(string symbolName)
  {
    var trimmedParameterName = IdentifierTrim.Replace(symbolName, "");
    if (trimmedParameterName == string.Empty)
    {
      return symbolName;
    }
    else
    {
      return char.ToLowerInvariant(trimmedParameterName[0]) + trimmedParameterName.Substring(1);
    }
  }
}