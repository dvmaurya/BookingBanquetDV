using BookingServices.Interfaces;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Logger;
using MimeKit;
using MimeKit.IO;
using MimeKit.IO.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace BookingBanquetDV.Source.Camera
{
    public static class PhotoUtility
    {
        private const string baseG4Regex = "src=\"data:image\\/([a-zA-Z]*);base64,([^\"]*)\"";

        private static readonly IMessageDialogService _messageDialogService =
            DIContainer.GetInstance<IMessageDialogService>();

        private static readonly IEncryptionProvider _encryptionProvider =
            DIContainer.GetInstance<IEncryptionProvider>();

        public static async Task SavePhotoAsync(StorageFile storeFile, string photoNameForFolder)
        {
            if (storeFile != null)
            {
                try
                {
                    StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.
                        CreateFolderAsync(ApplicationConstants.Document, CreationCollisionOption.OpenIfExists);

                    await storeFile.MoveAsync(storageFolder, photoNameForFolder);
                    await GetEncryptPhotoAsync(storeFile, storageFolder);
                    await ApplicationData.Current.TemporaryFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                catch (Exception ex)
                {
                    await _messageDialogService.ShowMessageDialogAsync(
                        title: string.Empty,
                        content: ApplicationConstants.UnableToSavePhoto);

                    FileLogger.Logger.Info("Save Photo Exception: ".ConcatString(ex.Message));
                }
            }
        }

        private static async Task GetEncryptPhotoAsync(StorageFile storeFile, StorageFolder storageFolder)
        {
            if (storeFile != null)
            {
                byte[] byteArray = await GetBytesAsync(storeFile);
                string base64String = Convert.ToBase64String(byteArray);
                await _encryptionProvider.EncryptAsync(storeFile.Name, base64String, storageFolder);
            }
        }

        private static async Task<byte[]> GetBytesAsync(StorageFile file)
        {
            byte[] fileBytes = null;

            if (file == null)
                return null;

            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            return fileBytes;
        }

        public static async Task<BitmapImage> GetDecrtypedPhotoAsync(StorageFile storageFile)
        {
            try
            {
                string text = await DecryptedPhotoAsync(storageFile.Name);

                if (!string.IsNullOrEmpty(text))
                {
                    using (InMemoryRandomAccessStream inMemoryRandomAccessStream = new InMemoryRandomAccessStream())
                    {
                        using (DataWriter writer = new DataWriter(inMemoryRandomAccessStream.GetOutputStreamAt(0)))
                        {
                            writer.WriteBytes(Convert.FromBase64String(text));

                            await writer.StoreAsync();
                        }

                        return await GetBitmapImage(inMemoryRandomAccessStream);
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static MimeEntity AddMimeEntity(string fileName, byte[] data, ContentType contentType)
        {
            using (MemoryStream stream = new MemoryStream(data, false))
            {
                MimePart attachment = CreateAttachment(contentType, fileName, stream);
                return attachment;
            }
        }

        private static MimePart CreateAttachment(ContentType contentType, string fileName, Stream stream)
        {
            MimePart attachment;

            if (contentType.IsMimeType("text", "*"))
            {
                attachment = new TextPart(contentType.MediaSubtype);
                foreach (Parameter param in contentType.Parameters)
                    attachment.ContentType.Parameters.Add(param);
            }
            else
            {
                attachment = new MimePart(contentType);
            }

            attachment.FileName = Path.GetFileName(fileName);
            LoadContent(attachment, stream);

            return attachment;
        }

        static void LoadContent(MimePart attachment, Stream stream)
        {
            MemoryBlockStream content = new MemoryBlockStream();
            BestEncodingFilter filter = new BestEncodingFilter();
            byte[] buf = new byte[4096];
            int index, length;
            int nread;

            while ((nread = stream.Read(buf, 0, buf.Length)) > 0)
            {
                filter.Filter(buf, 0, nread, out index, out length);
                content.Write(buf, 0, nread);
            }

            filter.Flush(buf, 0, 0, out index, out length);
            content.Position = 0;

            attachment.ContentTransferEncoding = filter.GetBestEncoding(EncodingConstraint.SevenBit);
            attachment.ContentObject = new ContentObject(content);
        }

        public static async Task<BitmapImage> GetBitmapImageAsync(StorageFile storageFile)
        {
            if (storageFile != null)
            {
                using (IRandomAccessStream randomAccessStream = await storageFile.OpenAsync(FileAccessMode.Read))
                    return await GetBitmapImage(randomAccessStream);
            }

            return null;
        }

        private async static Task<BitmapImage> GetBitmapImage(IRandomAccessStream randomAccessStream)
        {
            BitmapImage bitmapImage = new BitmapImage();

            try
            {
                await bitmapImage.SetSourceAsync(randomAccessStream);
            }
            catch
            {
                return null;
            }

            return bitmapImage;
        }

        public static async Task<byte[]> GetByteArrayFromEncryptedPhoto(StorageFile storageFile)
        {
            string text = await DecryptedPhotoAsync(storageFile.Name);

            if (!string.IsNullOrEmpty(text))
                return Convert.FromBase64String(text);

            return null;
        }

        public static async Task<string> DecryptedPhotoAsync(string photoName)
        {
            try
            {
                StorageFolder storageFolder =
                    await ApplicationData.Current.LocalFolder.GetFolderAsync(ApplicationConstants.Document);

                return await _encryptionProvider.DecryptAsync(photoName, storageFolder);
            }
            catch
            {
                return null;
            }
        }
    }
}
