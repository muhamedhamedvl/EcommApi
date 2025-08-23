using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WebApiEcomm.Core.Entites.Identity;
using WebApiEcomm.InfraStructure.Repositores.Service;

namespace WebApiEcomm.UnitTests.InfraStructure.Services
{
    [TestFixture]
    public class GenrateTokenTests
    {
        private GenrateToken _tokenService;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Token:Secret", "ThisIsASecretKeyForUnitTesting12345" },
                { "Token:Issuer", "TestIssuer" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new GenrateToken(configuration);
        }

        [Test]
        public void GetAndCreateTokenAsync_ShouldReturn_ValidJwtToken()
        {
            // Arrange
            var user = new AppUser
            {
                UserName = "testUser",
                Email = "test@example.com"
            };

            // Act
            var token = _tokenService.GetAndCreateTokenAsync(user);

            // Assert
            Assert.That(token, Is.Not.Null.And.Not.Empty, "Token should not be null or empty");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var nameClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Name || c.Type.Contains("name"));
            var emailClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Email || c.Type.Contains("email"));

            Assert.That(nameClaim, Is.Not.Null, "Name claim should exist");
            Assert.That(nameClaim.Value, Is.EqualTo("testUser"));

            Assert.That(emailClaim, Is.Not.Null, "Email claim should exist");
            Assert.That(emailClaim.Value, Is.EqualTo("test@example.com"));
        }
    }
}

