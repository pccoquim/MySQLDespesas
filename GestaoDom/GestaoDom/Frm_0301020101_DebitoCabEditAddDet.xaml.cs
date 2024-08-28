/*
Frm_0301020101_DebitoCabEditAddDet.xaml.cs
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
    /// Interaction logic for Frm_0301020101_DebitoCabEditAddDet.xaml
    /// </summary>
    public partial class Frm_0301020101_DebitoCabEditAddDet : Window
    {
        // private readonly List<Cls_030201_DebitosDetLinha> cls_030201_DebitosDetLinhas = new List<Cls_030201_DebitosDetLinha>();
        private readonly MySqlConnection conn;
        private MySqlTransaction transaction;
        private readonly string loginUserId;
        private readonly int selectedDebCabRef, selectedDebCabId;
        private string taxaIVA, taxaIVAcbx = "", nLinha = "";
        private bool combustivel = false;
        private readonly string codigo;
        private readonly string descr;
        private readonly string unidade;
        private string debDetViatura;
        private readonly string debDetConta;
        private int kmi;
        private decimal precoBase = 0;
        private decimal desconto1 = 0;
        private decimal desconto2 = 0;
        private decimal valorDesconto = 0;
        private decimal valorFinal = 0;
        private decimal valorTotal = 0;

        public Frm_0301020101_DebitoCabEditAddDet(string loginUserId, int selectedDebCabRef, int selectedDebCabId, string codigo, string descr, string unidade)
        {
            InitializeComponent();
            LoadCbx_IVA();
            Encolher();
            this.codigo = codigo;
            this.descr = descr;
            this.unidade = unidade;
            this.loginUserId = loginUserId;
            this.selectedDebCabRef = selectedDebCabRef;
            this.selectedDebCabId = selectedDebCabId;
            txt_Codigo.Text = codigo;
            txt_Descr.Text = descr;
            txt_Unidade.Text = unidade;
        }

        private void LoadCbx_IVA()
        {
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
            // ComboBox viatura
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

            if (Decimal.TryParse(txt_Quantidade.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Qtd) /*&&
                Decimal.TryParse(Txt_PrecoBase.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal PrecoUnitario) &&
                Decimal.TryParse(txt_Desconto1.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Desconto1) &&
                Decimal.TryParse(txt_Desconto2.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Desconto2)*/)
            {
                decimal ValorDesconto1 = (precoBase * desconto1) / 100;
                decimal ValorDesconto2 = (precoBase - ValorDesconto1) * desconto2 / 100;
                decimal ValorDesconto = ValorDesconto1 + ValorDesconto2;
                decimal PrecoUnitarioFinal = precoBase - ValorDesconto;
                decimal ValorFinal = Qtd * PrecoUnitarioFinal;
                valorDesconto = ValorDesconto;
                txt_ValorDesconto.Text = ValorDesconto.ToString("0.0000 €");
                valorFinal = PrecoUnitarioFinal;
                txt_ValorUnitario.Text = PrecoUnitarioFinal.ToString("0.0000 €");
                valorTotal = ValorFinal;
                txt_ValorTotal.Text = ValorFinal.ToString("0.00 €"); // "N" para formatar como o sistema usa.
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
                        cmd.Parameters.AddWithValue("@vtrID", debDetViatura);
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
                kmi = KMI;
            }
        }

        private void NumeracaoLinha()
        {
            // variavel para numeração
            int ultimoId = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                //Contrlo de erros
                try
                {
                    conn.Open();
                    // Seleção do código mais elevado
                    string query = "SELECT MAX(md_id_linha) FROM tbl_0302_movimentosdebito_det WHERE md_id_fatura = ?";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idFatura", selectedDebCabRef);
                        // Obtenção do resultado: nulo, paasa a um, valor soma um
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            ultimoId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception)
                {
                    // Mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1011)!");
                }
                nLinha = (ultimoId + 1).ToString();
            }
        }

        public decimal ValidarDebito(decimal debito)
        {
            decimal mov = Convert.ToDecimal(debito);
            decimal Saldo = 0;
            decimal valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT cntcred_saldo FROM tbl_0103_contascred WHERE cntcred_cod = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@conta", debDetConta);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                Saldo = Convert.ToDecimal(reader["cntcred_saldo"]);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1021)!");

                }
                valor = Saldo - mov;

                return valor;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void UpdateCab()
        {
            string insertQuery = "UPDATE tbl_0301_movimentosdebito SET fd_id_fatura = ?, fd_valor = fd_valor + ?, fd_datelastchg = ?, fd_timelastchg = ?";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn, transaction);
            insertCommand.Parameters.AddWithValue("@id_fatura", selectedDebCabId);
            insertCommand.Parameters.AddWithValue("@valor", valorTotal);
            insertCommand.Parameters.AddWithValue("@userchg", loginUserId);
            insertCommand.Parameters.AddWithValue("@datechg", Cls_0002_ActualDateTime.Date);
            insertCommand.Parameters.AddWithValue("@timechg", Cls_0002_ActualDateTime.Time);

            insertCommand.ExecuteNonQuery();
        }

        private void InsertDet()
        {
            string insertQuery = "INSERT INTO tbl_0302_movimentosdebito_det (md_id_fatura, md_id_linha, md_codartigo, md_codiva, md_quantidade, md_precobase, md_desconto1, md_desconto2, md_valordesconto, md_precofinal, md_combustivel, md_codviatura ,md_kmi, md_kmf, md_kmefetuados, md_status, md_usercreate, md_datecreate, md_timecreate, md_userlastchg, md_datelastchg, md_timelastchg) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn, transaction);
            insertCommand.Parameters.AddWithValue("@Id_fatura", selectedDebCabRef);
            insertCommand.Parameters.AddWithValue("@Id_linha", nLinha);
            insertCommand.Parameters.AddWithValue("@CodArtigo", codigo);
            insertCommand.Parameters.AddWithValue("@Codiva", cbx_IVA.SelectedValue);
            insertCommand.Parameters.AddWithValue("@Quantidade", txt_Quantidade.Text);
            insertCommand.Parameters.AddWithValue("@Precobase", precoBase);
            insertCommand.Parameters.AddWithValue("@Desconto1", desconto1);
            insertCommand.Parameters.AddWithValue("@Desconto2", desconto2);
            insertCommand.Parameters.AddWithValue("@Valordesconto", valorDesconto);
            insertCommand.Parameters.AddWithValue("@Precofinal", valorFinal);
            insertCommand.Parameters.AddWithValue("@Combustivel", combustivel);
            insertCommand.Parameters.AddWithValue("@Viatura", cbx_Viatura.SelectedValue);
            insertCommand.Parameters.AddWithValue("@KMI", txt_KMF.Text);
            insertCommand.Parameters.AddWithValue("@KMF", 0);
            insertCommand.Parameters.AddWithValue("@KME", 0);
            insertCommand.Parameters.AddWithValue("@status", 1);
            insertCommand.Parameters.AddWithValue("@user", loginUserId);
            insertCommand.Parameters.AddWithValue("@date", Cls_0002_ActualDateTime.Date);
            insertCommand.Parameters.AddWithValue("@time", Cls_0002_ActualDateTime.Time);
            insertCommand.Parameters.AddWithValue("@userchg", "0");
            insertCommand.Parameters.AddWithValue("@datechg", "0");
            insertCommand.Parameters.AddWithValue("@timechg", "0");

            insertCommand.ExecuteNonQuery();
        }

        private void UpdateDet()
        {
            string updateQuery = "UPDATE tbl_0302_movimentosdebito_det SET md_kmf = ?, md_kmefetuados = ? WHERE md_codviatura = ? AND md_kmi = ?";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn, transaction);
            updateCommand.Parameters.AddWithValue("@KMF", txt_KMF.Text);
            updateCommand.Parameters.AddWithValue("@KME", txt_KME.Text);
            updateCommand.Parameters.AddWithValue("@Viatura", cbx_Viatura.SelectedValue);
            updateCommand.Parameters.AddWithValue("@KMI", txt_KMI.Text);

            updateCommand.ExecuteNonQuery();
        }

        private void UpdateSaldo()
        {
            string updateQuery = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn, transaction);
            updateCommand.Parameters.AddWithValue("@Valor", valorTotal);
            updateCommand.Parameters.AddWithValue("@Conta", debDetConta);

            updateCommand.ExecuteNonQuery();
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
            Calculos();
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
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    precoBase = valor;
                    // Formate o valor com quatro casas decimais
                    textBox.Text = valor.ToString("0.0000 €");
                }
                else
                {
                    // Se a entrada não for um número válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido.");
                    textBox.Text = string.Empty;
                }
            }
            Calculos();
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
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    desconto1 = valor;
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
            Calculos();
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
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    desconto2 = valor;
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
            Calculos();
        }

        private void Txt_Desconto2_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }


        private void Cbx_IVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_IVA.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cbx_IVA.SelectedItem;
                taxaIVA = selectedRow["iva_taxa"].ToString();
            }
            else
            {
                taxaIVA = null; // Nenhum item selecionado
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            combustivel = true;
            Esticar();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            combustivel = false;
            Encolher();
        }

        private void Esticar()
        {
            lbl_Viatura.Visibility = Visibility.Visible;
            lbl_Viatura.Margin = new Thickness(10, 370, 0, 0);
            LoadCbx_Viatura();
            cbx_Viatura.Visibility = Visibility.Visible;
            cbx_Viatura.Margin = new Thickness(150, 375, 0, 0);
            lbl_Kmi.Visibility = Visibility.Visible;
            lbl_Kmi.Margin = new Thickness(10, 400, 0, 0);
            txt_KMI.Visibility = Visibility.Visible;
            txt_KMI.Margin = new Thickness(150, 405, 0, 0);
            lbl_KmF.Visibility = Visibility.Visible;
            lbl_KmF.Margin = new Thickness(10, 430, 0, 0);
            txt_KMF.Visibility = Visibility.Visible;
            txt_KMF.Margin = new Thickness(150, 435, 0, 0);
            lbl_KmEfetuados.Visibility = Visibility.Visible;
            lbl_KmEfetuados.Margin = new Thickness(10, 460, 0, 0);
            txt_KME.Visibility = Visibility.Visible;
            txt_KME.Margin = new Thickness(150, 465, 0, 0);
            btn_Save.Margin = new Thickness(150, 495, 0, 0);
            btn_Close.Margin = new Thickness(220, 495, 0, 0);
            this.Height = 580;
        }

        private void Encolher()
        {
            lbl_Viatura.Visibility = Visibility.Collapsed;
            lbl_Viatura.Margin = new Thickness(10, 370, 0, 0);
            cbx_Viatura.Visibility = Visibility.Collapsed;
            cbx_Viatura.Margin = new Thickness(150, 375, 0, 0);
            lbl_Kmi.Visibility = Visibility.Collapsed;
            lbl_Kmi.Margin = new Thickness(10, 370, 0, 0);
            txt_KMI.Visibility = Visibility.Collapsed;
            txt_KMI.Margin = new Thickness(150, 375, 0, 0);
            lbl_KmF.Visibility = Visibility.Collapsed;
            lbl_KmF.Margin = new Thickness(10, 370, 0, 0);
            txt_KMF.Visibility = Visibility.Collapsed;
            txt_KMF.Margin = new Thickness(150, 375, 0, 0);
            lbl_KmEfetuados.Visibility = Visibility.Collapsed;
            lbl_KmEfetuados.Margin = new Thickness(10, 370, 0, 0);
            txt_KME.Visibility = Visibility.Collapsed;
            txt_KME.Margin = new Thickness(150, 375, 0, 0);
            btn_Save.Margin = new Thickness(150, 375, 0, 0);
            btn_Close.Margin = new Thickness(220, 375, 0, 0);
            debDetViatura = "0";
            this.Height = 460;
        }

        private void Cbx_Viatura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            debDetViatura = cbx_Viatura.SelectedValue.ToString();
            Quilometros();
            txt_KMI.Text = kmi.ToString();
        }

        private void Txt_KMF_GotFocus(object sender, RoutedEventArgs e)
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

        private void Txt_KMF_LostFocus(object sender, RoutedEventArgs e)
        {
            int Kmi = Convert.ToInt32(txt_KMI.Text);
            int Kmf = Convert.ToInt32(txt_KMF.Text);
            int Kme = Kmf - Kmi;
            txt_KME.Text = Kme.ToString();
        }

        private void Txt_KMF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDebito(valorTotal) >= 0)
            {
                NumeracaoLinha();
                try
                {
                    // Inicie a transação
                    transaction = conn.BeginTransaction();
                    // Update do cabeçalho da fatura (se ainda não inserido)
                    UpdateCab();
                    // Insert do detalhe
                    InsertDet();
                    // Update para saldo de conta
                    UpdateSaldo();
                    // Update para calculo de quilometros
                    UpdateDet();
                    // Faça commit da transação se tudo estiver bem
                    transaction.Commit();
                    MessageBox.Show("Inclusáo concluida com sucesso!");
                }
                catch (Exception ex)
                {
                    // Em caso de erro, faça rollback da transação e mostre uma mensagem de erro
                    transaction.Rollback();
                    MessageBox.Show("Erro ao criar a referência: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                this.Close();
            }
            else
            {
                // Não existem alterações para gravar
                if (ShowConfirmation("Este movimento provoca um saldo negativo de " + Convert.ToDecimal(ValidarDebito(valorTotal)) + " na conta a débito. Deseja continuar?"))
                {
                    NumeracaoLinha();
                    try
                    {
                        // Inicie a transação
                        transaction = conn.BeginTransaction();
                        // Insira o cabeçalho da fatura (se ainda não inserido)
                        UpdateCab();
                        // Insira todas as linhas da fatura
                        InsertDet();
                        // Update para saldo de conta
                        UpdateSaldo();
                        // Update para calculo de quilometros
                        UpdateDet();
                        // Faça commit da transação se tudo estiver bem
                        transaction.Commit();
                        MessageBox.Show("Inclusáo concluida com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        // Em caso de erro, faça rollback da transação e mostre uma mensagem de erro
                        transaction.Rollback();
                        MessageBox.Show("Erro ao criar a referência: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}