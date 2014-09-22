using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
	public interface IOrgRepository
	{
        IEnumerable<Organisation> FindAllOrgs();
        User GetOrgByID(int id);
	}



}
