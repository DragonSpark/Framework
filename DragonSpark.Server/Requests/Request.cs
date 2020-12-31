using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public readonly struct Request<T>
	{
		public Request(ControllerBase owner, Query<T> parameter)
		{
			Owner     = owner;
			Parameter = parameter;
		}

		public ControllerBase Owner { get; }

		public Query<T> Parameter { get; }

		public Request<TOther> Subject<TOther>(TOther subject)
			=> new(Owner, new (Parameter.UserName, Parameter.Identity, subject));

		public void Deconstruct(out ControllerBase owner, out Query<T> parameter)
		{
			owner     = Owner;
			parameter = Parameter;
		}
	}
}