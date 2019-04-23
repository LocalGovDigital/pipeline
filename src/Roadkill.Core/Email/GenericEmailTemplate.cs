using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using Roadkill.Core.Configuration;
using System.Configuration;
using Roadkill.Core.Mvc.ViewModels;
using System.IO;
using System.Globalization;
using System.Reflection;
using Roadkill.Core.Database;

namespace Roadkill.Core.Email
{
    public abstract class GenericEmailTemplate<T> where T : EmailViewModel
    {
        protected ApplicationSettings ApplicationSettings;
        protected SiteSettings SiteSettings;
        protected IEmailClient EmailClient;
        protected IRepository Repository;

        public string HtmlView { get; set; }

        public string PlainTextView { get; set; }

        protected GenericEmailTemplate(ApplicationSettings applicationSettings, IRepository repository, IEmailClient emailClient)
        {
            if (applicationSettings == null)
                throw new ArgumentNullException("applicationSettings");

            if (repository == null)
                throw new ArgumentNullException("repository");

            ApplicationSettings = applicationSettings;
            Repository = repository;

            EmailClient = emailClient;
            if (EmailClient == null)
                EmailClient = new EmailClient();
        }

        /// <summary>
        /// Sends a notification email to the provided address, using the template provided.
        /// </summary>
        public virtual void Send(T model)
        {
            if (string.IsNullOrEmpty(model.ToAddress))
                throw new EmailException(null, "No email address", GetType().Name);

            if (string.IsNullOrEmpty(PlainTextView))
                throw new EmailException(null, "No plain text view can be found for {0}", GetType().Name);


            if (string.IsNullOrEmpty(HtmlView))
                throw new EmailException(null, "No HTML view can be found for {0}", GetType().Name);

            string plainTextContent = ReplaceTokens(model, PlainTextView);
            string htmlContent = ReplaceTokens(model, HtmlView);

            string emailTo = model.ToAddress;
            
            if (ConfigurationManager.AppSettings.AllKeys.Contains("environment"))
            {
                var environmentPrefix = ConfigurationManager.AppSettings["environment"];
                if (environmentPrefix == "DEV")
                {
                    var allowedEmails = ConfigurationManager.AppSettings["allowedEmailAddresses"];
                    var allowedEmailList = allowedEmails.Split(';');
                    if (!allowedEmailList.Contains(emailTo.ToLower()))
                    {
                        return;
                    }
                }
            }

            if (string.IsNullOrEmpty(emailTo))
                throw new EmailException(null, "The Model has an empty current or new email address");

            MailMessage message = new MailMessage();
            message.To.Add(emailTo);
            message.Subject = model.Subject;
            message.Body = htmlContent;
            message.IsBodyHtml = true;

            if (EmailClient.GetDeliveryMethod() == SmtpDeliveryMethod.SpecifiedPickupDirectory &&
                !string.IsNullOrEmpty(EmailClient.PickupDirectoryLocation) &&
                EmailClient.PickupDirectoryLocation.StartsWith("~"))
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string pickupRoot = EmailClient.PickupDirectoryLocation.Replace("~/", root);
                pickupRoot = pickupRoot.Replace("/", @"\");
                EmailClient.PickupDirectoryLocation = pickupRoot;
            }

            EmailClient.Send(message);
        }

        protected internal string ReadTemplateFile(string filename)
        {
            string templatePath = ApplicationSettings.EmailTemplateFolder;
            string textfilePath = Path.Combine(templatePath, filename);
            string culturePath = Path.Combine(templatePath, CultureInfo.CurrentUICulture.Name);

            // If there's templates for the current culture, then use those instead
            if (Directory.Exists(culturePath))
            {
                string culturePlainTextFile = Path.Combine(culturePath, filename);
                if (File.Exists(culturePlainTextFile))
                    textfilePath = culturePlainTextFile;
            }

            return File.ReadAllText(textfilePath);
        }

        protected internal virtual string ReplaceTokens(T model, string template)
        {
            if (SiteSettings == null)
                SiteSettings = Repository.GetSiteSettings();
            string result = template;

            var properties = model.GetType().GetProperties().Select(x => new KeyValuePair<string, string>(x.Name, x.GetValue(model).ToString()));

            foreach (var prop in properties)
            {
                result = result.Replace($"{{{prop.Key}}}", prop.Value);
            }
            result = result.Replace("{SITEURL}", SiteSettings.SiteUrl);
            result = result.Replace("{SITENAME}", SiteSettings.SiteName);

            if (HttpContext.Current != null)
                result = result.Replace("{REQUEST_IP}", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return result;
        }
    }
}
