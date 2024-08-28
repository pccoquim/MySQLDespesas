/*
Frm_030202_DebitoDetEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_030202_DebitoDetEdit.xaml
    /// </summary>
    public partial class Frm_030202_DebitoDetEdit : Window
    {
        public bool DDEdit { get; private set; }
        private bool Comb = false;
        private string Viatura = "";
        private string TaxaIVA = "";
        private string taxaIVAcbx = "";
        private readonly int linha, kmi, kmf, kme;
        private readonly string codigo, descr, unidade, data, iva, viatura;
        private readonly decimal quantidade, precoUnitario, desconto1, desconto2, valorDesconto, precoFinalUnitario, valorTotal;
        private readonly bool comb;
        private string DDCodigo, DDDescr, DDIVA, DDTaxaIVA, DDViatura;
        private decimal DDQuantidade, DDPrecoUnitario, DDDesconto1, DDDesconto2, DDValorDesconto, DDPrecoFinalUnitario, DDValorTotal, DDDiferenca;
        private bool DDComb;
        private int DDKMI, DDKMF, DDKME, DDIdLinha;

        public Frm_030202_DebitoDetEdit(int linha, string codigo, string descr, string unidade, decimal quantidade, decimal precoUnitario, decimal desconto1, decimal desconto2, decimal valorDesconto, decimal precoFinalUnitario, decimal valorTotal, string iva, bool comb, string viatura, int kmi, int kmf, int kme)
        {
            InitializeComponent();
            Encolher();
            Inicio();
            LoadCbx_IVA();
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
            this.iva = iva;
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
            cbx_IVA.SelectedValue = iva;
            if (comb == true)
            {
                ckb_Comb.IsChecked = true;
            }
            else
            {
                ckb_Comb.IsChecked = false;
            }
            cbx_Viatura.SelectedValue = viatura;
            txt_KMI.Text = kmi.ToString();
            txt_KMF.Text = kmf.ToString();
            txt_KME.Text = kme.ToString();
        }

        public event EventHandler<Cls_030202_DebitoDetEdit> EditCompleted;

        private void Inicio()
        {
            DDIdLinha = 0;
            DDCodigo = "0";
            DDDescr = "";
            DDQuantidade = 0;
            DDPrecoUnitario = 0;
            DDDesconto1 = 0;
            DDDesconto2 = 0;
            DDValorDesconto = 0;
            DDPrecoFinalUnitario = 0;
            DDValorTotal = 0;
            DDIVA = "0";
            DDTaxaIVA = "0";
            DDComb = false;
            DDViatura = "0";
            DDKMI = 0;
            DDKMF = 0;
            DDKME = 0;
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

        private void LoadCbx_IVA()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT iva_cod, iva_taxa FROM tbl_0206_taxasiva WHERE iva_status = 1 ORDER BY iva_taxa";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cbx_IVA.ItemsSource = dt.DefaultView;
                        taxaIVAcbx = "iva_taxa";
                        cbx_IVA.SelectedItem = null;
                        cbx_IVA.DisplayMemberPath = taxaIVAcbx;
                        cbx_IVA.SelectedValuePath = "iva_cod";
                        cbx_IVA.SelectedIndex = -1;
                        cbx_IVA.IsEditable = false;
                        // Manipular o evento de seleção do ComboBox
                        cbx_IVA.SelectionChanged += Cbx_IVA_SelectionChanged;

                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1076)!" + ex.Message);
                }
            }
        }

        private void LoadCbx_Viatura()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT vtr_cod, vtr_matricula, vtr_descr FROM tbl_0205_viaturas WHERE vtr_status = 1 ORDER BY vtr_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cbx_Viatura.ItemTemplate = CreateItemTemplate();
                        cbx_Viatura.ItemsSource = dt.DefaultView;
                        //  cbx_Viatura.DisplayMemberPath = "vtr_matricula";
                        cbx_Viatura.SelectedValuePath = "vtr_cod";
                        cbx_Viatura.SelectedIndex = -1;
                        cbx_Viatura.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Viatura (Erro: C1081)!" + ex.Message);
                }
            }
        }

        private DataTemplate CreateItemTemplate()
        {
            DataTemplate template = new DataTemplate();

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory matriculaTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            matriculaTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("vtr_matricula"));
            stackPanelFactory.AppendChild(matriculaTextBlockFactory);

            FrameworkElementFactory descrTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            descrTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("vtr_descr"));
            stackPanelFactory.AppendChild(descrTextBlockFactory);

            template.VisualTree = stackPanelFactory;

            return template;
        }

        private bool IsTextAllowed(string text)
        {
            // Define uma expressão regular para verificar se o texto contém apenas dígitos e um ponto
            Regex regex = new Regex("[^0-9.,-]+");
            return !regex.IsMatch(text);
        }

        private void Calculos()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            if (Decimal.TryParse(txt_Quantidade.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Qtd) &&
                Decimal.TryParse(txt_PrecoBase.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal PrecoUnitario) &&
                Decimal.TryParse(txt_Desconto1.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Desconto1) &&
                Decimal.TryParse(txt_Desconto2.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Desconto2))
            {
                decimal ValorDesconto1 = (PrecoUnitario * Desconto1) / 100;
                decimal ValorDesconto2 = (PrecoUnitario - ValorDesconto1) * Desconto2 / 100;
                decimal ValorDesconto = ValorDesconto1 + ValorDesconto2;
                decimal PrecoUnitarioFinal = PrecoUnitario - ValorDesconto;
                decimal ValorFinal = Qtd * PrecoUnitarioFinal;
                txt_ValorDesconto.Text = ValorDesconto.ToString("0.0000");
                txt_ValorUnitario.Text = PrecoUnitarioFinal.ToString("0.0000");
                txt_ValorTotal.Text = ValorFinal.ToString("0.0000"); // "N" para formatar como o sistema usa.
            }
        }

        private void Quilometros()
        {
            // variavel para numeração
            int KMI = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                //Contrlo de erros
                try
                {
                    conn.Open();
                    // Seleção do código mais elevado
                    string query = "SELECT MAX(md_kmi) FROM tbl_0302_movimentosdebito_det WHERE md_codviatura = ?";

                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@vtrID", DDViatura);
                        // Obtenção do resultado
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            KMI = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception)
                {
                    // Mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1011)!");
                }
                DDKMI = KMI;
            }
        }

        private void Cbx_IVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_IVA.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cbx_IVA.SelectedItem;
                TaxaIVA = selectedRow["iva_taxa"].ToString();
            }
            else
            {
                TaxaIVA = null; // Nenhum item selecionado
            }
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
            LoadCbx_Viatura();
            cbx_Viatura.Visibility = Visibility.Visible;
            cbx_Viatura.Margin = new Thickness(150, 400, 0, 0);
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
            btn_AddItems.Margin = new Thickness(150, 525, 0, 0);
            btn_Close.Margin = new Thickness(220, 525, 0, 0);
            this.Height = 610;
        }

        private void Encolher()
        {
            lbl_Viatura.Visibility = Visibility.Collapsed;
            lbl_Viatura.Margin = new Thickness(10, 400, 0, 0);
            cbx_Viatura.Visibility = Visibility.Collapsed;
            cbx_Viatura.Margin = new Thickness(150, 405, 0, 0);
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
            btn_AddItems.Margin = new Thickness(150, 405, 0, 0);
            btn_Close.Margin = new Thickness(220, 405, 0, 0);
            DDViatura = "0";
            this.Height = 490;
        }

        private void Cbx_Viatura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DDViatura = cbx_Viatura.SelectedValue.ToString();
            Quilometros();
            txt_KMI.Text = DDKMI.ToString();
            txt_KMF.Text = "0";
            txt_KME.Text = "0";
        }

        private void Txt_Quantidade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Quantidade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
            if (e.Text == ".")
            {
                // Substitua o ponto por uma vírgula
                e.Handled = true; // Impede que o ponto seja inserido
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, ",");
                textBox.CaretIndex = caretIndex + 1; // Coloca o cursor após a vírgula
            }
        }

        private void Txt_Quantidade_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculos();
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    // Formate o valor com quatro casas decimais
                    textBox.Text = valor.ToString("0.0000");
                }
                else
                {
                    // Se a entrada não for um número válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido.");
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Txt_Quantidade_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void Txt_PrecoBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_PrecoBase_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
            if (e.Text == ".")
            {
                // Substitua o ponto por uma vírgula
                e.Handled = true; // Impede que o ponto seja inserido
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, ",");
                textBox.CaretIndex = caretIndex + 1; // Coloca o cursor após a vírgula
            }
        }

        private void Txt_PrecoBase_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculos();
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    // Formate o valor com quatro casas decimais
                    textBox.Text = valor.ToString("0.0000");
                }
                else
                {
                    // Se a entrada não for um número válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido.");
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Txt_PrecoBase_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void Txt_Desconto1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Desconto1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
            if (e.Text == ".")
            {
                // Substitua o ponto por uma vírgula
                e.Handled = true; // Impede que o ponto seja inserido
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, ",");
                textBox.CaretIndex = caretIndex + 1; // Coloca o cursor após a vírgula
            }
        }

        private void Txt_Desconto1_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculos();
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    // Formate o valor com quatro casas decimais
                    textBox.Text = valor.ToString("0.0000");
                }
                else
                {
                    // Se a entrada não for um número válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido.");
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Txt_Desconto1_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void Txt_Desconto2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Desconto2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
            if (e.Text == ".")
            {
                // Substitua o ponto por uma vírgula
                e.Handled = true; // Impede que o ponto seja inserido
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, ",");
                textBox.CaretIndex = caretIndex + 1; // Coloca o cursor após a vírgula
            }
        }

        private void Txt_Desconto2_LostFocus(object sender, RoutedEventArgs e)
        {
            Calculos();
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    // Formate o valor com quatro casas decimais
                    textBox.Text = valor.ToString("0.0000");
                }
                else
                {
                    // Se a entrada não for um número válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido.");
                    textBox.Text = string.Empty;
                }
            }
        }

        private void Txt_Desconto2_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void Txt_KMF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_KMF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Txt_KMF_LostFocus(object sender, RoutedEventArgs e)
        {
            int Kmi = Convert.ToInt32(txt_KMI.Text);
            int Kmf = Convert.ToInt32(txt_KMF.Text);
            int Kme = Kmf - Kmi;
            txt_KME.Text = Kme.ToString();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDecimal(txt_Quantidade.Text) > 0)
            {
                if (Convert.ToDecimal(txt_PrecoBase.Text) > 0)
                {
                    if (cbx_IVA.SelectedIndex != -1)
                    {
                        if (ckb_Comb.IsChecked == true)
                        {
                            Comb = true;
                            Viatura = cbx_Viatura.SelectedValue.ToString();
                            if (cbx_Viatura.SelectedIndex != -1)
                            {
                                if (Convert.ToInt32(txt_KME.Text) > 0)
                                {
                                    DDEdit = true;
                                    DDIdLinha = Convert.ToInt32(txt_Linha.Text);
                                    if (decimal.TryParse(txt_Quantidade.Text, out decimal qtd))
                                    {
                                        DDQuantidade = qtd;
                                    }
                                    if (decimal.TryParse(txt_PrecoBase.Text, out decimal precoBase))
                                    {
                                        DDPrecoUnitario = precoBase;
                                    }
                                    if (decimal.TryParse(txt_Desconto1.Text, out decimal desconto1))
                                    {
                                        DDDesconto1 = desconto1;
                                    }
                                    if (decimal.TryParse(txt_Desconto2.Text, out decimal desconto2))
                                    {
                                        DDDesconto2 = desconto2;
                                    }
                                    if (decimal.TryParse(txt_ValorDesconto.Text, out decimal valorDesconto))
                                    {
                                        DDValorDesconto = valorDesconto;
                                    }
                                    if (decimal.TryParse(txt_ValorUnitario.Text, out decimal valorUnitario))
                                    {
                                        DDPrecoFinalUnitario = valorUnitario;
                                    }
                                    if (decimal.TryParse(txt_ValorTotal.Text, out decimal valorTotalAtual))
                                    {
                                        decimal diferenca = valorTotalAtual - valorTotal;
                                        DDValorTotal = valorTotalAtual;
                                        DDDiferenca = diferenca;
                                    }

                                    Cls_030202_DebitoDetEdit debitoDetRowEdit = new Cls_030202_DebitoDetEdit()
                                    {
                                        Line = DDIdLinha,
                                        Code = txt_Codigo.Text,
                                        Designation = txt_Descr.Text,
                                        Unit = txt_Unidade.Text,
                                        Quantity = DDQuantidade,
                                        PriceUnit = DDPrecoUnitario,
                                        Discount1 = DDDesconto1,
                                        Discount2 = DDDesconto2,
                                        DiscountValue = DDValorDesconto,
                                        FinalPriceUnit = DDPrecoFinalUnitario,
                                        Difference = DDDiferenca,
                                        TotalValue = DDValorTotal,
                                        Iva = cbx_IVA.SelectedValue.ToString(),
                                        IvaTax = TaxaIVA,
                                        Combustivel = Comb,
                                        Vehicle = viatura,
                                        InitialKm = Convert.ToInt32(txt_KMI.Text),
                                        FinalKm = Convert.ToInt32(txt_KMF.Text),
                                        CompletedKm = Convert.ToInt32(txt_KME.Text)
                                    };
                                    EditCompleted?.Invoke(this, debitoDetRowEdit);
                                    this.Close();
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Os quilimetros efetuados tem de ser superiores a zero!");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Não está selecionada nenhuma vistura");
                            }
                        }
                        else
                        {
                            Comb = false;
                            Viatura = "0";
                            DDEdit = true;
                            DDIdLinha = Convert.ToInt32(txt_Linha.Text);
                            if (decimal.TryParse(txt_Quantidade.Text, out decimal qtd))
                            {
                                DDQuantidade = qtd;
                            }
                            if (decimal.TryParse(txt_PrecoBase.Text, out decimal precoBase))
                            {
                                DDPrecoUnitario = precoBase;
                            }
                            if (decimal.TryParse(txt_Desconto1.Text, out decimal desconto1))
                            {
                                DDDesconto1 = desconto1;
                            }
                            if (decimal.TryParse(txt_Desconto2.Text, out decimal desconto2))
                            {
                                DDDesconto2 = desconto2;
                            }
                            if (decimal.TryParse(txt_ValorDesconto.Text, out decimal valorDesconto))
                            {
                                DDValorDesconto = valorDesconto;
                            }
                            if (decimal.TryParse(txt_ValorUnitario.Text, out decimal valorUnitario))
                            {
                                DDPrecoFinalUnitario = valorUnitario;
                            }
                            if (decimal.TryParse(txt_ValorTotal.Text, out decimal valorTotalAtual))
                            {
                                decimal diferenca = valorTotalAtual - valorTotal;
                                DDValorTotal = valorTotalAtual;
                                DDDiferenca = diferenca;
                            }

                            Cls_030202_DebitoDetEdit debitoDetRowEdit = new Cls_030202_DebitoDetEdit()
                            {
                                Line = DDIdLinha,
                                Code = txt_Codigo.Text,
                                Designation = txt_Descr.Text,
                                Unit = txt_Unidade.Text,
                                Quantity = DDQuantidade,
                                PriceUnit = DDPrecoUnitario,
                                Discount1 = DDDesconto1,
                                Discount2 = DDDesconto2,
                                DiscountValue = DDValorDesconto,
                                FinalPriceUnit = DDPrecoFinalUnitario,
                                TotalValue = DDValorTotal,
                                Iva = cbx_IVA.SelectedValue.ToString(),
                                IvaTax = TaxaIVA,
                                Combustivel = Comb,
                                Vehicle = viatura,
                                InitialKm = Convert.ToInt32(txt_KMI.Text),
                                FinalKm = Convert.ToInt32(txt_KMF.Text),
                                CompletedKm = Convert.ToInt32(txt_KME.Text)
                            };
                            EditCompleted?.Invoke(this, debitoDetRowEdit);
                            this.Close();
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("O IVA é obrigatório e não está selecionada nenhuma taxa!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("O preço tem de ser superior a zero!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("A quantidade tem de ser superior a zero!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DDEdit = false;
            this.Close();
        }
    }
}
