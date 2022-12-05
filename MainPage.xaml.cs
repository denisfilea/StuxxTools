using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace StuxxTools;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void SelectionClick(object sender, EventArgs e)
	{
        if (formatPicker.SelectedIndex != -1)
        {
            var pickedAudio = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select audio file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*"+ (string)formatPicker.ItemsSource[formatPicker.SelectedIndex] } },
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
        if (formatPicker.SelectedIndex != -1)
        {
            infoText.Text = "Please pick a " + (string)formatPicker.ItemsSource[formatPicker.SelectedIndex] + " file.";
        }
    }

}




