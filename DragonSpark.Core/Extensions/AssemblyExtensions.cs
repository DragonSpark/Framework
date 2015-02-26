using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AssemblyLocatorExtensions
	{
		// static readonly Dictionary<Assembly, TypeInfo[]> TypeCache = new Dictionary<IAssemblyLocator, TypeInfo[]>();

		public static Tuple<TAttribute, TypeInfo>[] GetAllTypesWith<TAttribute>( this IEnumerable<Assembly> target ) where TAttribute : Attribute
		{
			var result = target.SelectMany( assembly => assembly.DefinedTypes ).WhereDecorated<TAttribute>();
			return result;
			/*var result = from type in TypeCache.Ensure( target, ResolveTypes )
			             let attribute = type.GetAttribute<TAttribute>()
			             where attribute != null
			             select new Tuple<TAttribute, TypeInfo>( attribute, type );
			return result;*/
		}

		public static Tuple<TAttribute, TypeInfo>[] WhereDecorated<TAttribute>( this IEnumerable<TypeInfo> target ) where TAttribute : Attribute
		{
			var result = target.Where( info => info.IsDecoratedWith<TAttribute>() ).Select( info => new Tuple<TAttribute, TypeInfo>( info.GetAttribute<TAttribute>(), info ) ).ToArray();
			return result;
		}

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target )
		{
			return target.Select( info => info.AsType() );
		}

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target )
		{
			return target.Select( info => info.GetTypeInfo() );
		}

		/*static TypeInfo[] ResolveTypes( IAssemblyLocator target )
		{
			var query = from assembly in target.GetAllAssemblies()
			            from type in assembly.DefinedTypes
			            select type;
			var result = query.ToArray();
			return result;
		}*/
	}

	public static class AssemblyExtensions
	{
		/*public static string[] GetResourceNames( this Assembly @this )
		{
			var resName = @this.GetName().Name + ".g.resources";
			using (var stream = @this.GetManifestResourceStream(resName))
			using (var reader = new System.Resources.ResourceReader(stream))
			{
				var result = reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
				return result;
			}
		}
		 
		public static IEnumerable<Type> GetValidTypes( this Assembly target )
		{
			try
			{
				var result = target.DefinedTypes.Select( x => x.AsType() ).ToArray();
				return result;
			}
			catch ( ReflectionTypeLoadException e )
			{
				var messages = string.Join( System.Environment.NewLine, e.LoaderExceptions.Select( x => string.Concat( "- ", x.Message ) ) );
				Log.Warning( string.Format( "Could not get types for assembly: {0}.  Messages: {1}{2}", target.GetName(), System.Environment.NewLine, messages ) );
				return e.Types.NotNull().ToArray();
			}
		}

		public static bool IsValid( this Assembly target )
		{
			try
			{
				target.DefinedTypes.ToArray();
				return true;
			}
			catch ( ReflectionTypeLoadException )
			{
				return false;
			}
		}*/

		public static string GetRootNamespace( this Assembly target )
		{
			var root = target.FullName.Transform( x => x.Split( ',' ).FirstOrDefault() );
			var result = target.ExportedTypes.Where( x => x.Namespace.StartsWith( root ) ).Select( x => x.Namespace ).OrderBy( x => x.Length ).FirstOrDefault();
			return result;
		}

		public static string GetAssemblyName( this Assembly assembly )
		{
			var result = assembly.FullName.Transform( x => x.Substring( 0, x.IndexOf( ',' ) ) );
			return result;
		}
	}
}
