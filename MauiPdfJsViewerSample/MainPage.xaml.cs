using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.Layout.Font;
using iText.Layout.Properties;
using iText.Licensing.Base;

namespace MauiPdfJsViewerSample
{
    public partial class MainPage : ContentPage
    {
        private const string TheOpeningText = "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string fileName = "mauidotnet.pdf";
            // Define the file path for different platforms
#if ANDROID
            var docsDirectory =
                Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            var filePath = Path.Combine(docsDirectory.AbsoluteFile.Path, fileName);
#else
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
#endif

            try
            {
                using var licenseResourceStream = await FileSystem.OpenAppPackageFileAsync("itextkey.json");
                if (licenseResourceStream is FileStream)
                {
                    string absolutePath = (licenseResourceStream as FileStream).Name;
                    LicenseKey.LoadLicenseFile(new FileInfo(absolutePath));
                }

                using var resourceStream = await FileSystem.OpenAppPackageFileAsync("pdms-saleem-quranfont.ttf");
                if (resourceStream is FileStream)
                {
                    string absolutePath = (resourceStream as FileStream).Name;
                    FontSet set = new FontSet();
                    var added = set.AddFont(absolutePath);
                    PdfWriter writer = new PdfWriter(filePath);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    Document document = new Document(pdfDocument);
                    document.SetFontProvider(new FontProvider(set));
                    document.SetProperty(Property.FONT, new String[] { "_PDMS_Saleem_QuranFont" });
                    Paragraph paragraph = new Paragraph();
                    paragraph.SetFont(PdfFontFactory.CreateFont(absolutePath, PdfEncodings.IDENTITY_H));
                    paragraph.SetFontFamily(new String[] { "_PDMS_Saleem_QuranFont" });
                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                    paragraph.Add(TheOpeningText);
                    document.Add(paragraph);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"PDF generation failed: {ex.Message}", "OK");
            }


            // Display the PDF in a WebView or PDF viewer
#if ANDROID
            // Assuming you are using PDF.js for viewing the PDF inside a WebView
            pdfview.Source =
                $"file:///android_asset/pdfjs/web/viewer.html?file=file://{WebUtility.UrlEncode(filePath)}";
#else
            pdfview.Source = filePath;  // For non-Android platforms, just set the source to the file path
#endif
        }
    }
}