// -----------------------------------------------------------------------
// <copyright file="NotifyPropertyChangedBase.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.WPF
{
    using System.ComponentModel;

    /// <summary>
    /// Base class for classes that need to notify WPF of property changes.
    /// </summary>
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed notification event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property changed method
        /// </summary>
        /// <param name="propertyName">Name of property that changed</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}