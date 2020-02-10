using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	sealed class ClaimsTransactional : Transactional<Claim>
	{
		public static ClaimsTransactional Default { get; } = new ClaimsTransactional();

		ClaimsTransactional() : base(new DelegatedEqualityComparer<Claim, string>(x => x.Type),
		                             x => x.Item1.Value != x.Item2.Value) {}
	}

	public readonly struct Destination<T> where T : IdentityUser
	{
		public Destination(UserManager<T> manager, T user, ClaimsPrincipal principal)
		{
			Manager   = manager;
			User      = user;
			Principal = principal;
		}

		public UserManager<T> Manager { get; }

		public T User { get; }

		public ClaimsPrincipal Principal { get; }

		public void Deconstruct(out UserManager<T> manager, out T user, out ClaimsPrincipal principal)
		{
			manager   = Manager;
			user      = User;
			principal = Principal;
		}
	}

	public interface IUserSynchronizer<T> : IOperationResult<(ClaimsPrincipal Source, Destination<T> Destination), bool>
		where T : IdentityUser {}

	public class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly ITransactional<Claim> _transactional;
		readonly Func<Claim, bool>     _where;

		public UserSynchronizer(Func<Claim, bool> where) : this(ClaimsTransactional.Default, where) {}

		public UserSynchronizer(ITransactional<Claim> transactional, Func<Claim, bool> where)
		{
			_transactional = transactional;
			_where         = where;
		}

		public async ValueTask<bool> Get((ClaimsPrincipal Source, Destination<T> Destination) parameter)
		{
			var (source, (manager, user, principal)) = parameter;

			var transactions = _transactional.Get((source.Claims.Where(_where).Result(),
			                                       principal.Claims.Where(_where).Result()));

			if (transactions.Add.Length > 0)
			{
				await manager.AddClaimsAsync(user, transactions.Add.Open());
			}

			foreach (var (existing, input) in transactions.Update.Open())
			{
				await manager.ReplaceClaimAsync(user, existing, input);
			}

			if (transactions.Delete.Length > 0)
			{
				await manager.RemoveClaimsAsync(user, transactions.Delete.Open());
			}

			return transactions.Any();
		}
	}

	public interface IUserSynchronization : IOperation<ExternalLoginInfo> {}

	sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
	{
		readonly SignInManager<T>     _authentication;
		readonly IUserSynchronizer<T> _synchronizer;
		readonly UserManager<T>       _users;

		public UserSynchronization(SignInManager<T> authentication, UserManager<T> users,
		                           IUserSynchronizer<T> synchronizer)
		{
			_authentication = authentication;
			_synchronizer   = synchronizer;
			_users          = users;
		}

		public async ValueTask Get(ExternalLoginInfo parameter)
		{
			var id        = parameter.UniqueId();
			var user      = await _users.Users.SingleAsync(x => x.Id == id);
			var principal = await _authentication.CreateUserPrincipalAsync(user);

			if (await _synchronizer.Get((parameter.Principal, new Destination<T>(_users, user, principal))))
			{
				await _authentication.RefreshSignInAsync(user);
			}
		}
	}

	public interface IIdentityOperation<T> : IOperationResult<(ExternalLoginInfo Login, T User), IdentityResult> {}

	public sealed class CreateUserActions<T> : IIdentityOperation<T> where T : class
	{
		readonly Array<IIdentityOperation<T>> _actions;

		public CreateUserActions(CreateUserOperation<T> create, AddLoginOperation<T> login,
		                         AddClaimsOperation<T> claims)
			: this(An.Array<IIdentityOperation<T>>(create, login, claims)) {}

		public CreateUserActions(Array<IIdentityOperation<T>> actions) => _actions = actions;

		public async ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
		{
			var length = _actions.Length;
			for (var i = 0u; i < length; i++)
			{
				var current = await _actions[i].Get(parameter);
				if (!current.Succeeded)
				{
					return current;
				}
			}

			return IdentityResult.Success;
		}
	}

	public sealed class CreateUserOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly UserManager<T> _users;

		public CreateUserOperation(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.CreateAsync(parameter.User).ToOperation();
	}

	public sealed class AddLoginOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly UserManager<T> _users;

		public AddLoginOperation(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.AddLoginAsync(parameter.User, parameter.Login).ToOperation();
	}

	public sealed class AddClaimsOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly UserManager<T> _users;
		readonly IClaims        _claims;

		public AddClaimsOperation(UserManager<T> users, IClaims claims)
		{
			_users  = users;
			_claims = claims;
		}

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.AddClaimsAsync(parameter.User, _claims.Get(parameter.Login).Open())
			         .ToOperation();
	}

	public interface ICreateAction : IOperationResult<ExternalLoginInfo, IdentityResult> {}

	sealed class CreateAction<T> : ICreateAction where T : IdentityUser
	{
		readonly ICreateUser<T>           _create;
		readonly SignInManager<T>         _authentication;
		readonly ILogger<CreateAction<T>> _log;

		public CreateAction(ICreateUser<T> create, SignInManager<T> authentication, ILogger<CreateAction<T>> log)
		{
			_create         = create;
			_authentication = authentication;
			_log            = log;
		}

		public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
		{
			var (user, result) = await _create.Get(parameter);
			if (result.Succeeded)
			{
				_log.LogInformation("User {UserName} created an account using {Provider} provider.",
				                    user.UserName, parameter.LoginProvider);

				await _authentication.SignInAsync(user, false);
			}

			return result;
		}
	}

	public readonly struct CreateUserResult<T>
	{
		public CreateUserResult(T user, IdentityResult result)
		{
			User   = user;
			Result = result;
		}

		public T User { get; }

		public IdentityResult Result { get; }

		public void Deconstruct(out T user, out IdentityResult result)
		{
			user   = User;
			result = Result;
		}
	}

	public interface ICreateUser<T> : IOperationResult<ExternalLoginInfo, CreateUserResult<T>> {}

	public class CreateUser<T> : ICreateUser<T> where T : class
	{
		readonly CreateUserActions<T>       _actions;
		readonly Func<ExternalLoginInfo, T> _create;

		public CreateUser(CreateUserActions<T> actions, Func<ExternalLoginInfo, T> create)
		{
			_actions = actions;
			_create  = create;
		}

		public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
		{
			var user   = _create(parameter);
			var result = new CreateUserResult<T>(user, await _actions.Get((parameter, user)));
			return result;
		}
	}
}