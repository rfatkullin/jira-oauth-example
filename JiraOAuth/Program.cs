using Atlassian.Jira;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace JiraOAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load configs
            AuthorizationSettings settings = GetSettings();
            string consumerSecretInPem = LoadPrivateKey(settings.ConsumerSecretFilePath);

            var authorizer = new JiraAuthorizer(settings.AuthorizationBaseUrl, settings.ConsumerKey, consumerSecretInPem);
            
            // Getting request token and link for it authorization
            string requestToken = authorizer.GetRequestToken();
            string requestTokenAuthorizationLink = authorizer.ConstructRequestTokenLink(requestToken);

            // Here you need to copy this link, open it in a browser.
            // Jira ask you to authorize request token
            // After that press any key to continue
            Console.WriteLine(requestTokenAuthorizationLink);

            Console.WriteLine("Authorize request token and press any key...");
            Console.ReadKey();
            Console.WriteLine();

            // Getting access token and access token secret
            (string accessToken, string accessTokenSecret) = authorizer.GetAccessTokenAndSecret(requestToken);

            Console.WriteLine($"Access token: {accessToken}{Environment.NewLine}Access token secret: {accessTokenSecret}");

            // Now we can access to resources with access token
            Jira client = Jira.CreateOAuthRestClient(settings.BaseUrl,
                    settings.ConsumerKey,
                    KeyConverter.PemToXml(consumerSecretInPem),
                    accessToken,
                    accessTokenSecret,
                    Atlassian.Jira.OAuth.JiraOAuthSignatureMethod.RsaSha1);

            // Print email of authorized user
            Console.WriteLine(client.Users.GetMyselfAsync().Result.Email);
        }

        private static AuthorizationSettings GetSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return config.GetSection("AuthorizationSettings").Get<AuthorizationSettings>();
        }

        private static string LoadPrivateKey(string pathToFile)
        {
            return File.ReadAllText(pathToFile);
        }
    }
}
