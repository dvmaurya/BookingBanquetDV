using CommonServices.Logger;
using System;
using Windows.Storage;

namespace CommonServices.FileReaders
{
    public static class FileReader
    {
        public static string ReadJsonFromFile(string fileName)
        {
            string JsonContent = null;

            try
            {
                Uri appUri = new Uri(fileName);
                StorageFile anjFile = StorageFile.GetFileFromApplicationUriAsync(appUri).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                JsonContent = FileIO.ReadTextAsync(anjFile).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("FIle Not found: " + ex.Message);
            }

            return JsonContent;
        }
    }
}
