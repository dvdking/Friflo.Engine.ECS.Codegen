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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ClassSymbolProcessor(
  INamedTypeSymbol classSymbol,
  TypeDeclarationSyntax declarationSyntax)
{
  public INamedTypeSymbol ClassSymbol { get; } = classSymbol;

  public ConstructorDescriptor GetConstructorDescriptor()
  {
    ClassMembersAnalyzer classMembersAnalyzer = new(ClassSymbol, declarationSyntax);
    var members = classMembersAnalyzer.GetConstructorParameters();

    var lookup = members
      .ToLookup(member => member.ParameterName, StringComparer.Ordinal);

    IList<ConstructorParameter> duplicates = lookup
      .Where(nameGroup => nameGroup.Count() > 1)
      .Select(nameGroup => nameGroup.Last())
      .ToList();

    if (duplicates.Count > 0)
    {
      throw new DiagnosticException(Diagnostic.Create(
        DiagnosticDescriptors.DuplicateConstructorParameter,
        declarationSyntax.Identifier.GetLocation(),
        duplicates[0].ParameterName,
        ClassSymbol.Name));
    }

    return new(
      ClassSymbol,
      Accessibility.Public,
      members);
  }
}