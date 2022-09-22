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
        public CardController(IVCardService cardService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        public IActionResult NotFound() => View();
        public async Task<IActionResult> QrCode(int id )
        {
            var fileName = await _cardService.GenerateQrCodeAsync(id);

            try
            {
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                @ViewBag.QrCodeUri = imageUrl;

                _logger.LogInformation("Show generated qr code and about info page");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Invalid sent request for get about generated qr code and private info.Details{ex}");
                throw;
            }

            return View(await _cardService.GetVCardByIdAsync(id));
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
            catch (Exception ex )
            {
                _logger.LogInformation("Don't add a new record");
                _logger.LogInformation($"Details for fail insert operation{ex}");
                return RedirectToAction("Opps");
            }
        }

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
