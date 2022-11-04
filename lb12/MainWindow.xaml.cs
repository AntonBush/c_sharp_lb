using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static lb12.TicTacToe;

namespace lb12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public TicTacToe game { get; } = new TicTacToe();
        public Player current_player { get => game.current_player; }
 
        public Player left_top { get => game.cells.left_top; }
        public Player center_top { get => game.cells.center_top; }
        public Player right_top { get => game.cells.right_top; }

        public Player left_center { get => game.cells.left_center; }
        public Player center { get => game.cells.center; }
        public Player right_center { get => game.cells.right_center; }

        public Player left_bottom { get => game.cells.left_bottom; }
        public Player center_bottom { get => game.cells.center_bottom; }
        public Player right_bottom { get => game.cells.right_bottom; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void clickCell(object sender, RoutedEventArgs e)
        {
            var cell_n = int.Parse(((Control)sender).Name[1..]);
            var status = game.setCell(Cell.cells[cell_n]);
            switch (status)
            {
                case GameStatus.player_1_won:
                    MessageBox.Show($"Player {Player.player_1} won");
                    break;
                case GameStatus.player_2_won:
                    MessageBox.Show($"Player {Player.player_2} won");
                    break;
                case GameStatus.friendship_won:
                    MessageBox.Show("Friendship is magic");
                    break;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(current_player)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(left_top)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(center_top)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(right_top)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(left_center)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(center)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(right_center)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(left_bottom)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(center_bottom)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(right_bottom)));
        }

    }
}
