// -----------------------------------------------------------------------
// <copyright file="ClearableTextBox.xaml.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.WPF.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ClearableTextBox.xaml
    /// </summary>
    public partial class ClearableTextBox : UserControl
    {
        /// <summary>
        /// The text that is visible when the TextBox does not have any text.
        /// </summary>
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(ClearableTextBox));

        /// <summary>
        /// The input text property.
        /// </summary>
        public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register("InputText", typeof(string), typeof(ClearableTextBox));

        /// <summary>
        /// Instance of the clear Button in the control.
        /// </summary>
        private Button clearButton;

        /// <summary>
        /// Instance of the TextBlock for the hint message in the control.
        /// </summary>
        private TextBlock hintControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearableTextBox"/> class.
        /// </summary>
        public ClearableTextBox()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the hint text that appears when there is no input.
        /// </summary>
        public string HintText
        {
            get { return this.GetValue(HintTextProperty).ToString(); }
            set { this.SetValue(HintTextProperty, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the text binding for the input.
        /// </summary>
        public string InputText
        {
            get { return this.GetValue(InputTextProperty).ToString(); }
            set { this.SetValue(InputTextProperty, value.ToString()); }
        }

        /// <summary>
        /// ClearButton click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.searchText.Text = string.Empty;
            this.UnfocusElement(this.searchText);
        }

        /// <summary>
        /// SearchText loaded event handler.
        /// Obtains the embedded controls that the other event handlers use.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SearchText_Loaded(object sender, RoutedEventArgs e)
        {
            this.hintControl = (TextBlock)searchText.Template.FindName("hintText", searchText);
            this.clearButton = (Button)searchText.Template.FindName("clearButton", searchText);
        }

        /// <summary>
        /// SearchText KeyDown event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.UnfocusElement(this.searchText);
            }
        }

        /// <summary>
        /// SearchText TextChanged event handler.
        /// Sets the embedded control's visibilities depending on the search text's value.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchText.Text))
            {
                this.hintControl.Visibility = System.Windows.Visibility.Visible;
                this.clearButton.Visibility = System.Windows.Visibility.Hidden;
                return;
            }

            this.hintControl.Visibility = System.Windows.Visibility.Hidden;
            this.clearButton.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Removes input focus from the passed in control.
        /// </summary>
        /// <param name="control">Control to remove focus from.</param>
        private void UnfocusElement(FrameworkElement control)
        {
            if (!control.IsFocused)
            {
                return;
            }

            // Move to a parent that can take focus
            FrameworkElement parent = (FrameworkElement)control.Parent;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
            {
                parent = (FrameworkElement)parent.Parent;
            }

            DependencyObject scope = FocusManager.GetFocusScope(control);
            FocusManager.SetFocusedElement(scope, parent as IInputElement);
        }
    }
}