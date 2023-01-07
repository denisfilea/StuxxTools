using System.Diagnostics;
using Xabe.FFmpeg;
using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Model;
using MediaToolkit;

namespace StuxxTools;

public partial class MainPage : ContentPage
{
    FileBase pickedFile;
    public MainPage()
    {
        InitializeComponent();
    }

    // ===============
    // AUDIO CONVERTER
    // ===============

    private async void SelectionClick(object sender, EventArgs e)
    {
        if (formatPickerSource.SelectedIndex != -1)
        {
            pickedFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select audio file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*"+ (string)formatPickerSource.ItemsSource[formatPickerSource.SelectedIndex] } },
                { DevicePlatform.Android, new [] { "audio/*" } },
                { DevicePlatform.iOS, new[] { "public.audio" } },
                { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
                })
            });


            if (pickedFile == null)
                return;

            var audioname = "File name: " + pickedFile.FileName;

            fileName.Text = audioname;
        }
        else
        {
            await DisplayAlert("Alert", "Pick an audio format before choosing a file.", "OK");
        }
    }

    private void IndexChanged(object sender, EventArgs e)
    {
        if (formatPickerSource.SelectedIndex != -1)
        {
            infoText.Text = "Pick an " + (string)formatPickerSource.ItemsSource[formatPickerSource.SelectedIndex] + " file.";
        }
    }

    private async void ConvertClick(object sender, EventArgs e)
    {
        if (formatPickerDest.SelectedIndex != -1 && formatPickerSource.SelectedIndex != -1)
        {
            CSAudioConverter.AudioConverter audioConverter1 = new CSAudioConverter.AudioConverter();
            string extension = (string)formatPickerDest.ItemsSource[formatPickerDest.SelectedIndex];
            string filename = fileName.Text.Remove(0, 11);
            string sDestinationFile = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\[" + extension.ToUpper().Remove(0, 1) + "] " + filename.Remove(filename.Length - 4, 4) + extension;


            audioConverter1.DecodeMode = (CSAudioConverter.DecodeMode)Enum.Parse(typeof(CSAudioConverter.DecodeMode), "LocalCodecs");
            audioConverter1.DestinatioFile = sDestinationFile;
            audioConverter1.Format = (CSAudioConverter.Format)Enum.Parse(typeof(CSAudioConverter.Format), extension.ToUpper().Remove(0, 1));

            audioConverter1.SourceFiles.Clear();
            Options.Core.SourceFile sourceFile = new Options.Core.SourceFile(pickedFile.FullPath);
            audioConverter1.SourceFiles.Add(sourceFile);
            audioConverter1.Convert();

            Path.Text = "Path: " + sDestinationFile;

            await DisplayAlert("Status", "Conversion complete", "OK");

            Status.Text = "Status: Done!";
        }
        else
        {
            await DisplayAlert("Alert", "Pick an audio format before converting a file.", "OK");
        }
    }

    private void OpenFileLocation(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
    }


    // ===============
    // VIDEO CONVERTER
    // ===============

    private void videoIndexChanged(object sender, EventArgs e)
    {
        if (videoFormatPickerSource.SelectedIndex != -1)
        {
            videoInfoText.Text = "Pick an " + (string)videoFormatPickerSource.ItemsSource[videoFormatPickerSource.SelectedIndex] + " file.";
        }
    }

    private async void videoSelectionClick(object sender, EventArgs e)
    {
        if (videoFormatPickerSource.SelectedIndex != -1)
        {
            pickedFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select video file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*"+ (string)videoFormatPickerSource.ItemsSource[videoFormatPickerSource.SelectedIndex] } },
                { DevicePlatform.Android, new [] { "video/*" } },
                { DevicePlatform.iOS, new[] { "public.video" } },
                { DevicePlatform.MacCatalyst, new[] { "public.video" } }
                })
            });


            if (pickedFile == null)
                return;

            var videoname = "File name: " + pickedFile.FileName;

            videoFileName.Text = videoname;
        }
        else
        {
            await DisplayAlert("Alert", "Pick a video format before choosing a file.", "OK");
        }


    }

    private void videoOpenFileLocation(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
    }


    private async void videoConvertClick(object sender, EventArgs e)
    {
        if (videoFormatPickerDest.SelectedIndex != -1 && videoFormatPickerSource.SelectedIndex != -1)
        {

            string extension = (string)videoFormatPickerDest.ItemsSource[videoFormatPickerDest.SelectedIndex];
            string filename = videoFileName.Text.Remove(0, 11);
            string sDestinationFile = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\[" + extension.ToUpper().Remove(0, 1) + "] " + filename.Remove(filename.Length - 4, 4) + extension;

            var inputFile = new MediaFile { Filename = pickedFile.FullPath };
            var outputFile = new MediaFile { Filename = sDestinationFile };

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }

            videoPath.Text = "Path: " + sDestinationFile;

            await DisplayAlert("Status", "Conversion complete", "OK");

            videoStatus.Text = "Status: Done!";
        }

        else
        {
            await DisplayAlert("Alert", "Pick a video format before converting a file.", "OK");
        }
    }




}




