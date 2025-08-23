using NUnit.Framework;
using NUnit.Framework.Legacy;
using WebApiEcomm.Core.Sharing;

namespace WebApiEcomm.UnitTests.Core
{
    [TestFixture]
    public class EmailStringBodyTests
    {
        [Test]
        public void Send_ShouldContainEncodedEmailAndToken()
        {
            // Arrange
            string email = "test@example.com";
            string token = "my token with spaces";
            string component = "unused"; 

            // Act
            string result = EmailStringBody.Send(email, token, component);

            // Assert
            StringAssert.Contains(Uri.EscapeDataString(email), result, "Email should be encoded in HTML output.");
            StringAssert.Contains(Uri.EscapeDataString(token), result, "Token should be encoded in HTML output.");
        }

        [Test]
        public void Send_ShouldContainVerificationUrl()
        {
            // Arrange
            string email = "user@domain.com";
            string token = "secureToken123";
            string component = "x";

            // Act
            string result = EmailStringBody.Send(email, token, component);

            // Assert
            string expectedUrl = $"https://localhost:5001/api/auth/verify-email?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
            StringAssert.Contains(expectedUrl, result, "Verification URL should be correctly formatted.");
        }

        [Test]
        public void Send_ShouldContainHtmlStructure()
        {
            // Arrange
            string email = "check@site.com";
            string token = "token";
            string component = "c";

            // Act
            string result = EmailStringBody.Send(email, token, component);

            // Assert
            StringAssert.Contains("<html>", result);
            StringAssert.Contains("<body", result);
            StringAssert.Contains("Verify Email", result);
            StringAssert.Contains("Thank you for choosing Our Website!", result);
        }
    }
}
