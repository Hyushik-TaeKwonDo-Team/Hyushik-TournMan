using Hyushik_TournMan_Common.Results;
using Hyushik_TournMan_Common.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IAdminOrchestrator
    {
        OperationResult ImportParticipantCsvFile(Stream fileStream);
        OperationResult CreateNewTournament(string name);
        AdminViewModel GetAdminViewModel();
    }
}
