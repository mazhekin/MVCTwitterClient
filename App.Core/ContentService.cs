using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetSharp;
using Hammock.Authentication.OAuth;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace App.Core
{
    public interface IContentService
    {
        string GetRequestTokenXml();

        string GetAuthUrl(string requestTokenXml);

        OAuthAccessToken GetAccessToken(string requestTokenXml, string verifier);
        OAuthAccessToken GetAccessToken(string accessTokenXml);

        IEnumerable<TwitterStatus> GetTweets(OAuthAccessToken accessToken, int page, int pageSize);

        TwitterSearchResult Search(string q, int page, int pageSize);
        IEnumerable<TwitterUser> SearchForUsers(OAuthAccessToken accessToken, string q, int page, int pageSize);
    }

    public class ContentService : IContentService
    {
        private readonly string consumerKey = "slCCLcgj5V7AYiCvaFa4hQ";
        private readonly string consumerSecret = "FYmJOrrHr1dvV2T77uws2tIonF4UqGlOdonfkjLOaU";
        private readonly TwitterService twitterService;

        public ContentService()
        {
            this.twitterService = new TwitterService(consumerKey, consumerSecret);
        }

        string IContentService.GetRequestTokenXml()
        {
            var requestToken = this.twitterService.GetRequestToken();
            return (requestToken as Object).Serialize<OAuthRequestToken>();
        }

        string IContentService.GetAuthUrl(string requestTokenXml)
        {
            var requestToken = requestTokenXml.Deserialize<OAuthRequestToken>();
            var authUri = twitterService.GetAuthorizationUri(requestToken);
            return authUri.AbsoluteUri;
        }

        OAuthAccessToken IContentService.GetAccessToken(string requestTokenXml, string verifier)
        {
            var requestToken = requestTokenXml.Deserialize<OAuthRequestToken>();
            return this.twitterService.GetAccessToken(requestToken, verifier);
        }

        OAuthAccessToken IContentService.GetAccessToken(string accessTokenXml)
        {
            try
            {
                return accessTokenXml.Deserialize<OAuthAccessToken>();
            }
            catch 
            {
                return null;
            }
        }

        IEnumerable<TwitterStatus> IContentService.GetTweets(OAuthAccessToken accessToken, int page, int pageSize)
        {
            if (accessToken == null)
            {
                return new List<TwitterStatus>();
            }

            this.twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            var tweets = this.twitterService.ListTweetsOnHomeTimeline(page, pageSize);
            return tweets;
        }

        TwitterSearchResult IContentService.Search(string q, int page, int pageSize)
        {
            return this.twitterService.Search(q, page, pageSize);
        }

        IEnumerable<TwitterUser> IContentService.SearchForUsers(OAuthAccessToken accessToken, string q, int page, int pageSize)
        {
            this.twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            return this.twitterService.SearchForUser(q, page, pageSize);
        }
    }
}
