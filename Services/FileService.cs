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

        public static List<FileObject> GetFiles(string directoryPath, bool searchSubfolders)
        {
            List<FileObject> fileObjects = new List<FileObject>();

            GetFilesFromDirectory(directoryPath, fileObjects, searchSubfolders);

            return fileObjects;

        }

        private static void GetFilesFromDirectory(string directoryPath, List<FileObject> fileObjects, bool searchSubfolders)
        {

            FileInfo fileInfo;

            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (string filePath in filePaths)
            {

                try
                {

                    fileInfo = new FileInfo(filePath);
                    fileObjects.Add(new FileObject { Name = fileInfo.Name, FilePath = fileInfo.FullName, FileSize = fileInfo.Length, Content = File.ReadAllText(filePath) });

                }
                catch
                {

                }

            }

            if (searchSubfolders)
            {
                string[] subfolders = Directory.GetDirectories(directoryPath);

                foreach(string subfolder in subfolders)
                {

                    GetFilesFromDirectory(subfolder, fileObjects, searchSubfolders);

                }

            }

        }

    }

}
