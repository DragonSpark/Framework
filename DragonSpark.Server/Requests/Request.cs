using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests;

public readonly record struct Request<T>(ControllerBase Owner, Unique<T> Parameter)
{
	public Request<TOther> Subject<TOther>(TOther subject)
		=> new(Owner, new(Parameter.UserName, Parameter.Identity, subject));
}