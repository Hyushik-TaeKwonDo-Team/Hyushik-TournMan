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
        private double boardExp = StoredValues.BreakingBoardExponent;
        private int maxBoards = StoredValues.BreakingMaximumBoards;
        private int maxAttempts = StoredValues.BreakingMaximumAttempts;
        private double attempt_decay_rate = StoredValues.BreakingAttemptDecayRate;

        private double SPACER_PENALTY = StoredValues.BreakingSpacerPenalty;
        private double POWER_HOLD_PENALTY = StoredValues.BreakingPowerHoldPenalty; // we value a spead techneque over not having spacers

        private double judgeworth = StoredValues.BreakingJudgeWeight;
        private double maxScore = StoredValues.BreakingMaxScore;

        public double ScoreAll(BreakingResult BreakingResult)
        {
            List<double> stationscores = new List<double>();
            //calculate the score for each station 
            foreach(var station in BreakingResult.Stations)
            {
                stationscores.Add(scoreStation(station));
            }


            double averageScore = stationscores.Sum() / stationscores.Count;

            double Multiple_Technique_Coefficient = 1 + (0.1 * (stationscores.Count - 1));



            double avragejudgeScore = 0;

            //average the judges subjective parts
            if (BreakingResult.JudgeScores.Count > 0)
            {
                double judgeSum = 0;
                foreach (var judge in BreakingResult.JudgeScores)
                {
                    judgeSum = judgeSum + judge.SubjectiveScore;
                }

                avragejudgeScore = judgeSum / BreakingResult.JudgeScores.Count;

            }


            var beforeJudgeIntervention = averageScore * Multiple_Technique_Coefficient;

            var NormalJudgeVal = judgeworth*(avragejudgeScore / maxScore);





            var NormalScoreVal = (maxScore - judgeworth) * (beforeJudgeIntervention / (maxScore * (1 + (0.1 * (StoredValues.BreakingMaxStationCount - 1)))));



            return NormalScoreVal + NormalJudgeVal;

            
        }





        public double scoreStation(Station station)
        {

            var tScore = station.Technique.Value;
            var bScore = getBoardScore(station);

            var penalty = getMissPenalty(station);


            return tScore*bScore*penalty;
        }


        private double getMissPenalty(Station station)
        {
            /*
              This returns a percent of the total score for the station that the participent earns 
             */
            double penalty ;

            if (station.DidNotBreak || station.Attempts >= maxAttempts)
            {
                //if they diddent break use the max attempts 
                penalty = Math.Pow(Math.E, -attempt_decay_rate * (maxAttempts - 1));
            }
            else
            {   
                //if they broke it use the number attempts they took
                penalty = Math.Pow(Math.E, -attempt_decay_rate * (station.Attempts - 1));
            }
            

            return penalty;
        }


        private double getBoardScore(Station station)
        {

            var maxboardpart=StoredValues.PossibleBoardWidths.Max() * ( 1 + StoredValues.PossibleBoardDepths.Max());

            var boardBase = ((station.BoardWidth) * (1 + station.BoardDepth)) / maxboardpart;

            //BoardScore =((boardBase/(maxBoards+1))*(attempts+1))^boardExp
            var boardScore = Math.Pow(((boardBase / (maxBoards + 1)) * (station.BoardCount + 1)) , boardExp);


            if (station.BoardSpacers)
            {
                boardScore = boardScore - SPACER_PENALTY;
            }


            if (!station.SpeedHold)
            {
                boardScore = boardScore - POWER_HOLD_PENALTY;
            }



            return boardScore;
        }



    }

  

}
