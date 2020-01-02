using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Octokit;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public static class Extensions
	{
		public static RegistrationContext Registration(this ModelContext _) => RegistrationContext.Default;
	}

	public sealed class RegistrationContext
	{
		public static RegistrationContext Default { get; } = new RegistrationContext();

		RegistrationContext() {}

		public ForContext For => ForContext.Default;
	}

	public sealed class ForContext
	{
		public static ForContext Default { get; } = new ForContext();

		ForContext() {}

		public IssueCommentRegistrationContext IssueComments() => IssueCommentRegistrationContext.Default;
	}

	public sealed class IssueCommentRegistrationContext : RegistrationContext<IssueCommentPayload>
	{
		public static IssueCommentRegistrationContext Default { get; } = new IssueCommentRegistrationContext();

		IssueCommentRegistrationContext() : base(IsIssueComment.Default) {}
	}

	public class RegistrationContext<T> where T : ActivityPayload
	{
		readonly ICondition<EventMessage> _condition;

		public RegistrationContext(ICondition<EventMessage> condition) => _condition = condition;

		public IProcessorRegistration With<THandler>() where THandler : class, IHandler<T>
		{
			var registrations = new Registrations<T>(_condition, HandlerRegistration<THandler, T>.Default);
			var result        = new ProcessorRegistration(registrations);
			return result;
		}
	}
}
