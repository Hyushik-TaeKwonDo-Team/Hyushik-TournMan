﻿using Hyushik_TournMan_Common.Models;
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
        SaveBreakingJudgeScoreResult EnterJudgeScore(BreakingJudgeScore score, long entryId);
        IList<Technique> GetTopLevelTechniques();
        TechniqueValueResult CreateTechniqueValue(List<Technique> techniques);
        OperationResult SaveBreakingResult(BreakingResult breakingResult);
        Tournament GetTournamentById(long id);
        IList<Participant> GetParticipantsByTournId(long tournId);
        Participant GetParticipantById(long partId);
        IEnumerable<UserProfile> GetUsers();
        List<double> GetPossibleBoardWidths();
        List<double> GetPossibleBoardDepths();
    }
}
