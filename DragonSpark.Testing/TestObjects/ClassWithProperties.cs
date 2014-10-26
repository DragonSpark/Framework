namespace DragonSpark.Testing.Framework.Testing.TestObjects
{
	class ClassWithProperties
	{
		public string PropertyOne { get; set; }
 
		public int PropertyTwo { get; set; }

		public object PropertyThree { get; set; }

		public string PropertyFour { get; set; }

		public string this[ int index ]
		{
			get { return null; }
			set { }
		}

	}
}