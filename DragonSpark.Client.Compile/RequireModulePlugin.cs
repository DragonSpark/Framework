using DragonSpark.Extensions;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using SharpKit.Compiler;
using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.JavaScript.Ast;
using System;
using System.Globalization;
using System.Linq;
using Attribute = System.Attribute;

namespace DragonSpark.Client.Compile
{
	public static class CustomAttributeProviderExtensions
	{
		public static TAttribute Ensure<TAttribute>( this ICustomAttributeProvider target, IEntity entity ) where TAttribute : Attribute, new()
		{
			var result = target.GetCustomAttribute<TAttribute>( entity ) ?? target.Transform( x =>
			{
				var attribute = new TAttribute();
				target.AddCustomAttribute( entity, attribute );
				return attribute;
			} );
			return result;
		}

		public static bool IsModule( this ICustomAttributeProvider target, IEntity entity )
		{
			var result = target.GetCustomAttribute<ModuleAttribute>( entity ) != null;
			return result;
		}
	}

    public class RequireModulePlugin : ICompilerPlugin
    {
	    void ICompilerPlugin.Init( ICompiler compiler )
	    {
			Console.WriteLine( "Lifetime And Module: {0} -- {1}", typeof(LifetimeMode).AssemblyQualifiedName, typeof(ModuleAttribute).AssemblyQualifiedName );

		    compiler.BeforeConvertCsToJsEntity += entity => compiler.CsCompilation.Assemblies.SelectMany( x => x.TopLevelTypeDefinitions ).Where( x => compiler.CustomAttributeProvider.IsModule( x ) ).Apply( x =>
		    {
				var provider = compiler.CustomAttributeProvider;
					
				x.Methods.Apply( y => provider.Ensure<JsMethodAttribute>( y ).Name = JsCase( y.Name ) );

				x.Properties.Apply( y => provider.Ensure<JsPropertyAttribute>( y ).Name = JsCase( y.Name ) );

				x.Fields.Apply( y => provider.Ensure<JsFieldAttribute>( y ).Name = JsCase( y.Name ) );
		    } );

		    compiler.AfterConvertCsToJsEntity += ( entity, node ) => entity.As<DefaultResolvedMethod>( x => compiler.CustomAttributeProvider.GetCustomAttribute<ModuleAttribute>( x.DeclaringTypeDefinition ).NotNull( y =>
		    {
			    var constructor = entity.DeclaringTypeDefinition.GetConstructors().FirstOrDefaultOfType<IMethod>();
			    Equals( constructor, x ).IsTrue( () =>
			    {
				    var function = node.AsTo<JsVariableDeclarationStatement, JsFunction>( z => z.Declaration.Declarators.Select( a => a.Initializer ).FirstOrDefaultOfType<JsFunction>() );
				    function.NotNull( z =>
				    {
						constructor.Parameters.Where( a => compiler.CustomAttributeProvider.IsModule( a.Type.GetDefinition() ) ).Select( a => a.Name ).Apply( a => z.Parameters.Remove( a ) );
				    } );
			    } );
		    } ) );

		    compiler.AfterParseCs += () => compiler.CsCompilation.Assemblies.SelectMany( x => x.TopLevelTypeDefinitions ).Select( x => new { Definition = x, Attribute = compiler.CustomAttributeProvider.GetCustomAttribute<ModuleAttribute>( x ) } ).Where( x => x.Attribute != null ).Apply( x => x.Definition.GetConstructors().FirstOrDefault().With( y =>
		    {
			    var list = y.Parameters.Where( z => compiler.CustomAttributeProvider.IsModule( z.Type.GetDefinition() ) ).ToArray();
			    var names = string.Join( ", ", list.Select( z => z.Type.GetDefinition() ).Select( z => string.Format( @"""{0}/{1}""", z.ParentAssembly.AssemblyName, JsCase( z.Name ) ) ) );
			    var parameters = string.Join( ", ", list.Select( z => z.Name ) );

			    x.Attribute.PreCode = string.Format( "define( [ {0} ], function ( {1} ) {{", names, parameters );

			    x.Attribute.PostCode = string.Format( @"var result = instance{0};
return result;
}} );", /*x.Attribute.LifetimeMode == LifetimeMode.Singleton ? "()" :*/ string.Empty );
		    } ) );
	    }

	    static string JsCase( string name )
	    {
		    var result = string.Concat( name[0].ToString( CultureInfo.InvariantCulture ).ToLower(), name.Substring( 1 ) );
		    return result;
	    }
    }
}
