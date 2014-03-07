using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using Hyushik_TournMan_Common.Constants;
using Hyushik_TournMan_DAL.Contexts;
using CsvHelper;
using System.IO;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Security;
using Hyushik_TournMan_Common.Properties;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class AdminOrchestrator : BaseOrchestrator, IAdminOrchestrator
    {
        //everything needs to be 0 indexed the same
        public OperationResult AddParticipantsToRings(List<long> ringIds, List<long> participantIds, List<List<bool>> RingsVsParticipants)
        {
            var result = new OperationResult() { WasSuccessful = false };
            try
            {
                foreach(var p in RingsVsParticipants.Select((list, index) => new { index, list })){
                    var participant = GetParticipantById(participantIds[p.index]);
                    foreach (var r in p.list.Select((added, index) => new { index, added }))
                    {
                        var ring = GetRingById(ringIds[r.index]);
                        //needs to be added
                        if (r.added && !ring.SelectedParticipants.Exists(part=>part.ParticipantId==participant.ParticipantId))
                        {
                            ring.SelectedParticipants.Add(participant);
                        }
                        else if (!r.added && ring.SelectedParticipants.Exists(part => part.ParticipantId == participant.ParticipantId))
                        { //needs to be removed
                            ring.SelectedParticipants.Remove(participant);
                        }
                    }
                }
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                //TODO message
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;


        }


        public IDictionary<string, string[]> GetMappingOfUserNameToRoles()
        {
            var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;
            var users = GetUsers();

            var mapping = new Dictionary<string, string[]>();
            foreach(var user in users){
                mapping[user.UserName] = roles.GetRolesForUser(user.UserName);
            }

            return mapping;
        }

        private void RecursiveDeleteSubTechs(Technique tech)
        {

            if (tech.SubTechniques.Count > 0)
            {
                foreach (var subtech in tech.SubTechniques.ToList())
                {
                    RecursiveDeleteSubTechs(subtech);
                    _tournManContext.Techniques.Remove(subtech);
                }
            }
            tech.SubTechniques.Clear();
            return;
        }

        public OperationResult DeleteTechnique(long techId)
        {
            var result = new OperationResult() { WasSuccessful = false };
            Technique tech;
            try
            {
                tech = _tournManContext.Techniques.FirstOrDefault(t => t.Id == techId);

                if(tech.Parent!=null){
                    tech.Parent.SubTechniques.Remove(tech);
                }
                RecursiveDeleteSubTechs(tech);
                _tournManContext.Techniques.Remove(tech);
                _tournManContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            result.WasSuccessful = true;
            result.Message = String.Format(Resources.TechniqueDeletedMessage,tech.Name);
            return result;
        }

        public OperationResult AddTechnique(long parentId, string techName, int techWeight)
        {
            var result = new OperationResult() { WasSuccessful = false };
            Technique tech;
            try
            {
                tech = new Technique();
                
                tech.Name = techName;
                tech.Weight = techWeight;
                //MAGIC NUMBER OF TOP SHELF TECHNIQUEs
                if(parentId!=-1){
                    var parentTech = _tournManContext.Techniques.FirstOrDefault(t => t.Id == parentId);
                    tech.Parent = parentTech;
                }
                if (!tech.CanHaveWeight)
                {
                    tech.Weight = 0;
                }
                _tournManContext.Techniques.Add(tech);
                _tournManContext.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            result.WasSuccessful = true;
            result.Message = String.Format(Resources.TechniqueAddedMessage, tech.Name);
            return result;
        }

        public OperationResult UpdateTechnique(long techId, string techName, int techWeight)
        {
            var result = new OperationResult() { WasSuccessful = false };
            Technique tech;
            try
            {
                tech = _tournManContext.Techniques.FirstOrDefault(t => t.Id == techId);
                tech.Name = techName;
                tech.Weight = techWeight;
                if (!tech.CanHaveWeight)
                {
                    tech.Weight = 0;
                }
                _tournManContext.SaveChanges();
            }catch(Exception ex){
                result.Message = ex.Message;
                return result;
            }
            result.WasSuccessful = true;
            result.Message = String.Format(Resources.TechniqueAddedMessage, tech.Name);
            return result;
        }

        public OperationResult SetTournamentActiveStatus(long tournId, bool activeStatus)
        {
            var tourn = _tournManContext.Tournaments.FirstOrDefault(x=>x.Id==tournId);
            return SetTournamentActiveStatus(tourn, activeStatus);
        }

        public OperationResult SetTournamentActiveStatus(Tournament tourn, bool activeStatus)
        {
            var result = new OperationResult() { WasSuccessful = false };
            if(null==tourn){
                result.Message = Resources.TournamentNotFoundMessage;
                return result;
            }
            try{
                tourn.Active=activeStatus;
                _tournManContext.SaveChanges();
                result.WasSuccessful = true;
                if(activeStatus){
                    result.Message = String.Format(Resources.TournamentActivatedMessage, tourn.Name);
                }else{
                    result.Message = String.Format(Resources.TournamentDeactivatedMessage, tourn.Name);
                }
            }catch(Exception ex){
                result.Message = ex.Message;
            }
            return result ;
        }

        public OperationResult AddRole(string userName, string roleName)
        {
            var result = new OperationResult() { WasSuccessful = false};

            var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;

            if (!roles.GetRolesForUser(userName).Contains(roleName)) {
                roles.AddUsersToRoles(new[] { userName }, new[] { roleName });
                result.WasSuccessful = true;
                result.Message = String.Format(Resources.UserAddedRoleMessage, userName, roleName);
                return result;
            }
            result.Message = String.Format(Resources.UserAlreadyHasRoleMessage, userName, roleName);
            return result;

        }
        public OperationResult RemoveRole(string userName, string roleName)
        {
            var result = new OperationResult() { WasSuccessful = false };

            var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;

            if (roles.GetRolesForUser(userName).Contains(roleName))
            {
                roles.RemoveUsersFromRoles(new[] { userName }, new[] { roleName });
                result.WasSuccessful = true;
                result.Message = String.Format(Resources.UserRemovedRoleMessage, userName, roleName);
                return result;
            }
            result.Message = String.Format(Resources.UserDoesNotAlreadyHaveRoleMessage, userName, roleName);
            return result;
        }

        

        public IList<Tournament> GetAllTournaments()
        {
            return _tournManContext.Tournaments.ToList<Tournament>();
        }

        public IList<BoardSizeCount> GetTotalBoardSizeCountsByTournamentId(long id){
            return GetTotalBoardSizeCountsByTournament( GetTournamentById(id) );
        }

        public IList<BoardSizeCount> GetTotalBoardSizeCountsByTournament(Tournament tourn)
        {
            if(tourn==null){
                return new List<BoardSizeCount>();
            }

            var totalBoardSizesDict = new Dictionary<string,int>();
            var totalBoardSizesList = new List<BoardSizeCount>();
            foreach( var part in tourn.Participants){
                foreach ( var boardSizeCount in part.BoardSizeCounts)
                {
                    if (!totalBoardSizesDict.Keys.Contains(boardSizeCount.BoardSize))
                    {
                        totalBoardSizesDict[boardSizeCount.BoardSize] = 0;
                    }

                    totalBoardSizesDict[boardSizeCount.BoardSize] += boardSizeCount.Count;
                }
            }

            foreach (var boardSize in totalBoardSizesDict.Keys)
            {
                totalBoardSizesList.Add(
                    new BoardSizeCount()
                    {
                        BoardSize=boardSize,
                        Count = totalBoardSizesDict[boardSize]
                    }
                    );
            }
            return totalBoardSizesList;

        }

        public OperationResult CreateNewTournament(string name)
        {
            var result = new OperationResult() { WasSuccessful = false};
            
            if(String.IsNullOrWhiteSpace(name)){
                result.Message = Resources.TournamentMustHaveNameMessage;
                return result;
            }
            name = name.Trim();
            //check if tournament name is in use
            if ( _tournManContext.Tournaments.Any(t=>t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) )
            {
                result.Message = String.Format(Resources.TournamentNameInUseMessage, name);
                return result;
            }

            try{
                var newTourn = new Tournament(){ Name=name };
                _tournManContext.Tournaments.Add(newTourn);
                _tournManContext.SaveChanges();
            }catch (Exception ex)
            {
                result.Message=ex.Message;
                return result;
            }
            result.WasSuccessful = true;
            result.Message = String.Format(Resources.NewTournamentCreatedMessage, name);
            return result;
        }

        public OperationResult ImportParticipantCsvFile(Stream fileStream, long targetTournamentId)
        {

            var tournManContext = new TournManContext();
            try
            {
                var targetTourn = tournManContext.Tournaments.Where(t=>t.Id==targetTournamentId).First();
                ICsvParser csvParser = new CsvParser(new StreamReader(fileStream));
                CsvReader csvReader = new CsvReader(csvParser);
                string[] headers = { };
                string[] row;

                while (csvReader.Read())
                {
                    // Gets Headers if they exist
                    if (!headers.Any())
                    {
                        headers = csvReader.FieldHeaders;
                    }
                    row = new string[headers.Count()];
                    for (int j = 0; j < headers.Count(); j++)
                    {
                        row[j] = csvReader.GetField(j);
                    }
                    var part = MakeParticipantFromCSVLine(headers, row);
                    targetTourn.Participants.Add(part);
                }
                tournManContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new OperationResult()
                            {
                                WasSuccessful = false,
                                Message=ex.Message
                            };
            }
            return new OperationResult()
            {
                WasSuccessful = true,
                Message = Resources.CsvFileReadSuccessfullyMessage
            };
        }

        private Participant MakeParticipantFromCSVLine(string[] headers, string[] row)
        {
            int i = 0;
            var participant = new Participant();
            participant.Name = row[i++];
            participant.Email = row[i++];
            participant.Address = row[i++];
            participant.City = row[i++];
            participant.State = row[i++];
            participant.Zip = row[i++];
            participant.Phone = row[i++];
            participant.Gender = row[i++];

            participant.InstructorName = row[i++];
            participant.SchoolName = row[i++];
            participant.SchoolAddress = row[i++];
            participant.SchoolCity = row[i++];
            participant.SchoolState = row[i++];
            participant.SchoolZip = row[i++];
            participant.SchoolPhone = row[i++];
            participant.SchoolEmail = row[i++];

            participant.Rank = row[i++];
            participant.Age = row[i++];
            participant.Weight = row[i++];
            participant.Weapons = convertCSVBoolStringToBool(row[i++]);
            participant.Breaking = convertCSVBoolStringToBool(row[i++]);
            participant.Forms = convertCSVBoolStringToBool(row[i++]);
            participant.PointSparring = convertCSVBoolStringToBool(row[i++]);
            participant.OlympicSparring = convertCSVBoolStringToBool(row[i++]);

            BoardSizeCount boardCount;
            for (int j = i; j < headers.Count(); j++)
            {
                boardCount = new BoardSizeCount();
                boardCount.BoardSize = headers[j];
                boardCount.Count = Int32.Parse(row[j]);
                participant.BoardSizeCounts.Add(boardCount);
            }

            return participant;
        }
        

        private bool convertCSVBoolStringToBool(string input){
            if (Constants.CSV.YES_STRING == input)
            {
                return true;
            }
            else if (Constants.CSV.NO_STRING == input)
            {
                return false;
            }
            else
            {
                throw new Exception(String.Format(Resources.CsvParseYesNoErrorMessage, input));
            }
        }

    }
}
