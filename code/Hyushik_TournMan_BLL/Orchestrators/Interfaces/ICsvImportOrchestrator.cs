using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface ICsvImportOrchestrator
    {
        void importParticipantCsvFile(Stream fileStream);
    }
}
