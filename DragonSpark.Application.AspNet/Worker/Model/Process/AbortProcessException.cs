using System;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class AbortProcessException : Exception
{
	protected AbortProcessException(string reason, string message) : base(message) => Reason = reason;

	public string Reason { get; }
}