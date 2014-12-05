using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Services
{
    public interface ITwitterService
    {
        void TweetNewProject(string ProjectName, string ProjectUrl);

        void TweetProjectUpdated(string ProjectName, string ProjectUrl);

        void TweetNewProjectActivity(string ProjectName, string ProjectUrl, string Activity);
    }
}
