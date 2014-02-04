using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using Hyushik_TournMan_DAL.Contexts;
using Hyushik_TournMan_DAL.StoredValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class BaseOrchestrator
    {
        protected UsersContext _usersContext = new UsersContext();
        protected TournManContext _tournManContext = new TournManContext();

        public string GetPossibleBoardWidthsAsString()
        {
            return string.Join(",",GetPossibleBoardWidths());
        }

        public string GetPossibleBoardDepthsAsString()
        {
            return string.Join(",",GetPossibleBoardDepths());
        }

        public List<double> GetPossibleBoardWidths()
        {
            return StoredValues.PossibleBoardWidths;
        }

        public List<double> GetPossibleBoardDepths()
        {
            return StoredValues.PossibleBoardDepths;
        }

        public OperationResult SetPossibleBoardDepths(string sourceString)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var subresult = SetPossibleBoardDepths(
                        sourceString.Split(',').Select(
                            d => double.Parse(d.Trim())
                        ).ToList()
                    );
                result.Message = subresult.Message;
                result.WasSuccessful = subresult.WasSuccessful;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult SetPossibleBoardDepths(List<double> newVals)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                StoredValues.PossibleBoardDepths = newVals;
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            };
            return result;
        }

        public OperationResult SetPossibleBoardWidths(string sourceString)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var subresult = SetPossibleBoardWidths(    
                        sourceString.Split(',').Select(
                            d=>double.Parse(d.Trim())
                        ).ToList()  
                    );
                result.Message= subresult.Message;
                result.WasSuccessful = subresult.WasSuccessful;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult SetPossibleBoardWidths(List<double> newVals)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                StoredValues.PossibleBoardWidths = newVals;
                result.WasSuccessful = true;
            }catch(Exception ex){
                result.Message = ex.Message;
            }; 
            return result;
        }
 
        public BreakingScoringResult CalculateBreakingScore(BreakingResult breakingResult)
        {
            var result = new BreakingScoringResult() { WasSuccessful = false };
            try
            {
                var algo = new BreakingAlgorithim();
                result.Score = algo.ScoreAll(breakingResult);
            }catch(Exception ex){
                result.Message = ex.Message;
                return result;
            }

            result.WasSuccessful = true;

            return result;
        }


        public IEnumerable<UserProfile> GetUsers(){
            return _usersContext.UserProfiles;
        }

        public IList<Tournament> GetActiveTournaments()
        {
            return _tournManContext.Tournaments.Where(t => t.Active).ToList();
        }

        public IList<UserProfile> GetAllUsers()
        {
            return GetUsers().ToList();
        }

        public Tournament GetTournamentById(long id)
        {
            //returns null if not found
            return _tournManContext.Tournaments.Where(t => t.Id == id).FirstOrDefault();
        }

        public Participant GetParticipantById(long id)
        {
            //returns null if not found
            return _tournManContext.Participants.Where(p => p.ParticipantId == id).FirstOrDefault();
        }

        public IList<Technique> GetTopLevelTechniques()
        {
            return _tournManContext.Techniques.Where(t=>t.Parent==null).ToList();
        }

        public IList<Participant> GetParticipantsByTournId(long tournId)
        {
            return _tournManContext.Tournaments.First(t => t.Id == tournId).Participants;
        }

        public BreakingResult GetBreakingResultById(long id){
            return _tournManContext.BreakingResults.FirstOrDefault(br => br.Id == id);
        }

        public double GetStationFalloffProportion()
        {
            return StoredValues.StationFalloffProportion;
        }

        public void SetStationFalloffProportion(double value)
        {
            StoredValues.StationFalloffProportion=value;
        }

        public int GetMaxBreakingStationCount()
        {
            return StoredValues.MaxBreakingStationCount;
        }

        public void SetStationMaxBreakingStationCount(int value)
        {
            StoredValues.MaxBreakingStationCount = value;
        }

    }
}
