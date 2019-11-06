namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class BodyBuilder<T> : Model.Selection.Select<Partitioning, IBody<T>>, IBodyBuilder<T>
	{
		public static BodyBuilder<T> Default { get; } = new BodyBuilder<T>();

		BodyBuilder() : base(x => new Body<T>(x.Selection)) {}
	}

	public class BodyBuilder<T, TParameter> : IBodyBuilder<T>
	{
		readonly Create<T, TParameter> _create;
		readonly TParameter            _parameter;

		public BodyBuilder(Create<T, TParameter> create, TParameter parameter)
		{
			_create    = create;
			_parameter = parameter;
		}

		public IBody<T> Get(Partitioning parameter) => _create(_parameter, parameter.Selection, parameter.Limit);
	}
}