namespace DragonSpark.Model.Sequences.Query.Construction
{
	public class Builder<TIn, TOut, TParameter> : IContents<TIn, TOut>
	{
		readonly TParameter                    _argument;
		readonly Create<TIn, TOut, TParameter> _create;

		public Builder(Create<TIn, TOut, TParameter> create, TParameter argument)
		{
			_create   = create;
			_argument = argument;
		}

		public IContent<TIn, TOut> Get(Parameter<TIn, TOut> parameter)
			=> _create(parameter.Body, parameter.Stores, _argument, parameter.Limit);
	}
}