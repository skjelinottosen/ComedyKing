using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ComedyKing.App
{
    public static class FileWriter
    {
        //Location for logfile
        //C:\Users\andreot\AppData\Local\Packages\8AC7B0F3-C8B3-4F3A-8774-3BE0CC1743F4_p4danj10dwt2e\LocalState 
        public static async void AddMessageToFile_Async(String message)
        {
            string fileName = "log.txt";
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, message);         
        }
    }
}
