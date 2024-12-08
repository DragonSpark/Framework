namespace DragonSpark.Application.AspNet.Security.Identity.State;

public abstract class SystemServerSettings(string name, string domain)
{
	public string Name { get; set; } = name;

	public string Domain { get; set; } = domain;
}