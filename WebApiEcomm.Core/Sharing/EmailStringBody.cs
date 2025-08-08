using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Sharing
{
    public class EmailStringBody
    {
        public static string Send(string email , string token , string Component)
        {
            string encodedToken = Uri.EscapeDataString(token);

            return $@"
            <html>
            <head>
                <title>Email Verification</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <h1 style='color: #333;'>Welcome!</h1>
                <p>Thank you for registering with us. Please verify your email address to complete the registration process.</p>
                <p>Click the link below to verify your email:</p>
                <p>
                    <a href='https://localhost:5001/api/auth/verify-email?email={Uri.EscapeDataString(email)}&token={encodedToken}'
                       style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>
                       Verify Email
                    </a>
                </p>
                <p>If the link does not work, please copy and paste the following URL into your browser:</p>
                <p>
                    https://localhost:5001/api/auth/verify-email?email={Uri.EscapeDataString(email)}&token={encodedToken}
                </p>
                <p>If you did not register for an account, please ignore this email.</p>
                <p>Thank you for choosing Our Website!</p>
            </body>
            </html>";
        }
    }
}
