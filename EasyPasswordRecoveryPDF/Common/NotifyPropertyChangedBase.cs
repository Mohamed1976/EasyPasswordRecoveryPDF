using System;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace EasyPasswordRecoveryPDF.Common
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region [ INotifyPropertyChanged Members ]

        /// <summary>
        ///     Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("propertyExpression must represent a valid Member Expression");

            var propertyInfo = memberExpression.Member as System.Reflection.PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("propertyExpression must represent a valid Property on the object");

            handler(this, new PropertyChangedEventArgs(propertyInfo.Name));
        }

        /// <summary>
        /// Warns the developer if this object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        #endregion

        #region [ INotifyDataErrorInfo ]

        private ConcurrentDictionary<string, ICollection<ValidationResult>> propertyErrors =
            new ConcurrentDictionary<string, ICollection<ValidationResult>>();

        protected void ClearAllErrors()
        {
            foreach (var propertyName in propertyErrors.Keys)
            {
                ClearPropertyErrors(propertyName);
            }
        }

        protected void ClearPropertyErrors(string propertyName)
        {
            if (propertyErrors.ContainsKey(propertyName))
            {
                ICollection<ValidationResult> existingErrors = null;
                propertyErrors.TryRemove(propertyName, out existingErrors);
                NotifyErrorChanged(propertyName);
            }
        }

        protected void SetErrors(string propertyName, ICollection<ValidationResult> errors)
        {
            if (propertyErrors.ContainsKey(propertyName))
            {
                ICollection<ValidationResult> existingErrors = null;
                propertyErrors.TryRemove(propertyName, out existingErrors);
            }
            propertyErrors.TryAdd(propertyName, errors);
            NotifyErrorChanged(propertyName);
        }

        public bool HasErrors
        {
            get { return propertyErrors.Count > 0; }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) //Retrieve errors for entire object
                return propertyErrors.Values;
            else if (propertyErrors.ContainsKey(propertyName) &&
                    (propertyErrors[propertyName] != null) &&
                    propertyErrors[propertyName].Count > 0)
                return propertyErrors.GetOrAdd(propertyName, (key) => null);
            else
                return null;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void NotifyErrorChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        // object is valid
        public bool IsValid
        {
            get { return !this.HasErrors; }

        }

        #endregion
    }
}
