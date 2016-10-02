namespace DragonSpark.Testing.Objects.Composition
{
	class ParameterService : IParameterService
	{
		public ParameterService( Parameter parameter )
		{
			Parameter = parameter;
			parameter.Message = "WithInstance by ParameterService";
		}

		public object Parameter { get; }
	}
}