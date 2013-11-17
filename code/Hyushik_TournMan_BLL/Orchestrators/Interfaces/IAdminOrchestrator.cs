using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
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
        OperationResult ImportParticipantCsvFile(Stream fileStream, long targetTournamentId);
        OperationResult CreateNewTournament(string name);
        IList<Tournament> GetAllTournaments();
        Tournament GetTournamentById(long id);
        IList<BoardSizeCount> GetTotalBoardSizeCountsByTournamentId(long id);
        IList<UserProfile> GetAllUsers();
        IDictionary<string, string[]> GetMappingOfUserNameToRoles();
        OperationResult AddRole(string userName, string roleName);
        OperationResult RemoveRole(string userName, string roleName);
    }
}
