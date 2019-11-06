using System;

namespace DragonSpark.Reflection.Assemblies
{
	public sealed class AssemblyDetails
	{
		// ReSharper disable once TooManyDependencies -	These are properties taken from the assembly manifest.
		public AssemblyDetails(string title, string product, string company, string description, string configuration,
		                       string copyright, Version version)
		{
			Title         = title;
			Product       = product;
			Company       = company;
			Description   = description;
			Configuration = configuration;
			Copyright     = copyright;
			Version       = version;
		}

		public string Title { get; }

		public string Product { get; }

		public string Company { get; }

		public string Description { get; }

		public string Configuration { get; }

		public string Copyright { get; }

		public Version Version { get; }
	}
}