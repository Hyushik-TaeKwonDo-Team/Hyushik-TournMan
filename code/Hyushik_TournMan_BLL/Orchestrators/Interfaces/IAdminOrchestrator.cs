﻿using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Common.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators.Interfaces
{
    public interface IAdminOrchestrator
    {
        String GetQRCodeBase64StringFromLong(long val);
        OperationResult CreateRing(string name, long tournId);
        OperationResult DeleteRing(long ringId);
        OperationResult AddParticipantsToRings(List<long> ringIds, List<long> participantIds, List<List<bool>> RingsVsParticipants);
        IList<Tournament> GetActiveTournaments();
        OperationResult ImportParticipantCsvFile(Stream fileStream, long targetTournamentId);
        OperationResult CreateNewTournament(string name);
        IList<Tournament> GetAllTournaments();
        Tournament GetTournamentById(long id);
        Participant GetParticipantById(long id);
        IList<BoardSizeCount> GetTotalBoardSizeCountsByTournamentId(long id);
        IList<UserProfile> GetAllUsers();
        IDictionary<string, string[]> GetMappingOfUserNameToRoles();
        OperationResult AddRole(string userName, string roleName);
        OperationResult RemoveRole(string userName, string roleName);
        OperationResult SetTournamentActiveStatus(long tournId, bool activeStatus);
        OperationResult SetTournamentActiveStatus(Tournament tourn, bool activeStatus);
        IList<Technique> GetTopLevelTechniques();
        OperationResult UpdateTechnique(long techId, string techName, int techWeight);
        OperationResult AddTechnique(long parentId, string techName, int techWeight);
        OperationResult DeleteTechnique(long techId);
        void AddIndividualParticipant(long targetTournamentId, String[] info, Boolean[] events);

        double GetBreakingBoardExponent();
        void SetBreakingBoardExponent(double value);
        int GetBreakingMaxStationCount();
        void SetBreakingMaxStationCount(int value);
        int GetBreakingMaximumBoards();
        void SetBreakingMaximumBoards(int value);
        int GetBreakingMaximumAttempts();
        void SetBreakingMaximumAttempts(int value);
        double GetBreakingAttemptDecayRate();
        void SetBreakingAttemptDecayRate(double value);
        double GetBreakingSpacerPenalty();
        void SetBreakingSpacerPenalty(double value);
        double GetBreakingPowerHoldPenalty();
        void SetBreakingPowerHoldPenalty(double value);
        double GetBreakingJudgeWeight();
        void SetBreakingJudgeWeight(double value);
        int GetBreakingMaxScore();
        void SetBreakingMaxScore(int value);

        string GetPossibleBoardWidthsAsString();
        string GetPossibleBoardDepthsAsString();
        OperationResult SetPossibleBoardDepths(string sourceString); 
        OperationResult SetPossibleBoardDepths(List<double> newVals);
        OperationResult SetPossibleBoardWidths(string sourceString);
        OperationResult SetPossibleBoardWidths(List<double> newVals);
    }
}
