using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace Masoctinha
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SoftwareBitmap bitmap;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("Assets\\IMG_20210125_110317_977.jpg");
            await LoadImage(file);
            OcrEngine ocrEngine = null;
            ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();

            if (ocrEngine != null)
            {
                var ocrResult = await ocrEngine.RecognizeAsync(bitmap);
                tbTxt.Text = ocrResult.Text;
            }

        }

        private async Task LoadImage(StorageFile file) {

            using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                var imgsource = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
                bitmap.CopyToBuffer(imgsource.PixelBuffer);
            }

        } 

    }
}
