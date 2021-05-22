using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace העתק_את_כל_הקבצים
{
    public class ViewModelMain : INotifyPropertyChanged
    {
        private string sourceDirectori;
        private string toNewDirectori;

        public string SourceDirectori
        {
            get { return sourceDirectori; }
            set
            {

                sourceDirectori = value;
                NotifyPropertyChanged(nameof(SourceDirectori));
            }
        }

        public string ToNewDirectori
        {
            get { return toNewDirectori; }
            set
            {
                toNewDirectori = value;
                NotifyPropertyChanged(nameof(ToNewDirectori));
            }
        }


        public RelayCommandAsync Run { get; set; }
        public RelayCommandAsync SelectSourceDirectori { get; set; }
        public RelayCommandAsync SelectNewDirectori { get; set; }
        public RelayCommandAsync Tset { get; set; }

        int numFiles = 0;



        public int NumFiles
        {
            get { return numFiles; }
            set
            {

                numFiles = value;
                NotifyPropertyChanged();
            }
        }

        private int numProgres;

        public int NumProgres
        {
            get { return numProgres; }
            set
            {
                numProgres = value;
                NotifyPropertyChanged();
            }
        }

        private string nameFile;

        public string NameFile
        {
            get { return nameFile; }
            set
            {
                nameFile = value;
                NotifyPropertyChanged();
            }
        }



        public ViewModelMain()
        {
            Run = new RelayCommandAsync(FuncRun);
            SelectSourceDirectori = new RelayCommandAsync(FuncSelectSourceDirectori);
            SelectNewDirectori = new RelayCommandAsync(FuncSelectNewDirectori);
            Tset = new RelayCommandAsync(FuncTset);

        }
        Thread threadTest;


        private async Task FuncTset()
        {
            NumFiles = 0;
            threadTest = new Thread(TsetAlFile);

            threadTest.Start(SourceDirectori);
            // TsetAlFile(SourceDirectori);

        }

        private async Task FuncSelectNewDirectori()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ToNewDirectori = dialog.FileName;
            }
        }

        private async Task FuncSelectSourceDirectori()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SourceDirectori = dialog.FileName;
            }

        }
        Thread threadDirSearch;
        private async Task FuncRun()
        {
            NumProgres = 0;
            threadDirSearch = new Thread(DirSearch);
            threadDirSearch.Start(SourceDirectori);
         
            //DirSearch(SourceDirectori, ToNewDirectori);
            //MessageBox.Show("סיימתי :)");
        }

        void TsetAlFile(object sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories((string)sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        NumFiles++;
                        NameFile = f;
                    }
                    TsetAlFile(d);
                }
                Debug.WriteLine(NumFiles);

            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }



        void DirSearch(object sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories((string)sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        // Debug.WriteLine(f);
                        string fileName = Path.GetFileName(f);
                        string fileName1 = Guid.NewGuid().ToString() + "." + (fileName.Split('.').Length > 1 ? fileName.Split('.')[1] : "");
                        NameFile = f;
                        File.Copy(f, System.IO.Path.Combine(ToNewDirectori, fileName1));
                        NumProgres++;
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
