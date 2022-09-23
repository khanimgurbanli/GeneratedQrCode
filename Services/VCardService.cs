using Entities;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using Utils.Exceptions;

namespace Services
{
    public class VCardService : IVCardService
    {
        private readonly IBaseService<VCard> _baseService;
        private readonly IWebHostEnvironment _environment;
        public VCardService(IBaseService<VCard> baseService, IWebHostEnvironment environment)
        {
            _baseService = baseService;
            _environment=environment;
        }

        public async Task AddVCardAsync(VCard card)
        {
            var getDoctor = await _baseService.GetAsync(d => d.Id == card.Id);
            if (getDoctor != null) throw new AlreadyExistException("Already exist record");

            await _baseService.CreateAsync(card);
            await _baseService.SaveAsync();
        }

        public async Task DeleteVCardAsync(int id)
        {
            var deleteVCard = await _baseService.GetAsync(d => d.Id == id);
            if (deleteVCard == null) throw new NotFoundException("Not found record");

            _baseService.Delete(deleteVCard);
            await _baseService.SaveAsync();
        }

        public async Task<ICollection<VCard>> GetAllVCardsAsync() => await _baseService.GetAllAsync();


        public async Task<VCard> GetVCardByIdAsync(int id)
        {
            var getAllVCard = await _baseService.GetAsync(c => c.Id == id);
            return getAllVCard;
        }

        public async Task UpdateVCardAsync(int id)
        {
            await _baseService.GetAsync(d => d.Id == id);
            await _baseService.SaveAsync();
        }

        public async Task HttpClientVCardAsync()
        {
            // For requset url
            HttpClient client = new HttpClient();

            string resonponse = client.GetStringAsync("https://randomuser.me/api?results=50").Result;

            // Get data
            var result = JObject.Parse(resonponse);

            // Add db
            for (int i = 0; i < result["results"]!.ToArray().Length; i++)
                await _baseService.CreateAsync(new VCard
                {
                    Firstname = result["results"]![i]!["name"]!["first"]!.ToString(),
                    Surname = result["results"]![i]!["name"]!["last"]!.ToString(),
                    Email = result["results"]![i]!["email"]!.ToString(),
                    Country = result["results"]![i]!["location"]!["country"]!.ToString(),
                    City = result["results"]![i]!["location"]!["city"]!.ToString(),
                    Phone = result["results"]![i]!["phone"]!.ToString()
                });
            var check = await _baseService.SaveAsync();
        }

        public async Task<string> GenerateQrCodeAsync(int id)
        {
            var getdata = await GetVCardByIdAsync(id);
            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode
                   (
                      $"{getdata.Firstname}\n{getdata.Surname}\n{getdata.Country}\n{getdata.City}\n{getdata.Email}\n{getdata.Phone}", 200
                   );
            // barcode.AddBarcodeValueTextBelowBarcode();
            // Styling a QR code and adding annotation text
            barcode.SetMargins(10);
            barcode.ChangeBarCodeColor(Color.BlueViolet);
            string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(_environment.WebRootPath, "GeneratedQRCode/qrcode.png");
            barcode.SaveAsPng(filePath);
            string fileName = Path.GetFileName(filePath);

            return fileName;
        }
    }
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
