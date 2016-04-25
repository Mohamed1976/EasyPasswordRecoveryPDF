using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Services;
using EasyPasswordRecoveryPDF.Services.Interfaces;

namespace EasyPasswordRecoveryPDF.Common
{
    public class ViewModelBase : NotifyPropertyChangedBase, IDisposable
    {
        #region [ Defines ]

        protected readonly IFileService fileService;
        protected readonly IDialogService dialogService;
        protected readonly IFolderBrowserService folderPickerService;

        #endregion

        #region [ Constructor ]

        public ViewModelBase()
        {
            fileService = ServiceContainer.Instance.GetService<IFileService>();
            dialogService = ServiceContainer.Instance.GetService<IDialogService>();
            folderPickerService = ServiceContainer.Instance.GetService<IFolderBrowserService>();
        }

        #endregion

        #region [ Properties ]

        private bool isBusy;
        /// <summary>
        /// Gets or sets the IsBusy.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(() => IsBusy);
                }
            }
        }

        #endregion

        #region [ IDisposable Members ]

        ///<summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            OnDispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose(bool isDisposing)
        {

        }

        ~ViewModelBase()
        {
            OnDispose(false);
        }

        #endregion
    }
}