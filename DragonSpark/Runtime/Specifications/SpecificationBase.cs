namespace DragonSpark.Runtime.Specifications
{
	public abstract class SpecificationBase<TParameter> : ISpecification<TParameter>
	{
		bool ISpecification.IsSatisfiedBy( object parameter )
		{
			var item = parameter is TParameter ? (TParameter)parameter : default(TParameter);
			var result = IsSatisfiedBy( item );
			return result;
		}

		public bool IsSatisfiedBy( TParameter parameter )
		{
			return IsSatisfiedByParameter( parameter );
		}

		protected virtual bool IsSatisfiedByParameter( TParameter parameter )
		{
			var result = parameter != null;
			return result;
		}
	}
}