using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WGFmGamingService.Core.Exceptions;
using Tgs.GameInterface.WGFmGamingService.Core.Extensions;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	HttpClient client,
	IAesCryptography aesCryptography) : IGameApiClient
{
	private readonly TimeZoneInfo _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");

	public async ValueTask AccessMoneyWithdrawalAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		string playerCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var data = $"{playerCode}|{(int)AccessMoneyType.Withdrawal}|{orderId}|{amount}";

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","AccessMoney"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=AccessMoney&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<ReturnCodeResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		if (response.ReturnCode is not 1)
		{
			throw response.ReturnCode switch
			{
				301 => new ReturnCode_301_401_501_601_801_Exception(),
				302 => new ReturnCode_302_502_602_802_Exception(playerCode),
				303 => new ReturnCode_303_Exception(amount),
				305 => new ReturnCode_305_Exception(orderId),
				394 => new ReturnCode_394_Exception(),
				395 => new ReturnCode_395_Exception(),
				396 => new ReturnCode_396_Exception(orderId),
				397 => new ReturnCode_397_Exception(),
				398 => new ReturnCode_398_Exception(),
				399 => new ReturnCode_399_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			};
		}
	}

	public async ValueTask AccessMoneyDepositAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		string playerCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default)

	{
		var data = $"{playerCode}|{(int)AccessMoneyType.Deposit}|{orderId}|{amount}";

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","AccessMoney"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=AccessMoney&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<ReturnCodeResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		if (response.ReturnCode is not 1)
		{
			throw response.ReturnCode switch
			{
				301 => new ReturnCode_301_401_501_601_801_Exception(),
				302 => new ReturnCode_302_502_602_802_Exception(playerCode),
				303 => new ReturnCode_303_Exception(amount),
				305 => new ReturnCode_305_Exception(orderId),
				394 => new ReturnCode_394_Exception(),
				395 => new ReturnCode_395_Exception(),
				396 => new ReturnCode_396_Exception(orderId),
				397 => new ReturnCode_397_Exception(),
				398 => new ReturnCode_398_Exception(),
				399 => new ReturnCode_399_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			};
		}
	}

	public async ValueTask<bool> AccessMoneyQueryAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		string orderId,
		CancellationToken cancellationToken = default)
	{
		var data = orderId;

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","AccessMoneyQuery"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
				};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=AccessMoneyQuery&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<ReturnCodeResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		return response.ReturnCode is not 1
			? throw response.ReturnCode switch
			{
				401 => new ReturnCode_301_401_501_601_801_Exception(),
				402 => new ReturnCode_402_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			}
			: true;
	}

	public async ValueTask<(bool IsPlayer, decimal Balance)> GetBalanceAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		string playerCode,
		CancellationToken cancellationToken = default)
	{
		var data = playerCode;

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","GetBalance"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri(apiUrl, $"index.php?m=api&c=index&a=GetBalance&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<GetBalanceResponse>(
			url,
			request,
			cancellationToken).ConfigureAwait(false);

		return response.ReturnCode is not 1 and not 602
			? throw response.ReturnCode switch
			{
				601 => new ReturnCode_301_401_501_601_801_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			}
			: ((bool IsPlayer, decimal Balance))(response.ReturnCode == 602
			? (false, 0)
			: (true, decimal.Parse(response.Balance)));
	}

	public async ValueTask<IEnumerable<ShowBetInfo>> ShowBetListAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		ShowBetListDateType dateType,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		CancellationToken cancellationToken = default)
	{
		var startTimeEastern = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _timeZone);
		var endTimeEastern = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _timeZone);

		var data = $"|{dateType.Description()}|{startTimeEastern:yyyy-MM-dd HH:mm:ss}|{endTimeEastern:yyyy-MM-dd HH:mm:ss}";

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","ShowBetList"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=ShowBetList&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<ShowBetListResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		return response.ReturnCode is not 1
			? throw response.ReturnCode switch
			{
				501 => new ReturnCode_301_401_501_601_801_Exception(),
				502 => new ReturnCode_302_502_602_802_Exception(),
				503 => new ReturnCode_503_Exception(),
				504 => new ReturnCode_504_Exception(dateType.Description()),
				505 => new ReturnCode_505_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			}
			: response.BetInfo ?? Enumerable.Empty<ShowBetInfo>();
	}

	public async ValueTask<Uri> LoginAsync(
		Uri apiUrl,
		string partnerCode,
		string token,
		string lang,
		int version = 1,
		int isPhone = 0,
		CancellationToken cancellationToken = default)
	{
		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","PrepareLogin"},
			{"Token",WebUtility.UrlEncode(token)},
			{"PartnerCode",partnerCode},
			{"Lang",lang},
			{"Version",version},
			{"IsPhone",isPhone},
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=Login&Token={WebUtility.UrlEncode(token)}&PartnerCode={partnerCode}&Lang={lang}&Version={version}&IsPhone={isPhone}");

		var response = await GetAsync<LoginResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		return response.ReturnCode is not 1
			? throw response.ReturnCode switch
			{
				201 => new ReturnCode_201_Exception(),
				202 => new ReturnCode_202_Exception(),
				203 => new ReturnCode_203_Exception(lang),
				204 => new ReturnCode_204_Exception(request.GetValueOrDefault("Version")?.ToString() ?? string.Empty),
				106 => new ReturnCode_106_Exception(),
				298 => new ReturnCode_198_298_Exception(),
				299 => new ReturnCode_199_299_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			}
			: new(response.LoginUrl);
	}

	public async ValueTask<string> PrepareLoginAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		long timestamp,
		string playerCode,
		string playerNickname,
		string currencyCode,
		CancellationToken cancellationToken = default)
	{
		var data = $"{timestamp}|{playerCode}|{playerNickname}|{currencyCode}";

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","PrepareLogin"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=PrepareLogin&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<PrepareLoginResponse>(
			url,
			request,
			cancellationToken).ConfigureAwait(false);

		return response.ReturnCode is not 1
			? throw response.ReturnCode switch
			{
				101 => new ReturnCode_101_Exception(),
				103 => new ReturnCode_103_Exception(),
				104 => new ReturnCode_104_Exception(partnerCode),
				105 => new ReturnCode_105_Exception(),
				106 => new ReturnCode_106_Exception(),
				198 => new ReturnCode_198_298_Exception(),
				199 => new ReturnCode_199_299_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			}
			: response.Token;
	}

	public async ValueTask LogoutAsync(
		Uri apiUrl,
		string partnerCode,
		string partnerKey,
		string partnerIV,
		string playerCode,
		CancellationToken cancellationToken = default)
	{
		var data = playerCode;

		var dataEncrypt = await aesCryptography.GetAesEncryptAsync(
			data,
			partnerKey,
			partnerIV,
			cancellationToken)
			.ConfigureAwait(false);

		var request = new Dictionary<string, object> {
			{"m","api"},
			{"c","index"},
			{"a","Logout"},
			{"data",data},
			{"dataEncrypt",dataEncrypt}
		};

		var url = new Uri($"{apiUrl}?m=api&c=index&a=Logout&Hash={dataEncrypt}&PartnerCode={partnerCode}");

		var response = await GetAsync<ReturnCodeResponse>(
			url,
			request,
			cancellationToken)
			.ConfigureAwait(false);

		if (response.ReturnCode is not 1)
		{
			throw response.ReturnCode switch
			{
				801 => new ReturnCode_301_401_501_601_801_Exception(),
				802 => new ReturnCode_302_502_602_802_Exception(playerCode),
				803 => new ReturnCode_803_Exception(),
				_ => new ReturnCode_Unknown_Exception(response.ReturnCode),
			};
		}
	}

	private async ValueTask<TResult> GetAsync<TResult>(
		Uri url,
		Dictionary<string, object> request,
		CancellationToken cancellationToken = default)
	{
		_ = request.TryGetValue("a", out var action);

		var logPrefix = $"{nameof(GameApiClient)}.{action?.ToString() ?? "unknown"}".ToLower();

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client
			.GetAsync(
				requestUri: url,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);
		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = JsonSerializer.Serialize(request).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};

		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return string.IsNullOrEmpty(responseMessage)
			? throw new Exception("Game provider response content is empty.")
			: JsonSerializer.Deserialize<TResult>(responseMessage)!;
	}
}
