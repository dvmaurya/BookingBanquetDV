using BookingServices.Interfaces;
using CommonServices.Extensions;
using CommonServices.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace BookingServices.DataProvider
{
    public class EncryptionProvider : IEncryptionProvider
    {
        private readonly string _protectionDescriptor;

        public EncryptionProvider()
        {
            _protectionDescriptor = "LOCAL=user";
        }

        public async Task EncryptAsync(string fileName, string textToEncrypt, StorageFolder storageFolder)
        {
            await EncryptAsync(
                await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting),
                textToEncrypt).ConfigureAwait(true);
        }

        private async Task EncryptAsync(StorageFile storageFile, string textToEncrypt)
        {
            try
            {
                IBuffer buffer =
                    Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(
                        textToEncrypt,
                        Windows.Security.Cryptography.BinaryStringEncoding.Utf8);

                var provider = new DataProtectionProvider(_protectionDescriptor);

                IBuffer protectedBuffer = await provider.ProtectAsync(buffer);

                await FileIO.WriteBufferAsync(storageFile, protectedBuffer);
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
            }
        }

        public async Task<string> DecryptAsync(string fileName, StorageFolder storageFolder)
        {
            return
                await DecryptAsync(await storageFolder.GetFileAsync(fileName)).ConfigureAwait(true);
        }

        private async Task<string> DecryptAsync(StorageFile storageFile)
        {
            try
            {
                IBuffer Protectedbuffer = await FileIO.ReadBufferAsync(storageFile);

                DataProtectionProvider provider = new DataProtectionProvider(_protectionDescriptor);

                IBuffer unProtectedBuffer = await provider.UnprotectAsync(Protectedbuffer);

                return
                    Windows.Security.Cryptography.CryptographicBuffer.ConvertBinaryToString(
                        Windows.Security.Cryptography.BinaryStringEncoding.Utf8,
                        unProtectedBuffer);
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
                return null;
            }
        }
    }
}
