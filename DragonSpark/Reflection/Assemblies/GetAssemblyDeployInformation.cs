using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Assemblies;

public sealed class GetAssemblyDeployInformation : ISelect<Assembly, AssemblyDeployInformation>
{
	public static GetAssemblyDeployInformation Default { get; } = new();

	GetAssemblyDeployInformation() : this(GetAssemblyDetails.Default, AssemblyCreatedTime.Default) {}

	readonly ISelect<Assembly, AssemblyDetails> _details;
	readonly ISelect<Assembly, DateTimeOffset>  _created;

	public GetAssemblyDeployInformation(ISelect<Assembly, AssemblyDetails> details,
	                                    ISelect<Assembly, DateTimeOffset> created)
	{
		_details = details;
		_created = created;
	}

	public AssemblyDeployInformation Get(Assembly parameter)
	{
		var version = parameter.Attribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ??
		              _details.Get(parameter).Version.ToString();
		var result = new AssemblyDeployInformation(version, _created.Get(parameter));
		return result;
	}
}