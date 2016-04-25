using System;
using System.Collections.ObjectModel;
namespace EasyPasswordRecoveryPDF.Common
{
    public class ObservableCollectionExt<T> : ObservableCollection<T>
    {
        #region [ Defines ]

        private Boolean IsUpdating = false;

        #endregion

        #region [ Methods ]

        public void BeginUpdate()
        {
            this.IsUpdating = true;
        }

        public void EndUpdate()
        {
            this.IsUpdating = false;
            this.OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, null));
        }

        #endregion

        #region [ Overrides ]

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.IsUpdating)
                base.OnCollectionChanged(e);
        }

        #endregion
    }
}
