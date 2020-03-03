# jira-oauth-example
.Net Core (2.1) Jira OAuth 1.0 authorization example.

### Settings
Console app needs settings file *appsettings.json* with next structure:
```javascript
{
  "AuthorizationSettings": {
    "BaseUrl": "<JIRA_HOST_ADDRESS>",
    "AuthorizationBaseUrl": "<JIRA_HOST_ADDRESS>/plugins/servlet/oauth/",
    "ConsumerKey": "<CONSUMER_KEY>",
    "ConsumerSecretFilePath": "<PATH_TO_FILE_WITH_KEY_IN_PEM>"
  }
}
```
