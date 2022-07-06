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
        private int[,] _grid;
        private int _currentPlayer;

        public Game()
        {
            _grid = new int[_gridSizeX, _gridSizeY];
            _currentPlayer = 0;
        }

        #region Helper Methods
        private int GetNextYPos(int xPos)
        {
            throw new NotImplementedException();
        }
        #endregion Helper Methods

        #region GameCore
        public Task StartGame()
        {
            throw new NotImplementedException();
        }

        private void PlayerChange()
        {
            CheckForWin();
            _currentPlayer = (_currentPlayer + 1) % 2;
            throw new NotImplementedException();
        }

        private int CheckForWin()
        {
            throw new NotImplementedException();
        }
        #endregion GameCore

        #region Player
        public void PlayerAction(int player, int xPos)
        {
            throw new NotImplementedException();
        }

        private void ValidateAction(int xPos)
        {
            // Check if it is the players turn

            if (GetNextYPos(xPos) == -1)
            {
                throw new ArgumentException("The column is already full");
            }
        }
        #endregion Player
    }
}
