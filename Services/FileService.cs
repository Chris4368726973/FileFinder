using System.Collections.Generic;
using System.IO;

namespace FileFinder
{
    static class FileService
    {
        public static bool PathisValid(string directoryPath) => Directory.Exists(directoryPath);

        public static string GetFileContent(FileObject fileObject)
        {

            string content = "";

            try {

                content = File.ReadAllText(fileObject.FilePath);

            } catch { 
            
            
            }

            return content;

        }

        public static List<FileObject> GetFiles(string directoryPath)
        {
            List<FileObject> fileObjects = new List<FileObject>();

            FileInfo fileInfo;

            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (string filePath in filePaths)
            {

                try { 

                    fileInfo = new FileInfo(filePath);
                    fileObjects.Add(new FileObject { Name = fileInfo.Name, FilePath = fileInfo.FullName, FileSize = fileInfo.Length, Content=File.ReadAllText(filePath) });

                } catch {

                }

            }

            return fileObjects;

        }

    }

}
