using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery(
			new Uri("http://localhost:8088/"),
			"partnerCode",
			"partnerKey",
			"partnerIV",
			"test");

		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		_ = fakeGameApiClient
				.GetBalanceAsync(
					Arg.Is(query.ApiUrl),
					Arg.Is(query.PartnerCode),
					Arg.Is(query.PartnerKey),
					Arg.Is(query.PartnerIV),
					Arg.Is(query.PlayerCode))
				.Returns((true, 1));

		var service = new PlayerBalanceGetService(
			fakeGameApiClient);

		var actual = await service.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayer);
		Assert.Equal(1, actual.Balance);
	}
}
