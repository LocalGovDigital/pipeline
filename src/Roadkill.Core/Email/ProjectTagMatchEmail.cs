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
	public class ProjectTagMatchEmail : GenericEmailTemplate <ProjectTagMatchEmailViewModel>
    {
		private static string _htmlContent;
		private static string _plainTextContent;

		public ProjectTagMatchEmail(ApplicationSettings applicationSettings, IRepository repository, IEmailClient emailClient)
			: base(applicationSettings, repository, emailClient)
		{
		}

		public override void Send(ProjectTagMatchEmailViewModel model)
		{
			if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile($"{nameof(ProjectTagMatchEmailViewModel)}.txt");
			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile($"{nameof(ProjectTagMatchEmailViewModel)}.html");
            PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;
			base.Send(model);
		}
	}
}
