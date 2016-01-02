using DragonSpark.Diagnostics;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Patterns.Contracts;
using System;
using System.Globalization;
using System.Reflection;

namespace DragonSpark.Modularity
{
	/// <summary>
	/// Implements the <see cref="IModuleInitializer"/> interface. Handles loading of a module based on a type.
	/// </summary>
	public class ModuleInitializer : IModuleInitializer
	{
		private readonly IServiceLocator serviceLocator;
		private readonly IMessageLogger messageLoggerFacade;

		/// <summary>
		/// Initializes a new instance of <see cref="ModuleInitializer"/>.
		/// </summary>
		/// <param name="serviceLocator">The container that will be used to resolve the modules by specifying its type.</param>
		/// <param name="messageLoggerFacade">The logger to use.</param>
		public ModuleInitializer(IServiceLocator serviceLocator, IMessageLogger messageLoggerFacade)
		{
			if (serviceLocator == null)
			{
				throw new ArgumentNullException("serviceLocator");
			}

			if (messageLoggerFacade == null)
			{
				throw new ArgumentNullException("messageLoggerFacade");
			}

			this.serviceLocator = serviceLocator;
			this.messageLoggerFacade = messageLoggerFacade;
		}

		/// <summary>
		/// Initializes the specified module.
		/// </summary>
		/// <param name="moduleInfo">The module to initialize</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catches Exception to handle any exception thrown during the initialization process with the HandleModuleInitializationError method.")]
		public void Initialize([Required]ModuleInfo moduleInfo)
		{
			IModule moduleInstance = null;
			try
			{
				moduleInstance = this.CreateModule(moduleInfo);
				moduleInstance?.Initialize();
			}
			catch (Exception ex)
			{
				this.HandleModuleInitializationError(
					moduleInfo,
					moduleInstance?.GetType().GetTypeInfo().Assembly.FullName,
					ex);
			}
		}

		/// <summary>
		/// Handles any exception occurred in the module Initialization process,
		/// logs the error using the <see cref="ILoggerFacade"/> and throws a <see cref="ModuleInitializeException"/>.
		/// This method can be overridden to provide a different behavior. 
		/// </summary>
		/// <param name="moduleInfo">The module metadata where the error happenened.</param>
		/// <param name="assemblyName">The assembly name.</param>
		/// <param name="exception">The exception thrown that is the cause of the current error.</param>
		/// <exception cref="ModuleInitializeException"></exception>
		public virtual void HandleModuleInitializationError([Required]ModuleInfo moduleInfo, string assemblyName, [Required]Exception exception)
		{
			var result = exception is ModuleInitializeException ? exception : new ModuleInitializeException( moduleInfo.ModuleName, assemblyName, exception.Message, exception );

			messageLoggerFacade.Exception(result.ToString(), result);

			throw result;
		}

		/// <summary>
		/// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
		/// </summary>
		/// <param name="moduleInfo">The module to create.</param>
		/// <returns>A new instance of the module specified by <paramref name="moduleInfo"/>.</returns>
		protected virtual IModule CreateModule([Required]ModuleInfo moduleInfo)
		{
			return this.CreateModule(moduleInfo.ModuleType);
		}

		/// <summary>
		/// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
		/// </summary>
		/// <param name="typeName">The type name to resolve. This type must implement <see cref="IModule"/>.</param>
		/// <returns>A new instance of <paramref name="typeName"/>.</returns>
		protected virtual IModule CreateModule(string typeName)
		{
			Type moduleType = Type.GetType(typeName);
			if (moduleType == null)
			{
				throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedToGetType, typeName));
			}

			return (IModule)this.serviceLocator.GetInstance(moduleType);
		}
	}
}
