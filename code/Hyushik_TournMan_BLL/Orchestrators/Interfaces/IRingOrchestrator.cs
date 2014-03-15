using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IRingOrchestrator
    {
        List<SparringResult> GetSparringResultsByRingId(long ringId);
        long GetParticipantIdBySelection(ParticipantSelection selection);
        ParticipantSelectionOperationResult GetParticipantSelectionByRingId(long ringId);
        GetJudgeNamesAndScoresOperationResult GetBreakingJudgeOpinions(long entryId);
        List<BreakingResult> GetBreakingResultByRingId(long tournId);
        Tournament GetTournamentById(long id);
        Ring GetRingById(long id);
        BreakingScoringResult CalculateBreakingScore(BreakingResult breakingResult);
        IList<Participant> GetParticipantsByRingId(long ringId);
        List<WeaponResult> GetWeaponResultsByRingId(long ringId);
        List<FormResult> GetFormResultsByRingId(long ringId);
    }
}
