using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("roadkill_pagecontent")]
	public class PageContentEntity : Entity<Guid>
	{
		[Column("text")]
		private string _text;

		[Column("editedby")]
		private string _editedBy;

		[Column("editedon")]
		private DateTime _editedOn;

		[Column("versionnumber")]
		private int _versionNumber;

        [Column("projectstart")]
        private DateTime _projectStart;

        [Column("projectend")]
        private DateTime _projectEnd;

        [Column("projectEstimatedTime")]
        private bool _projectEstimatedTime;

        [Column("projectLanguage")]
        private string _projectLanguage;

        [Column("projectStatus")]
        private string _projectStatus;

		[ReverseAssociation("PageId")]
		private readonly EntityHolder<PageEntity> _page = new EntityHolder<PageEntity>();
		private int _pageId;

		public PageEntity Page
		{
			get
			{
				return Get(_page);
			}
			set
			{
				Set(_page, value);
			}
		}

		public int PageId
		{
			get { return Get(ref _pageId, "PageId"); }
			set { Set(ref _pageId, value, "PageId"); }
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				Set<string>(ref _text, value);
			}
		}

		public string EditedBy
		{
			get
			{
				return _editedBy;
			}
			set
			{
				Set<string>(ref _editedBy, value);
			}
		}

		public DateTime EditedOn
		{
			get
			{
				return _editedOn;
			}
			set
			{
				Set<DateTime>(ref _editedOn, value);
			}
		}

		public int VersionNumber
		{
			get
			{
				return _versionNumber;
			}
			set
			{
				Set<int>(ref _versionNumber, value);
			}
		}

        public DateTime ProjectStart
        {
            get
            {
                return _projectStart;
            }
            set
            {
                Set<DateTime>(ref _projectStart, value);
            }
        }

        public DateTime ProjectEnd
        {
            get
            {
                return _projectEnd;
            }
            set
            {
                Set<DateTime>(ref _projectEnd, value);
            }
        }

        public bool ProjectEstimatedTime
        {
            get
            {
                return _projectEstimatedTime;
            }
            set
            {
                Set<bool>(ref _projectEstimatedTime, value);
            }
        }

        public string ProjectLanguage
        {
            get
            {
                return _projectLanguage;
            }
            set
            {
                Set<string>(ref _projectLanguage, value);
            }
        }


        public string ProjectStatus
        {
            get
            {
                return _projectStatus;
            }
            set
            {
                Set<string>(ref _projectStatus, value);
            }
        }
	}
}
