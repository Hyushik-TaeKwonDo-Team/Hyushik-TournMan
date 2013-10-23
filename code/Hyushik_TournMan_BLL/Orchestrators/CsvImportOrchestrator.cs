using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Constants;
using Hyushik_TournMan_DAL.Contexts;
using CsvHelper;
using System.IO;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class CsvImportOrchestrator : ICsvImportOrchestrator
    {
        private TournManContext tournManContext = new TournManContext();

        public void importParticipantCsvFile(Stream fileStream)
        {

            var tournManContext = new TournManContext();

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
                tournManContext.Participants.Add(
                    MakeParticipantFromCSVLine(headers, row)
                    );
            }

            tournManContext.SaveChanges();
        }

        private Participant MakeParticipantFromCSVLine(string[] headers, string[] row)
        {
            var participant = new Participant();
            participant.Name = row[0];


            for (int j = Constants.CSV.PRE_BOARD_SIZE_COLUMN_COUNT; j < headers.Count(); j++)
            {
                //TODO
            }
            //TODO

            return participant;
        }
    }
}
