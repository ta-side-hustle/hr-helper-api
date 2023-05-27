using Domain.Exceptions.Base;
using Infrastructure.Authorization.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.UnitTests.Authorization.Exceptions;

[TestFixture]
public class AuthorizationExceptionTests
{
	[Test]
	[CustomAutoData]
	public void Constructor_ShouldHaveBaseTypeListException(AuthorizationFailure failure)
	{
		var sut = new AuthorizationException(failure);

		sut.Should().BeAssignableTo<ListException>("because it is accepting object with one or more errors");
	}

	[Test]
	[CustomAutoData]
	public void Constructor_ShouldHaveDefaultMessage(AuthorizationFailure failure)
	{
		var sut = new AuthorizationException(failure);

		sut.Message.Should().NotBeNullOrWhiteSpace();
	}

	[Test]
	[CustomAutoData]
	public void Constructor_ShouldNotThrow_WhenAuthorizationFailureHasZeroFailureReasons(AuthorizationFailure failure)
	{
		var act = () => new AuthorizationException(failure);

		act.Should().NotThrow();
	}

	[Test]
	[CustomAutoData]
	public void Constructor_ShouldThrowArgumentNullException_WhenReceivedNull()
	{
		var act = () => new AuthorizationException(null);

		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[Test]
	[CustomAutoData]
	public void Constructor_ShouldHaveNonEmptyMessagesProperty_WhenReceivedValidFailure(
		AuthorizationFailureReason reason1,
		AuthorizationFailureReason reason2,
		AuthorizationFailureReason reason3
	)
	{
		IEnumerable<AuthorizationFailureReason> reasons = new[] { reason1, reason2, reason3 };
		var failure = AuthorizationFailure.Failed(reasons);

		var sut = new AuthorizationException(failure);

		sut.Messages.Should().HaveCount(reasons.Count());
	}
}