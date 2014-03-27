using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class RingOrchestrator : BaseOrchestrator, IRingOrchestrator
    {
        public GetJudgeNamesAndScoresOperationResult GetBreakingJudgeOpinions(long entryId)
        {
            var result = new GetJudgeNamesAndScoresOperationResult() { WasSuccessful = false };
            try
            {

                var entry = _tournManContext.BreakingResults.First(be => be.Id == entryId);

                foreach (var score in entry.JudgeScores)
                {
                    var judge = _usersContext.UserProfiles.First(up => up.UserId == score.Judge_UserId);
                    result.JudgeIdToName.Add(judge.UserId, judge.UserName);
                    result.JudgeIdToScore.Add(judge.UserId, score.SubjectiveScore);
                }
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public List<BreakingResult> GetBreakingResultByRingId(long ringId)
        {
            return _tournManContext.BreakingResults.Where(br => br.Ring.Id == ringId).ToList();
        }
    }
}
