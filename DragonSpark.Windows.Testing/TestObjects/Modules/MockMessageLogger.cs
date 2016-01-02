using System;
using DragonSpark.Diagnostics;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	class MockMessageLogger : IMessageLogger
	{
		public string LastMessage { get; private set; }
		public string LastMessageCategory;
		
		public void Information( string message, Priority priority = Priority.Normal )
		{
			LastMessage = message;
			LastMessageCategory = nameof(Information);
		}

		public void Warning( string message, Priority priority = Priority.High )
		{
			LastMessage = message;
			LastMessageCategory = nameof(Warning);
		}

		public void Fatal( string message, Exception exception )
		{
			LastMessage = message;
			LastMessageCategory = nameof(Fatal);
		}

		public void Exception( string message, Exception exception )
		{
			LastMessage = message;
			LastMessageCategory = nameof(Exception);
		}
	}
}