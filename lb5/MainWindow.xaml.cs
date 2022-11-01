using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace lb5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> words { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> found_words { get; set; } = new ObservableCollection<string>();
        public string load_time {
            get => _load_time;
            set {
                _load_time = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("load_time"));
                }
            }
        }
        public string search_time
        {
            get => _search_time;
            set
            {
                _search_time = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("search_time"));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _load_time = "0";
            _search_time = "0";
            load_time = _load_time;
            search_time = _search_time;
        }

        string _load_time;
        string _search_time;

        void clickLoadFileButton(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                words.Clear();
                found_words.Clear();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                {
                    // Open document
                    foreach (var str in File.ReadAllText(dialog.FileName).Split().Select(s => s.Trim()).Where(s => s != ""))
                    {
                        if (!words.Contains(str))
                        {
                            words.Add(str);
                        }
                    }
                }
                stopwatch.Stop();
                load_time = stopwatch.Elapsed.ToString();
            }
        }

        void clickFindWordButton(object sender, RoutedEventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var found_word = words.ToList().Find(s => s.Contains(find_word_textbox.Text));
            stopwatch.Stop();
            if (found_word != null)
            {
                search_time = stopwatch.Elapsed.ToString();
                if (!found_words.Contains(found_word))
                {
                    found_words.Add(found_word);
                }
            }
        }
    }
}
