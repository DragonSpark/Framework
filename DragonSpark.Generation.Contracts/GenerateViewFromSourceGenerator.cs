using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DragonSpark.Generation.Contracts;

[Generator]
public sealed class GenerateViewFromSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.AddSource("GenerateFromAttribute.g.cs",
            """
            using System;

            namespace DragonSpark.Generation.Contracts;

            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
            public sealed class GenerateFromAttribute<T> : Attribute;
            """);

            ctx.AddSource("SkipGenerationAttribute.g.cs",
            """
            using System;

            namespace DragonSpark.Generation.Contracts;

            [AttributeUsage(AttributeTargets.Property)]
            public sealed class SkipGenerationAttribute : Attribute;
            """);
        });

        var typeDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
            (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
            (ctx, _) => ctx)
            .Where(ctx => ctx.Node is TypeDeclarationSyntax);

        var typeSymbols = typeDeclarations
            .Select((ctx, _) => ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol)
            .Where(s => s is not null);

        var candidates = typeSymbols
            .Where(s => s!.GetAttributes()
                .Any(a => a.AttributeClass?.Name == "GenerateFromAttribute"));

        var compilationProvider = context.CompilationProvider;

        var combined = candidates.Combine(compilationProvider);

        context.RegisterSourceOutput(combined, (spc, pair) =>
        {
            var (targetType, compilation) = pair;
            var processed = new HashSet<string>();
            Generate(spc, compilation, targetType!, processed);
        });
    }

    // ReSharper disable once TooManyArguments
    static void Generate(SourceProductionContext context,
                         Compilation compilation,
                         INamedTypeSymbol targetType,
                         HashSet<string> processed)
    {
        var attr = targetType.GetAttributes()
            .First(a => a.AttributeClass?.Name == "GenerateFromAttribute");

        var sourceType = (INamedTypeSymbol)attr.AttributeClass!.TypeArguments[0];

        string suffix = targetType.Name.StartsWith(sourceType.Name)
            ? targetType.Name.Substring(sourceType.Name.Length)
            : string.Empty;

        GenerateDtoForType(context, compilation, sourceType, targetType.Name,
                           targetType.ContainingNamespace, suffix, targetType, processed);
    }

    // ReSharper disable once CognitiveComplexity
    // ReSharper disable once TooManyArguments
    // ReSharper disable once ExcessiveIndentation
    // ReSharper disable once MethodTooLong
    // ReSharper disable once CyclomaticComplexity
    static void GenerateDtoForType(SourceProductionContext context,
                                   Compilation compilation,
                                   INamedTypeSymbol sourceType,
                                   string dtoName,
                                   INamespaceSymbol ns,
                                   string suffix,
                                   INamedTypeSymbol targetType,
                                   HashSet<string> processed)
    {
        if (!processed.Add(dtoName))
            return;

        var validateComplexTypeAttr = compilation.GetTypeByMetadataName(
            "DragonSpark.Application.AspNet.Components.Validation.ValidateComplexTypeAttribute");

        var hasValidateComplexType = validateComplexTypeAttr is not null;

        var targetProps = targetType.GetMembers()
            .OfType<IPropertySymbol>()
            .ToDictionary(p => p.Name, p => p);

        var sb = new StringBuilder();

        if (!ns.IsGlobalNamespace)
        {
            sb.AppendLine($"namespace {ns.ToDisplayString()};");
            sb.AppendLine();
        }

        sb.AppendLine("using System;");
        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine();

        sb.AppendLine($"public sealed partial class {dtoName}");
        sb.AppendLine("{");

        var props = sourceType.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(p => p.SetMethod is not null)
            .ToArray();

        foreach (var prop in props)
        {
            // Skip if target declares this property AND it has [SkipGeneration]
            if (targetProps.TryGetValue(prop.Name, out var targetProp) &&
                targetProp.GetAttributes().Any(a => a.AttributeClass?.Name == "SkipGenerationAttribute"))
            {
                continue;
            }

            // DataAnnotations
            var annotations = prop.GetAttributes()
                .Where(a => a.AttributeClass?.ContainingNamespace.ToDisplayString() ==
                            "System.ComponentModel.DataAnnotations");

            foreach (var attr in annotations)
            {
                var name = attr.AttributeClass!.Name;

                if (name.EndsWith("Attribute", System.StringComparison.Ordinal))
                    name = name.Substring(0, name.Length - "Attribute".Length);

                if (attr.ConstructorArguments.Length == 0)
                {
                    sb.AppendLine($"    [System.ComponentModel.DataAnnotations.{name}]");
                }
                else
                {
                    var args = string.Join(", ",
                        attr.ConstructorArguments.Select(a =>
                            a.Value is string s ? $"\"{s}\"" : a.Value?.ToString()));

                    sb.AppendLine($"    [System.ComponentModel.DataAnnotations.{name}({args})]");
                }
            }

            if (IsPrimitive(prop.Type))
            {
                sb.AppendLine($"    public {prop.Type.ToDisplayString()} {prop.Name} {{ get; set; }}");
            }
            else if (prop.Type is INamedTypeSymbol complex)
            {
                var nestedDtoName = $"{complex.Name}{suffix}";

                if (hasValidateComplexType)
                {
                    sb.AppendLine("    [DragonSpark.Application.AspNet.Components.Validation.ValidateComplexType]");
                }

                sb.AppendLine($"    public {nestedDtoName} {prop.Name} {{ get; set; }} = new();");

                GenerateDtoForType(context, compilation, complex, nestedDtoName,
                                   ns, suffix, targetType, processed);
            }

            sb.AppendLine();
        }

        // ToSource()
        sb.AppendLine($"    public {sourceType.ToDisplayString()} ToSource()");
        sb.AppendLine("    {");
        sb.AppendLine($"        var result = new {sourceType.ToDisplayString()}();");

        foreach (var prop in props)
        {
            if (targetProps.TryGetValue(prop.Name, out var targetProp) &&
                targetProp.GetAttributes().Any(a => a.AttributeClass?.Name == "SkipGenerationAttribute"))
            {
                continue;
            }

            if (IsPrimitive(prop.Type))
            {
                sb.AppendLine($"        result.{prop.Name} = this.{prop.Name};");
            }
            else if (prop.Type is INamedTypeSymbol)
            {
                sb.AppendLine($"        result.{prop.Name} = this.{prop.Name}?.ToSource();");
            }
        }

        sb.AppendLine("        return result;");
        sb.AppendLine("    }");

        // FromModel()
        sb.AppendLine();
        sb.AppendLine($"    public static {dtoName} From({sourceType.ToDisplayString()} model)");
        sb.AppendLine("    {");
        sb.AppendLine($"        if (model is null) return null!;");
        sb.AppendLine();
        sb.AppendLine($"        var result = new {dtoName}();");

        foreach (var prop in props)
        {
            // Skip if target declares this property AND it has [SkipGeneration]
            if (targetProps.TryGetValue(prop.Name, out var targetProp) &&
                targetProp.GetAttributes().Any(a => a.AttributeClass?.Name == "SkipGenerationAttribute"))
            {
                continue;
            }

            if (IsPrimitive(prop.Type))
            {
                sb.AppendLine($"        result.{prop.Name} = model.{prop.Name};");
            }
            else if (prop.Type is INamedTypeSymbol)
            {
                sb.AppendLine($"        result.{prop.Name} = model.{prop.Name} is null ? null! : {prop.Type.Name}{suffix}.From(model.{prop.Name});");
            }
        }

        sb.AppendLine();
        sb.AppendLine("        return result;");
        sb.AppendLine("    }");

        
        sb.AppendLine("}");

        var nsName = ns.IsGlobalNamespace ? "Global" : ns.ToDisplayString().Replace('.', '_');
        var hint = $"{nsName}_{dtoName}.g.cs";
        context.AddSource(hint, sb.ToString());
    }

    static bool IsPrimitive(ITypeSymbol type)
    {
        return type is INamedTypeSymbol { EnumUnderlyingType: not null } ||
               type.SpecialType switch
               {
                   SpecialType.System_String => true,
                   SpecialType.System_Int32 => true,
                   SpecialType.System_Int64 => true,
                   SpecialType.System_Boolean => true,
                   SpecialType.System_Double => true,
                   SpecialType.System_Single => true,
                   SpecialType.System_Decimal => true,
                   SpecialType.System_Byte => true,
                   SpecialType.System_Char => true,
                   SpecialType.System_DateTime => true,
                   _ => false
               };
    }
}
