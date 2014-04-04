using Hyushik_TournMan_BLL.Scoring;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Properties;
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


        public OperationResult CheckInParticipantToRing(long partId, long ringId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var part = GetParticipantById(partId);
                var ringsIn = _tournManContext.Rings.Where(r=>r.SelectedParticipants.Contains(part)).ToList();
                var ringToAddTo = GetRingById(ringId);
                
                foreach(var ring in ringsIn){
                    ring.SelectedParticipants.Remove(part);
                }
                if (!ringToAddTo.SelectedParticipants.Contains(part)) ringToAddTo.SelectedParticipants.Add(part);

                result.Message = String.Format(Resources.ParticipantCheckedInToRingMessage, part.Name, ringToAddTo.Name);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult DeleteFormsResult(long fid)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var fResult = _tournManContext.FormResults.First(w => w.Id == fid);
                foreach (var score in fResult.JudgeScores.ToList())
                {
                    _tournManContext.WeaponAndFormJudgeScores.Remove(score);
                }
                fResult.JudgeScores.Clear();
                _tournManContext.FormResults.Remove(fResult);
                _tournManContext.SaveChanges();
                //result.Message = String.Format(Resources.FormDeletedMessage, fResult.Participant.Name);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        
        public OperationResult DeleteWeaponsResult(long wid)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var wResult = _tournManContext.WeaponResults.First(w=>w.Id == wid);
                foreach(var score in wResult.JudgeScores.ToList()){
                    _tournManContext.WeaponAndFormJudgeScores.Remove(score);
                }
                wResult.JudgeScores.Clear();
                _tournManContext.WeaponResults.Remove(wResult);
                _tournManContext.SaveChanges();
                //result.Message = String.Format(Resources.WeaponDeletedMessage, wResult.Participant.Name);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult DeleteSparringResult(long sparId )
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var sparResult = _tournManContext.SparringResults.Where(sr=>sr.Id==sparId).ToList().First();
                //result.Message = String.Format(Resources.SparringDeletedMessage, sparResult.Victor.Name, sparResult.Defeated.Name, sparResult.RoundNumber);
                _tournManContext.SparringResults.Remove(sparResult);
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public OperationResult SaveSparringResult(SparringResult sparResult)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                _tournManContext.SparringResults.Add(sparResult);
                _tournManContext.SaveChanges();
                result.Message = String.Format(Resources.SparringResultCreatedMessage, sparResult.Victor.Name, sparResult.Defeated.Name, sparResult.RoundNumber);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public List<SparringResult> GetSparringResultsByRingId(long ringId)
        {
            return _tournManContext.SparringResults.Where(sr=>sr.Ring.Id==ringId).ToList();
        }

        public long GetParticipantIdBySelection(ParticipantSelection selection)
        {
            if (!selection.CreateNewParticipant)
            {
                return _tournManContext.Participants.First(p=>p.ParticipantId==selection.ParticipantId).ParticipantId;
            }
            var participant = new Participant(){Name=selection.NewParticipantName};
            _tournManContext.Participants.Add(participant);
            var ring = GetRingById(selection.RingId);
            ring.SelectedParticipants.Add(participant);
            ring.Tournament.Participants.Add(participant);
            _tournManContext.SaveChanges();
            return participant.ParticipantId;

        }


        public ParticipantSelectionOperationResult GetParticipantSelectionByRingId(long ringId)
        {
            var result = new ParticipantSelectionOperationResult() { WasSuccessful = false };
            try
            {
                var selection = new ParticipantSelection();
                var ring = GetRingById(ringId);
                selection.Participants = ring.SelectedParticipants;
                selection.RingId = ring.Id;
                selection.TournId = ring.Tournament.Id;
                result.ParticipantSelection = selection;
                result.WasSuccessful = true;
            }catch(Exception ex){
                result.Message = ex.Message;
            }
            return result;

        }


        public OperationResult CreateRing(string name, long tournId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var tourn = _tournManContext.Tournaments.First(be => be.Id == tournId);

                var ring = new Ring()
                {
                    Name = name,
                    Tournament = tourn
                };

                _tournManContext.Rings.Add(ring);
                _tournManContext.SaveChanges();
                result.Message = String.Format(Resources.RingCreatedMessage, ring.Name);
                result.WasSuccessful = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        public OperationResult DeleteBreakingEntry(long entryId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                var entry = _tournManContext.BreakingResults.First(be => be.Id == entryId);
                
                foreach(var score in entry.JudgeScores.ToList()){
                    _tournManContext.BreakingJudgeScores.Remove(score);
                }

                foreach (var station in entry.Stations.ToList())
                {
                    _tournManContext.Stations.Remove(station);
                }
               
                _tournManContext.BreakingResults.Remove(entry);
                _tournManContext.SaveChanges();
                //result.Message = String.Format(Resources.BreakingEntryDeletedMessage, entry.Participant.Name);
                result.WasSuccessful = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public WeaponResult GetWeaponResultById(long id)
        {
            return _tournManContext.WeaponResults.First(wr => wr.Id == id);
        }

        public FormResult GetFormResultById(long id)
        {
            return _tournManContext.FormResults.First(fr => fr.Id == id);
        }

        public List<WeaponResult> GetWeaponResultsByRingId(long ringId)
        {
            return _tournManContext.WeaponResults.Where(wr => wr.Ring.Id==ringId).ToList();
        }

        public List<FormResult> GetFormResultsByRingId(long ringId)
        {
            return _tournManContext.FormResults.Where(wr => wr.Ring.Id == ringId).ToList();
        }

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
                var changed = !Enumerable.SequenceEqual(newVals, GetPossibleBoardDepths());
                StoredValues.PossibleBoardDepths = newVals;
                result.WasSuccessful = true;
                if (changed) result.Message = Resources.PossibleBoardDepthsUpdatedMessage;
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
                var changed = !Enumerable.SequenceEqual(newVals, GetPossibleBoardWidths());
                StoredValues.PossibleBoardWidths = newVals;
                result.WasSuccessful = true;
                 if (changed) result.Message = Resources.PossibleBoardWidthsUpdatedMessage;
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
                result.Stations = breakingResult.Stations;
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

        public UserProfile GetUserByName(string name)
        {
            return GetUsers().First(u=>u.UserName.ToLower()==name.ToLower());
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

        public Ring GetRingById(long id)
        {
            //returns null if not found
            return _tournManContext.Rings.Where(t => t.Id == id).FirstOrDefault();
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

        public IList<Participant> GetParticipantsByRingId(long ringId)
        {
            return _tournManContext.Rings.First(t => t.Id == ringId).SelectedParticipants;
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
