using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IActiveTournamentOrchestrator
    {
        List<BreakingResult> GetBreakingResultByTournamentId(long tournId);
        Tournament GetTournamentById(long id);
    }
}
