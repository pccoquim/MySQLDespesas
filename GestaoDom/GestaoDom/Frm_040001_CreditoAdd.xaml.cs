/*
Frm_040001_CreditoAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data de alteração: 29.06.2024
Versão: 1.0.2
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
    /// Interaction logic for Frm_040001_CreditoAdd.xaml
    /// </summary>
    public partial class Frm_040001_CreditoAdd : Window
    {
        private readonly string loginUserId;
        private decimal valorIntroduce;
        private int number;

        public Frm_040001_CreditoAdd(string loginUserId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            LoadDefault();
            LoadNumeracao();
            LoadCbx_Terceiro();
            LoadCbx_TipoReceita();
            LoadCbx_ContaDebito();
            LoadCbx_ContaCredito();

        }

        private void LoadDefault()
        {
            Txt_Valor.Text = "0.00 €";
        }

        private void LoadNumeracao()
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
                    string query = "SELECT MAX(mc_cred_id) FROM tbl_0402_movimentoscredito";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1010)!");
                }
                number = ultimoId + 1;
            }
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
                    string query = "SELECT terc_cod, terc_descr FROM tbl_0102_terceiros WHERE terc_status = 1 AND terc_codtipo = 02 OR terc_codtipo = 03 AND terc_cod != 0 ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_Terceiro.ItemsSource = dt.DefaultView;
                        Cbx_Terceiro.DisplayMemberPath = "terc_descr";
                        Cbx_Terceiro.SelectedValuePath = "terc_cod";
                        Cbx_Terceiro.SelectedValue = -1;
                        Cbx_Terceiro.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1064)!" + ex.Message);
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
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_TipoReceita (Erro: C1065)!" + ex.Message);
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
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1066)!" + ex.Message);
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
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1067)!" + ex.Message);
                }
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
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }

        private void Txt_Valor_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void Txt_Valor_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_Valor.Text.Trim() != Txt_Valor.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do valor, não são permitidos espaços em branco nestes locais!");
            }
            if (sender is TextBox textBox)
            {
                if (decimal.TryParse(textBox.Text, out decimal valor))
                {
                    valorIntroduce = valor;
                    textBox.Text = string.Format("{0:F2} €", valor);
                }
                else
                {
                    // Se a entrada não for um número decimal válido, você pode lidar com isso aqui
                    MessageBox.Show("Valor inválido. Insira um número válido.");
                    textBox.Text = "0.00 €"; // Limpa o campo de texto
                    textBox.Focus(); // Volta o foco para a caixa de texto para inserir um novo valor
                }
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

        private bool IsTextAllowed(string text)
        {
            // Define uma expressão regular para verificar se o texto contém apenas dígitos e um ponto
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        public bool ValidarNDoc(string nDoc)
        {
            string document = nDoc;
            int existe = 0;
            bool valor = false;
            string codTerc = (Cbx_Terceiro.SelectedValue == null) ? "0" : Cbx_Terceiro.SelectedValue.ToString();
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT COUNT(*) FROM tbl_0402_movimentoscredito WHERE mc_codterc = ? AND mc_numerodoc = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@mc_codterc", codTerc);
                        cmd.Parameters.AddWithValue("@mc_nDoc", document);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1020)!" + ex.Message);

                }
                if (existe > 0)
                {
                    valor = true;
                }
                else
                {
                    valor = false;
                }
            }
            return valor;
        }

        public decimal ValidarDebito(string debito)
        {
            if (!decimal.TryParse(debito, out decimal mov))
            {
                System.Windows.MessageBox.Show("O valor do débito é inválido!");
                return 0;
            }
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            object terceiro = Cbx_Terceiro.SelectedValue;
            object tipoReceita = Cbx_TipoReceita.SelectedValue;
            object contaDebito = Cbx_ContaDebito.SelectedValue;
            object contaCredito = Cbx_ContaCredito.SelectedValue;

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
            // Verifica se a descrição está preenchida
            if (Ckb_Transf.IsChecked == true)
            {
                if (Txt_NDoc.Text != "")
                {
                    // Verifica os espaços no inicio e fim do campo
                    if (Txt_NDoc.Text.Trim() == Txt_NDoc.Text)
                    {
                        if (!ValidarNDoc(Txt_NDoc.Text))
                        {
                            if (contaDebito != null)
                            {
                                if (ValidarDebito(valorIntroduce.ToString()) >= 0)
                                {
                                    if (contaCredito != null)
                                    {
                                        if (Txt_Valor.Text != "")
                                        {
                                            // Obtem ligação
                                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                            {
                                                // Controlo de erros insert
                                                try
                                                {
                                                    conn.Open();
                                                    // Definição do insert 
                                                    string query = "INSERT INTO tbl_0402_movimentoscredito(mc_cred_id, mc_codterc, mc_numerodoc, mc_datamov, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status, mc_usercreate, mc_datecreate, mc_timecreate, mc_userlastchg, mc_datelastchg, mc_timelastchg) " +
                                                        "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                    // Definição de query e ligação
                                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                    {
                                                        // Atribuição de variaveis
                                                        cmd.Parameters.AddWithValue("@CredId", number);
                                                        cmd.Parameters.AddWithValue("@CodTerc", 0);
                                                        cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                        cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                        cmd.Parameters.AddWithValue("@TipoReceita", 0);
                                                        cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                        cmd.Parameters.AddWithValue("@ContaDebito", Cbx_ContaDebito.SelectedValue);
                                                        cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                        cmd.Parameters.AddWithValue("@Transf", true);
                                                        cmd.Parameters.AddWithValue("@status", 1);
                                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                        cmd.Parameters.AddWithValue("@Userlastchg", 0);
                                                        cmd.Parameters.AddWithValue("@Datalastchg", 0);
                                                        cmd.Parameters.AddWithValue("@Timelastchg", 0);
                                                        // execução do comando
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // mensagem de erro da ligação
                                                    System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1013)!" + ex.Message);
                                                    return;
                                                }
                                            }
                                            // Obtem ligação
                                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                            {
                                                // Controlo de erros update valor do saldo da conta débito
                                                try
                                                {
                                                    conn.Open();
                                                    // Definição do insert 
                                                    string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
                                                    // Definição de query e ligação
                                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                    {
                                                        // Atribuição de variaveis
                                                        cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                        cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaDebito.SelectedValue);
                                                        // execução do comando
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // mensagem de erro da ligação
                                                    System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1016)!" + ex.Message);
                                                    return;
                                                }
                                            }
                                            // Obtem ligação
                                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                            {
                                                // Controlo de erros update valor do saldo da conta a crédito
                                                try
                                                {
                                                    conn.Open();
                                                    // Definição do insert 
                                                    string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                    // Definição de query e ligação
                                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                    {
                                                        // Atribuição de variaveis
                                                        cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                        cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                        // execução do comando
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                    // Fecha o formulário                            
                                                    this.Close();
                                                    System.Windows.MessageBox.Show("Movimento a crédito inserido com sucesso! Referência do movimento: " + number);
                                                }
                                                catch (Exception ex)
                                                {
                                                    // mensagem de erro da ligação
                                                    System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1015)!" + ex.Message);
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
                                    // Não existem alterações para gravar
                                    if (ShowConfirmation("Este movimento provoca um saldo negativo de " + Convert.ToDecimal(ValidarDebito(Txt_Valor.Text)) + " na conta a débito. Deseja continuar?"))
                                    {
                                        if (contaCredito != null)
                                        {
                                            if (Txt_Valor.Text != "")
                                            {
                                                // Obtem ligação
                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros insert
                                                    try
                                                    {
                                                        conn.Open();
                                                        // Definição do insert 
                                                        string query = "INSERT INTO tbl_0402_movimentoscredito(mc_cred_id, mc_codterc, mc_numerodoc, mc_datamov, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status, mc_usercreate, mc_datecreate, mc_timecreate, mc_userlastchg, mc_datalastchg, mc_timelastchg) " +
                                                            "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@CredId", number);
                                                            cmd.Parameters.AddWithValue("@CodTerc", 0);
                                                            cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                            cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                            cmd.Parameters.AddWithValue("@TipoReceita", 0);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@ContaDebito", Cbx_ContaDebito.SelectedValue);
                                                            cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                            cmd.Parameters.AddWithValue("@Transf", true);
                                                            cmd.Parameters.AddWithValue("@status", 1);
                                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                            cmd.Parameters.AddWithValue("@UserLastChg", 0);
                                                            cmd.Parameters.AddWithValue("@DataLastChg", 0);
                                                            cmd.Parameters.AddWithValue("@TimeLastChg", 0);
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
                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros update valor do saldo da conta débito
                                                    try
                                                    {
                                                        conn.Open();
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
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
                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                {
                                                    // Controlo de erros update valor do saldo da conta a crédito
                                                    try
                                                    {
                                                        conn.Open();
                                                        // Definição do insert 
                                                        string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                        // Definição de query e ligação
                                                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                        {
                                                            // Atribuição de variaveis
                                                            cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                            cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                            // execução do comando
                                                            cmd.ExecuteNonQuery();
                                                        }
                                                        // Fecha o formulário                            
                                                        this.Close();
                                                        System.Windows.MessageBox.Show("Movimento a crédito inserido com sucesso! Referência do movimento: " + number);
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
                                    if (contaCredito != null)
                                    {
                                        if (Txt_Valor.Text != "")
                                        {
                                            // Obtem ligação
                                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                            {
                                                // Controlo de erros insert
                                                try
                                                {
                                                    conn.Open();
                                                    // Definição do insert 
                                                    string query = "INSERT INTO tbl_0402_movimentoscredito(mc_cred_id, mc_codterc, mc_numerodoc, mc_datamov, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status, mc_usercreate, mc_datecreate, mc_timecreate, mc_userlastchg, mc_datelastchg, mc_timelastchg) " +
                                                        "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                    // Definição de query e ligação
                                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                    {
                                                        // Atribuição de variaveis
                                                        cmd.Parameters.AddWithValue("@CredId", number);
                                                        cmd.Parameters.AddWithValue("@CodTerc", Cbx_Terceiro.SelectedValue);
                                                        cmd.Parameters.AddWithValue("@NDoc", Txt_NDoc.Text);
                                                        cmd.Parameters.AddWithValue("@Datamov", dataDB);
                                                        cmd.Parameters.AddWithValue("@TipoReceita", Cbx_TipoReceita.SelectedValue);
                                                        cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                        cmd.Parameters.AddWithValue("@ContaDebito", 0);
                                                        cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                        cmd.Parameters.AddWithValue("@Transf", false);
                                                        cmd.Parameters.AddWithValue("@status", 1);
                                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                        cmd.Parameters.AddWithValue("@Userlastchg", 0);
                                                        cmd.Parameters.AddWithValue("@Datelastchg", 0);
                                                        cmd.Parameters.AddWithValue("@Timelastchg", 0);
                                                        // execução do comando
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // mensagem de erro da ligação
                                                    System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1012)!" + ex.Message);
                                                    return;
                                                }
                                            }

                                            // Obtem ligação
                                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                            {
                                                // Controlo de erros update valor do saldo da conta crédito
                                                try
                                                {
                                                    conn.Open();
                                                    // Definição do insert 
                                                    string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                                                    // Definição de query e ligação
                                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                    {
                                                        // Atribuição de variaveis
                                                        cmd.Parameters.AddWithValue("@Valor", valorIntroduce);
                                                        cmd.Parameters.AddWithValue("@ContaCredito", Cbx_ContaCredito.SelectedValue);
                                                        // execução do comando
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                    // Fecha o formulário                            
                                                    this.Close();
                                                    System.Windows.MessageBox.Show("Movimento a crédito inserido com sucesso! Referência do movimento: " + number);
                                                }
                                                catch (Exception ex)
                                                {
                                                    // mensagem de erro da ligação
                                                    System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1014!" + ex.Message);
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
    }
}
