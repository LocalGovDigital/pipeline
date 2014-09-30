﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	/// <summary>
	/// Maps Lightspeed entity classes to the Roadkill domain objects.
	/// </summary>
	/// <remarks>(AutoMapper was tried for this, but had problems with the Mindscape.LightSpeed.Entity class)</remarks>
	public class FromEntity
	{
		public static Page ToPage(PageEntity entity)
		{
			if (entity == null)
				return null;

			Page page = new Page();
			page.Id = entity.Id;
			page.CreatedBy = entity.CreatedBy;
			page.CreatedOn = entity.CreatedOn;
			page.IsLocked = entity.IsLocked;
			page.ModifiedBy = entity.ModifiedBy;
			page.ModifiedOn = entity.ModifiedOn;
			page.Tags = entity.Tags;
			page.Title = entity.Title;
            page.ProjectStart = entity.ProjectStart;
            page.ProjectEnd = entity.ProjectEnd;
            page.ProjectEstimatedTime = entity.ProjectEstimatedTime;
            page.ProjectStatus = entity.ProjectStatus;
            page.ProjectLanguage = entity.ProjectLanguage;
            page.OrgID = entity.OrgID;


			return page;
		}

		public static PageContent ToPageContent(PageContentEntity entity)
		{
			if (entity == null)
				return null;

			PageContent pageContent = new PageContent();
			pageContent.Id = entity.Id;
			pageContent.EditedOn = entity.EditedOn;
			pageContent.EditedBy = entity.EditedBy;
			pageContent.Text = entity.Text;
			pageContent.VersionNumber = entity.VersionNumber;
			pageContent.Page = ToPage(entity.Page);
            pageContent.ProjectStart = entity.ProjectStart;
            pageContent.ProjectEnd = entity.ProjectEnd;
            pageContent.ProjectEstimatedTime = entity.ProjectEstimatedTime;
            pageContent.ProjectStatus = entity.ProjectStatus;
            pageContent.ProjectLanguage = entity.ProjectLanguage;
            pageContent.OrgID = entity.OrgID;

			return pageContent;
		}

		/// <summary>
		/// Intentionally doesn't populate the User.Password property (as this is only ever stored).
		/// </summary>
		public static User ToUser(UserEntity entity)
		{
			if (entity == null)
				return null;

			User user = new User();
			user.Id = entity.Id;
			user.ActivationKey = entity.ActivationKey;
			user.Email = entity.Email;
			user.Firstname = entity.Firstname;
            user.OrgID = entity.OrgID;
			user.IsActivated = entity.IsActivated;
			user.IsAdmin = entity.IsAdmin;
			user.IsEditor = entity.IsEditor;
			user.Lastname = entity.Lastname;
			user.Password = entity.Password;
			user.PasswordResetKey = entity.PasswordResetKey;
			user.Username = entity.Username;
			user.Salt = entity.Salt;

			return user;
		}


        public static Organisation ToOrg(OrgEntity entity)
        {
            if (entity == null)
                return null;

            Organisation org = new Organisation();
            org.Id = entity.Id;
            org.OrgName = entity.OrgName;

            return org;
        }

		public static IEnumerable<PageContent> ToPageContentList(IEnumerable<PageContentEntity> entities)
		{
			List<PageContent> list = new List<PageContent>();
			foreach (PageContentEntity entity in entities)
			{
				PageContent pageContent = ToPageContent(entity);
				list.Add(pageContent);
			}

			return list;
		}

		public static IEnumerable<Page> ToPageList(IEnumerable<PageEntity> entities)
		{
			List<Page> list = new List<Page>();
			foreach (PageEntity entity in entities)
			{
				Page page = ToPage(entity);
				list.Add(page);
			}

			return list;
		}

		public static IEnumerable<User> ToUserList(List<UserEntity> entities)
		{
			List<User> list = new List<User>();
			foreach (UserEntity entity in entities)
			{
				User page = ToUser(entity);
				list.Add(page);
			}

			return list;
		}

        public static IEnumerable<Organisation> ToOrgList(List<OrgEntity> entities)
        {
            List<Organisation> list = new List<Organisation>();
            foreach (OrgEntity entity in entities)
            {
                Organisation org = ToOrg(entity);
                list.Add(org);
            }

            return list;
        }
	}
}
