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

namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides a data summary class for creating and saving user details.
    /// </summary>
    public class OrgViewModel
    {

        /// <summary>
        /// The user's id
        /// </summary>
        public int Id { get; set; }

        public string OrgName { get; set; }

        /// <summary>
        /// Constructor used by none-controllers
        /// </summary>
        public OrgViewModel()
        {
        }

        /// <summary>
        /// Takes all properties on <see cref="Org"/> and fills them on in the OrgViewModel
        /// </summary>
        public OrgViewModel(Organisation org)
        {
            if (org == null)
                throw new ArgumentNullException("org");

            Id = org.Id;
            OrgName = org.OrgName;
        }

        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public List<SelectListItem> OrgsAsSelectList
        {
            get
            {

                ApplicationSettings appSettings = new ApplicationSettings();
                appSettings.DataStoreType = DataStoreType.Sqlite;
                appSettings.ConnectionString = "Data Source=|DataDirectory|\roadkill.sqlite;";
                appSettings.LoggingTypes = "none";
                appSettings.UseBrowserCache = false;
                //Log.ConfigureLogging(appSettings);

                LightSpeedRepository repository = new LightSpeedRepository(appSettings);

                IEnumerable<Organisation> OrgList;
                OrgList = repository.FindAllOrgs();

                List<SelectListItem> items = new List<SelectListItem>();

                foreach (Organisation org in OrgList)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = org.OrgName.ToString();
                    item.Value = org.ObjectId.ToString();

                    if (org.Id == Id)
                    { item.Selected = true; }

                    items.Add(item);
                }

                return items;
            }
        }






















    }
}
