using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IVCardService
    {
        Task AddVCardAsync(VCard card);
        Task HttpClientVCardAsync();
        Task DeleteVCardAsync(int id);
        Task UpdateVCardAsync(int id);
        Task GenerateQrCodeAsync();
        Task<VCard> GetVCardByIdAsync(int id);
        Task<ICollection<VCard>> GetAllVCardsAsync();
    }
}
