﻿namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public interface IMatchResultsRepository : IRepository<MatchResult>
    {
        void UpdateResults(List<MatchResult> results);
        DateTime GetMostRecentMatchTime();
    }

    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(DbContext context) : base(context) {}

        public void UpdateResults(List<MatchResult> results)
        {
            var newResults = results.Where(result => !TryUpdateResult(result)).ToList();
            AddResults(newResults);
        }

        public DateTime GetMostRecentMatchTime()
        {
            try
            {
                return GetAll().Max(x => x.TimePlayed);
            }
            catch (InvalidOperationException)
            {
                return DateTime.MinValue;
            }
        }

        private bool TryUpdateResult(MatchResult result)
        {
            foreach (var existingResult in Entities.Where(entity => result.Winner.Id == entity.Winner.Id || result.Winner.Id == entity.Loser.Id))
            {
                if (result.Loser.Id == existingResult.Winner.Id || result.Loser.Id == existingResult.Loser.Id)
                {
                    existingResult.Winner = result.Winner;
                    existingResult.Loser = result.Loser;
                    existingResult.WinnerWins = result.WinnerWins;
                    existingResult.LoserWins = result.LoserWins;
                    existingResult.TimePlayed = result.TimePlayed;
                    return true;
                }
            }
            return false;
        }

        private void AddResults(List<MatchResult> newResults)
        {
            Entities.AddRange(newResults);
        }
    }
}