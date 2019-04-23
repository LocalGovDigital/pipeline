--sprint 1
ALTER TABLE [roadkill_pages] ADD [projectAgileLifeCyclePhase] TEXT NULL ;
ALTER TABLE [roadkill_pagecontent] ADD [projectAgileLifeCyclePhase] TEXT NULL ;

--sprint 2
ALTER TABLE [roadkill_pages] ADD [department] TEXT NULL;
ALTER TABLE [roadkill_pagecontent] ADD [department] TEXT NULL;

n
ALTER TABLE [roadkill_pages] ADD [owner] TEXT NULL;
ALTER TABLE [roadkill_pagecontent] ADD [owner] TEXT NULL;

ALTER TABLE [roadkill_pages] ADD [ownerEmail] TEXT NULL;
ALTER TABLE [roadkill_pagecontent] ADD [ownerEmail] TEXT NULL;

ALTER TABLE [roadkill_pages] ADD [allowExternalCollaboration]  BIT DEFAULT FALSE;
ALTER TABLE [roadkill_pagecontent] ADD [allowExternalCollaboration] BIT DEFAULT FALSE;

--sprint 3
ALTER TABLE [roadkill_pages] ADD [fundingboundary] TEXT NULL ;
ALTER TABLE [roadkill_pagecontent] ADD [fundingboundary] TEXT NULL ;

CREATE TABLE [pipeline_statusupdate] (
  [Id] INTEGER NOT NULL
, [PageId] INTEGER NOT NULL
, [Author] TEXT NOT NULL
, [Text] TEXT NOT NULL
, [UpdateDate] DATE NOT NULL
, CONSTRAINT [pipeline_statusupdate] PRIMARY KEY ([Id])
);

ALTER TABLE [pipeline_relationship] ADD [approved] BIT default 1 ;
ALTER TABLE [pipeline_relationship] ADD [pending] BIT default 1 ;


ALTER TABLE [roadkill_pages] ADD [collaborationLevel]  TEXT NULL ;
ALTER TABLE [roadkill_pagecontent] ADD [collaborationLevel] TEXT NULL;


INSERT INTO [pipeline_orgs]([orgname],[email],[url],[twitter])VALUES ('None','','','');
INSERT INTO [pipeline_relationship_types]([reltypetext])VALUES ('Watcher');
INSERT INTO [pipeline_relationship_types]([reltypetext])VALUES ('Contributer');

