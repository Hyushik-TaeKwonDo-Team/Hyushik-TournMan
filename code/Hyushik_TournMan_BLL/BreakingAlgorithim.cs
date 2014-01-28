using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_DAL.StoredValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL
{
    public class BreakingAlgorithim
    {
        private double boardExp = 1.4;
        private  double NO_SPACER_BONUS = 1.3;




        public double ScoreAll(BreakingResult BreakingResult)
        {
            List<double> stationscores = new List<double>();
            //calculate the score for each station 
            foreach(var station in BreakingResult.Stations)
            {
                stationscores.Add(scoreStation(station));
            }
            stationscores.Sort();
            stationscores.Reverse();

            //apply the fall off deductions for mutapale stations
            for (var i = 0; i < stationscores.Count; i += 1)
            {
                stationscores[i] = stationscores[i] * Math.Pow(StoredValues.StationFalloffProportion,i);
            }


            double beforeBonus = stationscores.Sum();



            //average the judges subjective parts and use that to give a precentage boost to the total partisipent score

            double judgeAverage = 0;
            foreach (var judge in BreakingResult.JudgeScores)
            {
                judgeAverage = judgeAverage + judge.SubjectiveScore;
            }

            judgeAverage = judgeAverage / BreakingResult.JudgeScores.Count;

            var endScore = beforeBonus * (1 + (judgeAverage / 100));

            return endScore;
        }





        public double scoreStation(Station station)
        {

            var tScore = station.Technique.Value;
            var bScore = getBoardScore(station);
            var penalty = getMissPenalty(station.attempts);

            return (tScore+bScore)*penalty;
        }


        private double getMissPenalty(double attempts)
        {
            /*
              This returns a percent of the total score for the station that the participent earns 
             */
            double penalty = 1 / (Math.Sqrt(Math.Sqrt(attempts)));

            return penalty;
        }


        private double getBoardScore(Station station)
        {

            var boardBase = (station.BoardWidth) * station.BoardDepth;

            if (station.BoardCount == 1)
            {
                return boardBase;
            }

            var tmpScore = boardBase + Math.Pow((station.BoardCount - 1), boardExp);


            if (!station.BoardSpacers)
            {
                tmpScore = tmpScore * NO_SPACER_BONUS;
            }

            return tmpScore;
        }



    }

  

}
