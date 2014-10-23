using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Roadkill.Core.Localization;
using Roadkill.Core.Database;
using Roadkill.Core.Text;
using Roadkill.Core.Converters;
using Roadkill.Core.Database.LightSpeed;
using Roadkill.Core.Mvc.Controllers.Api;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Roadkill.Core.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net.Http;
using Roadkill.Core.Mvc.ViewModels;
using System.Web.Configuration;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using Roadkill.Core.Services;
using StructureMap;
using StructureMap.Attributes;



namespace Roadkill.Core.Mvc.ViewModels
{


    public class Activity
    {
        /// <summary>
        /// The user's id
        /// </summary>
        public int id { get; set; }

        public string userNames { get; set; }

        public string orgName { get; set; }

        public int projectId { get; set; }

        public string projectName { get; set; }

        public string activityName { get; set; }

        public DateTime activityDateTime { get; set; }

    }

    /// <summary>
    /// Provides a data summary class for creating and saving relationship details.
    /// </summary>
    public class ActivityViewModel
    {

        /// <summary>
        /// The user's id
        /// </summary>
        public int id { get; set; }

        public string userNames { get; set; }

        public string orgName { get; set; }

        public int projectId { get; set; }

        public string projectName { get; set; }

        public string activityName { get; set; }

        public DateTime activityDateTime { get; set; }


        /// <summary>
        /// Constructor used by none-controllers
        /// </summary>
        public ActivityViewModel()
        {
           
        }


        /// <summary>
        /// Constructor used by none-controllers
        /// </summary>
        public ActivityViewModel(Activity act)
        {

            id = act.id;
            userNames = act.userNames;
            orgName = act.orgName;
            projectId = act.projectId;
            projectName = act.projectName;
            activityName = act.activityName;
            activityDateTime = act.activityDateTime;


        }


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public IEnumerable<Activity> ActivityViewList
        {
            get
            {

                LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

                IEnumerable<Relationship> RelList;
                RelList = repository.FindAllRels();

                List<Activity> items = new List<Activity>();

                foreach (Relationship rel in RelList)
                {

                    Activity item = new Activity();
                    item.userNames = GetUser(rel.username);                    
                    item.activityName = GetRelType(rel.relTypeId);
                    item.activityDateTime = rel.relDateTime;

                    //get the page the relationship is related to
                    Page page = new Page();
                    page = repository.GetPageById(rel.pageId);
                    item.projectName = page.Title;

                    //get the orgainsation that owns the page
                    Organisation org = new Organisation();
                    org = repository.GetOrgByID(page.orgID);
                    item.orgName = org.OrgName;


                    items.Add(item);
                }

                return items;
            }
        }




        /// <summary>
        /// Returns the user name
        /// </summary>
        public string GetUser(string username)
        {

            string _usernames = "unknown";
            try
            {

                LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
                User _user;
                _user = repository.GetUserByUsername(username);
                _usernames = _user.Firstname + " " + _user.Lastname;
            }
            catch { }

            return _usernames;

        }


        /// <summary>
        /// Returns the relationship type
        /// </summary>
        public string GetRelType(int relTypeID)
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

            RelationshipType _reltype;
            _reltype = repository.GetRelTypeByID(relTypeID);

            return _reltype.relTypeText;

        }


        private static ApplicationSettings GetAppSettings()
        {

            ApplicationSettings appSettings = ObjectFactory.GetInstance<ApplicationSettings>();
            return appSettings;

        }






















    }
}
