﻿using System.Collections.Generic;
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

        public static List<FileObject> GetFiles(string directoryPath, Filter filter)
        {
            List<FileObject> fileObjects = new List<FileObject>();

            GetFilesFromDirectory(directoryPath, fileObjects, filter);

            return fileObjects;

        }

        private static void GetFilesFromDirectory(string directoryPath, List<FileObject> fileObjects, Filter filter)
        {

            FileInfo fileInfo;

            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (string filePath in filePaths)
            {

                try
                {

                    fileInfo = new FileInfo(filePath);

                    if (filter.IsFiletypeIncluded(fileInfo.Extension))
                    {
                        fileObjects.Add(new FileObject { Name = fileInfo.Name, FilePath = fileInfo.FullName, FileSize = fileInfo.Length });
                    }

                }
                catch
                {

                }

            }

            if (filter.SearchSubfolders)
            {
                string[] subfolders = Directory.GetDirectories(directoryPath);

                foreach(string subfolder in subfolders)
                {

                    GetFilesFromDirectory(subfolder, fileObjects, filter);

                }

            }

        }

        public static void OpenFile(string filePath)
        {

            System.Diagnostics.Process.Start(filePath);

        }

        public static void MoveFile(string fileName, string filePath, string destinationPath)
        {

            try
            {

                File.Copy(filePath, destinationPath + @"\" + fileName);

            } catch
            {

            }

        }

    }

}
