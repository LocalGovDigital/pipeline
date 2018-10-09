using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Database.Repositories.LightSpeed;
using Roadkill.Core.Plugins;
using StructureMap.Attributes;

namespace Roadkill.Core.Database
{
	/// <summary>
	/// Defines a repository for storing and retrieving Roadkill domain objects in a data store.
	/// </summary>
	public interface IRepository : IPageRepository, IUserRepository, ISettingsRepository, IDisposable, IRelRepository
	{
		void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache);
		void TestConnection(DataStoreType dataStoreType, string connectionString);
	    void SetPendingApprovedInProject(int projectid, string username, int organisationId);
	    void SetContributeApprovedInProject(int projectid, int id);

	    void SetContributeAutoApprovedInProject(int projectid, string username, int organisationId);


	    IList<FundingBoundary> FundingBoundaries { get;}

	}
}
