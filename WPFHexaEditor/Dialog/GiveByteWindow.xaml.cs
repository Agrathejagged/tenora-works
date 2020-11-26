//////////////////////////////////////////////
// Apache 2.0 - 2018-2019
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System.Windows;

namespace WpfHexaEditor.Dialog
{
    /// <summary>
    /// This Window is used to give a hex value for fill the selection with.
    /// </summary>
    internal partial class GiveByteWindow
    {
        public GiveByteWindow() => InitializeComponent();

        private void OKButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}