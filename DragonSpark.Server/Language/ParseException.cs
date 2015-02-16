using System;
using System.Runtime.Serialization;

namespace DragonSpark.Server.Legacy.Language
{
	[Serializable]
	public sealed class ParseException : Exception
	{
		public ParseException()
		{}

		#region Fields
		private readonly int position;
		#endregion

		#region Properties
		public ParseException( string message ) : base( message )
		{}

		public ParseException( string message, Exception innerException ) : base( message, innerException )
		{}

		public int Position
		{
			get { return position; }
		}
		#endregion

		public ParseException(string message, int position)
			: base(message)
		{
			this.position = position;
		}

		public override string ToString()
		{
			return string.Format(Res.ParseExceptionFormat, Message, position);
		}

		ParseException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
