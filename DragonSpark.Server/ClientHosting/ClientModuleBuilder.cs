using System.Diagnostics;
using DragonSpark.Client;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Server.ClientHosting
{
	public class AssemblyResource
	{
		readonly Assembly assembly;
		readonly string resourceName;

		public AssemblyResource( Assembly assembly, string resourceName )
		{
			this.assembly = assembly;
			this.resourceName = resourceName;
		}

		public Assembly Assembly
		{
			get { return assembly; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}
	}

	public abstract class ClientModuleBuilder : ClientModuleBuilder<ClientModule>
	{
		protected ClientModuleBuilder( string initialPath ) : base( initialPath )
		{}

		protected override ClientModule Create( AssemblyResource resource )
		{
			var result = new ClientModule { Path = CreatePath( resource ) };
			return result;
		}
	}

	public abstract class ClientModuleBuilder<TModule> : Factory<string, IEnumerable<TModule>> where TModule : ClientModule
	{
		readonly string initialPath;

		protected ClientModuleBuilder( string initialPath )
		{
			this.initialPath = initialPath;
		}

		public string InitialPath
		{
			get { return initialPath; }
		}

		protected override IEnumerable<TModule> CreateItem( string parameter )
		{
			var result = AppDomain.CurrentDomain.GetAssemblies()
				.Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() )
				.SelectMany( x => x.GetManifestResourceNames().Select( y => new AssemblyResource( x, y ) ) ).Where( IsResource ).Select( Create ).ToArray();
			return result;
		}

		protected virtual bool IsResource( AssemblyResource resource )
		{
			var result = resource.ResourceName.StartsWith( string.Concat( resource.Assembly.GetName().Name, ".", InitialPath ), StringComparison.InvariantCultureIgnoreCase );
			return result;
		}

		protected abstract TModule Create( AssemblyResource resource );

		protected string CreatePath( AssemblyResource resource )
		{
			var assembly = resource.Assembly.FromMetadata<ClientResourcesAttribute, string>( x => x.Name );
			var name = resource.Assembly.GetName().Name;
			var resourceName = resource.ResourceName.StartsWith( name ) ? DeterminePath( resource.ResourceName.Substring( name.Length + 1 ) ) : resource.ResourceName;

			var result = string.Concat( assembly, Path.AltDirectorySeparatorChar, Path.GetDirectoryName( resourceName ).Replace( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar ), Path.AltDirectorySeparatorChar, Path.GetFileNameWithoutExtension( resourceName ) );
			return result;
		}

		static string DeterminePath( string resourceName )
		{
			var parts = resourceName.ToStringArray( '.' );

			var lower = parts.FirstOrDefault( x => char.IsLetter( x[0] ) && char.IsLower( x[0] ) );

			var index = Array.IndexOf( parts, lower );

			var separator = Path.AltDirectorySeparatorChar.ToString();
			var result = index > -1 && index < parts.Length ? string.Concat( string.Join( separator, parts.Take( index ) ), separator, string.Join( ".", parts.Skip( index ).Take( parts.Length - index ) ) ) : resourceName;
			return result;
		}
	}
}