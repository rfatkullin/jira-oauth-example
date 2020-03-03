using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using System.Collections.Specialized;
using System.Web;

namespace JiraOAuth
{
    public class JiraAuthorizer
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecretInXml;
        private readonly string _authBaseUrl;

        public JiraAuthorizer(string authBaseUrl, string consumerKey, string consumerSecretInPem)
        {
            _authBaseUrl = authBaseUrl;
            _consumerKey = consumerKey;
            _consumerSecretInXml = KeyConverter.PemToXml(consumerSecretInPem);
        }

        public string GetRequestToken()
        {
            var client = new RestClient(_authBaseUrl)
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(_consumerKey,
                    _consumerSecretInXml,
                    OAuthSignatureMethod.RsaSha1),
            };

            var requestTokenRequest = new RestRequest("request-token", Method.POST);

            IRestResponse requestTokenResponse = client.Execute(requestTokenRequest);

            NameValueCollection responseParams = HttpUtility.ParseQueryString(requestTokenResponse.Content);

            return responseParams["oauth_token"];
        }

        public (string, string) GetAccessTokenAndSecret(string requestToken)
        {
            var client = new RestClient(_authBaseUrl)
            {
                Authenticator = OAuth1Authenticator.ForAccessToken(_consumerKey,
                   _consumerSecretInXml,
                   requestToken,
                   null,
                   OAuthSignatureMethod.RsaSha1),
            };

            var accesTokenRequest = new RestRequest("access-token", Method.POST);
            IRestResponse accesTokenResponse = client.Execute(accesTokenRequest);

            NameValueCollection responseParams = HttpUtility.ParseQueryString(accesTokenResponse.Content);

            return (responseParams["oauth_token"], responseParams["oauth_token_secret"]);
        }

        public string ConstructRequestTokenLink(string requestToken)
        {
            return $"{_authBaseUrl}/authorize?oauth_token={requestToken}";
        }
    }
}
