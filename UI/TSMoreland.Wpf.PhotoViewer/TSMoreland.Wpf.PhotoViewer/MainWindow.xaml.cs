﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TSMoreland.Wpf.PhotoViewer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly List<string> _files = new();
    private int _index = -1;
    private readonly Random _random = new();
    private readonly List<string> _toDelete = new();
    private byte[]? _buffer = null;

    public MainWindow()
    {
        InitializeComponent();
        Background = Brushes.DarkGray;

        KeyDown += MainWindow_KeyDown;
        MouseDown += MainWindow_MouseDown;
        Loaded += MainWindow_Loaded;

    }


    protected override void OnClosing(CancelEventArgs e)
    {
        ImageSource = null!;

        int count = 0;

        MainGrid.Children.Clear();

        if (_toDelete.Count == 0)
        {
            return;
        }

        System.Threading.Thread.Sleep(500);
        _files.Clear();

        MessageBox.Show($"Deleting {_toDelete.Count} files.");
        foreach (string filename in _toDelete)
        {
            try
            {
                File.Delete(filename);
                count++;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                MessageBox.Show(ex.Message);
            }
        }

        string location = Path.GetDirectoryName(AppContext.BaseDirectory)!;

        string deleteCommands = string.Join(Environment.NewLine, _toDelete.Select(f => $"del {f}"));
        File.WriteAllText(Path.Combine(location, "delete.bat"), $"@echo off{Environment.NewLine}{deleteCommands}");

        if (count > 0)
        {
            MessageBox.Show($"Removed {count} files.", "Files deleted.");
        }

        base.OnClosing(e);
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
            new FrameworkPropertyMetadata(true));

    private ValueTask LoadImage()
    {
        if (_index == -1)
        {
            return ValueTask.CompletedTask;
        }

        try
        {
            WindowState = WindowState.Normal;
            Left = 0;
            Top = 0;
            Width = SystemParameters.PrimaryScreenWidth - 0.5;
            Height = SystemParameters.PrimaryScreenWidth - 0.5;

            if (ImageSource is BitmapImage { StreamSource: MemoryStream stream })
            {
                if (_buffer is not null)
                {
                    ArrayPool<byte>.Shared.Return(_buffer);
                }
                stream.Dispose();
            }

            ImageSource = null!;

            long size = new FileInfo(_files[_index]).Length;
            _buffer = ArrayPool<byte>.Shared.Rent((int)size);

            using (FileStream fs = new(_files[_index], FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
            {
                size = fs.Read(_buffer, 0, (int)size);
                fs.Close();
            }

            MemoryStream ms = new(_buffer, 0, (int)size);

            BitmapImage image = new();
            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();

            ImageSource = image;

            Title = _files[_index];

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenWidth;
        }
        catch (Exception)
        {
            MessageBox.Show($"Failed to read file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

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
        else if (e.Key == Key.Delete)
        {
            int index = _index;
            _index++;
            string filename = _files[index];
            if (index != _index)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Delete {filename}?",
                    "Are you sure?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await RefreshImage();
                    _files.Remove(filename);
                    try
                    {
                        File.Delete(filename);
                        MessageBox.Show($"File {filename} removed.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.GetType().Name}: {ex.Message} {ex.InnerException} ");
                        _toDelete.Add(filename);
                    }
                }
            }
            return;
        }
        else
        {
            return;
        }

        await RefreshImage();
    }
    private async void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _index++;
        await RefreshImage();
    }

    private async Task RefreshImage()
    {
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
        WindowState = WindowState.Normal;
        Left = 0;
        Top = 0;
        Width = SystemParameters.PrimaryScreenWidth;
        Height = SystemParameters.PrimaryScreenWidth;
    }

    private async void SelectFileOrFolder_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog()
        {
            Multiselect = true,
            Filter = "Jpeg Images (.jpg)|*.jpg",
            InitialDirectory = Environment.CurrentDirectory,
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
            Viewer.Visibility = Visibility.Visible;
        }
        else
        {
            _index = -1;
        }
        await LoadImage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
