using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
	public interface IRelRepository
	{
        Relationship AddNewRel(Relationship rel, int reltypeid, string username, int orgID, int pageID, string reltext);
		/// <summary>
		/// Returns a list of tags for all pages. Each item is a list of tags seperated by ,
		/// e.g. { "tag1, tag2, tag3", "blah, blah2" } 
		/// </summary>
		/// <returns></returns>
        IEnumerable<Relationship> AllRels();
        void DeleteRel(Relationship rel);
        Relationship GetRelById(int id);
        Relationship SaveOrUpdateRel(Relationship rel);
        IEnumerable<Relationship> FindRelsCreatedBy(string username);
 	}
}
