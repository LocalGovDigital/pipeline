﻿using System;
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
    /// <summary>
    /// Provides a data summary class for creating and saving relationship details.
    /// </summary>
    public class RelViewModel
    {

        /// <summary>
        /// The user's id
        /// </summary>
        public int id { get; set; }

        public string userName { get; set; }

        public int orgID { get; set; }

        public int pageID { get; set; }

        public int relTypeID { get; set; }

        public string reltext { get; set; }

        public DateTime reldatetime { get; set; }

        public string reltypetext { get; set; }

        public string orgtext { get; set; }

        public string usertext { get; set; }

        public string PageTitle { get; set; }

        public string PageUrl { get; set; }

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
            if (id == null)
                throw new ArgumentNullException("rel");

            id = rel.id;
            userName = rel.username;
            orgID = rel.orgID;
            pageID = rel.pageId;
            relTypeID = rel.relTypeId;
            reltext = rel.relText;
            reldatetime = rel.relDateTime;
            reltypetext = GetRelType(relTypeID);
            orgtext = GetOrg(rel.orgID);
            usertext = GetUser(rel.username);
        }


        /// <summary>
        /// Gets an IEnumerable{SelectListItem} of project statuses, as a default
        /// SelectList doesn't add option value attributes.
        /// </summary>
        public List<SelectListItem> RelTypesAsSelectList
        {
            get
            {

                LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());

                IEnumerable<RelationshipType> RelList;
                RelList = repository.FindAllRelTypes();

                List<SelectListItem> items = new List<SelectListItem>();

                foreach (RelationshipType rel in RelList)
                {

                    SelectListItem item = new SelectListItem();
                    item.Text = rel.relTypeText.ToString();
                    item.Value = rel.id.ToString();

                    if (rel.id == relTypeID)
                    { item.Selected = true; }

                    items.Add(item);
                }

                return items;
            }
        }

        /// <summary>
        /// Returns the organisation name
        /// </summary>
        public string GetOrg(int orgID)
        {

            string _orgname = "unknown";
            try
            {

                LightSpeedRepository repository = new LightSpeedRepository(GetAppSettings());
                Organisation _org;
                _org = repository.GetOrgByID(orgID);
                _orgname = _org.OrgName;
            }
            catch { }

            return _orgname;

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
        /// Returns the organisation name
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
