using AuthenticationService.Contracts.Incoming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace IntegrationTests.Controller
{
    public class AccountControllerTests
    {
        [Fact]
        public async void RegisterUser_ValidEntity_OkObjectResult()
        {
            var createUser = new RegistrationUserDto()
            {
                UserName = "ValidEntity1",
                Password = "ValidEntity1",
                Email = "ValidEntity1@string.com",
                Roles = new string[] { "Administrator", "Moderator", "User" }
            };
            var contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);

            Assert.Equal(HttpStatusCode.OK, responseRegisterUser.StatusCode);
        }

        [Fact]
        public async void RegisterUser_UsernameAlreadyTaken_BadRequest()
        {
            var createUser = new RegistrationUserDto()
            {
                UserName = "TestTakenUsername1",
                Password = "TestTakenUsername1",
                Email = "TestTakenUsername1@string.com",
                Roles = new string[] { "Administrator", "Moderator", "User" }
            };
            var contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);
            responseRegisterUser.EnsureSuccessStatusCode();

            var responseRepeatRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);

            var exceptedErrorMessage = "Username 'TestTakenUsername1' is already taken.";

            Assert.Contains(exceptedErrorMessage, await responseRepeatRegisterUser.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, responseRepeatRegisterUser.StatusCode);
        }

        [Fact]
        public async void RegisterUser_EmailAlreadyTaken_BadRequest()
        {
            var createUser = new RegistrationUserDto()
            {
                UserName = "TestTakenEmail1",
                Password = "TestTakenEmail1",
                Email = "TestTakenEmail1@string.com",
                Roles = new string[] { "Administrator", "Moderator", "User" }
            };
            var contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);
            responseRegisterUser.EnsureSuccessStatusCode();

            createUser = new RegistrationUserDto()
            {
                UserName = "TestTakenEmail2",
                Password = "TestTakenEmail2",
                Email = "TestTakenEmail1@string.com",
                Roles = new string[] { "Administrator", "Moderator", "User" }
            };
            contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRepeatRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);

            var exceptedErrorMessage = "Email 'TestTakenEmail1@string.com' is already taken.";

            Assert.Contains(exceptedErrorMessage, await responseRepeatRegisterUser.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, responseRepeatRegisterUser.StatusCode);
        }

        [Fact]
        public async void RegisterUser_InvalidRoles_BadRequest()
        {
            var createUser = new RegistrationUserDto()
            {
                UserName = "TestRoles1",
                Password = "TestRoles1",
                Email = "TestRoles1@string.com",
                Roles = new string[] { "Administrators", "InvalidRole" }
            };
            var contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);

            var exceptedErrorMessage = "Invalid roles. Possible roles: Administrator, Moderator and User";

            Assert.Contains(exceptedErrorMessage, await responseRegisterUser.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, responseRegisterUser.StatusCode);
        }

        [Fact]
        public async void RegisterUser_EmptyUsername_BadRequest()
        {
            var createUser = new RegistrationUserDto()
            {
                UserName = "",
                Password = "EmptyUsername",
                Email = "EmptyUsername@string.com",
                Roles = new string[] { "Administrator" }
            };
            var contentRequest = new StringContent(JsonConvert.SerializeObject(createUser),
                Encoding.UTF8, "application/json");

            var responseRegisterUser = await TestFixture.Client.PostAsync($"/api/account/register", contentRequest);

            var exceptedErrorMessage = "Username is required field";

            Assert.Contains(exceptedErrorMessage, await responseRegisterUser.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, responseRegisterUser.StatusCode);
        }
    }
}
