using DragonSpark.Testing.Framework;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing
{
	public static class Customizations
	{
		// public class Default : Types<OutputCustomization, ServiceLocatorCustomization, AssignLocationCustomization, RegisterFromConventionAttribute, AutoConfiguredMoqCustomization> {}
		public class Default : Default<object> {}
		public class DefaultMock : Default<AutoConfiguredMoqCustomization> {}
		public abstract class Default<T> : Types<T, ServiceLocatorCustomization> {}

		public class Assigned : Assigned<object, object> {}
		public class AssignedMock : Assigned<object, AutoConfiguredMoqCustomization> {}
		public abstract class Assigned<T, U> : Types<T, ServiceLocatorCustomization, AssignLocationCustomization, U> {}

		public class Register : Register<object, object> {}
		public class RegisterMock : Register<object, AutoConfiguredMoqCustomization> {}
		public abstract class Register<T, U> : Types<T, ServiceLocatorCustomization, RegisterFromConventionAttribute, U> {}

		public class Full : Full<object, object> {}
		public class FullMock : Full<object, AutoConfiguredMoqCustomization> {}
		public abstract class Full<T, U> : Types<T, ServiceLocatorCustomization, Register<T, U>, U> {}
	}
}