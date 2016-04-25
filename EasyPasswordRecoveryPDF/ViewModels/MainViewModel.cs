using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Interfaces;
using EasyPasswordRecoveryPDF.Model;
using EasyPasswordRecoveryPDF.Views;
using EasyPasswordRecoveryPDF.Pdf.Decryption;
using EasyPasswordRecoveryPDF.Pdf.Decryption.Interfaces;
using EasyPasswordRecoveryPDF.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region [ Constructor ]

        public MainViewModel()
        {
            TabItems = new ObservableCollection<ITabViewModel>();
            TabItems.Add(new DictionaryManagerViewModel("List",
                Application.Current.TryFindResource("Dictionary") as DrawingBrush));
            TabItems.Add(new SmartManagerViewModel("Smart",
                Application.Current.TryFindResource("Smart") as DrawingBrush));
            TabItems.Add(new BruteForceManagerViewModel("Brute",
                Application.Current.TryFindResource("BruteForce") as DrawingBrush));
            SelectedTabItem = TabItems.First();
            PdfFiles = new ObservableCollection<PdfFile>();

            InitializeCommands();
            InitializeBackgroundWorkers();
            InitializeViewProperties();
        }

        #endregion

        #region [ Properties ]

        private ObservableCollection<ITabViewModel> tabItems = null;
        public ObservableCollection<ITabViewModel> TabItems
        {
            get { return tabItems; }
            private set { SetProperty(ref this.tabItems, value); }
        }

        private ITabViewModel selectedTabItem = null;
        public ITabViewModel SelectedTabItem
        {
            get { return selectedTabItem; }
            set { SetProperty(ref this.selectedTabItem, value); }
        }

        private AvailableViews selectedView = AvailableViews.Home;
        public AvailableViews SelectedView
        {
            get { return selectedView; }
            private set { SetProperty(ref this.selectedView, value); }
        }

        /// <summary>
        /// Gets or sets the Status property. This property displays
        /// the status message.
        /// </summary>
        private string status = string.Empty;
        public string Status
        {
            get { return status; }
            set { this.SetProperty(ref this.status, value); }
        }

        private PdfFile selectedFile = null;
        public PdfFile SelectedFile
        {
            get { return selectedFile; }
            set { SetProperty(ref this.selectedFile, value); }
        }

        private ObservableCollection<PdfFile> pdfFiles = null;
        public ObservableCollection<PdfFile> PdfFiles
        {
            get { return pdfFiles; }
            set { SetProperty(ref this.pdfFiles, value); }
        }

        private ValidationMode validationModeRequested = ValidationMode.None;
        public ValidationMode ValidationModeRequested
        {
            get { return validationModeRequested; }
            set { SetProperty(ref this.validationModeRequested, value); }
        }

        private string currentPassword = string.Empty;
        public string CurrentPassword
        {
            get { return currentPassword; }
            set { SetProperty(ref this.currentPassword, value); }
        }

        public ulong passwordCount = 0;
        public ulong PasswordCount
        {
            get { return passwordCount; }
            set { SetProperty(ref this.passwordCount, value); }
        }

        private ulong speed = 0;
        public ulong Speed
        {
            get { return speed; }
            set { SetProperty(ref this.speed, value); }
        }


        #endregion

        #region [ Commands ]

        private void InitializeCommands()
        {
            SelectViewCmd = new RelayCommand<object>(OnSelectViewCmdExecute);
            AddFileCmd = new RelayCommand(OnAddFileCmdExecute, OnAddFileCmdCanExecute);
            RemoveFileCmd = new RelayCommand(OnRemoveFileCmdExecute, OnRemoveFileCmdCanExecute);
            ShowFilePropertiesCmd = new RelayCommand(OnShowFilePropertiesCmdExecute, OnShowFilePropertiesCmdCanExecute);
            OpenFileCmd = new RelayCommand(OnOpenFileCmdExecute, OnOpenFileCmdCanExecute);
            UnlockCmd = new RelayCommand(OnUnlockCmdExecute, OnUnlockCmdCanExecute);
            DecryptCmd = new RelayCommand(OnDecryptCmdExecute, OnDecryptCmdCanExecute);
            CancelCmd = new RelayCommand(OnCancelCmdExecute, OnCancelCmdCanExecute);
            RequestNavigateCmd = new RelayCommand<object>(OnRequestNavigateCmdExecute);
            SelectFolderCmd = new RelayCommand(OnSelectFolderCmdExecute);
        }

        public RelayCommand<object> SelectViewCmd { get; private set; }
        public RelayCommand AddFileCmd { get; private set; }
        public RelayCommand RemoveFileCmd { get; private set; }
        public RelayCommand ShowFilePropertiesCmd { get; private set; }
        public RelayCommand OpenFileCmd { get; private set; }
        public RelayCommand UnlockCmd { get; private set; }
        public RelayCommand DecryptCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand ExportCmd { get; private set; }
        public RelayCommand<object> RequestNavigateCmd { get; private set; }
        public RelayCommand SelectFolderCmd { get; private set; }

        private void OnSelectViewCmdExecute(object parameter)
        {
            SelectedView = (AvailableViews)parameter;
        }

        private async void OnSelectFolderCmdExecute()
        {
            string saveDir = await folderPickerService.ShowFolderBrowserDialogAsync("Select folder for password file");
            if (!string.IsNullOrEmpty(saveDir))
            {
                SelectedSavePath = saveDir;
            }
        }

        private void OnRequestNavigateCmdExecute(object parameter)
        {
            Uri uri = (Uri)parameter;
            if (uri != null)
            {
                System.Diagnostics.Process.Start(uri.ToString());
            }
        }

        private async void OnAddFileCmdExecute()
        {
            string[] filenames = await fileService.ShowOpenFileDialogAsync("Select PDF file to decrypt",
                ".pdf", false, Tuple.Create("PDF Files", "*.pdf"));

            if (filenames != null)
            {
                IsBusy = true;
                loadPdfBackgroundWorker.RunWorkerAsync(filenames[0]);
            }
        }

        private bool OnAddFileCmdCanExecute()
        {
            return SelectedFile == null && !IsBusy;
        }

        private void OnRemoveFileCmdExecute()
        {
            PdfFiles.Clear();
            SelectedFile = null;
            ValidationModeRequested = ValidationMode.None;
        }

        private bool OnRemoveFileCmdCanExecute()
        {
            return SelectedFile != null && !IsBusy;
        }

        private bool OnShowFilePropertiesCmdCanExecute()
        {
            return SelectedFile != null && !IsBusy;
        }

        private async void OnShowFilePropertiesCmdExecute()
        {
            FilePropertiesView filePropertiesView = new FilePropertiesView();
            FilePropertiesViewModel filePropertiesViewModel =
                new FilePropertiesViewModel(filePropertiesView, SelectedFile);
            bool? result = await dialogService.InitDialog(filePropertiesView, filePropertiesViewModel);
            filePropertiesViewModel.Dispose();
        }

        private async void OnOpenFileCmdExecute()
        {
            WebBrowserView webBrowserView = new WebBrowserView();
            WebBrowserViewModel webBrowserViewModel =
                new WebBrowserViewModel(webBrowserView, SelectedFile.Info.FullName);

            bool? result = await dialogService.InitDialog(webBrowserView, webBrowserViewModel);
            webBrowserViewModel.Dispose();
        }

        private bool OnOpenFileCmdCanExecute()
        {
            return SelectedFile != null && !IsBusy;
        }

        private async void OnUnlockCmdExecute()
        {
            PdfLoginView pdfLoginView = new PdfLoginView();
            PdfLoginViewModel pdfLoginViewModel =
                new PdfLoginViewModel(pdfLoginView, selectedFile.EncryptionRecordInfo);
            bool? result = await dialogService.InitDialog(pdfLoginView, pdfLoginViewModel);

            if (!string.IsNullOrEmpty(pdfLoginViewModel.RecoveredOwnerPassword))
            {
                SelectedFile.SetPassword(pdfLoginViewModel.RecoveredOwnerPassword,
                    PasswordValidity.OwnerPasswordIsValid);
                await WriteToFile(PasswordValidity.OwnerPasswordIsValid, pdfLoginViewModel.RecoveredOwnerPassword);
            }

            if (!string.IsNullOrEmpty(pdfLoginViewModel.RecoveredUserPassword))
            {
                SelectedFile.SetPassword(pdfLoginViewModel.RecoveredUserPassword,
                    PasswordValidity.UserPasswordIsValid);
                await WriteToFile(PasswordValidity.UserPasswordIsValid, pdfLoginViewModel.RecoveredUserPassword);
            }
        }

        private bool OnUnlockCmdCanExecute()
        {
            return SelectedFile != null && !IsBusy &&
                (SelectedFile.UserPasswordIsSet || SelectedFile.OwnerPasswordIsSet);
        }

        private void OnDecryptCmdExecute()
        {
            Status = string.Empty;

            ClearAllErrors();

            if (SelectedFile == null)
            {
                Status = "Please select a PDF file to decrypt.";
                SetErrors("SelectedFile", new List<ValidationResult>() { new ValidationResult(false, Status) });
            }
            else if (!SelectedFile.IsEncrypted)
            {
                Status = "No encryption dictionary found in PDF.";
            }
            else if (!SelectedFile.OwnerPasswordIsSet && !SelectedFile.UserPasswordIsSet)
            {
                Status = "Both the User password and the Owner password are empty.";
            }
            else if (ValidationModeRequested == ValidationMode.None)
            {
                Status = "Please select the password you want to recover.";
                SetErrors("ValidationModeRequested", new List<ValidationResult>() { new ValidationResult(false, Status) });
            }
            else if ((ValidationModeRequested & ValidationMode.ValidateOwnerPassword) == ValidationMode.ValidateOwnerPassword &&
                (ValidationModeRequested & ValidationMode.ValidateUserPassword) == ValidationMode.ValidateUserPassword &&
                !string.IsNullOrEmpty(SelectedFile.RecoveredOwnerPassword) &&
                !string.IsNullOrEmpty(SelectedFile.RecoveredUserPassword))
            {
                Status = "The requested passwords are already recovered.";
            }
            else if ((ValidationModeRequested == ValidationMode.ValidateOwnerPassword &&
                !string.IsNullOrEmpty(SelectedFile.RecoveredOwnerPassword)) ||
                ((ValidationModeRequested == ValidationMode.ValidateUserPassword &&
                !string.IsNullOrEmpty(SelectedFile.RecoveredUserPassword))))
            {
                Status = "The requested password is already recovered.";
            }
            else if (SelectedTabItem is ITabViewModel)
            {
                string errormsg = string.Empty;
                IPasswordIterator passwordIterator = SelectedTabItem.PasswordIterator;
                if (passwordIterator.Initialize(SelectedFile.PasswordCharset,
                    SelectedFile.MaxPasswordSize,
                    ref errormsg) == Constants.Success)
                {
                    IsBusy = true;
                    CurrentPassword = string.Empty;
                    passwordIterator.PasswordCasing = PasswordCasing;
                    passwordIterator.RemoveWhitespace = RemoveWhitespace;
                    decryptBackgroundWorker.RunWorkerAsync(new object[]
                    { SelectedFile.EncryptionRecordInfo, passwordIterator, ValidationModeRequested});
                }
                else
                {
                    SetErrors("SelectedTabItem", new List<ValidationResult>() { new ValidationResult(false, errormsg) });
                    Status = errormsg;
                }
            }
        }

        private bool OnDecryptCmdCanExecute()
        {
            return !decryptBackgroundWorker.IsBusy;
        }

        private void OnCancelCmdExecute()
        {
            decryptBackgroundWorker.CancelAsync();
        }

        private bool OnCancelCmdCanExecute()
        {
            return decryptBackgroundWorker != null && decryptBackgroundWorker.IsBusy;
        }

        #endregion

        #region [ BackgroundWorkers ] 

        #region [ InitializeBackgroundWorkers ] 

        /// <summary>
        /// Decrypt PDF file on a separate thread.
        /// </summary>
        private BackgroundWorker decryptBackgroundWorker = null;

        /// <summary>
        /// Load PDF file on a separate thread.
        /// </summary>
        private BackgroundWorker loadPdfBackgroundWorker = null;

        private void InitializeBackgroundWorkers()
        {
            decryptBackgroundWorker = new BackgroundWorker();
            decryptBackgroundWorker.WorkerReportsProgress = true;
            decryptBackgroundWorker.WorkerSupportsCancellation = true;
            decryptBackgroundWorker.DoWork +=
                new DoWorkEventHandler(DecryptWorkerDoWork);
            decryptBackgroundWorker.ProgressChanged +=
                new ProgressChangedEventHandler(DecryptWorkerProgressChanged);
            decryptBackgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(DecryptWorkerRunWorkerCompleted);

            loadPdfBackgroundWorker = new BackgroundWorker();
            loadPdfBackgroundWorker.WorkerReportsProgress = false;
            loadPdfBackgroundWorker.WorkerSupportsCancellation = false;
            loadPdfBackgroundWorker.DoWork +=
                new DoWorkEventHandler(LoadPdfWorkerDoWork);
            loadPdfBackgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(LoadPdfWorkerCompleted);
        }

        #endregion

        #region [ DecryptBackgroundWorker ] 

        private void DecryptWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            TimeSpan ts;
            const int timeSlice = 50;
            const int nrOfSpeedMeasurements = 50;
            ulong passwordCount = 0, prevPasswordCount = 0;
            object[] args = (object[])e.Argument;
            IDecryptor decryptor = DecryptorFactory.Get((EncryptionRecord)args[0]);
            IPasswordIterator passwordIterator = (IPasswordIterator)args[1];
            ValidationMode validationMode = (ValidationMode)args[2];
            IMovingAverage avg = new SimpleMovingAverage(nrOfSpeedMeasurements);
            PasswordValidity passwordValidity = PasswordValidity.Invalid;
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            string currentPassword = passwordIterator.GetNextPassword();
            while (!string.IsNullOrEmpty(currentPassword) &&
                validationMode != ValidationMode.None &&
                !decryptBackgroundWorker.CancellationPending)
            {
                passwordValidity = decryptor.ValidatePassword(currentPassword, validationMode);

                passwordCount++;
                ts = stopWatch.Elapsed;
                if (ts.Milliseconds > timeSlice || passwordValidity != PasswordValidity.Invalid)
                {
                    avg.AddSample((60000 * (passwordCount - prevPasswordCount)) / ((ulong)ts.Milliseconds + 1)); //Avoid div by zero
                    decryptBackgroundWorker.ReportProgress(0, new object[]
                    { passwordValidity, currentPassword, (ulong)avg.Average, passwordCount });
                    prevPasswordCount = passwordCount;
                    stopWatch.Restart();

                    if ((passwordValidity & PasswordValidity.OwnerPasswordIsValid) == PasswordValidity.OwnerPasswordIsValid)
                    {
                        validationMode &= ~ValidationMode.ValidateOwnerPassword;
                    }

                    if ((passwordValidity & PasswordValidity.UserPasswordIsValid) == PasswordValidity.UserPasswordIsValid)
                    {
                        validationMode &= ~ValidationMode.ValidateUserPassword;
                    }
                }
                currentPassword = passwordIterator.GetNextPassword();
            }

            if (decryptBackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = passwordCount;
            }
        }

        private void DecryptWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                object[] args = (object[])e.UserState;
                PasswordValidity passwordValidity = (PasswordValidity)args[0];
                CurrentPassword = args[1].ToString();
                Speed = (ulong)args[2];
                PasswordCount = (ulong)args[3];
                if (passwordValidity != PasswordValidity.Invalid)
                {
                    SelectedFile.SetPassword(CurrentPassword, passwordValidity);
                    WriteToFile(passwordValidity, CurrentPassword);
                    Status = string.Format("{0}: {1}", passwordValidity.ToString(), CurrentPassword);
                }
            }
        }

        private void DecryptWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = string.Empty;

            if (e.Error != null)
            {
                result = string.Format("An unexpected error occured while loading PDF file: {0}.", e.Error.Message);
            }
            if (e.Cancelled)
            {
                result = "PDF decryption process was canceled by user.";
            }
            else
            {
                PasswordCount = (ulong)e.Result;
                result = "Finished decryption process.";
            }

            Status = result;
            IsBusy = false;
        }

        #endregion

        #region [ LoadPdfWorkerCompleted ]

        private void LoadPdfWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            string errorMsg = string.Empty;
            PdfFile pdfFile = new PdfFile(e.Argument.ToString());
            pdfFile.Open(ref errorMsg);
            e.Result = new object[] { errorMsg, pdfFile };
        }

        private void LoadPdfWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = string.Empty;

            if (e.Error != null)
            {
                result = string.Format("An unexpected error occured while loading PDF file: {0}.", e.Error.Message);
            }
            else
            {
                object[] args = (object[])e.Result;
                if (string.IsNullOrEmpty(args[0].ToString()))
                {
                    PdfFiles.Add(args[1] as PdfFile);
                    SelectedFile = PdfFiles.First();
                }
                else
                {
                    result = args[0].ToString();
                }
            }

            Status = result;
            IsBusy = false;
        }

        #endregion

        #endregion

        #region [ Methods ]

        private Task WriteToFile(PasswordValidity passwordValidity, string CurrentPassword)
        {
            string destinationFilePath = string.Empty;

            string summaryMsg = string.Format("{0},{1},{2}={3}{4}",
                DateTime.Now.ToString(CultureInfo.CurrentCulture),
                SelectedFile.Info.FullName, passwordValidity.ToString(),
                CurrentPassword, Environment.NewLine);
            FileMode fileMode = OverwritePasswordFile ? FileMode.Create : FileMode.Append;
            if (SaveOptionsPasswordFile == SaveOptions.UseSourceFolder)
            {
                destinationFilePath = string.Format("{0}\\{1}", SelectedFile.Info.DirectoryName, PasswordStorageFilename);
            }
            else if (SaveOptionsPasswordFile == SaveOptions.UseCustomFolder)
            {
                destinationFilePath = string.Format("{0}\\{1}", SelectedSavePath, PasswordStorageFilename);
            }

            return FileHelpers.WriteToFileAsync(destinationFilePath, fileMode, summaryMsg);
        }

        #endregion

        #region [ About View ]

        #region [ Methods ]

        private void InitializeViewProperties()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] titleAttr = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (titleAttr.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)titleAttr[0];
                if (!string.IsNullOrEmpty(titleAttribute.Title))
                {
                    Title = String.Format("About {0}", titleAttribute.Title);
                }
            }

            object[] productAttr = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (productAttr.Length > 0)
            {
                AssemblyProductAttribute productAttribute = (AssemblyProductAttribute)productAttr[0];
                if (!string.IsNullOrEmpty(productAttribute.Product))
                {
                    ProductName = productAttribute.Product;
                }
            }

            Version = assembly.GetName().Version.ToString();

            object[] copyrightAttr = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (copyrightAttr.Length > 0)
            {
                AssemblyCopyrightAttribute copyrightAttribute = (AssemblyCopyrightAttribute)copyrightAttr[0];
                if (!string.IsNullOrEmpty(copyrightAttribute.Copyright))
                {
                    Copyright = copyrightAttribute.Copyright;
                }
            }

            object[] descriptionAttr = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (descriptionAttr.Length > 0)
            {
                AssemblyDescriptionAttribute descriptionAttribute = (AssemblyDescriptionAttribute)descriptionAttr[0];
                if (!string.IsNullOrEmpty(descriptionAttribute.Description))
                {
                    Description = descriptionAttribute.Description;
                }
            }

            object[] companyAttr = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (companyAttr.Length > 0)
            {
                AssemblyCompanyAttribute companyAttribute = (AssemblyCompanyAttribute)companyAttr[0];
                if (!string.IsNullOrEmpty(companyAttribute.Company))
                {
                    companyName = companyAttribute.Company;
                }
            }

            ITextSharpVersion = iTextSharp.text.Version.GetInstance().GetVersion;
            GenerexVersion = string.Format("Generex version {0}.", Constants.GenerexVersion);
        }

        #endregion

        #region [ Properties ]

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            private set { SetProperty(ref this.title, value); }
        }

        private string productName = string.Empty;
        public string ProductName
        {
            get { return productName; }
            private set { SetProperty(ref this.productName, value); }
        }

        private string version = string.Empty;
        public string Version
        {
            get { return version; }
            private set { SetProperty(ref this.version, value); }
        }

        private string copyright = string.Empty;
        public string Copyright
        {
            get { return copyright; }
            private set { SetProperty(ref this.copyright, value); }
        }

        private string companyName = string.Empty;
        public string CompanyName
        {
            get { return companyName; }
            private set { SetProperty(ref this.companyName, value); }
        }

        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            private set { SetProperty(ref this.description, value); }
        }

        private string iTextSharpVersion = string.Empty;
        public string ITextSharpVersion
        {
            get { return iTextSharpVersion; }
            private set { SetProperty(ref this.iTextSharpVersion, value); }
        }

        private string generexVersion = string.Empty;
        public string GenerexVersion
        {
            get { return generexVersion; }
            private set { SetProperty(ref this.generexVersion, value); }
        }

        #endregion

        #endregion

        #region [ Settings View ]

        public bool RemoveWhitespace
        {
            get
            {
                return Settings.Default.RemoveWhitespaceFromString;
            }
            set
            {
                Settings.Default.RemoveWhitespaceFromString = value;
                Settings.Default.Save();
                OnPropertyChanged(() => RemoveWhitespace);
            }
        }

        public CharsetCasing PasswordCasing
        {
            get
            {
                return (CharsetCasing)Settings.Default.PasswordCasing;
            }
            set
            {
                Settings.Default.PasswordCasing = (ushort)value;
                Settings.Default.Save();
                OnPropertyChanged(() => PasswordCasing);
            }
        }

        public string SelectedSavePath
        {
            get
            {
                string selectedSavePath = string.Empty;
                if (string.IsNullOrEmpty(Settings.Default.SelectedSavePath))
                {
                    selectedSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    selectedSavePath = Settings.Default.SelectedSavePath;
                }
                return selectedSavePath;
            }
            set
            {
                Settings.Default.SelectedSavePath = value;
                Settings.Default.Save();
                OnPropertyChanged(() => SelectedSavePath);
            }
        }

        public string PasswordStorageFilename
        {
            get
            {
                return Settings.Default.PasswordStorageFilename;
            }

            set
            {
                string errorMsg = string.Empty;
                if (FileHelpers.FileNameIsValid(value, out errorMsg) == Constants.Success)
                {
                    ClearPropertyErrors("PasswordStorageFilename");
                    Settings.Default.PasswordStorageFilename = value;
                    Settings.Default.Save();
                    OnPropertyChanged(() => PasswordStorageFilename);
                }
                else
                {
                    SetErrors("PasswordStorageFilename", new List<ValidationResult>()
                    { new ValidationResult(false, errorMsg) });
                }
            }
        }

        public SaveOptions SaveOptionsPasswordFile
        {
            get
            {
                return (SaveOptions)Settings.Default.SaveOptions;
            }
            set
            {
                Settings.Default.SaveOptions = (ushort)value;
                Settings.Default.Save();
                OnPropertyChanged(() => SaveOptionsPasswordFile);
            }
        }

        public bool OverwritePasswordFile
        {
            get
            {
                return Settings.Default.OverwritePasswordFile;
            }
            set
            {
                Settings.Default.OverwritePasswordFile = value;
                Settings.Default.Save();
                OnPropertyChanged(() => OverwritePasswordFile);
            }
        }

        #endregion
    }
}