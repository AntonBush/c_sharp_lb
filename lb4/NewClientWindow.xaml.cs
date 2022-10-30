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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bank = lb3.bank;

namespace lb4;

/// <summary>
/// Логика взаимодействия для NewClientWindow.xaml
/// </summary>
public partial class NewClientWindow : Window
{
    public event EventHandler<NewClientEventArgs>? client_created;
    public NewClientWindow()
    {
        InitializeComponent();
        Closed += (_, _) => { _handleClientCreated(new NewClientEventArgs(null)); };
    }

    void clickSaveButton(object sender, RoutedEventArgs e)
    {
        if (_isValid())
        {
            Closed += (_, _) => { _onClientCreated(); };
            Close();
        }
    }

    void clickCancelButton(object sender, RoutedEventArgs e)
    {
        Close();
    }

    static uint _client_id_counter = 0;

    bool _isValid()
    {
        string error_message = "";
        bool is_valid = true;
        if (first_name_textbox.Text == "")
        {
            error_message += "First name must not be empty\n";
            is_valid = false;
        }
        if (second_name_textbox.Text == "")
        {
            error_message += "Second name must not be empty\n";
            is_valid = false;
        }
        if (patronymic_textbox.Text == "")
        {
            error_message += "Patronymic must not be empty\n";
            is_valid = false;
        }
        if (age_textbox.Text == "")
        {
            error_message += "Age must not be empty\n";
            is_valid = false;
        }
        uint age;
        if (!uint.TryParse(age_textbox.Text, out age))
        {
            error_message += "Age must be unsigned integer\n";
            is_valid = false;
        }
        if (workplace_textbox.Text == "")
        {
            error_message += "Workplace must not be empty\n";
            is_valid = false;
        }
        if (error_message != "")
        {
            MessageBox.Show(error_message);
        }
        return is_valid;
    }

    void _handleClientCreated(NewClientEventArgs args)
    {
        var handler = client_created;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    void _onClientCreated()
    {
        var client = new bank.Client(_client_id_counter++
                                    , new bank.PersonName(first_name_textbox.Text
                                                            , second_name_textbox.Text
                                                            , patronymic_textbox.Text
                                                            )
                                    , uint.Parse(age_textbox.Text)
                                    , workplace_textbox.Text
                                    );
        _handleClientCreated(new NewClientEventArgs(client));
    }
}

public class NewClientEventArgs : EventArgs
{
    public bank.Client? client { get; set; }
    public NewClientEventArgs(bank.Client? client) => this.client = client;
}
