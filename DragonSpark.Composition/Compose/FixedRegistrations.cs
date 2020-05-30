namespace DragonSpark.Composition.Compose
{
	sealed class FixedRegistrations : IRegistrations
	{
		readonly IRegistration _context;

		public FixedRegistrations(IRegistration context) => _context = context;

		public IRegistration Get(IRelatedTypes parameter) => _context;
	}
}