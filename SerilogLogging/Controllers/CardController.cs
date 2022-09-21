using Entities;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services;
using System.Drawing;
using Utils.Exceptions;

namespace Logging_Serilog.Controllers
{
    public class CardController : Controller
    {
        private readonly IVCardService _cardService;
        private readonly ILogger<CardController> _logger;
        private readonly IWebHostEnvironment _environment;
        public CardController(IVCardService cardService, ILogger<CardController> logger, IWebHostEnvironment environment)
        {
            _environment = environment;
            _cardService = cardService;
            _logger = logger;
        }

        public IActionResult NotFound() => View();
        public async Task<IActionResult> QrCode(int id )
        {
            var getdata = await _cardService.GetVCardByIdAsync(id);
            try
            {
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

                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                @ViewBag.QrCodeUri = imageUrl;

                _logger.LogInformation("Show generated qr code and about info page");
            }
            catch (Exception)
            {
                _logger.LogInformation("Invalid sent request for get about generated qr code and private info");
                throw;
            }

            return View(getdata);
        }


        public async Task<IActionResult> Index()
        {
            var getdata = await _cardService.GetAllVCardsAsync();
            return View(getdata);
        }

        public IActionResult Add() => View();
        [HttpPost]
        public async Task<IActionResult> Add(VCard vCard)
        {
            try
            {
                await _cardService.AddVCardAsync(vCard);
                _logger.LogInformation("Added succesfully new record");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Don't add a new record");
                _logger.LogInformation($"Details for fail insert operation{ex}");
                return RedirectToAction("Opps");
            }
        }
        // public IActionResult GetDataFromApi() => View();


        public async Task<IActionResult> GetDataFromApi()
        {
            try
            {
                await _cardService.HttpClientVCardAsync();
                _logger.LogInformation("Added succesfully new record");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Don't add a new record");
                _logger.LogInformation($"Details for fail insert operation{ex}");
                return RedirectToAction("Opps");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _cardService.DeleteVCardAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Don't remove record");
                _logger.LogInformation($"Details for fail delete operation{ex}");
            }
            return View();
        }

        public IActionResult Update() => View();
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                await _cardService.UpdateVCardAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Don't edit record infos");
                _logger.LogInformation($"Details for fail update operation{ex}");
            }
            return View();
        }

        public IActionResult GetById() => View();
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                await _cardService.UpdateVCardAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Don't edit record infos");
                _logger.LogInformation($"Details for fail update operation{ex}");
            }
            return View();
        }
    }
}
