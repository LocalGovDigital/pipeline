﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	/// <summary>
	/// Maps a Roadkill domain objects (or list of) to a Lightspeed entity classes to the.
	/// These methods deliberatly don't return a new <see cref="Entity"/> as that causes its 
	/// state to be marked as New, which can have side effects.
	/// </summary>
	public class ToEntity
	{
		public static void FromUser(User user, UserEntity entity)
		{
			entity.ActivationKey = user.ActivationKey;
			entity.Email = user.Email;
			entity.Firstname = user.Firstname;
            entity.orgID = user.orgID;
            entity.EmailSubscriber = user.EmailSubscriber;
			entity.IsActivated = user.IsActivated;
			entity.IsAdmin = user.IsAdmin;
			entity.IsEditor = user.IsEditor;
			entity.Lastname = user.Lastname;
			entity.Password = user.Password;
			entity.PasswordResetKey = user.PasswordResetKey;
			entity.Salt = user.Salt;
			entity.Username = user.Username;
		}

		public static void FromPage(Page page, PageEntity entity)
		{
			entity.CreatedBy = page.CreatedBy;
			entity.CreatedOn = page.CreatedOn;
			entity.IsLocked = page.IsLocked;
			entity.ModifiedOn = page.ModifiedOn;
			entity.ModifiedBy = page.ModifiedBy;
            entity.ProjectStart = page.ProjectStart;
            entity.ProjectEnd = page.ProjectEnd;
            entity.ProjectEstimatedTime = page.ProjectEstimatedTime;
            entity.ProjectStatus = page.ProjectStatus;
            entity.ProjectLanguage = page.ProjectLanguage;
            entity.orgID = page.orgID;
			entity.Tags = page.Tags;
			entity.Title = page.Title;

		    entity.Owner = page.Owner;
		    entity.OwnerEmail = page.OwnerEmail;
		    entity.Department = page.Department;
            entity.ProjectAgileLifeCyclePhase = page.ProjectAgileLifeCyclePhase;
		    entity.CollaborationLevel = page.CollaborationLevel;
		    entity.FundingBoundary = page.FundingBoundary;
        }

		public static void FromPageContent(PageContent pageContent, PageContentEntity entity)
		{
			entity.EditedOn = pageContent.EditedOn;
			entity.EditedBy = pageContent.EditedBy;
			entity.Text = pageContent.Text;
			entity.VersionNumber = pageContent.VersionNumber;
		}

        public static void FromRelationship(Relationship rel, RelEntity entity)
        {
            entity.id = rel.id;
            entity.username = rel.username;
            entity.orgID = rel.orgID;
            entity.pageId = rel.pageId;
            entity.relText = rel.relText;
            entity.relTypeId = rel.relTypeId;

        }
	}
}
