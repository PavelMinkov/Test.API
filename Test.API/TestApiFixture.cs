using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;
using static Test.API.TestHelpers;

namespace Test.API
{
    [TestFixture]
    public class TestApiFixture
    {
        private RestClient client;
        private string baseUrl = "https://site.com";

        [SetUp]
        public void Setup()
        {
            client = new RestClient(baseUrl);
        }

        // Test case 1: Verify that the endpoint returns the correct total number of failed login attempts for all users.
        [Test]
        public void GetTotalLoginFailsForAllUsersTest()
        {
            var request = new RestRequest("/loginfailtotal", Method.Get);

            var response = client.Execute(request);

            AssertResponseStructure(response);
        }

        // Test case 2: Verify that the endpoint returns the correct total number of failed login attempts for a specific user.
        [Test]
        public void GetTotalLoginFailsForUserTest()
        {
            var specificUser = "example_user"; // Specify a specific userS

            var request = new RestRequest("/loginfailtotal", Method.Get);
            request.AddParameter("user_name", specificUser);

            var response = client.Execute(request);

            AssertResponseStructure(response);
        }

        // Test case 3: Verify that the endpoint returns only users with a number of failed logins above a specified value.
        [Test]
        public void GetUsersWithFailedLoginsAboveLimitTest()
        {
            var failCount = 5; // Specify a fail count limit

            var request = new RestRequest("/loginfailtotal", Method.Get);
            request.AddParameter("fail_count", failCount);

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(response.Content, Contains.Substring("users"));

            var jsonResponse = JsonConvert.DeserializeObject<List<LoginFailResponse>>(response.Content);
            foreach (var user in jsonResponse)
            {
                Assert.That(user.TotalFailAttempts, Is.GreaterThanOrEqualTo(failCount), $"User {user.UserName} should have fail attempts above specified count.");
            }
        }

        // Test case 4: Verify that the endpoint returns a limited number of results when the fetch_limit parameter is set.
        [Test]
        public void GetLimitedResultsWithFetchLimitTest()
        {
            var fetchLimit = 10; // Specify a fetch limit (Assuming there are 10 users)

            var request = new RestRequest("/loginfailtotal", Method.Get);
            request.AddParameter("fetch_limit", fetchLimit);

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsonResponse = JsonConvert.DeserializeObject<List<LoginFailResponse>> (response.Content);
            Assert.That(jsonResponse.Count, Is.EqualTo(fetchLimit), "Number of users in response does not match fetch limit.");
        }

        // Test case 5: Verify that the endpoint resets the total number of failed login attempts for a specific user.
        [Test]
        public void ResetLoginFailCountForSpecificUserTest()
        {
            var specificUser = "example_user";

            var request = new RestRequest("/resetloginfailtotal", Method.Put);
            request.AddParameter("Username", specificUser);

            var response = client.Execute(request);

            AssertResponseStructure(response);

            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, bool>>(response.Content);
            Assert.That(jsonResponse.ContainsKey("success"), Is.True, "success field not found in JSON response.");

            bool resetSuccess = jsonResponse["success"];
            Assert.That(resetSuccess, Is.True, "Reset operation was not successful.");
        }

        // Test case 6: Verify that the endpoint returns an error if the specified user does not exist.
        [Test]
        public void ResetLoginFailCountForNonExistingUserTest()
        {
            var nonExistingUser = "non_existing_user";

            var request = new RestRequest("/resetloginfailtotal", Method.Put);
            request.AddParameter("Username", nonExistingUser);

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));

            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            Assert.That(jsonResponse.ContainsKey("error"), Is.True, "error field not found in JSON response.");

            string errorMessage = jsonResponse["error"];
            Assert.That(errorMessage, Is.EqualTo("User not found"), "Error message does not match.");
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }
    }
}
