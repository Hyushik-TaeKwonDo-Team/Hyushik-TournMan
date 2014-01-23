using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class ActiveTournamentOrchestrator : BaseOrchestrator, IActiveTournamentOrchestrator
    {
        public List<BreakingResult> GetBreakingResultByTournamentId(long tournId)
        {
            return _tournManContext.BreakingResults.Where(br => br.Tournament.Id == tournId).ToList();
        }
    }
}
