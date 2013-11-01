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
using Hyushik_TournMan_Common.Properties;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class AdminOrchestrator : IAdminOrchestrator
    {
        private TournManContext tournManContext = new TournManContext();

        public Tournament GetTournamentById(long id)
        {
            //returns null if not found
            return tournManContext.Tournaments.Where(t=>t.Id==id).FirstOrDefault();
        }

        public IList<Tournament> GetAllTournaments()
        {
            return tournManContext.Tournaments.ToList<Tournament>();
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
            if ( tournManContext.Tournaments.Any(t=>t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) )
            {
                result.Message = String.Format(Resources.TournamentNameInUseMessage, name);
                return result;
            }

            try{
                var newTourn = new Tournament(){ Name=name };
                tournManContext.Tournaments.Add(newTourn);
                tournManContext.SaveChanges();
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
