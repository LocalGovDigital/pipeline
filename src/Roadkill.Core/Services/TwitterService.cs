using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinqToTwitter;
using Roadkill.Core.Configuration;
using StructureMap;

namespace Roadkill.Core.Services
{
    public class TwitterService : ITwitterService
    {
        private SiteSettings _siteSettings;

        public SiteSettings SiteSettings
        {
            get
            {
                if (_siteSettings == null)
                    _siteSettings = ObjectFactory.GetInstance<SettingsService>().GetSiteSettings();

                return _siteSettings;
            }
        }

        private string GetShortUrl(string ProjectUrl)
        {
            string ShortUrl = "";

            if (!String.IsNullOrWhiteSpace(SiteSettings.BitlyAccessToken) && !String.IsNullOrWhiteSpace(ProjectUrl))
            {
                try
                {
                    using (var hc = new System.Net.Http.HttpClient())
                    {
                        hc.BaseAddress = new Uri("https://api-ssl.bitly.com");

                        ShortUrl = hc.GetStringAsync("/v3/shorten?access_token=" + _siteSettings.BitlyAccessToken + "&format=txt&longUrl=" + System.Uri.EscapeUriString(ProjectUrl)).Result;
                    }
                }
                catch { }
            }

            return ShortUrl;
        }

        public virtual void TweetNewProject(string ProjectName, string ProjectUrl)
        {
            if (SiteSettings.EnableTweetingOfNewProjects)
            {
                var Message = SiteSettings.NewProjectTweetTemplate;
                var ShortUrl = GetShortUrl(ProjectUrl);                

                // Definitely need to come back and look at this...
                Message = Message.Replace("{url}", ShortUrl);
                int AvailableSpace = Message.Length - (Message.Contains("{name}") ? 6 : 0);
                Message = Message.Replace("{name}", ProjectName.Substring(0, AvailableSpace));

                Tweet(Message);
            }
        }

        public virtual void TweetProjectUpdated(string ProjectName, string ProjectUrl)
        {
            if (SiteSettings.EnableTweetingOfEditProjects)
            {
                var Message = SiteSettings.EditProjectTweetTemplate;
                var ShortUrl = GetShortUrl(ProjectUrl);

                // Definitely need to come back and look at this...
                Message = Message.Replace("{url}", ShortUrl);
                int AvailableSpace = Message.Length - (Message.Contains("{name}") ? 6 : 0);
                Message = Message.Replace("{name}", ProjectName.Substring(0, AvailableSpace));

                Tweet(Message);
            }
        }

        public virtual void TweetNewProjectActivity(string ProjectName, string ProjectUrl, string Activity)
        {
            if (SiteSettings.EnableTweetingOfProjectActivity)
            {
                var Message = SiteSettings.NewProjectActivityTweetTemplate;
                var ShortUrl = GetShortUrl(ProjectUrl);

                // Definitely need to come back and look at this...
                Message = Message.Replace("{activity}", Activity.ToLower());
                Message = Message.Replace("{url}", ShortUrl);
                int AvailableSpace = Message.Length - (Message.Contains("{name}") ? 6 : 0);
                Message = Message.Replace("{name}", ProjectName.Substring(0, AvailableSpace));

                Tweet(Message);
            }
        }

        private void Tweet(string Message)
        {
            if (String.IsNullOrWhiteSpace(Message))
            {
                return;
            }

            var TwitterAuth = new SingleUserAuthorizer
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = _siteSettings.TwitterConsumerKey,
                        ConsumerSecret = _siteSettings.TwitterConsumerSecret,
                        OAuthToken = _siteSettings.TwitterOAuthToken,
                        OAuthTokenSecret = _siteSettings.TwitterOAuthTokenSecret
                    }
                };

            using (var TwitterCtx = new LinqToTwitter.TwitterContext(TwitterAuth))
            {
                var Status = TwitterCtx.TweetAsync(Message).Result;
            }
        }
    }
}
