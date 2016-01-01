using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;
using Microsoft.CSharp;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public static class CompilerHelper
	{
		readonly static string ModuleTemplate = $@"using System;
			using {typeof(IModule).Namespace};

			namespace TestModules
			{{
				#module#
				public class #className#Class : {typeof(IModule).Name}
				{{
					public void Initialize()
					{{
					   Console.WriteLine(""#className#.Start"");
					}}
					
					public void Load()
					{{
						throw new NotImplementedException();
					}}
				}}
			}}";

		readonly static string[] References = new[] { typeof(object), typeof(IModule), typeof(DynamicModuleInfo) }.Select( type => type.Assembly.CodeBase.Replace( @"file:///", string.Empty ) ).Concat( "System.Runtime.dll".ToItem() ).ToArray();
		

		/*public static Assembly CompileFileAndLoadAssembly(string input, string output, params string[] references)
		{
			return CompileFile(input, output, references).CompiledAssembly;
		}*/

		public static CompilerResults CompileFile(string input, string output, params string[] references)
		{
			CreateOutput(output);

			var codeProvider = new CSharpCodeProvider();
			var cp = new CompilerParameters(References.Concat( references ).ToArray(), output);

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(input))
			{
				if (stream == null)
				{
					throw new ArgumentException("input");
				}

				var reader = new StreamReader(stream);
				var source = reader.ReadToEnd();
				var results = codeProvider.CompileAssemblyFromSource(cp, source);
				ThrowIfCompilerError(results);
				return results;
			}
		}

		static void CreateOutput(string output)
		{
			string dir = Path.GetDirectoryName(output);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			else
			{
				//Delete the file if exists
				if (File.Exists(output))
				{
					try
					{
						File.Delete(output);
					}
					catch (UnauthorizedAccessException)
					{
						//The file might be locked by Visual Studio, so rename it
						if (File.Exists(output + ".locked"))
							File.Delete(output + ".locked");
						File.Move(output, output + ".locked");
					}
				}
			}
		}

		static CompilerResults CompileCode(string code, string output)
		{
			CreateOutput(output);

			var results = new CSharpCodeProvider().CompileAssemblyFromSource( new CompilerParameters( References, output ), code );

			ThrowIfCompilerError(results);

			return results;
		}

		public static string GenerateDynamicModule(string assemblyName, string moduleName, string outpath, params string[] dependencies)
		{
			CreateOutput(outpath);

			// Create temporary module.
			var moduleCode = ModuleTemplate.Replace("#className#", assemblyName).Replace( "#module#", !string.IsNullOrEmpty(moduleName) ? $@"[Module(ModuleName = ""{moduleName}"") #dependencies#]" : string.Empty );

			var depString = dependencies.Aggregate( string.Empty, ( current, module ) => current + $@", ModuleDependency(""{module}"")" );

			var code = moduleCode.Replace("#dependencies#", depString);

			CompileCode(code, outpath);

			return outpath;
		}

		public static string GenerateDynamicModule(string assemblyName, string moduleName, params string[] dependencies)
		{
			string assemblyFile = assemblyName + ".dll";
			string outpath = Path.Combine(assemblyName, assemblyFile);

			return GenerateDynamicModule(assemblyName, moduleName, outpath, dependencies);
		}

		static void ThrowIfCompilerError(CompilerResults results)
		{
			if (results.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Compilation failed.");
				foreach (CompilerError error in results.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				Assert.False(results.Errors.HasErrors, sb.ToString());
			}
		}

		public static void CleanUpDirectory(string path)
		{
			var skip = new[] { "NotAValidDotNetDll.txt.dll", "Prism.StructureMap.Wpf.dll" };

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			else
			{
				foreach (var file in Directory.GetFiles(path).Where( s => !skip.Contains( Path.GetFileName( s ) ) ) )
				{
					try
					{
						File.Delete(file);
					}
					catch (UnauthorizedAccessException)
					{
						//The file might be locked by Visual Studio, so rename it
						if (File.Exists(file + ".locked"))
							File.Delete(file + ".locked");
						File.Move(file, file + ".locked");
					}
				}
			}
		}
	}
}