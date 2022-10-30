using lb3.bank;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bank = lb3.bank;

namespace lb4;

/// <summary>
/// Логика взаимодействия для ClientWindow.xaml
/// </summary>
public partial class ClientWindow : Window
{
    public ObservableClient client { get; }
    public ObservableCollection<bank.Account> accounts { get; }

    public ClientWindow()
    {
        InitializeComponent();
        DataContext = this;
        account_list.DataContext = this;
        client = new ObservableClient();
        accounts = new ObservableCollection<bank.Account>();
    }

    public void setClient(bank.Client client)
    {
        this.client.client = client;
        _updateAccounts();
        if (_account_window != null)
        {
            _account_window.Close();
        }
    }

    void clickNewButton(object sender, RoutedEventArgs e)
    {
        if (client.accounts == null)
        {
            throw new Exception("Client is null");
        }

        client.accounts.Add(new bank.Account(_account_id_counter++));
        _updateAccounts();
    }

    void clickOpenButton(object sender, RoutedEventArgs e)
    {
        if (client.accounts == null)
        {
            throw new Exception("Client is null");
        }

        var selected_account = (bank.Account?)account_list.SelectedItem;
        if (selected_account == null)
        {
            MessageBox.Show("Nothing selected");
            return;
        }

        if (_account_window == null)
        {
            _account_window = new AccountWindow();
            EventHandler window_close_handler = (_, _) => { _account_window.Close(); };
            Closed += window_close_handler;
            _account_window.Closed += (_, _) => { _account_window = null; Closed -= window_close_handler; };
            _account_window.data_updated += (_, _) => { _updateAccounts(); };
            _account_window.Show();
        }

        _account_window.setAccount(selected_account);
    }

    void clickRemoveButton(object sender, RoutedEventArgs e)
    {
        if (client.accounts == null)
        {
            throw new Exception("Client is null");
        }

        var selected_account = (bank.Account?)account_list.SelectedItem;
        if (selected_account == null)
        {
            MessageBox.Show("Nothing selected");
            return;
        }

        client.accounts.Remove(selected_account);
        _updateAccounts();
    }

    static uint _account_id_counter = 0;
    AccountWindow? _account_window { get; set; }

    void _updateAccounts()
    {
        if (client.accounts == null)
        {
            throw new Exception("Client is null");
        }

        accounts.Clear();
        foreach (var account in client.accounts)
        {
            accounts.Add(account);
        }
    }
}

public class ObservableClient : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public uint? id { get { return _client?.id; } }
    public bank.PersonName? name { get { return _client?.name; } }
    public uint? age { get { return _client?.age; } }
    public string? workplace { get { return _client?.workplace; } }
    public List<bank.Account>? accounts { get { return _client?.accounts; } }

    public bank.Client? client
    {
        set
        {
            _client = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("id"));
                PropertyChanged(this, new PropertyChangedEventArgs("name"));
                PropertyChanged(this, new PropertyChangedEventArgs("age"));
                PropertyChanged(this, new PropertyChangedEventArgs("workplace"));
            }
        }
    }

    bank.Client? _client;
}
