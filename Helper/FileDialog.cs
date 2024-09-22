﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ookii.Dialogs.Wpf;

namespace FileFinder
{
    public static class FileDialog
    {
        public static string OpenFileDialog()
        {

            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Select a folder",
                UseDescriptionForTitle = true,

            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                
                return dialog.SelectedPath;

            }

            return null;

        }

    }
}
