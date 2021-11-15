using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TSMoreland.Wpf.PhotoViewer.Annotations;

namespace TSMoreland.Wpf.PhotoViewer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly List<string> _files = new();
    private int _index = -1;
    private bool _shuffle;
    private readonly Random _random = new ();

    public MainWindow()
    {
        InitializeComponent();
        Background = Brushes.DarkGray;

        this.KeyDown += MainWindow_KeyDown;
        this.Loaded += MainWindow_Loaded;

    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set 
        { 
            SetValue(ImageSourceProperty, value);
            OnPropertyChanged();
        }
    }
    public bool Shuffle 
    {
        get => (bool)GetValue(ShuffleProperty);
        set 
        { 
            SetValue(ShuffleProperty, value);
            OnPropertyChanged();
        }
    }

    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(MainWindow),
            new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty ShuffleProperty =
        DependencyProperty.Register(nameof(Shuffle), typeof(bool), typeof(MainWindow),
            new FrameworkPropertyMetadata(false));

    private ValueTask LoadImage()
    {
        if (_index == -1)
        {
            return ValueTask.CompletedTask;
        }

        this.WindowState = WindowState.Normal;
        this.Left = 0;
        this.Top = 0;
        this.Width = SystemParameters.PrimaryScreenWidth - 0.5;
        this.Height = SystemParameters.PrimaryScreenWidth - 0.5;

        var uri = new Uri(_files[_index]);
        ImageSource = new BitmapImage(uri);
        Title = _files[_index];

        this.Width = SystemParameters.PrimaryScreenWidth;
        this.Height = SystemParameters.PrimaryScreenWidth;

        return ValueTask.CompletedTask;
    }

    private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left)
        {
            _index--;
        }
        else if (e.Key == Key.Right)
        {
            _index++;
        }
        else
        {
            return;
        }

        if (!_files.Any())
        {
            _index = -1;
        }
        else
        {
            if (_index < 0)
            {
                _index = _files.Count - 1;
            }

            if (_index >= _files.Count)
            {
                _index = 0;
            }
        }

        await LoadImage();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;
        this.Left = 0;
        this.Top = 0;
        this.Width = SystemParameters.PrimaryScreenWidth - 1;
        this.Height = SystemParameters.PrimaryScreenWidth - 1;
    }

    private async void SelectFileOrFolder_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog()
        {
            Multiselect = true,
            Filter = "Jpeg Images (.jpg)|*.jpg",
        };

        bool? result = dialog.ShowDialog();
        if (result != true)
        {
            return;
        }

        _files.Clear();
        if (!Shuffle)
        {
            _files.AddRange(dialog.FileNames);
        }
        else
        {
            var fileList = new List<string>(dialog.FileNames);
            while (fileList.Any())
            {
                int index = _random.Next(0, fileList.Count);
                _files.Add(fileList[index]);
                fileList.RemoveAt(index);
            }

            _files.AddRange(dialog.FileNames);
        }

        if (_files.Any())
        {
            _index = 0;
            ControlPanel.Visibility = Visibility.Collapsed;
        }
        else
        {
            _index = -1;
        }
        await LoadImage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}