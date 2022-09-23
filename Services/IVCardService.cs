using Entities;
namespace Services
{
    public interface IVCardService
    {
        Task AddVCardAsync(VCard card);
        Task HttpClientVCardAsync();
        Task DeleteVCardAsync(int id);
        Task UpdateVCardAsync(int id);
        Task<string> GenerateQrCodeAsync(int id);
        Task<VCard> GetVCardByIdAsync(int id);
        Task<ICollection<VCard>> GetAllVCardsAsync();
    }
}
