using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using System;
using System.Globalization;
using System.Text.Encodings.Web;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class KeyApplicationLocation : ISelect<KeyApplicationLocationInput, Uri>
{
	public static KeyApplicationLocation Default { get; } = new();

	KeyApplicationLocation() : this(UrlEncoder.Default, PrimaryAssemblyDetails.Default) {}

	readonly UrlEncoder _encoder;
	readonly string     _uri;
	readonly string     _name;

	public KeyApplicationLocation(UrlEncoder encoder, AssemblyDetails details)
		: this(encoder, KeyApplicationLocationTemplate.Default, encoder.Encode(details.FullName)) {}

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