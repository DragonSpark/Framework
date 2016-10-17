using DragonSpark.Extensions;
using JetBrains.Annotations;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;

namespace DragonSpark.Windows.Legacy.Entity
{
	[Serializable]
	public class EntityValidationException : Exception
	{
		public EntityValidationException() {}

		public EntityValidationException( string message ) : base( message ) {}
		public EntityValidationException( string message, Exception innerException ) : base( message, innerException ) {}

		public EntityValidationException( DbContext context, DbEntityValidationException innerException ) : base( DetermineMessage( context, innerException ), innerException ) {}

		protected EntityValidationException( [NotNull] SerializationInfo info, StreamingContext context ) : base( info, context ) {}

		static string DetermineMessage( DbContext context, DbEntityValidationException error )
		{
			var result = string.Concat( "Validation Exceptions were encountered while commiting entities to storage.  These are the exceptions:", Environment.NewLine, 
				string.Join( string.Empty, error.EntityValidationErrors.Select( x => $"-= {x.Entry.Entity.GetType()} [State: {x.Entry.State}] [Key: {DetermineKey( context, x )}] =-{Environment.NewLine}{string.Join( Environment.NewLine, x.ValidationErrors.Select( y => string.Concat( "	- ", y.ErrorMessage ) ).ToArray() )}" ) ) );
			return result;
		}

		static string DetermineKey( DbContext context, DbEntityValidationResult validationResult )
		{
			var key = context.AsTo<IObjectContextAdapter, ObjectContext>( y => y.ObjectContext ).ExtractKey( validationResult.Entry.Entity );
			var result = string.Join( ", ", key.EntityKeyValues.Select( x => x.ToString() ) );
			return result;
		}
	}
}