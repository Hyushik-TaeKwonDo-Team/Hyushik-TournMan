using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IActiveTournamentOrchestrator
    {
        long GetParticipantIdFromImage(Stream stream);
        long GetParticipantIdFromBitmap(System.Drawing.Bitmap img);
        Tournament GetTournamentById(long id);
        OperationResult CheckInParticipantToRing(long partId, long tournId, long ringId);
    }
}
