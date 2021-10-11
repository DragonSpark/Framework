using DragonSpark.Application.Entities.Editing;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	// TODO: These classes should eventually go away and are meant to assist scoped-to-singleton transition:

	public sealed class ProcessInstanceScopes : InstanceScopes
	{
		public ProcessInstanceScopes(DbContext context) : base(context, EmptyBoundary.Default) {}
	}

	public sealed class AmbientProcessScopes : AmbientAwareScopes
	{
		public AmbientProcessScopes(ProcessInstanceScopes previous, IAmbientContext context)
			: base(previous, context) {}
	}

	public sealed class ProcessSave<T> : Update<T> where T : class
	{
		public ProcessSave(AmbientProcessScopes scopes) : base(scopes) {}
	}
}