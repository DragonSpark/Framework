using DragonSpark.Extensions;
using DragonSpark.Modularity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Modularity
{
	sealed class DirectoryModuleInfoProvider : MarshalByRefObject, IModuleInfoProvider
	{
		readonly IModuleInfoBuilder builder;
		readonly DirectoryInfo directory;

		public DirectoryModuleInfoProvider( IModuleInfoBuilder builder, IEnumerable<string> assemblies, string directoryPath )
		{
			this.builder = builder;
			directory = new DirectoryInfo( directoryPath );

			assemblies.Each( Load );

			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainOnReflectionOnlyAssemblyResolve;
		}

		public IEnumerable<ModuleInfo> GetModuleInfos()
		{
			Assembly assembly = FromLoaded( typeof(IModule).Assembly.FullName );
			Type moduleType = assembly.GetType(typeof(IModule).FullName);

			var result = GetModuleInfos(moduleType).ToArray();
			return result;
		}

		static Assembly FromLoaded( string name, Func<Assembly, string> resolve = null  )
		{
			var resolver = resolve ?? ( assembly => assembly.FullName );
			var assemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();
			var result = assemblies.FirstOrDefault( asm => string.Equals(resolver( asm ), name, StringComparison.OrdinalIgnoreCase));
			return result;
		}

		Assembly CurrentDomainOnReflectionOnlyAssemblyResolve( object sender, ResolveEventArgs args )
		{
			var result = FromLoaded( args.Name ) ?? Load( args, directory );
			return result;
		}

		static Assembly Load( ResolveEventArgs args, DirectoryInfo directory )
		{
			var assemblyName = new AssemblyName( args.Name );
			var filename = Path.Combine( directory.FullName, $"{assemblyName.Name}.dll" );
			var result = File.Exists( filename ) ? Assembly.ReflectionOnlyLoadFrom( filename ) : ReflectionOnlyLoad( args.Name );
			return result;
		}

		static Assembly ReflectionOnlyLoad( string assemblyName )
		{
			try
			{
				return Assembly.ReflectionOnlyLoad( assemblyName );
			}
			catch ( FileNotFoundException )
			{
				var dllName = assemblyName.Contains(',') ? assemblyName.Substring(0, assemblyName.IndexOf(',')) : assemblyName;
				var name = dllName.Replace(".dll", string.Empty);
				return FromLoaded( name, assembly => assembly.GetName().Name );
			}
		}

		static Assembly Load(string assemblyPath)
		{
			try
			{
				var result = Assembly.ReflectionOnlyLoadFrom( assemblyPath ).WithSelf( assembly => assembly.GetExportedTypes() );
				return result;
			}
			catch ( FileLoadException ) // Couldn't load a dependency.
			{}
			catch (BadImageFormatException ) // skip non-.NET Dlls
			{}
			catch (FileNotFoundException ) // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain
			{ }
			return null;
		}

		IEnumerable<DynamicModuleInfo> GetModuleInfos(Type moduleType)
		{
			var loaded = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().Select( assembly => Path.GetFileName(assembly.Location) ).ToArray();

			var names = directory.GetFiles("*.dll")
				.Where(file => !loaded.Any( s => string.Equals(file.Name, s, StringComparison.OrdinalIgnoreCase) ) )
				.Select( info => info.FullName );

			var valid = names.Select( Load ).NotNull();

			var result = valid.SelectMany( assembly => assembly
				.GetExportedTypes()
				.Where( moduleType.IsAssignableFrom )
				.Where( t => t != moduleType )
				.Where( t => !t.IsAbstract )
				.Select( builder.CreateModuleInfo )
				.OfType<DynamicModuleInfo>()
				);
			return result;
		}

		/// <summary>
		/// Disposes the associated <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="disposing">When <see langword="true"/>, disposes the associated <see cref="TextWriter"/>.</param>
		void Dispose(bool disposing)
		{
			if (disposing)
			{
				AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomainOnReflectionOnlyAssemblyResolve;
			}
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		/// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}