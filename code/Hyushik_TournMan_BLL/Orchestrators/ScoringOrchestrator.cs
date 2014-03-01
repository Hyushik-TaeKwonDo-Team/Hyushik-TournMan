using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyushik_TournMan_Common.Properties;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class ScoringOrchestrator: BaseOrchestrator, IScoringOrchestrator
    {

        public OperationResult UpdateStationAttempts(long stationId, int attempts, bool didNotBreak)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var station = _tournManContext.Stations.First(st=>st.Id==stationId);
                station.Attempts = attempts;
                station.DidNotBreak = didNotBreak;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }


        public OperationResult ScoreWeaponEntry(long entryId, int score, string userName)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var entry = GetWeaponResultById(entryId);
                var user = GetUserByName(userName);
                var previousScore = entry.JudgeScores.FirstOrDefault(js=>js.Judge_UserId==user.UserId);
                if(null!=previousScore){
                    previousScore.Score = score;
                }else{
                    entry.JudgeScores.Add(new WeaponAndFormJudgeScore()
                    {
                        Score=score,
                        Judge_UserId = user.UserId
                    });
                }
                result.Message = String.Format(Resources.FormScoreJudgedMessage, entry.Participant.Name, score);
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult ScoreFormEntry(long entryId, int score, string userName)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var entry = GetFormResultById(entryId);
                var user = GetUserByName(userName);
                var previousScore = entry.JudgeScores.FirstOrDefault(js => js.Judge_UserId == user.UserId);
                if (null != previousScore)
                {
                    previousScore.Score = score;
                }
                else
                {
                    entry.JudgeScores.Add(new WeaponAndFormJudgeScore()
                    {
                        Score = score,
                        Judge_UserId = user.UserId
                    });
                }
                _tournManContext.SaveChanges();
                result.Message = String.Format(Resources.WeaponScoreJudgedMessage, entry.Participant.Name, score);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult NewWeaponEntry(long tournId, long partId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var entry = new WeaponResult(){
                    Tournament = GetTournamentById(tournId),
                    Participant = GetParticipantById(partId)
                };
                _tournManContext.WeaponResults.Add(entry);
                _tournManContext.SaveChanges();
                result.Message = String.Format(Resources.WeaponScoreAddedMessage, entry.Participant.Name);
                result.WasSuccessful = true;
            }catch(Exception ex){
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult NewFormEntry(long tournId, long partId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var entry = new FormResult()
                {
                    Tournament = GetTournamentById(tournId),
                    Participant = GetParticipantById(partId)
                };
                _tournManContext.FormResults.Add(entry);
                _tournManContext.SaveChanges();
                result.Message = String.Format(Resources.FormScoreAddedMessage, entry.Participant.Name);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public SaveBreakingJudgeScoreResult EnterBreakingJudgeScore(BreakingJudgeScore score, long entryId)
        {
            var result = new SaveBreakingJudgeScoreResult();
            BreakingResult entry;
            try
            {
                entry = GetBreakingResultById(entryId);
                var previousScore = entry.JudgeScores.FirstOrDefault(js=>js.Judge_UserId==score.Judge_UserId);
                if(null!=previousScore){
                    entry.JudgeScores.Remove(previousScore);
                }
                result.TournamentId = entry.Tournament.Id;
                entry.JudgeScores.Add(score);
                _tournManContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            result.Message = String.Format(Resources.BreakingJudgeScoreEnteredMessage,entry.Participant.Name);
            result.WasSuccessful = true;
            return result;
        }


        public OperationResult SaveBreakingResult(BreakingResult breakingResult)
        {
            var result = new OperationResult();
            try
            {
                result.Message = String.Format(Resources.BreakingResultCreatedMessage, breakingResult.Participant.Name);
                _tournManContext.BreakingResults.Add(breakingResult);
                _tournManContext.SaveChanges();
                
            }catch(Exception ex){
                result.Message = ex.Message;
                return result;
            }
            result.WasSuccessful = true;
            return result;
        }


        public TechniqueValueResult CreateTechniqueValue(List<Technique> techniques)
        {
            var result = new TechniqueValueResult();
            try {
                if (!techniques.Exists(t => t.Selected))
                {
                    result.HasTechniqueValue = false;
                    result.WasSuccessful = true;
                    return result;
                }
                result.TechniqueValue = GetTechniqueValueFromTechnique(techniques.First(x => x.Selected));
                result.HasTechniqueValue = true;
                result.WasSuccessful = true;
            }catch (Exception ex){
                result.Message = ex.Message;
                result.WasSuccessful = false;
            }
            
            return result;
        }

        public TechniqueValue GetTechniqueValueFromTechnique(Technique tech)
        {
            if (!tech.IsLeaf())
            {
                return GetTechniqueValueFromTechnique(tech.SubTechniques.First(st => st.Selected));
            }
            var techniqueValue = new TechniqueValue();
            if(String.IsNullOrWhiteSpace(tech.NameNote)){
                techniqueValue.Name = tech.Name;
            }else{
                techniqueValue.Name = tech.Name;
            }
            
            techniqueValue.Value = tech.Weight;
            techniqueValue.Name.Trim();
            return techniqueValue;
        }
    }
}
