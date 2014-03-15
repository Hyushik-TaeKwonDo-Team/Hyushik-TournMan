using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IScoringOrchestrator
    {
        long GetParticipantIdBySelection(ParticipantSelection selection);
        OperationResult UpdateStationAttempts(long stationId, int attempts, bool didNotBreak);
        OperationResult DeleteBreakingEntry(long entryId);
        SaveBreakingJudgeScoreResult EnterBreakingJudgeScore(BreakingJudgeScore score, long entryId);
        IList<Technique> GetTopLevelTechniques();
        TechniqueValueResult CreateTechniqueValue(List<Technique> techniques);
        OperationResult SaveBreakingResult(BreakingResult breakingResult);
        Tournament GetTournamentById(long id);
        Ring GetRingById(long id);
        IList<Participant> GetParticipantsByTournId(long tournId);
        IList<Participant> GetParticipantsByRingId(long ringId);
        Participant GetParticipantById(long partId);
        IEnumerable<UserProfile> GetUsers();
        List<double> GetPossibleBoardWidths();
        List<double> GetPossibleBoardDepths();

        OperationResult NewWeaponEntry(long tournId, long partId);
        OperationResult NewFormEntry(long tournId, long partId);
        OperationResult ScoreWeaponEntry(long entryId, int score, string userName);
        OperationResult ScoreFormEntry(long entryId, int score, string userName);
    }
}
