namespace DragonSpark.Composition.Compose;

public interface IIncludingRegistration : IRegistration
{
	IRegistration Include(IServiceTypes related);
}