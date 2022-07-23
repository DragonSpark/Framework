using DragonSpark.Model.Selection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class ParseToken : ISelect<string, ClaimsPrincipal>
{
	readonly TokenValidationParameters _validation;
	readonly JwtSecurityTokenHandler   _handler;

	public ParseToken(TokenValidation validation) : this(validation, TokenHandler.Default) {}

	public ParseToken(TokenValidationParameters validation, JwtSecurityTokenHandler handler)
	{
		_validation = validation;
		_handler    = handler;
	}

	public ClaimsPrincipal Get(string parameter) => _handler.ValidateToken(parameter, _validation, out _);
}