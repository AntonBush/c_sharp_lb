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

namespace lb4;

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
        var new_client_window = new NewClientWindow();
        new_client_window.client_created += new EventHandler<NewClientEventArgs>(registerNewClient);
        Closed += (_, _) => { new_client_window.Close(); };
        new_client_window.Show();
    }

    void registerNewClient(object? sender, NewClientEventArgs e)
    {
        if (e.client != null)
        {
            clients.Add(e.client);
        }
    }

    void clickOpenButton(object sender, RoutedEventArgs e)
    {
        var selected_client = (bank.Client?)client_list.SelectedItem;
        if (selected_client == null)
        {
            MessageBox.Show("Nothing selected");
            return;
        }

        if (_client_window == null)
        {
            _client_window = new ClientWindow();
            EventHandler main_window_close_handler = (_, _) => { _client_window.Close(); };
            Closed += main_window_close_handler;
            _client_window.Closed += (_, _) => { _client_window = null; Closed -= main_window_close_handler; };
            _client_window.Show();
        }

        _client_window.setClient(selected_client);
   }

    void clickRemoveButton(object sender, RoutedEventArgs e)
    {
        var selected_client = (bank.Client?)client_list.SelectedItem;
        if (selected_client == null)
        {
            MessageBox.Show("Nothing selected");
            return;
        }

        clients.Remove(selected_client);
    }

    ClientWindow? _client_window { get; set; }
}
