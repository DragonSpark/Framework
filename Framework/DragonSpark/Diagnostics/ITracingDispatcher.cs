namespace DragonSpark.Diagnostics
{
	/*public interface ITracingDispatcher
	{
		void Dispatch( IMethodInvocation invocation );
	}

	public interface IMethodInvocation
	{
		IParameterCollection Inputs { get; }

		IParameterCollection Arguments { get; }

		IDictionary<string, object> InvocationContext { get; }

		object Target { get; }

		MethodBase MethodBase { get; }

		IMethodReturn CreateMethodReturn( object returnValue, params object[] outputs );

		IMethodReturn CreateExceptionMethodReturn( Exception ex );
	}

	public interface IParameterCollection : IList
	{
		object this[ string parameterName ] { get; set; }

		string ParameterName( int index );

		ParameterInfo GetParameterInfo( int index );

		ParameterInfo GetParameterInfo( string parameterName );

		bool ContainsParameter( string parameterName );
	}

	public interface IMethodReturn
	{
		IParameterCollection Outputs { get; }

		object ReturnValue { get; set; }

		Exception Exception { get; set; }

		IDictionary<string, object> InvocationContext { get; }
	}

	[SecurityCritical]
	public sealed class MethodInvocation : IMethodInvocation
	{
		readonly MethodBase callMessage;
		readonly InputParameterCollection inputParams;
		readonly ParameterCollection allParams;
		readonly Dictionary<string, object> invocationContext = new Dictionary<string, object>();
		readonly object target;
		readonly object[] arguments;

		public MethodInvocation(MethodBase callMessage, object[] arguments, object target)
		{
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(callMessage, "callMessage");

			this.callMessage = callMessage;
			this.target = target;
			this.arguments = arguments; // callMessage.Args;
			inputParams = new InputParameterCollection(callMessage, this.arguments);
			allParams = new ParameterCollection(arguments, MethodBase.GetParameters(), info => true);
		}

		/// <summary>
		/// Gets the inputs for this call.
		/// </summary>
		/// <value>The input collection.</value>
		public IParameterCollection Inputs
		{
			[SecuritySafeCritical]
			get { return inputParams; }
		}

		/// <summary>
		/// Collection of all parameters to the call: in, out and byref.
		/// </summary>
		/// <value>The arguments collection.</value>
		IParameterCollection IMethodInvocation.Arguments
		{
			[SecuritySafeCritical]
			get { return allParams; }
		}

		/// <summary>
		/// Retrieves a dictionary that can be used to store arbitrary additional
		/// values. This allows the user to pass values between call handlers.
		/// </summary>
		/// <value>The invocation context dictionary.</value>
		public IDictionary<string, object> InvocationContext
		{
			[SecuritySafeCritical]
			get { return invocationContext; }
		}

		/// <summary>
		/// The object that the call is made on.
		/// </summary>
		/// <value>The target object.</value>
		public object Target
		{
			[SecuritySafeCritical]
			get { return target; }
		}

		/// <summary>
		/// The method on Target that we're aiming at.
		/// </summary>
		/// <value>The target method base.</value>
		public MethodBase MethodBase
		{
			[SecuritySafeCritical]
			get { return callMessage; }
		}

		/// <summary>
		/// Factory method that creates the correct implementation of
		/// IMethodReturn.
		/// </summary>
		/// <remarks>In this implementation we create an instance of <see cref="MethodReturn"/>.</remarks>
		/// <param name="returnValue">Return value to be placed in the IMethodReturn object.</param>
		/// <param name="outputs">All arguments passed or returned as out/byref to the method. 
		/// Note that this is the entire argument list, including in parameters.</param>
		/// <returns>New IMethodReturn object.</returns>
		[SecuritySafeCritical]
		public IMethodReturn CreateMethodReturn(object returnValue, params object[] outputs)
		{
			return new MethodReturn(callMessage, returnValue, outputs, invocationContext);
		}

		/// <summary>
		/// Factory method that creates the correct implementation of
		/// IMethodReturn in the presence of an exception.
		/// </summary>
		/// <param name="ex">Exception to be set into the returned object.</param>
		/// <returns>New IMethodReturn object</returns>
		[SecuritySafeCritical]
		public IMethodReturn CreateExceptionMethodReturn(Exception ex)
		{
			return new MethodReturn(ex, invocationContext);
		}

		/// <summary>
		/// Gets the collection of arguments being passed to the target.
		/// </summary>
		/// <remarks>This method exists because the underlying remoting call message
		/// does not let handlers change the arguments.</remarks>
		/// <value>Array containing the arguments to the target.</value>
		internal object[] Arguments
		{
			get { return arguments; }
		}
	}

	[SecurityCritical]
	class OutputParameterCollection : ParameterCollection
	{
		public OutputParameterCollection(MethodBase callMessage, object[] arguments) : base(arguments, callMessage.GetParameters(), parameterInfo => parameterInfo.ParameterType.IsByRef)
		{}
	}

	[SecurityCritical]
	class MethodReturn : IMethodReturn
	{
		// private readonly MethodBase callMessage;
		readonly ParameterCollection outputs;
		readonly IDictionary<string, object> invocationContext;
		readonly object[] arguments = new object[0];
		object returnValue;
		Exception exception;

		/// <summary>
		/// Creates a new <see cref="MethodReturn"/> object that contains a
		/// return value.
		/// </summary>
		/// <param name="callMessage">The original call message that invoked the method.</param>
		/// <param name="returnValue">Return value from the method.</param>
		/// <param name="arguments">Collections of arguments passed to the method (including the new
		/// values of any out params).</param>
		/// <param name="invocationContext">Invocation context dictionary passed into the call.</param>
		public MethodReturn(MethodBase callMessage, object returnValue, object[] arguments, IDictionary<string, object> invocationContext)
		{
			// this.callMessage = callMessage;
			this.invocationContext = invocationContext;
			this.arguments = arguments;
			this.returnValue = returnValue;
			this.outputs = new OutputParameterCollection(callMessage, arguments);
		}

		/// <summary>
		/// Creates a new <see cref="MethodReturn"/> object that contains an
		/// exception thrown by the target.
		/// </summary>
		/// <param name="ex">Exception that was thrown.</param>
		/// <param name="callMessage">The original call message that invoked the method.</param>
		/// <param name="invocationContext">Invocation context dictionary passed into the call.</param>
		public MethodReturn(Exception ex, IDictionary<string, object> invocationContext)
		{
			// this.callMessage = callMessage;
			this.invocationContext = invocationContext;
			this.exception = ex;
			this.outputs = new ParameterCollection(this.arguments, new ParameterInfo[0], pi => false);
		}

		/// <summary>
		/// The collection of output parameters. If the method has no output
		/// parameters, this is a zero-length list (never null).
		/// </summary>
		/// <value>The output parameter collection.</value>
		public IParameterCollection Outputs
		{
			[SecuritySafeCritical]
			get { return outputs; }
		}

		/// <summary>
		/// Return value from the method call.
		/// </summary>
		/// <remarks>This value is null if the method has no return value.</remarks>
		/// <value>The return value.</value>
		public object ReturnValue
		{
			[SecuritySafeCritical]
			get { return returnValue; }

			[SecuritySafeCritical]
			set
			{
				returnValue = value;
			}
		}

		/// <summary>
		/// If the method threw an exception, the exception object is here.
		/// </summary>
		/// <value>The exception, or null if no exception was thrown.</value>
		public Exception Exception
		{
			[SecuritySafeCritical]
			get { return exception; }
			
			[SecuritySafeCritical]
			set
			{
				exception = value;
			}
		}

		/// <summary>
		/// Retrieves a dictionary that can be used to store arbitrary additional
		/// values. This allows the user to pass values between call handlers.
		/// </summary>
		/// <remarks>This is guaranteed to be the same dictionary that was used
		/// in the IMethodInvocation object, so handlers can set context
		/// properties in the pre-call phase and retrieve them in the after-call phase.
		/// </remarks>
		/// <value>The invocation context dictionary.</value>
		public IDictionary<string, object> InvocationContext
		{
			[SecuritySafeCritical]
			get { return invocationContext; }
		}

		/#1#// <summary>
		/// Constructs a <see cref="IMethodReturnMessage"/> for the remoting
		/// infrastructure based on the contents of this object.
		/// </summary>
		/// <returns>The <see cref="IMethodReturnMessage"/> instance.</returns>
		[SecurityCritical]
		public IMethodReturnMessage ToMethodReturnMessage()
		{
			if (exception == null)
			{
				return
					new ReturnMessage(returnValue, arguments, arguments.Length,
						callMessage.LogicalCallContext, callMessage);
			}
			else
			{
				return new ReturnMessage(exception, callMessage);
			}
		}#1#
	}

	class ParameterCollection : IParameterCollection
	{
		/// <summary>
		/// An internal struct that maps the index in the arguments collection to the
		/// corresponding <see cref="ParameterInfo"/> about that argument.
		/// </summary>
		private struct ArgumentInfo
		{
			public int Index;
			public string Name;
			public ParameterInfo ParameterInfo;

			/// <summary>
			/// Construct a new <see cref="ArgumentInfo"/> object linking the
			/// given index and <see cref="ParameterInfo"/> object.
			/// </summary>
			/// <param name="index">Index into arguments array (zero-based).</param>
			/// <param name="parameterInfo"><see cref="ParameterInfo"/> for the argument at <paramref name="index"/>.</param>
			public ArgumentInfo(int index, ParameterInfo parameterInfo)
			{
				this.Index = index;
				this.Name = parameterInfo.Name;
				this.ParameterInfo = parameterInfo;
			}
		}

		private readonly List<ArgumentInfo> argumentInfo;
		private readonly object[] arguments;

		/// <summary>
		/// Construct a new <see cref="ParameterCollection"/> that wraps the
		/// given array of arguments.
		/// </summary>
		/// <param name="arguments">Complete collection of arguments.</param>
		/// <param name="argumentInfo">Type information about each parameter.</param>
		/// <param name="isArgumentPartOfCollection">A <see cref="Predicate{ParameterInfo}"/> that indicates
		/// whether a particular parameter is part of the collection. Used to filter out only input
		/// parameters, for example.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
			Justification = "Validation done by Guard class")]
		public ParameterCollection(object[] arguments, ParameterInfo[] argumentInfo, Predicate<ParameterInfo> isArgumentPartOfCollection)
		{
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(arguments, "arguments");
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(isArgumentPartOfCollection, "isArgumentPartOfCollection");

			this.arguments = arguments;
			this.argumentInfo = new List<ArgumentInfo>();
			for (int argumentNumber = 0; argumentNumber < argumentInfo.Length; ++argumentNumber)
			{
				if (isArgumentPartOfCollection(argumentInfo[argumentNumber]))
				{
					this.argumentInfo.Add(new ArgumentInfo(argumentNumber, argumentInfo[argumentNumber]));
				}
			}
		}

		/// <summary>
		/// Fetches a parameter's value by name.
		/// </summary>
		/// <param name="parameterName">parameter name.</param>
		/// <value>value of the named parameter.</value>
		public object this[string parameterName]
		{
			get { return arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index]; }

			set { arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index] = value; }
		}

		private int IndexForInputParameterName(string paramName)
		{
			for (int i = 0; i < argumentInfo.Count; ++i)
			{
				if (argumentInfo[i].Name == paramName)
				{
					return i;
				}
			}
			throw new ArgumentException("Invalid parameter Name", "paramName");
		}

		/// <summary>
		/// Gets the value of a parameter based on index.
		/// </summary>
		/// <param name="index">Index of parameter to get the value for.</param>
		/// <value>Value of the requested parameter.</value>
		public object this[int index]
		{
			get { return arguments[argumentInfo[index].Index]; }
			set { arguments[argumentInfo[index].Index] = value; }
		}

		/// <summary>
		/// Gets the ParameterInfo for a particular parameter by index.
		/// </summary>
		/// <param name="index">Index for this parameter.</param>
		/// <returns>ParameterInfo object describing the parameter.</returns>
		public ParameterInfo GetParameterInfo(int index)
		{
			return argumentInfo[index].ParameterInfo;
		}

		/// <summary>
		/// Gets the <see cref="ParameterInfo"/> for the given named parameter.
		/// </summary>
		/// <param name="parameterName">Name of parameter.</param>
		/// <returns><see cref="ParameterInfo"/> for the requested parameter.</returns>
		public ParameterInfo GetParameterInfo(string parameterName)
		{
			return argumentInfo[IndexForInputParameterName(parameterName)].ParameterInfo;
		}

		/// <summary>
		/// Gets the name of a parameter based on index.
		/// </summary>
		/// <param name="index">Index of parameter to get the name for.</param>
		/// <returns>Name of the requested parameter.</returns>
		public string ParameterName(int index)
		{
			return argumentInfo[index].Name;
		}

		/// <summary>
		/// Does this collection contain a parameter value with the given name?
		/// </summary>
		/// <param name="parameterName">Name of parameter to find.</param>
		/// <returns>True if the parameter name is in the collection, false if not.</returns>
		public bool ContainsParameter(string parameterName)
		{
			return argumentInfo.Any(info => info.Name == parameterName);
		}

		/// <summary>
		/// Adds to the collection. This is a read only collection, so this method
		/// always throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="value">Object to add.</param>
		/// <returns>Nothing, always throws.</returns>
		/// <exception cref="NotSupportedException">Always throws this.</exception>
		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Checks to see if the collection contains the given object.
		/// </summary>
		/// <remarks>Tests for the object using object.Equals.</remarks>
		/// <param name="value">Object to find.</param>
		/// <returns>true if object is in collection, false if it is not.</returns>
		public bool Contains(object value)
		{
			return
				argumentInfo.Exists(
					delegate(ArgumentInfo info)
					{
						var argument = arguments[info.Index];

						if (argument == null)
						{
							return value == null;
						}

						return argument.Equals(value);
					});
		}

		/// <summary>
		/// Remove all items in the collection. This collection is fixed-size, so this
		/// method always throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <exception cref="NotSupportedException">This is always thrown.</exception>
		public void Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns the index of the given object, or -1 if not found.
		/// </summary>
		/// <param name="value">Object to find.</param>
		/// <returns>zero-based index of found object, or -1 if not found.</returns>
		public int IndexOf(object value)
		{
			return argumentInfo.FindIndex(
				delegate(ArgumentInfo info)
				{
					return arguments[info.Index].Equals(value);
				});
		}

		/// <summary>
		/// Inserts a new item. This is a fixed-size collection, so this method throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="index">Index to insert at.</param>
		/// <param name="value">Always throws.</param>
		/// <exception cref="NotSupportedException">Always throws this.</exception>
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Removes the given item. This is a fixed-size collection, so this method throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="value">Always throws.</param>
		/// <exception cref="NotSupportedException">Always throws this.</exception>
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Removes the given item. This is a fixed-size collection, so this method throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="index">Always throws.</param>
		/// <exception cref="NotSupportedException">Always throws this.</exception>
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Is this collection read only?
		/// </summary>
		/// <value>No, it is not read only, the contents can change.</value>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Is this collection fixed size?
		/// </summary>
		/// <value>Yes, it is.</value>
		public bool IsFixedSize
		{
			get { return true; }
		}

		/// <summary>
		/// Copies the contents of this collection to the given array.
		/// </summary>
		/// <param name="array">Destination array.</param>
		/// <param name="index">index to start copying from.</param>
		public void CopyTo(Array array, int index)
		{
			int destIndex = 0;
			argumentInfo.GetRange(index, argumentInfo.Count - index).ForEach(
				delegate(ArgumentInfo info)
				{
					array.SetValue(arguments[info.Index], destIndex);
					++destIndex;
				});
		}

		/// <summary>
		/// All number of items in the collection.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get { return argumentInfo.Count; }
		}

		/// <summary>
		/// Gets a synchronized version of this collection. WARNING: Not implemented completely,
		/// DO NOT USE THIS METHOD.
		/// </summary>
		public object SyncRoot
		{
			get { return this; }
		}

		/// <summary>
		/// Is the object synchronized for thread safety?
		/// </summary>
		/// <value>No, it isn't.</value>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an enumerator object to support the foreach construct.
		/// </summary>
		/// <returns>Enumerator object.</returns>
		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < argumentInfo.Count; ++i)
			{
				yield return arguments[argumentInfo[i].Index];
			}
		}
	}

	[SecurityCritical]
	class InputParameterCollection : ParameterCollection
	{
		public InputParameterCollection( MethodBase method, object[] arguments)
			: base(arguments, method.GetParameters(), info => !info.IsOut )
		{}
	}*/
}