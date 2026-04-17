using System.Threading.Tasks;
using Windows.Storage;

namespace BookingServices.Interfaces
{
    public interface IEncryptionProvider
    {
        Task EncryptAsync(string fileName, string textToEncrypt, StorageFolder storageFolder);
        Task<string> DecryptAsync(string fileName, StorageFolder storageFolder);
    }
}
