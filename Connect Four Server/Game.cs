using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Four_Server
{
    internal class Game
    {
        private const int _gridSizeX = 7;
        private const int _gridSizeY = 6;
        private int?[,] _grid;
        private int _currentPlayer;

        public int CurrentPlayer
        {
            get
            {
                return _currentPlayer;
            }
        }

        public bool GameIsRunning
        {
            get; private set;
        }

        public event EventHandler OnGameStarted;
        public event EventHandler OnGameEnded;
        public event EventHandler OnNextRound;

        public Game()
        {
            _grid = new int?[_gridSizeX, _gridSizeY];
            _currentPlayer = new Random().Next(2);
            GameIsRunning = true;
        }

        #region Helper Methods
        private int GetNextYPos(int xPos)
        {
            for (int yPos = 0; yPos < _gridSizeY; yPos++)
            {
                if (_grid[xPos, yPos] != null)
                {
                    return yPos - 1;
                }
            }

            return _gridSizeY - 1;
        }

        public string PrintGrid()
        {
            string grid = " ";

            for (int i = 1; i < _gridSizeX; i++)
            {
                grid += i + " + ";
            }
            grid += _gridSizeX + "\n";

            for (int yPos = 0; yPos < _gridSizeY; yPos++)
            {
                string[] element = new string[_gridSizeX];
                for (int xPos = 0; xPos < _gridSizeX; xPos++)
                {
                    element[xPos] += ((_grid[xPos, yPos] == null) ? " " :
                        (_grid[xPos, yPos] == 0) ? "x" : "o");
                }
                grid += " " + string.Join(" | ", element) + "\n";

                if (yPos != _gridSizeY - 1)
                    grid = grid + string.Join("+", Enumerable.Repeat("---", _gridSizeX)) + "\n";
            }

            return grid;
        }
        #endregion Helper Methods

        #region GameCore
        private void NextRound()
        {
            _currentPlayer = (_currentPlayer + 1) % 2;
            OnNextRound?.Invoke(this, EventArgs.Empty);
        }

        private int CheckForWinOrDraw()
        {
            for (int currentPlayer = 0; currentPlayer < 2; currentPlayer++)
            {
                for (int xPos = 0; xPos < _gridSizeX; xPos++)
                {
                    for (int yPos = 0; yPos < _gridSizeY; yPos++)
                    {
                        // Not working
                        if (
                            // horizontal
                            (xPos + 3 < _gridSizeX
                            && _grid[xPos, yPos] == currentPlayer && _grid[xPos + 1, yPos] == currentPlayer
                            && _grid[xPos + 2, yPos] == currentPlayer && _grid[xPos + 3, yPos] == currentPlayer)
                            ||
                            // vertical
                            (yPos + 3 < _gridSizeY
                            && _grid[xPos, yPos] == currentPlayer && _grid[xPos, yPos + 1] == currentPlayer
                            && _grid[xPos, yPos + 2] == currentPlayer && _grid[xPos, yPos + 3] == currentPlayer)
                            ||
                            //diagonal down right
                            (xPos + 3 < _gridSizeX && yPos + 3 < _gridSizeY
                            && _grid[xPos, yPos] == currentPlayer && _grid[xPos + 1, yPos + 1] == currentPlayer
                            && _grid[xPos + 2, yPos + 2] == currentPlayer && _grid[xPos + 3, yPos + 3] == currentPlayer)
                            ||
                            //diagonal down left
                            (xPos - 3 >= 0 && yPos + 3 < _gridSizeY
                            && _grid[xPos, yPos] == currentPlayer && _grid[xPos - 1, yPos + 1] == currentPlayer
                            && _grid[xPos - 2, yPos + 2] == currentPlayer && _grid[xPos - 3, yPos + 3] == currentPlayer))
                        {
                            OnGameEnded?.Invoke(this, EventArgs.Empty);
                            GameIsRunning = false;
                            return currentPlayer;
                        }
                    }
                }
            }
            return -1;
        }
        #endregion GameCore

        #region Player
        public void PlayerAction(int player, int xPos)
        {
            ValidateAction(player, xPos);
            _grid[xPos, GetNextYPos(xPos)] = player;
            NextRound();
        }

        private void ValidateAction(int player, int xPos)
        {
            if (!GameIsRunning)
                throw new InvalidPlayerAction("The game is over");

            if (player != _currentPlayer)
                throw new InvalidPlayerAction("The player is not on the move");

            if (GetNextYPos(xPos) == -1)
                throw new InvalidPlayerAction("The column is already filled up");
        }
        #endregion Player
    }
}
