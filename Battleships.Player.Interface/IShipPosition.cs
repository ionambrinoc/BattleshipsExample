namespace Battleships.Player.Interface
{
    /// <summary>
    ///     Objects returned by GetShipPositions must implement this interface. Each
    ///     object should represent the position of a single ship.
    /// </summary>
    public interface IShipPosition
    {
        IGridSquare StartingSquare { get; }
        IGridSquare EndingSquare { get; }
    }
}