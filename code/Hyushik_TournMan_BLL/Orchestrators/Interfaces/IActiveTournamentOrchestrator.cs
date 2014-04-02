using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IActiveTournamentOrchestrator
    {
        Tournament GetTournamentById(long id);
        OperationResult CheckInParticipantToRing(long partId, long ringId);
    }
}
