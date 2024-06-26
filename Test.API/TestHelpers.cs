﻿using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;

namespace Test.API
{
    public static class TestHelpers
    {
        public static void AssertResponseStructure(RestResponse response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsonResponse = JsonConvert.DeserializeObject<LoginFailResponse>(response.Content);
            Assert.NotNull(jsonResponse, "Failed to deserialize JSON response.");
            Assert.NotNull(jsonResponse.TotalFailAttempts, "TotalFailAttempts field not found in JSON response.");

            int totalFailAttempts = jsonResponse.TotalFailAttempts;
            Assert.That(totalFailAttempts, Is.GreaterThanOrEqualTo(3), "Total fail attempts should be at least 3.");
        }
    }
}