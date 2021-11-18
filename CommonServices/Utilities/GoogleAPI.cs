using CommonServices.Extensions;
using CommonServices.Logger;
using CommonServices.Model;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace CommonServices.Utilities
{
    public static class GoogleAPI
    {
        public static async Task SendToGoogleAPIAsync(Users users)
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/drive-dotnet-quickstart.json
            string[] Scopes = { DriveService.Scope.DriveReadonly };
            string ApplicationName = "Drive API .NET Quickstart";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string filename = "credentials.json";

            UserCredential credential = null;

            try
            {
                StorageFile storageFile = await storageFolder.GetFileAsync(filename);
                FileStream stream = File.Open(storageFile.Path, FileMode.Open);

                if(stream != null)
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = @"C:\Users\Durgvijay.Maurya\AppData\Local\Packages\b0acbfad-6fee-426a-b8fd-09165057d944_ezmkw7yy4pf9r\LocalState\token.json";
                    //File.Create(credPath);
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                    .Files;
                Console.WriteLine("Files:");
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        Console.WriteLine("{0} ({1})", file.Name, file.Id);
                    }
                }
                else
                {
                    Console.WriteLine("No files found.");
                }
                Console.Read();
            }
            catch(Exception ex)
            {
                FileLogger.Logger.Info("Exception: ".ConcatString(ex.Message));
            }
        }
    }
}
