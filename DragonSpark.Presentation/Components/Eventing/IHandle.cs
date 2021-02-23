﻿using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Eventing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mikoskinen/Blazor.EventAggregator
	/// </summary>
	/// <summary>
	/// Describes a class which can handle a particular type of message.
	/// </summary>
	/// <typeparam name = "TMessage">The type of message to handle.</typeparam>
	public interface IHandle<TMessage>
    {
        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <returns>A task that represents the operation.</returns>
        Task HandleAsync(TMessage message);
    }
}
