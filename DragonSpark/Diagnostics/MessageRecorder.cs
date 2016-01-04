using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

namespace DragonSpark.Diagnostics
{
	public interface IMessageLocator : IFactory<Message[]>
	{}

	public class MessageLocator : FactoryBase<Message[]>, IMessageLocator
	{
		readonly IMessageRecorder[] recorders;

		public MessageLocator( IUnityContainer container, RecordingMessageLogger logger ) : this( container.DetermineLogger() as RecordingMessageLogger, logger )
		{}

		MessageLocator( params RecordingMessageLogger[] loggers ) : this( loggers.NotNull().Select( messageLogger => messageLogger.Recorder ).Distinct().ToArray() )
		{}

		MessageLocator( params IMessageRecorder[] recorders )
		{
			this.recorders = recorders;
		}

		protected override Message[] CreateItem() => recorders.SelectMany( recorder => recorder.Messages ).ToArray();
	}

	public class RecordingMessageLogger : MessageLoggerBase
	{
		public RecordingMessageLogger() : this( new MessageRecorder() )
		{}

		public RecordingMessageLogger( IMessageRecorder recorder ) : this( recorder, ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public RecordingMessageLogger( IMessageRecorder recorder, IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{
			Recorder = recorder;
		}

		public IMessageRecorder Recorder { get; }

		protected override void Write( Message message ) => Recorder.Record( message );
	}

	public class MessageRecorder : IMessageRecorder
	{
		readonly IList<Message> source = new System.Collections.ObjectModel.Collection<Message>();
		readonly IReadOnlyCollection<Message> messages;

		public MessageRecorder()
		{
			messages = new ReadOnlyCollection<Message>( source );
		}

		public void Record( Message message ) => source.Add( message );

		public IEnumerable<Message> Messages => messages;
	}
}