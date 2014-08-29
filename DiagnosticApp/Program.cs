namespace DiagnosticApp
{
    using Battleships.ExamplePlayer;
    using Battleships.Player.Interface;
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var player = new ExamplePlayer();
            var listOfPositions = player.GetShipPositions();
            var ships = player.GetGrid();

            for (var r = 'A'; r <= 'J'; r++)
            {
                for (var c = 1; c <= 10; c++)
                {
                    Console.Write(ships.Contains(new GridSquare(r, c)) ? "+" : "O");
                }
                Console.Write('\n');
            }

            foreach (var gridSquare in ships)
            {
                var square = (GridSquare)gridSquare;
                Console.WriteLine(square.Column + " " + square.Row);
            }

            Console.Read();
        }
    }
}