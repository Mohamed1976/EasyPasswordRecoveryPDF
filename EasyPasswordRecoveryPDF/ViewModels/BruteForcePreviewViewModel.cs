using System.Windows;
using System.Collections.ObjectModel;
using EasyPasswordRecoveryPDF.Common;
using EasyPasswordRecoveryPDF.Model;

namespace EasyPasswordRecoveryPDF.ViewModels
{
    public class BruteForcePreviewViewModel : ViewModelBase
    {
        #region [ Defines ]

        private Window BruteForcePreviewView = null;

        #endregion

        #region [ Constructor ]

        public BruteForcePreviewViewModel(Window bruteForcePreviewView,
            ObservableCollection<IteratorCounter> iteratorCounters,
            ObservableCollectionExt<CharExt> charList)
        {
            BruteForcePreviewView = bruteForcePreviewView;
            RequestedIterations = iteratorCounters;
            CharList = charList;
        }

        #endregion

        #region [ Properties ]

        private ObservableCollection<IteratorCounter> requestedIterations = null;
        public ObservableCollection<IteratorCounter> RequestedIterations
        {
            get { return requestedIterations; }
            private set { SetProperty(ref this.requestedIterations, value); }
        }

        private ObservableCollectionExt<CharExt> charList = null;
        public ObservableCollectionExt<CharExt> CharList
        {
            get { return charList; }
            private set { SetProperty(ref this.charList, value); }
        }

        #endregion
    }
}
