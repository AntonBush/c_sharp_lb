using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lb12
{
    public class TicTacToe
    {
        public class Cell
        {
            public static Cell left_top = new(0, 0);
            public static Cell center_top = new(0, 1);
            public static Cell right_top = new(0, 2);

            public static Cell left_center = new(1, 0);
            public static Cell center = new(1, 1);
            public static Cell right_center = new(1, 2);

            public static Cell left_bottom = new(2, 0);
            public static Cell center_bottom = new(2, 1);
            public static Cell right_bottom = new(2, 2);

            public static Cell[] cells = new Cell[9] {
                left_top, center_top, right_top,
                left_center, center, right_center,
                left_bottom, center_bottom, right_bottom,
            };

            public int row { get; }
            public int column { get; }

            Cell(int row, int column)
            {
                this.row = row;
                this.column = column;
            }
        }

        public class Player
        {
            public static Player none = new("");
            public static Player player_1 = new("◯");
            public static Player player_2 = new("❌");

            Player(string s)
            {
                _s = s;
            }
            public override string ToString()
            {
                return _s;
            }

            readonly string _s;
        }

        public enum GameStatus
        {
            none,
            friendship_won,
            player_1_won,
            player_2_won
        }

        public struct Cells
        {
            public Player left_top { get; set; } = Player.none;
            public Player center_top { get; set; } = Player.none;
            public Player right_top { get; set; } = Player.none;

            public Player left_center { get; set; } = Player.none;
            public Player center { get; set; } = Player.none;
            public Player right_center { get; set; } = Player.none;

            public Player left_bottom { get; set; } = Player.none;
            public Player center_bottom { get; set; } = Player.none;
            public Player right_bottom { get; set; } = Player.none;

            public Cells()
            {
            }
        }

        public Cells cells
        {
            get
            {
                Cells temp = new()
                {
                    left_top = _cells[0, 0],
                    center_top = _cells[0, 1],
                    right_top = _cells[0, 2],

                    left_center = _cells[1, 0],
                    center = _cells[1, 1],
                    right_center = _cells[1, 2],

                    left_bottom = _cells[2, 0],
                    center_bottom = _cells[2, 1],
                    right_bottom = _cells[2, 2]
                };

                return temp;
            }
        }

        public Player current_player { get; private set; } = Player.player_1;

        public TicTacToe()
        {
            reset();
        }

        public GameStatus setCell(Cell cell)
        {
            if (_cells[cell.row, cell.column] != Player.none)
            {
                return GameStatus.none;
            }

            _cells[cell.row, cell.column] = current_player;

            Player winner = _checkWinner();
            if (winner == Player.player_1)
            {
                reset();
                return GameStatus.player_1_won;
            }

            if (winner == Player.player_2)
            {
                reset();
                return GameStatus.player_2_won;
            }

            if (_checkFull())
            {
                reset();
                return GameStatus.friendship_won;
            }

            if (current_player == Player.player_1)
            {
                current_player = Player.player_2;
            }
            else
            {
                current_player = Player.player_1;
            }

            return GameStatus.none;
        }

        public void reset()
        {
            current_player = Player.player_1;
            foreach (var cell in Cell.cells)
            {
                _cells[cell.row, cell.column] = Player.none;
            }
        }

        Player[,] _cells = new Player[3, 3];

        Player _checkWinner()
        {
            if (_cells[1, 1] != Player.none)
            {
                if ((_cells[0, 0] == _cells[1, 1] && _cells[1, 1] == _cells[2, 2])
                   || (_cells[2, 0] == _cells[1, 1] && _cells[1, 1] == _cells[0, 2])
                   || (_cells[1, 0] == _cells[1, 1] && _cells[1, 1] == _cells[1, 2])
                   || (_cells[0, 1] == _cells[1, 1] && _cells[1, 1] == _cells[2, 1])
                   )
               {
                    return _cells[1, 1];
                }
            }
            if (_cells[0, 0] != Player.none)
            {
                if ((_cells[0, 0] == _cells[0, 1] && _cells[0, 1] == _cells[0, 2])
                    || (_cells[0, 0] == _cells[1, 0] && _cells[1, 0] == _cells[2, 0])
                    )
                {
                    return _cells[0, 0];
                }
            }
            if (_cells[2, 2] != Player.none)
            {
                if ((_cells[2, 0] == _cells[2, 1] && _cells[2, 1] == _cells[2, 2])
                    || (_cells[0, 2] == _cells[1, 2] && _cells[1, 2] == _cells[2, 2])
                    )
                {
                    return _cells[2, 2];
                }
            }
            return Player.none;
        }
        bool _checkFull()
        {
            foreach (var c in _cells)
            {
                if (c == Player.none)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
