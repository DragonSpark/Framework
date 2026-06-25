namespace DragonSpark.Composition.Compose.Deferred;

sealed class DeferredRegistrationStateAccessor : HostAccessor<DeferredRegistrations>, IDeferredRegistrationStateAccessor
{
    public static DeferredRegistrationStateAccessor Default { get; } = new();

    DeferredRegistrationStateAccessor() : base(typeof(DeferredRegistrationStateAccessor)) {}
}