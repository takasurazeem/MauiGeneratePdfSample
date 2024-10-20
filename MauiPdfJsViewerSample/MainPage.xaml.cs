using iText.IO.Image;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using System.Net;
using iText.Layout;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout.Font;
using iText.Layout.Properties;
using System.Xml;

namespace MauiPdfJsViewerSample
{
    public partial class MainPage : ContentPage
    {
        string TheOpeningText = "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string fileName = "mauidotnet.pdf";
            string arabicText = "بسم الله الرحمن الرحيم"; // Sample Arabic text

            // Define the file path for different platforms
#if ANDROID
            var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            var filePath = Path.Combine(docsDirectory.AbsoluteFile.Path, fileName);
#else
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
#endif

            try
            {
                // Get the application directory
                string appDirectory = FileSystem.AppDataDirectory;

                // Construct the full path to your font file
                string fontFilePath = Path.Combine(appDirectory, "Resources", "Raw", "pdms-saleem-quranfont.ttf");

                PdfWriter writer = new PdfWriter(filePath);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument);
                FontSet set = new FontSet();
                set.AddFont(fontFilePath);
                //set.AddFont("NotoSansTamil-Regular.ttf");
                //set.AddFont("FreeSans.ttf");
                document.SetFontProvider(new FontProvider(set));
                document.SetProperty(Property.FONT, new String[] { "_PDMS_Saleem_QuranFont" });
                Paragraph paragraph = new Paragraph();
                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                paragraph.Add(TheOpeningText);
                document.Add(paragraph);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"PDF generation failed: {ex.Message}", "OK");
            }


            // Display the PDF in a WebView or PDF viewer
#if ANDROID
            // Assuming you are using PDF.js for viewing the PDF inside a WebView
            pdfview.Source = $"file:///android_asset/pdfjs/web/viewer.html?file=file://{WebUtility.UrlEncode(filePath)}";
#else
            pdfview.Source = filePath;  // For non-Android platforms, just set the source to the file path
#endif
        }


        private async Task<byte[]> ConvertImageSourceToStreamAsync(string imageName)
        {
            using var ms = new MemoryStream();
            using (var stream = await FileSystem.OpenAppPackageFileAsync(imageName))
                await stream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }

}
