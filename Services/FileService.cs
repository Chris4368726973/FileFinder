using System;
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

            } 
            catch (Exception)
            { 
            
            
            }

            return content;

        }

        public static List<FileObject> GetFiles(string directoryPath, Filter filter)
        {

            var fileObjects = new List<FileObject>();

            try
            {

                foreach (var file in Directory.EnumerateFiles(directoryPath,"*",SearchOption.AllDirectories))
                {

                    try
                    {                      

                        if (filter.IsFiletypeIncluded(Path.GetExtension(file)))
                        {

                            var fileInfo = new FileInfo(file);

                            fileObjects.Add(new FileObject
                            {
                                Name = fileInfo.Name,
                                FilePath = fileInfo.FullName,
                                FileSize = fileInfo.Length
                            });
                        }

                    }
                    catch
                    {

                    }

                }             

            }
            catch (Exception)
            {

            }

            return fileObjects;

        }   

        public static void OpenFile(string filePath)
        {

            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception)
            {

            }

        }

        public static void MoveFile(string fileName, string filePath, string destinationPath)
        {

            try
            {
                File.Copy(filePath, destinationPath + @"\" + fileName);
            } 
            catch (Exception)
            {

            }

        }

    }

}
