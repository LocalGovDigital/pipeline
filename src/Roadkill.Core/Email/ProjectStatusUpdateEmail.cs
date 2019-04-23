using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Globalization;
using Roadkill.Core.Configuration;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Database;

namespace Roadkill.Core.Email
{
	public class ProjectStatusUpdateEmail : GenericEmailTemplate <EmailProjectStatusUpdateViewModel>
    {
		private static string _htmlContent;
		private static string _plainTextContent;

		public ProjectStatusUpdateEmail(ApplicationSettings applicationSettings, IRepository repository, IEmailClient emailClient)
			: base(applicationSettings, repository, emailClient)
		{
		}

		public override void Send(EmailProjectStatusUpdateViewModel model)
		{
			if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile($"{nameof(EmailProjectStatusUpdateViewModel)}.txt");
			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile($"{nameof(EmailProjectStatusUpdateViewModel)}.html");
            PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;
			base.Send(model);
		}
	}
}
