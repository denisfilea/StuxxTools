using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace StuxxTools;

public partial class MainPage : ContentPage
{
    FileBase pickedAudio;
	public MainPage()
	{
		InitializeComponent();
	}

	private async void SelectionClick(object sender, EventArgs e)
	{
        if (formatPickerSource.SelectedIndex != -1)
        {
            pickedAudio = await FilePicker.Default.PickAsync(new PickOptions
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


            if (pickedAudio == null)
                return;

            var audioname = "File name: " + pickedAudio.FileName;

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
            infoText.Text = "Please pick an " + (string)formatPickerSource.ItemsSource[formatPickerSource.SelectedIndex] + " file.";
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
            Options.Core.SourceFile sourceFile = new Options.Core.SourceFile(pickedAudio.FullPath);
            audioConverter1.SourceFiles.Add(sourceFile);
            audioConverter1.Convert();

            Path.Text = sDestinationFile;

            await DisplayAlert("Status", "Conversion complete", "OK");
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

}




