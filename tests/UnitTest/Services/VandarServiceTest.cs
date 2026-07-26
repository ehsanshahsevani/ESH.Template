namespace UnitTest.Services;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class VandarServiceTest : object
{
	// remove stray default constructor and keep only the output helper field + ctor
	private readonly ITestOutputHelper _output;

	public VandarServiceTest(ITestOutputHelper output) : base()
	{
		_output = output;
	}

	// =====================
	// CONFIGURE BEFORE RUN
	// =====================
	// Set these values before running the test (hard-coded per your request)
	// Use official Vandar API host and v3 path (no extra /api segment)
	private const string BaseUrl = "https://api.vandar.io"; // Vandar API base URL
	private const string BusinessName = "real3d"; // <-- set your business name here
	private const string BearerToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiMTEyMWNhNWRkZTVkZTJhMGEwZGI4ZGIyMTk2YzUwOGQ4ZWIyYzI4ZDk3OTAzMWQ1MDcwZGIzZWQxZTVmY2ViY2ExOTc3ODYxMmQ5NTAxNDMiLCJpYXQiOjE3NjM3ODg4MTkuODk4OTMxLCJuYmYiOjE3NjM3ODg4MTkuODk4OTM2LCJleHAiOjE3NjQyMjA4MTkuODg3MTA0LCJzdWIiOiIzODE1MCIsInNjb3BlcyI6WyIqIl19.QGcWgYD4osd--rQVQx3VnyiVvXtyGDlDxtyjcTMbBP4-09o5VgLCj12pLSH2qThoaqMpZx9OI-rLuaLgYw5ZvFJE4-2cZ8HR6D96U4myZ60RE2u2SexPgGvKfFGW2gL7JcCsPC8O2lT-SsTExcKhFNa7HtV5xAJnRUVsjs0exojuYkVr83J8nDZiJRtQ86KPXaHOqx4qhkxrizSLlKm4G7zGTYvqUGc7D7eq1a2SyQmg95dYbTslXOWJmsnpVQhwUyMWhSOCDW67uLJxO-Uxhrflf4GnKXIob-vFJjQCm3p8UOHd9jhutV4evW8dIoNYbBphAYHhXYWJBN9mnGVtoU1HdBpbyViNhF3By07BloCbsKdWelEsgQ9JnEr9-0_fbJB5SrTjlvrotajTjN-WWIp0SjSuD9EZN-IRb_Ranh7bBUZc0Uv_GrD-hHRhlxehkRXbIB_8nC2O0vexxcE33VltJ0K2pCCU9JR_rCKiVjv3pDkuV9XOfcr0kTl7H12yLyCpsEvecRFELcsP-Q4TM28RVwgPjl7-CQoc1zzlA0orby_SZbEHIL6fbMukLyHzgdpRbXhZ_NHAG8meTHHAHfSd3kGmusjsKWJUnqdR01lpWSwPxPCfEr5gXckBpBnjDctN2R-DXRpwgiYdq4K7Rr3lOk0GBz7i_PloqrLMeJM"; // <-- set your Bearer token here

	// Test data provided by you
	//private const string TestNationalCode = "2550216989";
	private const string TestNationalCode = "2281456110";
	//private const string TestMobile = "09172033053";
	private const string TestMobile = "09114958775";

	/// <summary>
	/// تستِ ساده‌ای که مستقیم به endpoint شاهکار درخواست می‌زند.
	/// قبل از اجرا مقدار BusinessName و BearerToken را در همین فایل مقداردهی کنید.
	/// این تست صرفاً بررسی می‌کند که دسترسی به سرویس وجود دارد و پاسخ قابل خواندن است.
	/// در حالت ایده‌آل با داده‌های شما وضعیت برابر با "MATCHED" بازمی‌گردد.
	/// </summary>
	[Fact(DisplayName = "Vandar Shahkar Inquiry - integration quick check")]
	public async Task Shahkar_Inquiry_ShouldReturnMatched_WhenDataIsCorrect()
	{
		// pre-checks
		if (string.IsNullOrWhiteSpace(BusinessName) || BusinessName == "YOUR_BUSINESS_NAME")
		{
			Assert.True(false, "BusinessName is not configured in the test file. Open VandarServiceTest.cs and set BusinessName.");
		}

		if (string.IsNullOrWhiteSpace(BearerToken) || BearerToken == "YOUR_BEARER_TOKEN")
		{
			Assert.True(false, "BearerToken is not configured in the test file. Open VandarServiceTest.cs and set BearerToken.");
		}

		// build request
		var url = $"{BaseUrl}/v3/business/{Uri.EscapeDataString(BusinessName)}/customers/inquiry/shahkar";

		using var client = new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Post, url);

		request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

		var body = new
		{
			mobile = TestMobile,
			national_code = TestNationalCode,
			track_id = Guid.NewGuid().ToString()
		};

		var json = JsonSerializer.Serialize(body);
		request.Content = new StringContent(json, Encoding.UTF8, "application/json");

		_output.WriteLine($"POST {url}");
		_output.WriteLine($"Request body: {json}");

		HttpResponseMessage response;

		try
		{
			response = await client.SendAsync(request);
		}
		catch (Exception ex)
		{
			Assert.True(false, $"Request failed: {ex.Message}");
			return; // unreachable but keeps compiler happy
		}

		var responseBody = await response.Content.ReadAsStringAsync();

		_output.WriteLine($"Response status: {(int)response.StatusCode} {response.ReasonPhrase}");
		_output.WriteLine("Response body:");
		_output.WriteLine(responseBody);

		// Try to parse response JSON and assert expected behavior
		try
		{
			using var doc = JsonDocument.Parse(responseBody);

			if (response.IsSuccessStatusCode == true)
			{
				// expect data.status == "MATCHED"
				if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Null)
				{
					if (data.TryGetProperty("status", out var statusProp))
					{
						var status = statusProp.GetString();
						_output.WriteLine($"data.status = {status}");

						Assert.Equal("MATCHED", status);
						return;
					}

					Assert.True(false, "Response 'data' exists but 'status' field not found.");
				}

				Assert.True(false, "Response success but 'data' field not found or null.");
			}
			else
			{
				// non-success: try to read code like "UNMATCHED"
				if (doc.RootElement.TryGetProperty("code", out var codeProp))
				{
					var code = codeProp.GetString();
					_output.WriteLine($"Response code: {code}");
					Assert.Equal("UNMATCHED", code);
				}

				// if no code, fail with server response
				Assert.True(false, $"Request returned failure status {(int)response.StatusCode}. See output for details.");
			}
		}
		catch (JsonException ex)
		{
			Assert.True(false, $"Failed to parse response JSON: {ex.Message}");
		}
	}

	/// <summary>
	/// بررسی سرویس KYC و تولید درصد تطابق (similarity_percentage).
	/// قبل از اجرا BusinessName و BearerToken را بررسی کنید.
	/// این تست درخواست POST به /v3/business/{business}/customers/inquiry/kyc می‌زند و مقدار "similarity_percentage" را بررسی می‌کند.
	/// پیشنهاد: اگر می‌خواهید تست عبور کند، مقدارهای first/last/national/birthday واقعی و متعلق به شما را قرار دهید.
	/// </summary>
	[Fact(DisplayName = "Vandar KYC Inquiry - integration quick check")]
	public async Task Kyc_Inquiry_ShouldReturnSimilarityPercentage()
	{
		// pre-checks
		if (string.IsNullOrWhiteSpace(BusinessName) || BusinessName == "YOUR_BUSINESS_NAME")
		{
			Assert.True(false, "BusinessName is not configured in the test file. Open VandarServiceTest.cs and set BusinessName.");
		}

		if (string.IsNullOrWhiteSpace(BearerToken) || BearerToken == "YOUR_BEARER_TOKEN")
		{
			Assert.True(false, "BearerToken is not configured in the test file. Open VandarServiceTest.cs and set BearerToken.");
		}

		var url = $"{BaseUrl}/v3/business/{Uri.EscapeDataString(BusinessName)}/customers/inquiry/kyc";

		using var client = new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Post, url);

		request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

		// sample payload - use jalaali birthday as numeric like 13500101
		var body = new
		{
			first_name = "سید فریدالدین",
			last_name = "رضوانی",
			national_code = TestNationalCode,
			// Vandar expects field name `birth_date` and numeric Jalaali format (e.g. 13780928)
			// birth_date = 13780928,
			birth_date = 13720531,
			track_id = Guid.NewGuid().ToString()
		};

		var json = JsonSerializer.Serialize(body);
		request.Content = new StringContent(json, Encoding.UTF8, "application/json");

		_output.WriteLine($"POST {url}");
		_output.WriteLine($"Request body: {json}");

		HttpResponseMessage response;

		try
		{
			response = await client.SendAsync(request);
		}
		catch (Exception ex)
		{
			Assert.True(false, $"Request failed: {ex.Message}");
			return;
		}

		var responseBody = await response.Content.ReadAsStringAsync();

		_output.WriteLine($"Response status: {(int)response.StatusCode} {response.ReasonPhrase}");
		_output.WriteLine("Response body:");
		_output.WriteLine(responseBody);

		try
		{
			using var doc = JsonDocument.Parse(responseBody);

			if (response.IsSuccessStatusCode == true)
			{
				if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Null)
				{
					if (data.TryGetProperty("similarity_percentage", out var simProp) && simProp.ValueKind == JsonValueKind.Number)
					{
						var similarity = simProp.GetInt32();
						_output.WriteLine($"similarity_percentage = {similarity}");

						// recommended acceptance threshold is 80
						Assert.True(similarity >= 80, $"Similarity percentage is below threshold: {similarity}");

						return;
					}

					Assert.True(false, "Response 'data' exists but 'similarity_percentage' field not found or not a number.");
				}

				Assert.True(false, "Response success but 'data' field not found or null.");
			}
			else
			{
				Assert.True(false, $"Request returned failure status {(int)response.StatusCode}. See output for details.");
			}
		}
		catch (JsonException ex)
		{
			Assert.True(false, $"Failed to parse response JSON: {ex.Message}");
		}
	}

	// New test: Card inquiry (bank card compatibility) - strictly follow documented request
	[Fact(DisplayName = "Vandar Card Inquiry - documented endpoint")]
	public async Task Card_Inquiry_DocumentedEndpoint()
	{
		// pre-checks
		if (string.IsNullOrWhiteSpace(BusinessName) || BusinessName == "YOUR_BUSINESS_NAME")
		{
			Assert.True(false, "BusinessName is not configured in the test file. Open VandarServiceTest.cs and set BusinessName.");
		}

		if (string.IsNullOrWhiteSpace(BearerToken) || BearerToken == "YOUR_BEARER_TOKEN")
		{
			Assert.True(false, "BearerToken is not configured in the test file. Open VandarServiceTest.cs and set BearerToken.");
		}

		var cardNumber = "6037701642801184";
		var customer = TestNationalCode; // national code, mobile or customer id

		// Build URL exactly as in the documentation
		var url = $"{BaseUrl}/v3/business/{Uri.EscapeDataString(BusinessName)}/customers/{Uri.EscapeDataString(customer)}/cards/{Uri.EscapeDataString(cardNumber)}/inquiry";

		using var client = new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Post, url);

		request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
		// Content-Type will be set by StringContent below

		var body = new
		{
			first_name = "احسان",
			last_name = "شاهسونی",
			legal_name = string.Empty,
			track_id = "abc"
		};

		var json = JsonSerializer.Serialize(body);
		request.Content = new StringContent(json, Encoding.UTF8, "application/json");

		_output.WriteLine($"POST {url}");
		_output.WriteLine($"Request body: {json}");

		HttpResponseMessage response;
		try
		{
			response = await client.SendAsync(request);
		}
		catch (Exception ex)
		{
			Assert.True(false, $"Request failed: {ex.Message}");
			return;
		}

		var responseBody = await response.Content.ReadAsStringAsync();
		_output.WriteLine($"Response status: {(int)response.StatusCode} {response.ReasonPhrase}");
		_output.WriteLine("Response body:");
		_output.WriteLine(responseBody);

		// Parse response and assert expected field if success; otherwise fail and show server response.
		try
		{
			using var doc = JsonDocument.Parse(responseBody);

			if (response.IsSuccessStatusCode)
			{
				if (doc.RootElement.TryGetProperty("card_compatibility", out var compatProp))
				{
					var compat = compatProp.GetString();
					_output.WriteLine($"card_compatibility = {compat}");
					Assert.Equal("MATCHED", compat);
					return;
				}

				if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Null)
				{
					if (data.TryGetProperty("card_compatibility", out var c2))
					{
						var compat = c2.GetString();
						_output.WriteLine($"data.card_compatibility = {compat}");
						Assert.Equal("MATCHED", compat);
						return;
					}
				}

				Assert.True(false, "Response success but 'card_compatibility' field not found.");
			}
			else
			{
				// Fail and surface server message
				string serverMessage = null;
				if (doc.RootElement.TryGetProperty("message", out var msg)) serverMessage = msg.GetString();
				if (doc.RootElement.TryGetProperty("status", out var st)) serverMessage = (serverMessage ?? string.Empty) + (serverMessage == null ? string.Empty : " ") + st.ToString();

				Assert.True(false, $"Request failed with status {(int)response.StatusCode}. Server response: {serverMessage ?? responseBody}");
			}
		}
		catch (JsonException ex)
		{
			Assert.True(false, $"Failed to parse response JSON: {ex.Message}");
		}
	}

	// New test: Register card for a customer (separate test, do not modify existing tests)
	[Fact(DisplayName = "Vandar Register Card - integration quick check")]
	public async Task RegisterCard_ShouldReturnCard_WhenRequestIsValid()
	{
		// pre-checks
		if (string.IsNullOrWhiteSpace(BusinessName) || BusinessName == "YOUR_BUSINESS_NAME")
		{
			Assert.True(false, "BusinessName is not configured in the test file. Open VandarServiceTest.cs and set BusinessName.");
		}

		if (string.IsNullOrWhiteSpace(BearerToken) || BearerToken == "YOUR_BEARER_TOKEN")
		{
			Assert.True(false, "BearerToken is not configured in the test file. Open VandarServiceTest.cs and set BearerToken.");
		}

		var cardNumber = "6037701642801184"; // user's card
		var customer = TestNationalCode; // can be national code, mobile or customer id
		var trackId = "abc";

		var url = $"{BaseUrl}/v3/business/{Uri.EscapeDataString(BusinessName)}/customers/{Uri.EscapeDataString(customer)}/cards";

		using var client = new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Post, url);

		request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

		// follow documented payload exactly: card, is_default, has_inquiry, track_id
		var body = new
		{
			card = cardNumber,
			is_default = true,
			has_inquiry = true,
			track_id = trackId
		};

		var json = JsonSerializer.Serialize(body);
		request.Content = new StringContent(json, Encoding.UTF8, "application/json");

		_output.WriteLine($"POST {url}");
		_output.WriteLine($"Request body: {json}");

		HttpResponseMessage response;

		try
		{
			response = await client.SendAsync(request);
		}
		catch (Exception ex)
		{
			Assert.True(false, $"Request failed: {ex.Message}");
			return;
		}

		var responseBody = await response.Content.ReadAsStringAsync();

		_output.WriteLine($"Response status: {(int)response.StatusCode} {response.ReasonPhrase}");
		_output.WriteLine("Response body:");
		_output.WriteLine(responseBody);

		try
		{
			using var doc = JsonDocument.Parse(responseBody);

			if (response.IsSuccessStatusCode)
			{
				if (doc.RootElement.TryGetProperty("card", out var cardProp))
				{
					var returned = cardProp.GetString();
					_output.WriteLine($"card = {returned}");
				}
				else
				{
					Assert.True(false, "Response success but 'card' field not found.");
				}

				// optional: check compatibility field if present
				if (doc.RootElement.TryGetProperty("card_compatibility", out var compatProp))
				{
					var compat = compatProp.GetString();
					_output.WriteLine($"card_compatibility = {compat}");
				}

				// check track_id echo
				if (doc.RootElement.TryGetProperty("track_id", out var trackProp))
				{
					var returnedTrack = trackProp.GetString();
					Assert.Equal(trackId, returnedTrack);
				}

				return;
			}
			else
			{
				string serverMessage = null;
				if (doc.RootElement.TryGetProperty("message", out var msg)) serverMessage = msg.GetString();
				Assert.True(false, $"Request failed with status {(int)response.StatusCode}. Server response: {serverMessage ?? responseBody}");
			}
		}
		catch (JsonException ex)
		{
			Assert.True(false, $"Failed to parse response JSON: {ex.Message}");
		}
	}
}
