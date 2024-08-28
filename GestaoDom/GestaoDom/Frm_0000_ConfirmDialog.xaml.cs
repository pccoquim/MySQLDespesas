/*
#################################################################
#   Projeto:        GestaoDom                                   #
#   Arquivo:        frm_0000_ConfirmDialog.xaml.cs              #
#   Autor:          Paulo da Cruz Coquim                        #
#   Data:           07.06.2024                                  #
#   Data alteração: 28.08.2024                                  #
#   Versão:         1.1.2.0                                     #
#################################################################
*/
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class Frm_0000_ConfirmDialog : Window
    {
        public Frm_0000_ConfirmDialog()
        {
            InitializeComponent();
        }

        public bool Confirmed { get; private set; }

        public Frm_0000_ConfirmDialog(string message)
        {
            InitializeComponent();
            Txt_Message.Text = message;
        }

        private void Btn_Yes_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }

        private void Btn_No_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
