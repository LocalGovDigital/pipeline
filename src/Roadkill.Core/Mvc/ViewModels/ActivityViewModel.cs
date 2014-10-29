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
        /// Gets the number of organisations signed up to Pipeline
        /// </summary>
        public static int OrganisationCount()
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
            return repository.AllOrgsCount();

        }



        /// <summary>
        /// Gets the number of organisations signed up to Pipeline
        /// </summary>
        public static int PageCount()
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
            return repository.AllPagesCount();

        }


        /// <summary>
        /// Gets the number of organisations signed up to Pipeline
        /// </summary>
        public static int UsersCount()
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
            return repository.AllUsersCount();

        }


        /// <summary>
        /// Gets the number of organisations signed up to Pipeline
        /// </summary>
        public static IEnumerable<Activity> WhatsHotList()
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
            return repository.WhatsHotList();

        }




        private static ApplicationSettings GetAppSettings()
        {

            ApplicationSettings appSettings = ObjectFactory.GetInstance<ApplicationSettings>();
            return appSettings;

        }


    }
}






















