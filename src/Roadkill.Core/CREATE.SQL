--BEGIN TRANSACTION;
CREATE TABLE  roadkill_users (
	id	uniqueidentifier NOT NULL,
	activationkey	TEXT,
	email	NTEXT,
	firstname	NTEXT,
	iseditor	BIT,
	isadmin	BIT,
	isactivated	BIT,
	lastname	NTEXT,
	password	NTEXT,
	passwordresetkey	NTEXT,
	salt	NTEXT,
	username	NTEXT,
	orgid	INT,
	emailSubscriber	BIT DEFAULT 1,
	createdon	DATETIME DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE roadkill_siteconfiguration (
	id	uniqueidentifier NOT NULL,
	version	TEXT,
	content	NTEXT,
	PRIMARY KEY(id)
);

CREATE TABLE roadkill_pages (
	id	[int] IDENTITY(1,1) NOT NULL,
	title	TEXT,
	tags	TEXT,
	createdby	TEXT,
	createdon	DATETIME,
	islocked	BIT,
	modifiedby	TEXT,
	modifiedon	DATETIME,
	projectstart	DATETIME,
	projectend	DATETIME,
	projectestimatedtime	BIT,
	projectlanguage	TEXT,
	projectstatus	TEXT,
	orgid	INTEGER,
	projectAgileLifeCyclePhase	TEXT,
	department	TEXT,
	owner	TEXT,
	ownerEmail	TEXT,
	allowExternalCollaboration	BIT DEFAULT 0,
	fundingboundary	TEXT,
	collaborationLevel	TEXT
);

CREATE TABLE roadkill_pagecontent (
	id	uniqueidentifier NOT NULL,
	editedby	TEXT,
	editedon	DATETIME,
	versionnumber	INT,
	text	NTEXT,
	pageid	INT,
	projectstart	DATETIME,
	projectend	DATETIME,
	projectestimatedtime	BIT,
	projectlanguage	TEXT,
	projectstatus	TEXT,
	orgid	INTEGER,
	projectAgileLifeCyclePhase	TEXT,
	department	TEXT,
	owner	TEXT,
	ownerEmail	TEXT,
	allowExternalCollaboration	BIT DEFAULT 0,
	fundingboundary	TEXT,
	collaborationLevel	TEXT,
	PRIMARY KEY(id)
);

CREATE TABLE pipeline_statusupdate (
	id	[int] IDENTITY(1,1) NOT NULL,
	PageId	INTEGER NOT NULL,
	Author	TEXT NOT NULL,
	ext	TEXT NOT NULL,
	UpdateDate	DATE NOT NULL,
	PRIMARY KEY(Id)
);

CREATE TABLE  pipeline_relationship_types (
	id	[int] IDENTITY(1,1) NOT NULL,
	reltypetext	TEXT,
	PRIMARY KEY(id)
);

CREATE TABLE  pipeline_relationship (
	id	[int] IDENTITY(1,1) NOT NULL,
	username	NTEXT,
	pageid	INTEGER,
	reltypeid	INTEGER,
	reltext	TEXT,
	reldatetime	DATETIME DEFAULT (current_timestamp),
	orgid	INTEGER,
	approved	BIT DEFAULT 1,
	pending	BIT DEFAULT 1,
	useremail	TEXT,
	userid	TEXT,
	PRIMARY KEY(id)
);

CREATE TABLE  pipeline_orgs (
	id	[int] IDENTITY(1,1) NOT NULL,
	orgname	TEXT,
	email	TEXT,
	url	TEXT,
	twitter	TEXT
);


--CREATE VIEW [council_users] AS SELECT orgname FROM [pipeline_orgs]
--JOIN [roadkill_users]
--ON [pipeline_orgs].id = [roadkill_users].orgid
--GROUP BY orgname
--ORDER BY orgname;
--COMMIT;
