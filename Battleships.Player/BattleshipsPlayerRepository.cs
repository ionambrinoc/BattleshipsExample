namespace Battleships.Player
{
    using Battleships.Core.Models;
    using System;

    public interface IBattleshipsPlayerRepository
    {
        PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer);
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId);
    }

    public class BattleshipsPlayerRepository : IBattleshipsPlayerRepository
    {
        public PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer)
        {
            throw new NotImplementedException();
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId)
        {
            throw new NotImplementedException();
        }
    }
}
