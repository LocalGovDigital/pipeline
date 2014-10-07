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



namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides a data summary class for creating and saving relationship details.
    /// </summary>
    public class RelViewModel
    {

        /// <summary>
        /// The user's id
        /// </summary>
        public int Id { get; set; }

        public string userName { get; set; }

        public int orgID { get; set; }

        public int pageID { get; set; }

        public int relTypeID { get; set; }

        public string reltext { get; set; }

        public DateTime reldatetime { get; set; }



        /// <summary>
        /// Constructor used by none-controllers
        /// </summary>
        public RelViewModel()
        {
        }

        /// <summary>
        /// Takes all properties on <see cref="Rel"/> and fills them on in the RelViewModel
        /// </summary>
        public RelViewModel(Relationship rel)
        {
            if (Id == null)
                throw new ArgumentNullException("rel");

            Id = rel.Id;
            userName = rel.username;
            orgID = rel.orgId;
            pageID = rel.pageId;
            relTypeID = rel.relTypeId;
            reltext = rel.relText;
            reldatetime = rel.relDateTime;
        }

        /// <summary>
        /// Returns the organisation name
        /// </summary>
        public string GetOrg(int orgID)
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

            Organisation _org;
            _org = repository.GetOrgByID(orgID);

            return _org.OrgName;

        }


        /// <summary>
        /// Returns the organisation name
        /// </summary>
        public string GetRelType(int relTypeID)
        {

            LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

            RelationshipType _reltype;
            _reltype = repository.GetRelTypeByID(relTypeID);

            return _reltype.relTypeText;

        }


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public static List<SelectListItem> RelTypesAsNewSelectList()
        {
         

                LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

                IEnumerable<RelationshipType> RelList;
                RelList = repository.FindAllRelTypes();

                List<SelectListItem> items = new List<SelectListItem>();

                foreach (RelationshipType reltype in RelList)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = reltype.relTypeText.ToString();
                    item.Value = reltype.Id.ToString();

                    items.Add(item);
                }

                items.Sort((x, y) => string.Compare(x.Text, y.Text));

                return items;
          

        }


 
 
        private static ApplicationSettings GetAppSettings()
        {

            ApplicationSettings appSettings = new ApplicationSettings();

            appSettings.DataStoreType = DataStoreType.Sqlite;
            appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
            appSettings.LoggingTypes = "none";
            appSettings.UseBrowserCache = false;

            return appSettings;

        }






















    }
}
