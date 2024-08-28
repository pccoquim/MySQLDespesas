/*
Frm_030203_DebitosDetDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using System;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_030203_DebitosDetDelete.xaml
    /// </summary>
    public partial class Frm_030203_DebitoDetDelete : Window
    {
        public bool DDDelete { get; private set; }
        private readonly int linha, kmi, kmf, kme;
        private readonly string codigo, descr, unidade, data, taxaIva, viatura;
        private readonly decimal quantidade, precoUnitario, desconto1, desconto2, valorDesconto, precoFinalUnitario, valorTotal;
        private readonly bool comb;
        private int DDIdLinha;
        private decimal DDValorTotal;
        public Frm_030203_DebitoDetDelete(int linha, string codigo, string descr, string unidade, decimal quantidade, decimal precoUnitario, decimal desconto1, decimal desconto2, decimal valorDesconto, decimal precoFinalUnitario, decimal valorTotal, string taxaIva, bool comb, string viatura, int kmi, int kmf, int kme)
        {
            InitializeComponent();
            Encolher();
            Inicio();
            this.linha = linha;
            this.codigo = codigo;
            this.descr = descr;
            this.unidade = unidade;
            this.quantidade = quantidade;
            this.precoUnitario = precoUnitario;
            this.desconto1 = desconto1;
            this.desconto2 = desconto2;
            this.valorDesconto = valorDesconto;
            this.precoFinalUnitario = precoFinalUnitario;
            this.valorTotal = valorTotal;
            this.taxaIva = taxaIva;
            this.comb = comb;
            this.viatura = viatura;
            this.kmi = kmi;
            this.kmf = kmf;
            this.kme = kme;

            txt_Linha.Text = linha.ToString();
            txt_Codigo.Text = codigo;
            txt_Descr.Text = descr;
            txt_Unidade.Text = unidade;
            txt_Quantidade.Text = quantidade.ToString();
            txt_PrecoBase.Text = precoUnitario.ToString();
            txt_Desconto1.Text = desconto1.ToString();
            txt_Desconto2.Text = desconto2.ToString();
            txt_ValorDesconto.Text = valorDesconto.ToString();
            txt_ValorUnitario.Text = precoFinalUnitario.ToString();
            txt_ValorTotal.Text = valorTotal.ToString();
            txt_Iva.Text = taxaIva;
            if (comb == true)
            {
                ckb_Comb.IsChecked = true;
            }
            else
            {
                ckb_Comb.IsChecked = false;
            }
            txt_Viatura.Text = viatura;
            txt_KMI.Text = kmi.ToString();
            txt_KMF.Text = kmf.ToString();
            txt_KME.Text = kme.ToString();
            DDValorTotal = valorTotal;
        }

        public event EventHandler<Cls_030203_DebitoDetDelete> DeleteCompleted;

        private void Inicio()
        {
            DDIdLinha = 0;
            DDValorTotal = 0;
            txt_Quantidade.Text = "0,0000";
            txt_PrecoBase.Text = "0,0000";
            txt_Desconto1.Text = "0,0000";
            txt_Desconto2.Text = "0,0000";
            txt_ValorDesconto.Text = "0,0000";
            txt_ValorUnitario.Text = "0,0000";
            txt_ValorTotal.Text = "0,0000";
            txt_KMI.Text = "0";
            txt_KMF.Text = "0";
            txt_KME.Text = "0";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Esticar();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Encolher();
        }

        private void Esticar()
        {
            lbl_Viatura.Visibility = Visibility.Visible;
            lbl_Viatura.Margin = new Thickness(10, 400, 0, 0);
            txt_Viatura.Visibility = Visibility.Visible;
            txt_Viatura.Margin = new Thickness(150, 400, 0, 0);
            lbl_Kmi.Visibility = Visibility.Visible;
            lbl_Kmi.Margin = new Thickness(10, 430, 0, 0);
            txt_KMI.Visibility = Visibility.Visible;
            txt_KMI.Margin = new Thickness(150, 435, 0, 0);
            lbl_KmF.Visibility = Visibility.Visible;
            lbl_KmF.Margin = new Thickness(10, 460, 0, 0);
            txt_KMF.Visibility = Visibility.Visible;
            txt_KMF.Margin = new Thickness(150, 465, 0, 0);
            lbl_KmEfetuados.Visibility = Visibility.Visible;
            lbl_KmEfetuados.Margin = new Thickness(10, 490, 0, 0);
            txt_KME.Visibility = Visibility.Visible;
            txt_KME.Margin = new Thickness(150, 495, 0, 0);
            btn_Delete.Margin = new Thickness(150, 525, 0, 0);
            btn_Close.Margin = new Thickness(220, 525, 0, 0);
            this.Height = 610;
        }

        private void Encolher()
        {
            lbl_Viatura.Visibility = Visibility.Collapsed;
            lbl_Viatura.Margin = new Thickness(10, 400, 0, 0);
            txt_Viatura.Visibility = Visibility.Collapsed;
            txt_Viatura.Margin = new Thickness(150, 405, 0, 0);
            lbl_Kmi.Visibility = Visibility.Collapsed;
            lbl_Kmi.Margin = new Thickness(10, 400, 0, 0);
            txt_KMI.Visibility = Visibility.Collapsed;
            txt_KMI.Margin = new Thickness(150, 405, 0, 0);
            lbl_KmF.Visibility = Visibility.Collapsed;
            lbl_KmF.Margin = new Thickness(10, 400, 0, 0);
            txt_KMF.Visibility = Visibility.Collapsed;
            txt_KMF.Margin = new Thickness(150, 405, 0, 0);
            lbl_KmEfetuados.Visibility = Visibility.Collapsed;
            lbl_KmEfetuados.Margin = new Thickness(10, 400, 0, 0);
            txt_KME.Visibility = Visibility.Collapsed;
            txt_KME.Margin = new Thickness(150, 405, 0, 0);
            btn_Delete.Margin = new Thickness(150, 405, 0, 0);
            btn_Close.Margin = new Thickness(220, 405, 0, 0);
            this.Height = 490;
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            // Mensagem para confirmação da eliminação
            if (ShowConfirmation("Tem a certeza, que deseja eliminar a viatura?"))
            {
                DDDelete = true;
                DDIdLinha = Convert.ToInt32(txt_Linha.Text);
                if (decimal.TryParse(txt_ValorTotal.Text, out decimal valorTotal))
                {
                    DDValorTotal = valorTotal;
                }
                Cls_030203_DebitoDetDelete debitoDetRowDelete = new Cls_030203_DebitoDetDelete()
                {
                    Line = DDIdLinha,
                    TotalValue = DDValorTotal,
                };
                DeleteCompleted?.Invoke(this, debitoDetRowDelete);
                this.Close();
            }
            else
            {
                DDDelete = false;
                return;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DDDelete = false;
            this.Close();
        }
    }
}
