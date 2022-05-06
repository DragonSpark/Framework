namespace DragonSpark.Application.Runtime;

public interface ILargeOrderAware
{
	public uint? Order { get; set; }
}