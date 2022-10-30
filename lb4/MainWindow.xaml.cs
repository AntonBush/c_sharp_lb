using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using bank = lb3.bank;

namespace lb4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<bank.Client> clients { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            clients = new ObservableCollection<bank.Client>();
        }

        void clickNewButton(object sender, RoutedEventArgs e)
        {
            clients.Add(new bank.Client(_client_id_counter++, new bank.PersonName("Anton", "Bushev", "Aleks"), 23, "NONE"));
        }

        void clickOpenButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open");
        }

        void clickRemoveButton(object sender, RoutedEventArgs e)
        {
            var selected_item = (bank.Client?)client_list.SelectedItem;
            if (selected_item == null)
            {
                MessageBox.Show("Nothing selected");
                return;
            }

            clients.Remove(selected_item);
        }

        uint _client_id_counter = 0;
    }
}
