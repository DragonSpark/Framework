using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Application.Compose {
	public sealed class AuthenticationContext : IResult<ApplicationProfileContext>
	{
		readonly ApplicationProfileContext             _subject;
		readonly CommandContext<AuthenticationBuilder> _configure;

		public AuthenticationContext(ApplicationProfileContext subject)
			: this(subject, Start.A.Command<AuthenticationBuilder>().By.Empty) {}

		public AuthenticationContext(ApplicationProfileContext subject, CommandContext<AuthenticationBuilder> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public AuthenticationContext Then(ICommand<AuthenticationBuilder> command) => Then(command.Execute);

		public AuthenticationContext Then(System.Action<AuthenticationBuilder> command)
			=> new AuthenticationContext(_subject, _configure.Append(command));

		public ApplicationProfileContext Get() => _subject.Then(new AuthenticationContextCommand(_configure));
	}
}