using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Text.Encodings.Web;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

sealed class KeyApplicationLocation : ISelect<KeyApplicationLocationInput, Uri>
{
	readonly UrlEncoder _encoder;
	readonly string     _uri;
	readonly string     _name;

	public KeyApplicationLocation(IHostEnvironment environment)
		: this(UrlEncoder.Default, PrimaryAssemblyDetails.Default,
		       environment.IsProduction() ? string.Empty : $" - {environment.EnvironmentName}") {}

	public KeyApplicationLocation(UrlEncoder encoder, AssemblyDetails details, string environment)
		: this(encoder, KeyApplicationLocationTemplate.Default, encoder.Encode($"{details.Product}{environment}")) {}

	public KeyApplicationLocation(UrlEncoder encoder, string uri, string name)
	{
		_encoder = encoder;
		_uri     = uri;
		_name    = name;
	}

	public Uri Get(KeyApplicationLocationInput parameter)
	{
		var (identifier, key) = parameter;
		var location = string.Format(CultureInfo.InvariantCulture, _uri, _name, _encoder.Encode(identifier), key);
		return new(location);
	}
}