using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public readonly struct Request<T>
	{
		public Request(ControllerBase owner, Unique<T> parameter)
		{
			Owner     = owner;
			Parameter = parameter;
		}

		public ControllerBase Owner { get; }

		public Unique<T> Parameter { get; }

		public Request<TOther> Subject<TOther>(TOther subject)
			=> new(Owner, new (Parameter.UserName, Parameter.Identity, subject));

		public void Deconstruct(out ControllerBase owner, out Unique<T> parameter)
		{
			owner     = Owner;
			parameter = Parameter;
		}
	}
}