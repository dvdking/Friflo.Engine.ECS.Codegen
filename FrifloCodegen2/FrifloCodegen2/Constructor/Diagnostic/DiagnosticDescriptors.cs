﻿// Copyright 2022 Flavien Charlon
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

using Microsoft.CodeAnalysis;

public class DiagnosticDescriptors
{
  public static DiagnosticDescriptor DuplicateConstructorParameter { get; } = new(
    "AC0001",
    "Duplicate parameter name for auto-generated constructor",
    "The parameter '{0}' is duplicated in the auto-generated constructor for '{1}'.",
    "QuickConstructor",
    DiagnosticSeverity.Error,
    true);

  public static DiagnosticDescriptor InvalidParameterName { get; } = new(
    "AC0002",
    "Invalid parameter name",
    "The parameter name '{0}' in class '{1}' is not a valid identifier.",
    "QuickConstructor",
    DiagnosticSeverity.Error,
    true);
}