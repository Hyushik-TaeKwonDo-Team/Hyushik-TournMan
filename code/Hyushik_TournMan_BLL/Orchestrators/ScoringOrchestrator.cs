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

        public SaveBreakingJudgeScoreResult EnterJudgeScore(BreakingJudgeScore score, long entryId)
        {
            var result = new SaveBreakingJudgeScoreResult();
            BreakingResult entry;
            try
            {
                
                entry = GetBreakingResultById(entryId);
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
            if(!techniques.Exists(t=>t.Selected)){
                result.HasTechniqueValue = false;
                result.WasSuccessful = true;
                return result;
            }
            result.TechniqueValue = GetTechniqueValueFromTechniques(techniques.Where(x => x.Selected));
            result.HasTechniqueValue = true;
            result.WasSuccessful = true;
            return result;
        }

        public TechniqueValue GetTechniqueValueFromTechnique(Technique tech)
        {
            var techniqueValue = new TechniqueValue();
            techniqueValue.Name = techniqueValue.Name + " "+tech.Name.Trim();
            techniqueValue.Value += tech.Weight;
            if(!tech.IsLeaf()){
                var secondVal = GetTechniqueValueFromTechniques(tech.SubTechniques.Where(st=>st.Selected));
                techniqueValue.Name = techniqueValue.Name + " " + secondVal.Name.Trim();
                techniqueValue.Value += secondVal.Value;
            }
            techniqueValue.Name.Trim();
            return techniqueValue;
        }

        public TechniqueValue GetTechniqueValueFromTechniques(IEnumerable<Technique> techs)
        {
            var techniqueValue = new TechniqueValue();
            foreach (var tech in techs)
            {
                var val = GetTechniqueValueFromTechnique(tech);
                techniqueValue.Name = techniqueValue.Name + " " + val.Name;
                techniqueValue.Value += val.Value;
            }
            return techniqueValue;
        }
    }
}
