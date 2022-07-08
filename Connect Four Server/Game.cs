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
        private string[,] _grid;
        private string _currentPlayer;

        public string CurrentPlayer
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

        public event EventHandler OnGameEnded;
        public event EventHandler OnNextRound;

        public Game()
        {
            _grid = new string[_gridSizeX, _gridSizeY];
            _currentPlayer = new Random().Next(2) == 0 ? "x" : "o";
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
                    element[xPos] += (_grid[xPos, yPos] == null ? " " : _grid[xPos, yPos]);
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
            _currentPlayer = _currentPlayer == "x" ? "o" : "x";
            OnNextRound?.Invoke(this, EventArgs.Empty);
        }

        private bool WinOrDraw()
        {
            string currentPlayer = "x";
            for (int i = 0; i < 2; i++)
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
                            GameIsRunning = false;
                            OnGameEnded?.Invoke(this, EventArgs.Empty);
                            return true;
                        }
                    }
                }
                currentPlayer = "o";
            }
            return false;
        }
        #endregion GameCore

        #region Player
        public void PlayerAction(string player, int xPos)
        {
            ValidateAction(player, xPos);
            _grid[xPos, GetNextYPos(xPos)] = player;
            if (WinOrDraw())
                return;
            NextRound();
        }

        private void ValidateAction(string player, int xPos)
        {
            if (!GameIsRunning)
                throw new InvalidPlayerAction("The game is over");

            if ((player != "x" && player != "o") || player != _currentPlayer)
                throw new InvalidPlayerAction("The player is not on the move");

            if (xPos < 0 || xPos >= _gridSizeX)
                throw new InvalidPlayerAction("The given position is out of range");

            if (GetNextYPos(xPos) == -1)
                throw new InvalidPlayerAction("The column is already filled up");
        }
        #endregion Player
    }
}
