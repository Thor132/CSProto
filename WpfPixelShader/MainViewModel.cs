
namespace WpfPixelShader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using System.Windows;
    using CSHelperLibrary.WPF;

    public class MainViewModel
    {
        private bool enabled;

        public ICommand EnableCommand
        {
            get
            {
                return new RelayCommand(
                    param =>
                    {
                        this.enabled = true;
                    });
            }
        }

        public ICommand DisableCommand
        {
            get
            {
                return new RelayCommand(
                    param =>
                    {
                        this.enabled = false;
                    },
                    param => this.enabled);
            }
        }
    }
}
