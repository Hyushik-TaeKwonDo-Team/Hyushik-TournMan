using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.QrCheckin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class ActiveTournamentOrchestrator : BaseOrchestrator, IActiveTournamentOrchestrator
    {

        public long GetParticipantIdFromImage(Stream imageStream)
        {
            try
            {
                return GetParticipantIdFromBitmap(new Bitmap(Image.FromStream(imageStream, true, true)));
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public long GetParticipantIdFromBitmap(System.Drawing.Bitmap img){
            var qrGen = new QrGen();

            try{
                var idString = qrGen.getInfoFromImage(img);
                return Int64.Parse(idString);
            }catch(Exception ex){
                return -1;
            }
            
        }


    }
}
