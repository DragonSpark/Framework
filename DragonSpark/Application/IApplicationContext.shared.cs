using System;

namespace DragonSpark.Application
{
	public interface IApplicationContext
	{
		Uri Location { get; }
	}

	/*
	public interface IInitializable
	{
		event EventHandler Initialized;

		void Initialize();
	}
	*/

	/*public interface IApplication
	{
		ApplicationDetails ApplicationDetails { get; }

		Uri Location { get; }
	}*/
}