/*
Frm_030103_DebitoCabDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_030103_DebitoCabDelete.xaml
    /// </summary>
    public partial class Frm_030103_DebitoCabDelete : Window
    {
        private readonly int selectedDebCabRef, selectedDebCabId;
        public Frm_030103_DebitoCabDelete(int selectedDebCabRef, int selectedDebCabId)
        {
            InitializeComponent();
            this.selectedDebCabId = selectedDebCabId;
            this.selectedDebCabRef = selectedDebCabRef;
        }
    }
}
