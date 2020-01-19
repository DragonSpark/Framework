using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies
{
	sealed class AssemblyDetailsSelector : ISelect<Assembly, AssemblyDetails>
	{
		public static AssemblyDetailsSelector Default { get; } = new AssemblyDetailsSelector();

		AssemblyDetailsSelector() : this(AssemblyTitle.Default.Get, AssemblyProduct.Default.Get,
		                                 AssemblyCompany.Default.Get, AssemblyDescription.Default.Get,
		                                 AssemblyConfiguration.Default.Get, AssemblyCopyright.Default.Get,
		                                 AssemblyVersion.Default.Get) {}

		readonly Func<Assembly, string>  _title, _product, _company, _description, _configuration, _copyright;
		readonly Func<Assembly, Version> _version;

		// ReSharper disable once TooManyDependencies -	These are properties taken from the assembly manifest.
		public AssemblyDetailsSelector(Func<Assembly, string> title, Func<Assembly, string> product,
		                               Func<Assembly, string> company, Func<Assembly, string> description,
		                               Func<Assembly, string> configuration, Func<Assembly, string> copyright,
		                               Func<Assembly, Version> version)
		{
			_title         = title;
			_product       = product;
			_company       = company;
			_description   = description;
			_configuration = configuration;
			_copyright     = copyright;
			_version       = version;
		}

		public AssemblyDetails Get(Assembly parameter)
			=> new AssemblyDetails(_title(parameter), _product(parameter), _company(parameter), _description(parameter),
			                       _configuration(parameter), _copyright(parameter), _version(parameter));
	}
}