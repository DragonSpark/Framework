using System;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity
{
	public class LoadOperationEventArgs : EventArgs
	{
		public LoadOperation Operation { get; set; }
	}
}