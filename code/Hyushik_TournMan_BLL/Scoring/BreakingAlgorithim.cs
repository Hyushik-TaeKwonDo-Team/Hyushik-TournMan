using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_DAL.StoredValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Scoring
{
    public class BreakingAlgorithim
    {
        private double boardExp = 1.4;
        private  double NO_SPACER_BONUS = 1.3;
        private double attempt_decay_rate = 0.2;
        private double SPEED_HOLD_BONUS = 1.3;




        public double ScoreAll(BreakingResult BreakingResult)
        {
            List<double> stationscores = new List<double>();
            //calculate the score for each station 
            foreach(var station in BreakingResult.Stations)
            {
                stationscores.Add(scoreStation(station));
            }


            double averageScore = stationscores.Sum() / stationscores.Count;

            double Multiple_Technique_Coefficient = 1 + (0.1 * stationscores.Count - 1);






            double tieBreaking = 0;

            //average the judges subjective parts
            if (BreakingResult.JudgeScores.Count > 0)
            {
                double judgeSum = 0;
                foreach (var judge in BreakingResult.JudgeScores)
                {
                    judgeSum = judgeSum + judge.SubjectiveScore;
                }

                tieBreaking = judgeSum / BreakingResult.JudgeScores.Count;

            }




            return (averageScore * Multiple_Technique_Coefficient) + (tieBreaking/3);

            
        }





        public double scoreStation(Station station)
        {

            var tScore = station.Technique.Value;
            var bScore = getBoardScore(station);
            var penalty = getMissPenalty(station.Attempts);


            return (tScore+bScore)*penalty;
        }


        private double getMissPenalty(double attempts)
        {
            /*
              This returns a percent of the total score for the station that the participent earns 
             */
            double penalty = Math.Pow(Math.E, -attempt_decay_rate * (attempts - 1));

            return penalty;
        }


        private double getBoardScore(Station station)
        {

            var boardBase = (station.BoardWidth) * (1+station.BoardDepth);

            if (station.BoardCount == 1)
            {
                return boardBase;
            }

            var tmpScore = boardBase + Math.Pow((station.BoardCount - 1), boardExp);


            if (!station.BoardSpacers)
            {
                tmpScore = tmpScore * NO_SPACER_BONUS;
            }

            if (station.SpeedHold)
            {
                tmpScore = tmpScore * SPEED_HOLD_BONUS;
            }
            
            return tmpScore;
        }



    }

  

}
