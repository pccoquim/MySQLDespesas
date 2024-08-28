/*
Frm_03010202_DebitoCabEditEditDet.xaml.cs
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
using System.Windows.Media;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_03010202_DebitoCabEditEditDet.xaml
    /// </summary>
    public partial class Frm_03010202_DebitoCabEditEditDet : Window
    {
        //private readonly List<Cls_030201_DebitosDetLinha> cls_030201_DebitosDetLinhas = new List<Cls_030201_DebitosDetLinha>();
        private readonly MySqlConnection conn;
        private MySqlTransaction transaction;
        private readonly string loginUserId;
        private readonly int selectedDebCabId, selectedDebCabRef;
        private string debDetViatura;
        private readonly string debCabConta;
        private int debDetKmi;
        private readonly string taxaIVA = "";
        private string taxaIVAC = "";
        private readonly string taxaIVAcbx = "";
        private readonly string nLinha = "";
        private readonly bool combustivel = false;
        private bool combustivelC = false;
        private readonly string linha = "";
        private string codArtigo = "";
        private string descrArtigo = "";
        private string descrUnidade = "";
        private string codIVA = "";
        private string codViatura = "";
        private string codStatus = "";
        private int linhaId = 0, faturaId = 0, linhaNum = 0, kmi = 0, kmf = 0, kme = 0;
        private decimal quantidade = 0;
        private decimal precoBase = 0;
        private readonly decimal desconto1 = 0;
        private decimal desconto2 = 0;
        private decimal valorDesconto = 0;
        private decimal valorUnitarioFinal = 0;
        private decimal valorTotal = 0;
        private decimal quantidadeC = 0, precoBaseC = 0, desconto1C = 0, desconto2C = 0, valorDescontoC = 0, valorUnitarioFinalC = 0, valorTotalC = 0;
        public Frm_03010202_DebitoCabEditEditDet(int linhaId, int faturaId, int linhaNum, string codArtigo, string descrArtigo, string descrUnidade, decimal quantidade, decimal precoBase, decimal desconto1, decimal desconto2, decimal valorDesconto, decimal valorUnitarioFinal, decimal valorTotal, string codIVA, bool combustivel, string viatura, int kmi, int kmf, int kme, string codStatus, string loginUserId, int selectedDebCabId, int selectedDebCabRef)
        {
            InitializeComponent();
            LoadCbx_IVA();
            LoadCbx_Viatura();
            LoadCbx_Status();
            Encolher();
            this.linhaId = linhaId;
            this.faturaId = faturaId;
            this.linhaNum = linhaNum;
            this.codArtigo = codArtigo;
            this.descrArtigo = descrArtigo;
            this.descrUnidade = descrUnidade;
            this.quantidade = quantidade;
            this.precoBase = precoBase;
            this.desconto1 = desconto1;
            this.desconto2 = desconto2;
            this.valorDesconto = valorDesconto;
            this.valorUnitarioFinal = valorUnitarioFinal;
            this.valorTotal = valorTotal;
            this.codIVA = codIVA;
            this.combustivel = combustivel;
            this.codViatura = viatura;
            this.kmi = kmi;
            this.kmf = kmf;
            this.kme = kme;
            this.codStatus = codStatus;
            txt_Linha.Text = linhaNum.ToString();
            txt_Codigo.Text = codArtigo.ToString();
            txt_Descr.Text = descrArtigo.ToString();
            txt_Unidade.Text = descrUnidade.ToString();
            txt_Quantidade.Text = quantidade.ToString("0.0000");
            txt_PrecoBase.Text = precoBase.ToString("0.0000 €");
            txt_Desconto1.Text = desconto1.ToString();
            txt_Desconto2.Text = desconto2.ToString();
            txt_ValorDesconto.Text = valorDesconto.ToString("0.0000 €");
            txt_ValorUnitario.Text = valorUnitarioFinal.ToString("0.0000 €");
            txt_ValorTotal.Text = valorTotal.ToString("0.00 €");
            cbx_IVA.SelectedValue = codIVA;
            if (combustivel == true)
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
            Cbx_Status.SelectedValue = codStatus;
            this.loginUserId = loginUserId;
            this.selectedDebCabId = selectedDebCabId;
            this.selectedDebCabRef = selectedDebCabRef;
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
                        //taxaIVAcbx = "iva_taxa";
                        cbx_IVA.SelectedItem = null;
                        cbx_IVA.DisplayMemberPath = "iva_taxa";
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
            // ComboBox viaturas
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

        private void LoadCbx_Status()
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT status_cod, status_descr, status_cor FROM tbl_0001_status ORDER BY status_descr";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        // Limpa a combobox antes de adicionar os itens
                        Cbx_Status.Items.Clear();
                        Cbx_Status.ItemsSource = dt.DefaultView;
                        Cbx_Status.DisplayMemberPath = "status_descr";
                        Cbx_Status.SelectedValuePath = "status_cod";
                        Cbx_Status.SelectedIndex = -1;
                        Cbx_Status.IsEditable = false;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Status (Erro: C1084)!");
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

            if (Decimal.TryParse(txt_Quantidade.Text.Replace(",", "."), NumberStyles.Number, culture, out decimal Qtd))
            {
                decimal ValorDesconto1 = (precoBaseC * desconto1C) / 100;
                decimal ValorDesconto2 = (precoBaseC - ValorDesconto1) * desconto2C / 100;
                decimal ValorDesconto = ValorDesconto1 + ValorDesconto2;
                decimal PrecoUnitarioFinal = precoBaseC - ValorDesconto;
                decimal ValorFinal = Qtd * PrecoUnitarioFinal;
                valorDescontoC = ValorDesconto;
                txt_ValorDesconto.Text = ValorDesconto.ToString("0.0000 €");
                valorUnitarioFinalC = PrecoUnitarioFinal;
                txt_ValorUnitario.Text = PrecoUnitarioFinal.ToString("0.0000 €");
                valorTotalC = ValorFinal;
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
                debDetKmi = KMI;
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
                        cmd.Parameters.AddWithValue("@conta", debCabConta);
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
            insertCommand.Parameters.AddWithValue("@CodArtigo", codArtigo);
            insertCommand.Parameters.AddWithValue("@Codiva", cbx_IVA.SelectedValue);
            insertCommand.Parameters.AddWithValue("@Quantidade", txt_Quantidade.Text);
            insertCommand.Parameters.AddWithValue("@Precobase", precoBase);
            insertCommand.Parameters.AddWithValue("@Desconto1", desconto1);
            insertCommand.Parameters.AddWithValue("@Desconto2", desconto2);
            insertCommand.Parameters.AddWithValue("@Valordesconto", valorDesconto);
            insertCommand.Parameters.AddWithValue("@Precofinal", valorUnitarioFinal);
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
            updateCommand.Parameters.AddWithValue("@Conta", debCabConta);

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
                    quantidadeC = valor;
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
                    precoBaseC = valor;
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
                    desconto1C = valor;
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

        private void Cbx_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbx_Status.SelectedIndex == 0)
            {
                Cbx_Status.Foreground = Brushes.Green;
            }
            else if (Cbx_Status.SelectedIndex == 1)
            {
                Cbx_Status.Foreground = Brushes.Red;
            }
            else
            {
                Cbx_Status.Foreground = Brushes.Blue;
            }
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
                    desconto2C = valor;
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
                taxaIVAC = selectedRow["iva_taxa"].ToString();
            }
            else
            {
                taxaIVAC = null; // Nenhum item selecionado
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            combustivelC = true;
            Esticar();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            combustivelC = false;
            Encolher();
        }

        private void Esticar()
        {
            lbl_Viatura.Visibility = Visibility.Visible;
            lbl_Viatura.Margin = new Thickness(10, 400, 0, 0);
            LoadCbx_Viatura();
            cbx_Viatura.Visibility = Visibility.Visible;
            cbx_Viatura.Margin = new Thickness(150, 405, 0, 0);
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
            btn_Save.Margin = new Thickness(150, 525, 0, 0);
            btn_Close.Margin = new Thickness(220, 525, 0, 0);
            this.Height = 615;
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
            btn_Save.Margin = new Thickness(150, 405, 0, 0);
            btn_Close.Margin = new Thickness(220, 405, 0, 0);
            debDetViatura = "0";
            this.Height = 490;
        }

        private void Cbx_Viatura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            debDetViatura = cbx_Viatura.SelectedValue.ToString();
            Quilometros();
            txt_KMI.Text = debDetKmi.ToString();
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
            int KmiC = Convert.ToInt32(txt_KMI.Text);
            int KmfC = Convert.ToInt32(txt_KMF.Text);
            int KmeC = KmfC - KmiC;
            txt_KME.Text = KmeC.ToString();
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
