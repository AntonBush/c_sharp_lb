using lb3.bank;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
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
/// Логика взаимодействия для AccountWindow.xaml
/// </summary>
public partial class AccountWindow : Window
{
    public event EventHandler? data_updated;
    public ObservableAccount account { get; }
    public ObservableCollection<string> history { get; }

    public AccountWindow()
    {
        InitializeComponent();
        DataContext = this;
        history_list.DataContext = this;
        account = new ObservableAccount();
        history = new ObservableCollection<string>();
    }
    public void setAccount(bank.Account account)
    {
        this.account.account = account;
        _updateHistory();
    }

    void clickDepositButton(object sender, RoutedEventArgs e)
    {
        if (!_isValid())
        {
            return;
        }

        uint money = uint.Parse(money_textbox.Text);
        account.deposit(money);

        _updateHistory();
    }
    void clickWithdrawButton(object sender, RoutedEventArgs e)
    {
        if (!_isValid())
        {
            return;
        }

        uint money = uint.Parse(money_textbox.Text);
        if (!account.withdraw(money))
        {
            MessageBox.Show("Not enough money on balance");
            return;
        }

        _updateHistory();
    }
    void _updateHistory()
    {
        if (account.history == null)
        {
            throw new Exception("Client is null");
        }

        history.Clear();
        foreach (var str in account.history)
        {
            history.Add(str);
        }
        _handleDataUpdated();
    }

    bool _isValid()
    {
        uint number;
        if (!uint.TryParse(money_textbox.Text, out number))
        {
            MessageBox.Show("Money must be unsigned integer");
            return false;
        }

        return true;
    }

    void _handleDataUpdated()
    {
        if (data_updated != null)
        {
            data_updated(this, new EventArgs());
        }
    }
}

public class ObservableAccount : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public uint? id { get { return _account?.id; } }
    public uint? balance { get { return _account?.balance; } }
    public List<string>? history { get { return _account?.history; } }

    public bank.Account? account
    {
        set
        {
            _account = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("id"));
                PropertyChanged(this, new PropertyChangedEventArgs("balance"));
            }
        }
    }

    public void deposit(uint money)
    {
        if (_account == null)
        {
            throw new Exception("Account is null");
        }

        _account.deposite(money);
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("balance"));
        }
    }

    public bool withdraw(uint money)
    {
        if (_account == null)
        {
            throw new Exception("Account is null");
        }

        var success = _account.withdraw(money);
        if (success && PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("balance"));
        }
        return success;
    }

    bank.Account? _account;
}

