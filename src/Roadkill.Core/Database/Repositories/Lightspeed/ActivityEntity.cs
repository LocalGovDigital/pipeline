using System;
using Mindscape.LightSpeed;


namespace Roadkill.Core.Database.LightSpeed
{
    [Table("pipeline_relationship", IdentityMethod = IdentityMethod.IdentityColumn)]
    [Cached(ExpiryMinutes = 1)]
    public class ActivityEntity : Entity<int>
    {
        private int _id;

        private string _userNames;

        private string _orgName;

        private string _projectName;

        private string _activityName;

        private DateTime _activityDateTime; 

        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                Set<int>(ref _id, value);
            }
        }

        public string userNames
        {
            get
            {
                return _userNames;
            }
            set
            {
                Set<string>(ref _userNames, value);
            }
        }

        public string orgName
        {
            get
            {
                return _orgName;
            }
            set
            {
                Set<string>(ref _orgName, value);
            }
        }

        public string projectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                Set<string>(ref _projectName, value);
            }
        }

        public string activityName
        {
            get
            {
                return _activityName;
            }
            set
            {
                Set<string>(ref _activityName, value);
            }
        }

        public DateTime activityDateTime
        {
            get
            {
                return _activityDateTime;
            }
            set
            {
                Set<DateTime>(ref _activityDateTime, value);
            }
        }




    }
}
