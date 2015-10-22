using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Entity
{
	public class EntityValidationException : Exception
	{
		public EntityValidationException( DbContext context, DbEntityValidationException innerException ) : base( DetermineMessage( context, innerException ), innerException )
		{}

		static string DetermineMessage( DbContext context, DbEntityValidationException error )
		{
			var result = string.Concat( "Validation Exceptions were encountered while commiting entities to storage.  These are the exceptions:", Environment.NewLine, 
				string.Join( string.Empty, error.EntityValidationErrors.Select( x => $"-= {x.Entry.Entity.GetType()} [State: {x.Entry.State}] [Key: {DetermineKey( context, x )}] =-{Environment.NewLine}{string.Join( Environment.NewLine, x.ValidationErrors.Select( y => string.Concat( (string)"	- ", (string)y.ErrorMessage ) ).ToArray() )}" ) ) );
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