using DragonSpark.Model.Sequences;

namespace DragonSpark.Grok.Chat;

sealed class Tools : Instances<Tool>
{
    public Tools(IEnumerable<IToolRegistration> registrations) : base(registrations.Select(x => x.Get())) {}
}