using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

namespace Infrastructure.UnitTests;

[AttributeUsage(AttributeTargets.Method)]
public class CustomAutoDataAttribute : AutoDataAttribute
{
	public CustomAutoDataAttribute() : base(CreateFixture)
	{
	}

	private static IFixture CreateFixture()
	{
		var fixture = new Fixture();

		fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true, GenerateDelegates = true });

		return fixture;
	}
}