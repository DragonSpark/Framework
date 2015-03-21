using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Prism.Modularity
{
	sealed class DirectoryModuleInfoProvider : MarshalByRefObject, IModuleInfoProvider
	{
		readonly IModuleInfoBuilder builder;
		readonly DirectoryInfo directory;

		public DirectoryModuleInfoProvider( IModuleInfoBuilder builder, IEnumerable<string> assemblies, string directoryPath )
		{
			this.builder = builder;
			directory = new DirectoryInfo( directoryPath );
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainOnReflectionOnlyAssemblyResolve;
			foreach ( var assembly in assemblies )
			{
				Load( assembly );
			}
		}

		public IEnumerable<ModuleInfo> GetModuleInfos()
		{
			Assembly assembly = FromLoaded( typeof(IModule).Assembly.FullName );
			Type moduleType = assembly.GetType(typeof(IModule).FullName);

			var result = GetModuleInfos(moduleType).ToArray();
			return result;
		}

		Assembly CurrentDomainOnReflectionOnlyAssemblyResolve( object sender, ResolveEventArgs args )
		{
			var result = FromLoaded( args.Name ) ?? Load( args, directory );
			return result;
		}

		static Assembly FromLoaded( string name )
		{
			var assemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();
			var result = assemblies.FirstOrDefault( asm => string.Equals(asm.FullName, name, StringComparison.OrdinalIgnoreCase));
			return result;
		}

		static Assembly Load( ResolveEventArgs args, DirectoryInfo directory )
		{
			AssemblyName assemblyName = new AssemblyName( args.Name );
			string filename = Path.Combine( directory.FullName, assemblyName.Name + ".dll" );
			var result = File.Exists( filename ) ? Assembly.ReflectionOnlyLoadFrom( filename ) : Assembly.ReflectionOnlyLoad( args.Name );
			return result;
		}

		static Assembly Load(string assemblyPath)
		{
			try
			{
				var result = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
				return result;
			}
			catch (BadImageFormatException)
			{
				// skip non-.NET Dlls
			}
			catch (FileNotFoundException)
			{
				// Continue loading assemblies even if an assembly can not be loaded in the new AppDomain
			}
			return null;
		}

		private IEnumerable<ModuleInfo> GetModuleInfos(Type moduleType)
		{
			var loaded = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().Select( assembly => Path.GetFileName(assembly.Location) ).ToArray();

			var fileInfos = directory.GetFiles("*.dll")
				.Where(file => !loaded.Any( s => string.Equals(file.Name, s, StringComparison.OrdinalIgnoreCase) ) )
				.Select( info => info.FullName );

			var valid = fileInfos.Select( Load );

			var result = valid.SelectMany(assembly => assembly
				.GetExportedTypes()
				.Where(moduleType.IsAssignableFrom)
				.Where(t => t != moduleType)
				.Where(t => !t.IsAbstract)
				.Select(builder.CreateModuleInfo)
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
				AppDomain.Unload(AppDomain.CurrentDomain);
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