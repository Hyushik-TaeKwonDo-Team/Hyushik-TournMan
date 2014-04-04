using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Properties;
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
        public OperationResult SetRingSparringResultsPublicStatus(long ringId, bool status)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {

                var ring = _tournManContext.Rings.First(r => r.Id == ringId);

                ring.SparringResultsPublic = status;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                if (status)
                {
                    result.Message = String.Format(Resources.SetSparringResultsPublicMessage, ring.Name);
                }
                else
                {
                    result.Message = String.Format(Resources.SetSparringResultsPrivateMessage, ring.Name);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }


        public OperationResult SetRingFormResultsPublicStatus(long ringId, bool status)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {

                var ring = _tournManContext.Rings.First(r => r.Id == ringId);

                ring.FormResultsPublic = status;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                if (status)
                {
                    result.Message = String.Format(Resources.SetFormResultsPublicMessage, ring.Name);
                }
                else
                {
                    result.Message = String.Format(Resources.SetFormResultsPrivateMessage, ring.Name);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        public OperationResult SetRingWeaponResultsPublicStatus(long ringId, bool status)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {

                var ring = _tournManContext.Rings.First(r => r.Id == ringId);

                ring.WeaponResultsPublic = status;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                if (status)
                {
                    result.Message = String.Format(Resources.SetWeaponResultsPublicMessage, ring.Name);
                }
                else
                {
                    result.Message = String.Format(Resources.SetWeaponResultsPrivateMessage, ring.Name);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        public OperationResult SetRingBreakingResultsPublicStatus(long ringId, bool status){
            var result = new OperationResult() { WasSuccessful = false };
            try
            {

                var ring = _tournManContext.Rings.First(r => r.Id == ringId);

                ring.BreakingResultsPublic = status;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                if(status){
                    result.Message = String.Format(Resources.SetBreakingResultsPublicMessage, ring.Name);
                }else{
                    result.Message = String.Format(Resources.SetBreakingResultsPrivateMessage, ring.Name);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }


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
