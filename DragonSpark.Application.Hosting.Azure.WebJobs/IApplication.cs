using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public interface IApplication : IOperation<Guid> {}
}