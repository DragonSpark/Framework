using DragonSpark.Extensions;

namespace DragonSpark.Testing.Framework
{
	public class TestingContext<TSubject> : TestingContext where TSubject : class 
	{
		protected virtual TSubject ResolveSubject()
		{
			var result = Locator.GetInstance<TSubject>( SubjectName );
			return result;
		}

		protected override void InitializeSubject()
		{
			Subject = ResolveSubject();
			base.InitializeSubject();
		}

		protected virtual string SubjectName
		{
			get
			{
				var result = ResolveCurrentMethod().Transform( m => m.FromMetadata<SubjectAttribute, string>( a => a.Name ) );
				return result;
			}
		}

		protected TSubject Subject { get; private set; }
	}
}