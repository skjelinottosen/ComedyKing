using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComedyKing.App
{
    public static class FileLogger
    {
        public static void LogExceptions(string message)
        {
            FileWriter.AddMessageToFile_Async(message);
        }
    }
}
