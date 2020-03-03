namespace JiraOAuth
{
    public class AuthorizationSettings
    {
        /// <summary>
        /// Jira base url.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Authorization base url
        /// For example, http://HOST_ADDRESS/plugins/servlet/oauth/
        /// </summary>
        public string AuthorizationBaseUrl { get; set; }

        /// <summary>
        /// Just consumer key. You can get it in Jira settings page.
        /// In Apps link section.
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Path to private key file in PEM format.
        /// File must contains just key without any headers or prefix rows.        
        /// </summary>
        public string ConsumerSecretFilePath { get; set; }
    }
}
