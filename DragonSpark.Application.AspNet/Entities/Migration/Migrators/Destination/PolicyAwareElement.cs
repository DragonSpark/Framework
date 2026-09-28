namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

/*sealed class PolicyAwareElement<TFrom, TTo> : PolicyAware<MappingInput<TFrom>, TTo>, IElement<TFrom, TTo> 
	where TFrom : class where TTo : class 
{
	public PolicyAwareElement(IElement<TFrom, TTo> previous)
		: base(previous.Then().Structure().Out(), DurableConnectionPolicy.Default) {}

	Task<TTo> ISelect<Stop<MappingInput<TFrom>>, Task<TTo>>.Get(Stop<MappingInput<TFrom>> parameter)
		=> base.Get(parameter).AsTask();
}*/