using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using CsvHelper;
using System.IO;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class CsvImportOrchestrator : ICsvImportOrchestrator
    {
        public void importParticipantCsvFile(Stream fileStream){

            ICsvParser csvParser = new CsvParser(new StreamReader(fileStream));
            CsvReader csvReader = new CsvReader(csvParser);
            string[] headers = { };
            List<string[]> rows = new List<string[]>();
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
                rows.Add(row);
            }

        }

    }
}
