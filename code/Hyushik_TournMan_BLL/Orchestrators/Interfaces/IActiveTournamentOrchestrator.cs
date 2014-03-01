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
        GetJudgeNamesAndScoresOperationResult GetBreakingJudgeOpinions(long entryId);
        List<BreakingResult> GetBreakingResultByTournamentId(long tournId);
        Tournament GetTournamentById(long id);
        BreakingScoringResult CalculateBreakingScore(BreakingResult breakingResult);
        IList<Participant> GetParticipantsByTournId(long tournId);
        List<WeaponResult> GetWeaponResultsByTournId(long tournId);
        List<FormResult> GetFormResultsByTournId(long tournId);
    }
}
