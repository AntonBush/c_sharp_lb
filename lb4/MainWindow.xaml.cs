using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            var clients = new List<bank.Client>();
            clients.Add(new lb3.bank.Client(0, new bank.PersonName("Anton", "Bushev", "Aleks"), 22, "NONE"));
            client_list.ItemsSource = clients;
        }

        void clickNewButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New");
        }

        void clickOpenButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open");
        }

        void clickRemoveButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Remove");
        }
    }
}
