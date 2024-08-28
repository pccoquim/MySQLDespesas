/*
Frm_040002_CreditoEdit.xaml.cs
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
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_040002_CreditoEdit.xaml
    /// </summary>
    public partial class Frm_040002_CreditoEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedCredId;
        string CredID = "", CodTerc = "", NDoc = "", Data = "", CodTipoReceita = "", CDebito = "", CCredito = "", Valor = "", Status = "";
        private bool Transf { get; set; }

        public string DataFormatadaParaUI => dataFormatadaParaUI;

        private readonly string dataFormatadaParaUI;
        private decimal dif = 0;
        public Frm_040002_CreditoEdit(string loginUserId, int selectedCredId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedCredId = selectedCredId;
            LoadCbx_Terceiro();
            LoadCbx_TipoReceita();
            LoadCbx_ContaDebito();
            LoadCbx_ContaCredito();
            LoadCbx_Status();
            LoadCredito();
        }

        private void LoadCbx_Terceiro()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT terc_cod, terc_descr FROM tbl_0102_terceiros WHERE terc_status = 1 AND terc_cod != 0 ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_Terceiro.ItemsSource = dt.DefaultView;
                        Cbx_Terceiro.DisplayMemberPath = "terc_descr";
                        Cbx_Terceiro.SelectedValuePath = "terc_cod";
                        Cbx_Terceiro.SelectedValue = 0;
                        Cbx_Terceiro.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1068)!");
                }
            }
        }

        private void LoadCbx_TipoReceita()
        {
            // ComboBox tiporeceita
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT tr_cod, tr_descr FROM tbl_0104_tiporeceita WHERE tr_status = 1 AND tr_cod != 0 ORDER BY tr_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_TipoReceita.ItemsSource = dt.DefaultView;
                        Cbx_TipoReceita.DisplayMemberPath = "tr_descr";
                        Cbx_TipoReceita.SelectedValuePath = "tr_cod";
                        Cbx_TipoReceita.SelectedValue = 0;
                        Cbx_TipoReceita.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_TipoReceita (Erro: C1069)!");
                }
            }
        }

        private void LoadCbx_ContaDebito()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT cntdeb_cod, cntdeb_descr FROM tbl_0103_contasdeb WHERE cntdeb_status = 1 AND cntdeb_cod != 0 ORDER BY cntdeb_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_ContaDebito.ItemsSource = dt.DefaultView;
                        Cbx_ContaDebito.DisplayMemberPath = "cntdeb_descr";
                        Cbx_ContaDebito.SelectedValuePath = "cntdeb_cod";
                        Cbx_ContaDebito.SelectedValue = -1;
                        Cbx_ContaDebito.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1070)!");
                }
            }
        }

        private void LoadCbx_ContaCredito()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT cntcred_cod, cntcred_descr FROM tbl_0103_contascred WHERE cntcred_status = 1 AND cntcred_cod != 0 ORDER BY cntcred_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_ContaCredito.ItemsSource = dt.DefaultView;
                        Cbx_ContaCredito.DisplayMemberPath = "cntcred_descr";
                        Cbx_ContaCredito.SelectedValuePath = "cntcred_cod";
                        Cbx_ContaCredito.SelectedIndex = -1;
                        Cbx_ContaCredito.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1071)!");
                }
            }
        }

        private void LoadCbx_Status()
        {
            // ComboBox status
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT status_cod, status_descr FROM tbl_0001_status ORDER BY status_descr";
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
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Status (Erro: C1072)!");
                }
            }
        }

        private void LoadCredito()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT mc_id, mc_cred_id, mc_numerodoc, mc_datamov, mc_codterc, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status FROM tbl_0402_movimentoscredito WHERE mc_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@mcID", selectedCredId);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                CredID = reader["mc_cred_id"].ToString();
                                NDoc = reader["mc_numerodoc"].ToString();
                                Data = reader["mc_datamov"].ToString();
                                CodTerc = reader["mc_codterc"].ToString();
                                CodTipoReceita = reader["mc_codtiporeceita"].ToString();
                                CDebito = reader["mc_contadebito"].ToString();
                                CCredito = reader["mc_contacredito"].ToString();
                                Valor = reader["mc_valor"].ToString();
                                Transf = Convert.ToBoolean(reader["mc_transf"]);
                                Status = reader["mc_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1073)!" + ex.Message);
                }
                string dataDB = Data.ToString();

                if (DateTime.TryParseExact(dataDB, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                {
                    string dataFormatadaParaUI = dataFormatada.ToString("dd.MM.yyyy");

                    Txt_Data.Text = dataFormatadaParaUI; // Aqui, definimos a data formatada para a propriedade "Data".
                }
                else
                {
                    System.Windows.MessageBox.Show("Data inválida!");
                }
                Txt_Ref.Text = CredID;
                Txt_NDoc.Text = NDoc;
                Cbx_Terceiro.SelectedValue = CodTerc;
                Cbx_TipoReceita.SelectedValue = CodTipoReceita;
                Cbx_ContaDebito.SelectedValue = CDebito;
                Cbx_ContaCredito.SelectedValue = CCredito;
                if (Transf == true)
                {
                    this.Ckb_Transf.IsChecked = true;
                }
                else
                {
                    this.Ckb_Transf.IsChecked = false;
                }
                Txt_Valor.Text = Valor;
                Cbx_Status.SelectedValue = Status;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Lbl_TipoReceita.IsEnabled = false;
            Cbx_TipoReceita.SelectedIndex = -1;
            Cbx_TipoReceita.IsEnabled = false;
            Lbl_Terceiro.IsEnabled = false;
            Cbx_Terceiro.SelectedIndex = -1;
            Cbx_Terceiro.IsEnabled = false;
            Lbl_ContaDebito.IsEnabled = true;
            Cbx_ContaDebito.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Lbl_TipoReceita.IsEnabled = true;
            Cbx_TipoReceita.IsEnabled = true;
            Lbl_Terceiro.IsEnabled = true;
            Cbx_Terceiro.IsEnabled = true;
            Lbl_ContaDebito.IsEnabled = false;
            Cbx_ContaDebito.SelectedIndex = -1;
            Cbx_ContaDebito.IsEnabled = false;
        }

        public bool ValidarNDoc(string nDoc)
        {
            int existe = 0;
            bool valor;
            string codTerc;
            if (Cbx_Terceiro.SelectedValue == null)
            {
                codTerc = "0";
            }
            else
            {
                codTerc = Cbx_Terceiro.SelectedValue.ToString();
            }

            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT COUNT(*) FROM tbl_0402_movimentoscredito WHERE mc_codterc = ? AND mc_numerodoc = ? AND mc_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mc_codterc", codTerc);
                        cmd.Parameters.AddWithValue("@mc_nDoc", nDoc);
                        cmd.Parameters.AddWithValue("@mc_id", selectedCredId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1022)!");

                }
                if (existe > 0)
                {
                    valor = true;
                }
                else
                {
                    valor = false;
                }
                return valor;
            }
        }

        private void Txt_NDoc_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_NDoc.Text.Trim() != Txt_NDoc.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_NDoc_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValidDate(Txt_Data.Text))
            {
                MessageBox.Show("A data inserida não é válida. Use o formato dd/mm/yyyy.", "Erro de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                //txt_Data.Focus();
            }
        }

        private void Txt_Data_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var text = textBox.Text.Replace(".", "");
                if (text.Length >= 2 && text.Length < 4)
                {
                    textBox.Text = text.Insert(2, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
                else if (text.Length >= 4)
                {
                    textBox.Text = text.Insert(2, ".").Insert(5, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
            }
        }

        private bool IsValidDate(string date)
        {
            // Verifica se a data é válida usando DateTime.TryParseExact.
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, result: out DateTime parsedDate);
        }

        private void Ckb_Transf_KeyDown(object sender, KeyEventArgs e)
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

        private void Cbx_Terceiro_KeyDown(object sender, KeyEventArgs e)
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

        private void Cbx_TipoReceita_KeyDown(object sender, KeyEventArgs e)
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

        private void Cbx_ContaDebito_KeyDown(object sender, KeyEventArgs e)
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

        private void Cbx_ContaCredito_KeyDown(object sender, KeyEventArgs e)
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

        private void Cbx_Status_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Valor_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_Valor.Text.Trim() != Txt_Valor.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do valor, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_Valor_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Valor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsTextAllowed(string text)
        {
            // Define uma expressão regular para verificar se o texto contém apenas dígitos e um ponto
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        public decimal ValidarDebito(string debito)
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
                        cmd.Parameters.AddWithValue("@cntcred_cod", Cbx_ContaDebito.SelectedValue);
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
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1023)!");

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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            object terceiro = Cbx_Terceiro.SelectedValue;
            object tipoReceita = Cbx_TipoReceita.SelectedValue;
            object debito = Cbx_ContaDebito.SelectedValue;
            object credito = Cbx_ContaCredito.SelectedValue;

            string dataTextBox = Txt_Data.Text;
            string dataDB = "";

            if (DateTime.TryParseExact(dataTextBox, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
            {
                dataDB = dataFormatada.ToString("yyyyMMdd");
            }
            else
            {
                System.Windows.MessageBox.Show("A data está num formato incorreto!");
            }

            if (Ckb_Transf.IsChecked == true)
            {
                if (Txt_NDoc.Text != "")
                {
                    // Verifica os espaços no inicio e fim do campo
                    if (Txt_NDoc.Text.Trim() == Txt_NDoc.Text)
                    {
                        if (!ValidarNDoc(Txt_NDoc.Text))
                        {
                            if (debito != null)
                            {
                                dif = Convert.ToDecimal(Txt_Valor.Text) - Convert.ToDecimal(Valor);
                                if (ValidarDebito(dif.ToString()) >= 0)
                                {
                                    if (credito != null)
                                    {
                                        if (Txt_Valor.Text != "")
                                        {
                                            if (Convert.ToString(Cbx_Terceiro.SelectedValue) != CodTerc || Convert.ToString(Cbx_TipoReceita.SelectedValue) != CodTipoReceita || Convert.ToString(Cbx_ContaDebito.SelectedValue) != CDebito || Convert.ToString(Cbx_ContaCredito.SelectedValue) != CCredito || Ckb_Transf.IsChecked != Transf || Txt_NDoc.Text != NDoc || Txt_Data.Text != dataFormatadaParaUI || Txt_Valor.Text != Valor)
                                            {
                                                // Obtem ligação
                                                using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros insert
                                                    try
                                                    {
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0402_movimentoscredito SET mc_codterc = ?, mc_numerodoc = ?, mc_datamov = ?, mc_codtiporeceita = ?, mc_contacredito = ?, mc_contadebito = ?, mc_valor = ?, mc_transf = ?, mc_status = ?, mc_userlastchg = ?, mc_datelastchg = ?, mc_timelastchg = ? WHERE mc_cred_id = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@CodTerc", 0);
                                                            cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                            cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                            cmd.Parameters.AddWithValue("@TipoReceita", 0);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@ContaDebito", Cbx_ContaDebito.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@Valor", Convert.ToDecimal(Txt_Valor.Text));
                                                            cmd.Parameters.AddWithValue("@Transf", true);
                                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                            cmd.Parameters.AddWithValue("@CredId", selectedCredId);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // mensagem de erro da ligação
                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: U1021)!" + ex.Message);
                                                        return;
                                                    }
                                                }


                                                // Obtem ligação
                                                using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros update valor do saldo da conta débito
                                                    try
                                                    {
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@Valor", dif);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaDebito.SelectedValue);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // mensagem de erro da ligação
                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1022)!" + ex.Message);
                                                        return;
                                                    }
                                                }

                                                // Obtem ligação
                                                using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros update valor do saldo da conta a crédito
                                                    try
                                                    {
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@Valor", dif);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                        // Fecha o formulário                            
                                                        this.Close();
                                                        System.Windows.MessageBox.Show("Movimento a crédito alterado com sucesso!");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // mensagem de erro da ligação
                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1023)!" + ex.Message);
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Não existem alterações para gravar
                                                if (ShowConfirmation("Não foram efetuadas alterações para guardar. Deseja sair das alterações?"))
                                                {
                                                    this.Close();
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            System.Windows.MessageBox.Show("O campo: valor é de preenchimento obrigatório e não está preenchido!");
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("O campo: conta a crédito é de preenchimento obrigatório e não está selecionada nenhuma conta!");
                                    }
                                }
                                else
                                {
                                    // saldo negativo
                                    if (ShowConfirmation("Este movimento provoca um saldo negativo de " + Convert.ToDecimal(ValidarDebito(Txt_Valor.Text)) + " na conta a débito. Deseja continuar?"))
                                    {
                                        if (credito != null)
                                        {
                                            if (Txt_Valor.Text != "")
                                            {
                                                if (Convert.ToString(Cbx_Terceiro.SelectedValue) != CodTerc || Convert.ToString(Cbx_TipoReceita.SelectedValue) != CodTipoReceita || Convert.ToString(Cbx_ContaDebito.SelectedValue) != CDebito || Convert.ToString(Cbx_ContaCredito.SelectedValue) != CCredito || Ckb_Transf.IsChecked != Transf || Txt_NDoc.Text != NDoc || Txt_Data.Text != dataFormatadaParaUI || Txt_Valor.Text != Valor)
                                                {
                                                    // Obtem ligação
                                                    using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                    {
                                                        // Controlo de erros insert
                                                        try
                                                        {
                                                            // Definição do insert 
                                                            string query = "UPDATE tbl_0402_movimentoscredito SET mc_codterc = ?, mc_numerodoc = ?, mc_datamov = ?, mc_codtiporeceita = ?, mc_contacredito = ?, mc_contadebito = ?, mc_valor = ?, mc_transf = ?, mc_status = ?, mc_userlastchg = ?, mc_datelastchg = ?, mc_timelastchg = ? WHERE mc_cred_id = ?";
                                                            // Definição de query e ligação
                                                            using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                            {
                                                                // Atribuição de variaveis
                                                                cmd.Parameters.AddWithValue("@CodTerc", 0);
                                                                cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                                cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                                cmd.Parameters.AddWithValue("@TipoReceita", 0);
                                                                cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                                cmd.Parameters.AddWithValue("@ContaDebito", Cbx_ContaDebito.SelectedValue);
                                                                cmd.Parameters.AddWithValue("@Valor", Convert.ToDecimal(Txt_Valor.Text));
                                                                cmd.Parameters.AddWithValue("@Transf", true);
                                                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                cmd.Parameters.AddWithValue("@CredId", selectedCredId);
                                                                // execução do comando
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            // mensagem de erro da ligação
                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1014)!" + ex.Message);
                                                            return;
                                                        }
                                                    }

                                                    // Obtem ligação
                                                    using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                    {
                                                        // Controlo de erros update valor do saldo da conta débito
                                                        try
                                                        {
                                                            // Definição do insert 
                                                            string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
                                                            // Definição de query e ligação
                                                            using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                            {
                                                                // Atribuição de variaveis
                                                                cmd.Parameters.AddWithValue("@Valor", dif);
                                                                cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaDebito.SelectedValue);
                                                                // execução do comando
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            // mensagem de erro da ligação
                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1017)!" + ex.Message);
                                                            return;
                                                        }
                                                    }

                                                    // Obtem ligação
                                                    using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                    {
                                                        // Controlo de erros update valor do saldo da conta a crédito
                                                        try
                                                        {
                                                            // Definição do insert 
                                                            string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                            // Definição de query e ligação
                                                            using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                            {
                                                                // Atribuição de variaveis
                                                                cmd.Parameters.AddWithValue("@Valor", dif);
                                                                cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                                // execução do comando
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                            // Fecha o formulário                            
                                                            this.Close();
                                                            System.Windows.MessageBox.Show("Movimento a crédito alterado com sucesso!");

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            // mensagem de erro da ligação
                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1018)!" + ex.Message);
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // Não existem alterações para gravar
                                                    if (ShowConfirmation("Não foram efetuadas alterações para guardar. Deseja sair das alterações?"))
                                                    {
                                                        this.Close();
                                                    }
                                                    else
                                                    {
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                System.Windows.MessageBox.Show("O campo: valor é de preenchimento obrigatório e não está preenchido!");
                                            }
                                        }
                                        else
                                        {
                                            System.Windows.MessageBox.Show("O campo: conta a crédito é de preenchimento obrigatório e não está selecionada nenhuma conta!");
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: conta a débito é de preenchimento obrigatório e não está selecionada nenhuma conta!");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Já existe este número de documento para este terceiro!");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("O campo: nº de documento é de preenchimento obrigatório e não está preenchido!");
                }
            }
            else
            {
                if (terceiro != null)
                {
                    if (tipoReceita != null)
                    {
                        if (Txt_NDoc.Text != "")
                        {
                            // Verifica os espaços no inicio e fim do campo
                            if (Txt_NDoc.Text.Trim() == Txt_NDoc.Text)
                            {
                                if (!ValidarNDoc(Txt_NDoc.Text))
                                {
                                    if (credito != null)
                                    {
                                        if (Txt_Valor.Text != "")
                                        {
                                            if (Convert.ToString(Cbx_Terceiro.SelectedValue) != CodTerc || Convert.ToString(Cbx_TipoReceita.SelectedValue) != CodTipoReceita || Convert.ToString(Cbx_ContaDebito.SelectedValue) != CDebito || Convert.ToString(Cbx_ContaCredito.SelectedValue) != CCredito || Ckb_Transf.IsChecked != Transf || Txt_NDoc.Text != NDoc || Txt_Data.Text != dataFormatadaParaUI || Txt_Valor.Text != Valor)
                                            {
                                                dif = Convert.ToDecimal(Txt_Valor.Text) - Convert.ToDecimal(Valor);

                                                // Obtem ligação
                                                using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros 
                                                    try
                                                    {
                                                        // Definição do update
                                                        string query = "UPDATE tbl_0402_movimentoscredito SET mc_codterc = ?, mc_numerodoc = ?, mc_datamov = ?, mc_codtiporeceita = ?, mc_contacredito = ?, mc_contadebito = ?, mc_valor = ?, mc_transf = ?, mc_status = ?, mc_userlastchg = ?, mc_datelastchg = ?, mc_timelastchg = ? WHERE mc_cred_id = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@CodTerc", Cbx_Terceiro.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                            cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                            cmd.Parameters.AddWithValue("@TipoReceita", Cbx_TipoReceita.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@ContaDebito", 0);
                                                            cmd.Parameters.AddWithValue("@Valor", Convert.ToDecimal(Txt_Valor.Text));
                                                            cmd.Parameters.AddWithValue("@Transf", false);
                                                            cmd.Parameters.AddWithValue("@Status", Cbx_Status.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                            cmd.Parameters.AddWithValue("@CredId", selectedCredId);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // mensagem de erro da ligação
                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: U1019)!" + ex.Message);
                                                        return;
                                                    }
                                                }

                                                // Obtem ligação
                                                using (MySqlConnection con = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros update valor do saldo da conta crédito
                                                    try
                                                    {
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@Valor", dif);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                        // Fecha o formulário                            
                                                        this.Close();
                                                        System.Windows.MessageBox.Show("Movimento a crédito alterado com sucesso!");
                                                    }
                                                    catch (Exception)
                                                    {
                                                        // mensagem de erro da ligação
                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1020!");
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Não existem alterações para gravar
                                                if (ShowConfirmation("Não foram efetuadas alterações para guardar. Deseja sair das alterações?"))
                                                {
                                                    this.Close();
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            System.Windows.MessageBox.Show("O campo: valor é de preenchimento obrigatório e não está preenchido!");
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("O campo: conta a crédito é de preenchimento obrigatório e não está selecionada nenhuma conta!");
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Já existe este número de documento para este terceiro!");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("O campo: nº de documento é de preenchimento obrigatório e não está preenchido!");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("O campo: tipo de receita é de preenchimento obrigatório e não está selecionado um tipo de receita!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("O campo: terceiro é de preenchimento obrigatório e não está selecionado um terceiro!");
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
