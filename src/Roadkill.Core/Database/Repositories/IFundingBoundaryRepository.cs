using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Database.Repositories.LightSpeed;

namespace Roadkill.Core.Database.Repositories
{
    public interface IFundingBoundaryRepository
    {
        IEnumerable<FundingBoundary> FindAllFundingBoundary();
    }
}
