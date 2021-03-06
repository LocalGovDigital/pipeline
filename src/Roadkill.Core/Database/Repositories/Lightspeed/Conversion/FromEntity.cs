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
		//public static Page ToPage(PageEntity entity)
		//{
		//	if (entity == null)
		//		return null;

		//    Page page = Page.FromPageEntity(entity);
		//	return page;
		//}

		//public static PageContent ToPageContent(PageContentEntity entity)
		//{
		//	if (entity == null)
		//		return null;

		//	PageContent pageContent =  PageContent.FromPageContentEntity(entity);
			

		//	return pageContent;
		//}

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
            user.orgID = entity.orgID;
            user.EmailSubscriber = entity.EmailSubscriber;
			user.IsActivated = entity.IsActivated;
			user.IsAdmin = entity.IsAdmin;
			user.IsEditor = entity.IsEditor;
			user.Lastname = entity.Lastname;
			user.Password = entity.Password;
			user.PasswordResetKey = entity.PasswordResetKey;
			user.Username = entity.Username;
			user.Salt = entity.Salt;
            user.createdOn = entity.createdOn;

			return user;
		}


        public static Organisation ToOrg(OrgEntity entity)
        {
            if (entity == null)
                return null;

            Organisation org = new Organisation();
            org.Id = entity.Id;
            org.OrgName = entity.OrgName;
            org.email = entity.email;
            org.url = entity.url;
            org.twitter = entity.twitter;

            return org;
        }


        public static Relationship ToRel(RelEntity entity)
        {
            if (entity == null)
                return null;

            Relationship rel = new Relationship();
            rel.id = entity.Id;
            rel.orgID = entity.orgID;
            rel.username = entity.username;
            rel.pageId = entity.pageId;
            rel.relTypeId = entity.relTypeId;
            rel.relText = entity.relText;
            rel.relDateTime = entity.relDateTime;
            rel.approved = entity.Approved;
            rel.pending = entity.Pending;
            rel.userid = entity.UserId;

            return rel;
        }


        public static RelationshipType ToRelType(RelTypeEntity entity)
        {
            if (entity == null)
                return null;

            RelationshipType reltype = new RelationshipType();
            reltype.id = entity.Id;
            reltype.relTypeText = entity.relTypeText;

            return reltype;
        }


		public static IEnumerable<PageContent> ToPageContentList(IEnumerable<PageContentEntity> entities)
		{
			List<PageContent> list = new List<PageContent>();
			foreach (PageContentEntity entity in entities)
			{
				PageContent pageContent = PageContent.FromPageContentEntity(entity);
				list.Add(pageContent);
			}

			return list;
		}

		public static IEnumerable<Page> ToPageList(IEnumerable<PageEntity> entities)
		{
			List<Page> list = new List<Page>();
			foreach (PageEntity entity in entities)
			{
				Page page = Page.FromPageEntity(entity);
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

        public static List<Relationship> ToRelList(List<RelEntity> entities)
        {
            List<Relationship> list = new List<Relationship>();
            foreach (RelEntity entity in entities)
            {
                Relationship pageContent = ToRel(entity);
                list.Add(pageContent);
            }

            return list;
        }

        public static List<RelationshipType> ToRelTypeList(List<RelTypeEntity> entities)
        {
            List<RelationshipType> list = new List<RelationshipType>();
            foreach (RelTypeEntity entity in entities)
            {
                RelationshipType pageContent = ToRelType(entity);
                list.Add(pageContent);
            }

            return list;
        }


}
}
