using System.Diagnostics;
using MediaToolkit.Model;
using MediaToolkit;
using ImageProcessor.Imaging.Formats;
using ImageProcessor;
using MediaToolkit.Util;
using Aspose.Words;

namespace StuxxTools;

public partial class MainPage : ContentPage
{
    FileBase pickedFile;
    string docExtension;
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
                PickerTitle = "Select an audio file",
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
                PickerTitle = "Select a video file",
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


    // ===============
    // IMAGE CONVERTER
    // ===============

    private void imageIndexChanged(object sender, EventArgs e)
    {
        if (imageFormatPickerSource.SelectedIndex != -1)
        {
            imageInfoText.Text = "Pick an " + (string)imageFormatPickerSource.ItemsSource[imageFormatPickerSource.SelectedIndex] + " file.";
        }
    }

    private async void imageSelectionClick(object sender, EventArgs e)
    {
        if (imageFormatPickerSource.SelectedIndex != -1)
        {
            pickedFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*"+ (string)imageFormatPickerSource.ItemsSource[imageFormatPickerSource.SelectedIndex] } },
                { DevicePlatform.Android, new [] { "image/*" } },
                { DevicePlatform.iOS, new[] { "public.image" } },
                { DevicePlatform.MacCatalyst, new[] { "public.image" } }
                })
            });


            if (pickedFile == null)
                return;

            var imageName = "File name: " + pickedFile.FileName;

            imageFileName.Text = imageName;
        }
        else
        {
            await DisplayAlert("Alert", "Pick an image format before choosing a file.", "OK");
        }


    }

    private void imageOpenFileLocation(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
    }


    private async void imageConvertClick(object sender, EventArgs e)
    {
        if (imageFormatPickerDest.SelectedIndex != -1 && imageFormatPickerSource.SelectedIndex != -1)
        {

            string extension = (string)imageFormatPickerDest.ItemsSource[imageFormatPickerDest.SelectedIndex];
            string filename = imageFileName.Text.Remove(0, 11);
            string sDestinationFile = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\[" + extension.ToUpper().Remove(0, 1) + "] " + filename.Remove(filename.Length - 4, 4) + extension;

            ISupportedImageFormat format = null;

            if (extension == ".jpeg")
            {
                format = new JpegFormat { Quality = 70 };
            }

            if (extension == ".png")
            {
                format = new PngFormat { Quality = 70 };
            }

            if (extension == ".jpg")
            {
                format = new JpegFormat { Quality = 70 };
            }

            if (extension == ".bmp")
            {
                format = new BitmapFormat { Quality = 70 };
            }

            if (extension == ".gif")
            {
                format = new GifFormat { Quality = 70 };
            }

            if (extension == ".tiff")
            {
                format = new TiffFormat { Quality = 70 };
            }

            Size size = new Size(150, 0);
            using (FileStream inStream = new FileStream(pickedFile.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream outStream = new FileStream(sDestinationFile, FileMode.Create))
                {

                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {

                        imageFactory.Load(inStream)
                                    .Format(format)
                                    .Save(outStream);
                    }
                }
            }

            imagePath.Text = "Path: " + sDestinationFile;

            await DisplayAlert("Status", "Conversion complete", "OK");

            imageStatus.Text = "Status: Done!";
        }

        else
        {
            await DisplayAlert("Alert", "Pick an image format before converting a file.", "OK");
        }
    }

    // ===============
    // DOC CONVERTER
    // ===============

    private void Swap(object sender, EventArgs e)
    {
        docInfo.Text = docInfo.Text.Equals("PDF to Word") ? "Word to PDF" : "PDF to Word";
    }

    private void docOpenFileLocation(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
    }

    private async void docSelectionClick(object sender, EventArgs e)
    {
        if (docInfo.Text.Equals("PDF to Word"))
        {
            pickedFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a PDF file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*.pdf"} },
                { DevicePlatform.Android, new [] { "documents/*" } },
                { DevicePlatform.iOS, new[] { "public.documents" } },
                { DevicePlatform.MacCatalyst, new[] { "public.documents" } }
                })
            });


            if (pickedFile == null)
                return;

            docExtension = ".docx";
        }

        if (docInfo.Text.Equals("Word to PDF"))
        {
            pickedFile = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a word document file",
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.WinUI, new [] { "*.docx"} },
                { DevicePlatform.Android, new [] { "documents/*" } },
                { DevicePlatform.iOS, new[] { "public.documents" } },
                { DevicePlatform.MacCatalyst, new[] { "public.documents" } }
                })
            });


            if (pickedFile == null)
                return;


            docExtension = ".pdf";

        }

    }

    private async void docConvertClick(object sender, EventArgs e)
    {
        if (docInfo.Text == "PDF to Word")
        {
            string filename = pickedFile.FileName;
            string sDestinationFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\[" + docExtension.ToUpper().Remove(0, 1) + "] " + filename.Remove(filename.Length - 4, 4) + docExtension;

            Aspose.Words.Document doc = new Aspose.Words.Document(pickedFile.FullPath);
            doc.Save(sDestinationFile);

            await DisplayAlert("Status", "Conversion complete", "OK");
        }

    }


}




