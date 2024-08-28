/*
Frm_010201_TerceiroAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_010201_TerceiroAdd.xaml
    /// </summary>
    public partial class Frm_010201_TerceiroAdd : Window
    {
        private readonly string loginUserId;
        public Frm_010201_TerceiroAdd(string loginUserId)
        {
            InitializeComponent();
            LoadTerceiros();
            this.loginUserId = loginUserId;
        }

        private void LoadTerceiros()
        {
            int ultimoId = 0;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string queryNumeration = "SELECT MAX(terc_cod) FROM tbl_0102_terceiros";

                    using (MySqlCommand cmdNumeration = new MySqlCommand(queryNumeration, conn))
                    {
                        object result = cmdNumeration.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            ultimoId = Convert.ToInt32(result);
                        }
                    }
                    // ComboBox tipo

                    // defenição da consulta
                    string queryTipo = "SELECT tipoterc_cod, tipoterc_descr FROM tbl_0101_tipoterceiro WHERE tipoterc_status = 1 ORDER BY tipoterc_descr";
                    using (MySqlCommand cmdTipo = new MySqlCommand(queryTipo, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTipo);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        // Limpa a combobox antes de adicionar os itens
                        cbx_Tipo.Items.Clear();
                        cbx_Tipo.ItemsSource = dt.DefaultView;
                        cbx_Tipo.DisplayMemberPath = "tipoterc_descr";
                        cbx_Tipo.SelectedValuePath = "tipoterc_cod";
                        cbx_Tipo.SelectedIndex = -1;
                        cbx_Tipo.IsEditable = false;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro C1003 - Erro de componentes da cbx!");
                }

                txt_Cod.Text = string.Format("{0:000}", ultimoId + 1);
            }
        }

        public bool ValidarTerceiro(string terceiro)
        {
            int existe = 0;
            bool valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta de validação de login
                    string query = "SELECT COUNT(*) FROM tbl_0102_terceiros WHERE terc_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@terc_descr", terceiro);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro C1002 - Ocorreu um erro a ligar à base de dados");
                }

                if (existe > 0)
                {
                    if (ShowConfirmation("Já existe um terceiro com esta descrição. Deseja manter?"))
                    {
                        valor = false;
                    }
                    else
                    {
                        valor = true;
                    }
                }
                else
                {
                    valor = false;
                }
            }
            return valor;
        }

        public bool ValidarNIF(string nif)
        {
            // Verificar o comprimento do NIF
            if (nif.Length != 9)
            {
                return false;
            }

            // Calcular o dígito de controlo
            int[] pesos = { 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 8; i++)
            {
                soma += (int)Char.GetNumericValue(nif[i]) * pesos[i];
            }

            int resto = soma % 11;
            int digitoControlo = (resto < 2) ? 0 : 11 - resto;

            // Comparar o dígito de controlo calculado com o último dígito do NIF
            return digitoControlo == Convert.ToInt32(Char.GetNumericValue(nif[8]));
        }

        public bool ExistsNIF(string nif)
        {
            int existe = 0;
            bool valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta de validação de login
                    string query = "SELECT COUNT(*) FROM tbl_0102_terceiros WHERE terc_nif = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@terc_nif", nif);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro C1001 - Ocorreu um erro a ligar à base de dados.");
                }
            }
            if (existe == 0)
            {
                valor = false;
            }
            else
            {
                valor = true;
            }
            return valor;
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Txt_Descr_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: descrição não pode iniciar ou finalizar com espaços!");
            }
            // Verifica se já existe o tipo de terceiro
            string terceiro = txt_Descr.Text;
            if (!ValidarTerceiro(terceiro))
            {
                return;
            }
        }

        private void Txt_Morada1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Morada1_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Morada1.Text.Trim() != txt_Morada1.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: morada1 não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Morada2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Morada2_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Morada2.Text.Trim() != txt_Morada2.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: morada2 não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_CP_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_CP_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_CP.Text.Trim() != txt_CP.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: código postal não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Localidade_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Localidade_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Localidade.Text.Trim() != txt_Localidade.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: localidade não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_NIF_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_NIF_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_NIF.Text.Trim() != txt_NIF.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: NIF não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Tlf_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Tlf_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Tlf.Text.Trim() != txt_Tlf.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: telefone não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Email_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && cbx_Tipo.SelectedIndex != -1 && txt_Morada1.Text != "" && txt_Morada2.Text != "" && txt_CP.Text != "" && txt_Localidade.Text != "" && txt_NIF.Text != "" && txt_Tlf.Text != "" && txt_Email.Text != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
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

        private void Txt_Email_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Email.Text.Trim() != txt_Email.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: email não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Cod.Text != "")
            {
                if (txt_Descr.Text.Trim() == txt_Descr.Text)
                {
                    if (txt_Descr.Text != "")
                    {
                        string terceiro = txt_Descr.Text;
                        if (!ValidarTerceiro(terceiro))
                        {
                            if (cbx_Tipo.SelectedIndex != -1)
                            {
                                if (txt_Morada1.Text.Trim() == txt_Morada1.Text)
                                {
                                    if (txt_Morada1.Text != "")
                                    {
                                        if (txt_Morada2.Text.Trim() == txt_Morada2.Text)
                                        {
                                            if (txt_CP.Text.Trim() == txt_CP.Text)
                                            {
                                                if (txt_CP.Text != "")
                                                {
                                                    if (txt_Localidade.Text.Trim() == txt_Localidade.Text)
                                                    {
                                                        if (txt_Localidade.Text != "")
                                                        {
                                                            if (txt_Tlf.Text.Trim() == txt_Tlf.Text)
                                                            {
                                                                if (txt_Email.Text.Trim() == txt_Email.Text)
                                                                {
                                                                    if (txt_NIF.Text.Trim() == txt_NIF.Text)
                                                                    {
                                                                        // Verifica se o NIF está preenchido ou se é utilizado o por defeito
                                                                        if (txt_NIF.Text == "")
                                                                        {
                                                                            if (ShowConfirmation("O campo: NIF não está preenchido. Deseja utilizar o NIF por defeito (999999990)?"))
                                                                            {
                                                                                string NIF = "999999990";
                                                                                // obtem ligação
                                                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        conn.Open();
                                                                                        // insert do terceiro
                                                                                        string query = "INSERT INTO tbl_0102_terceiros(terc_cod, terc_descr, terc_codtipo, terc_morada1, terc_morada2, terc_cp, terc_localidade, terc_nif, terc_tlf, terc_email, terc_status, terc_usercreate, terc_datecreate, terc_timecreate, terc_userlastchg, terc_datelastchg, terc_timelastchg) " +
                                                                                            "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                                                        // definição de query e ligação
                                                                                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                                                        {
                                                                                            // Tribuição de valores a variaveis
                                                                                            cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                                                                            cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                            cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                            cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                            cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                            cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                            cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                            cmd.Parameters.AddWithValue("@NIF", NIF);
                                                                                            cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                            cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
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
                                                                                        System.Windows.MessageBox.Show("Tercero inserido com exito!");
                                                                                        this.Close();
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        // mensagem de erro da ligação
                                                                                        System.Windows.MessageBox.Show("Erro: I1001 - Ocorreu um erro ao ligar à base de dados." + ex.Message);
                                                                                        return;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else if (txt_NIF.Text != "")
                                                                        {
                                                                            string nif = txt_NIF.Text.Trim();
                                                                            if (ValidarNIF(nif))
                                                                            {
                                                                                if (!ExistsNIF(nif))
                                                                                {
                                                                                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                    {   // obtem ligação
                                                                                        try
                                                                                        {
                                                                                            conn.Open();
                                                                                            // insert do cliente
                                                                                            string query = "INSERT INTO tbl_0102_terceiros(terc_cod, terc_descr, terc_codtipo, terc_morada1, terc_morada2, terc_cp, terc_localidade, terc_nif, terc_tlf, terc_email, terc_status, terc_usercreate, terc_datecreate, terc_timecreate, terc_userlastchg, terc_datelastchg, terc_timelastchg) " +
                                                                                                "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                                                            // definição de query e ligação
                                                                                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                                                            {
                                                                                                // Tribuição de valores a variaveis
                                                                                                cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                                                                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                                cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                                cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                                cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                                cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                                cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                                cmd.Parameters.AddWithValue("@NIF", txt_NIF.Text);
                                                                                                cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
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
                                                                                            // Fecha o formulário
                                                                                            System.Windows.MessageBox.Show("Terceiro inserido com exito!");
                                                                                            this.Close();
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {
                                                                                            // mensagem de erro da ligação
                                                                                            System.Windows.MessageBox.Show("Erro I1002 - Ocorreu um erro ao ligar à base de dados." + ex.Message);
                                                                                            return;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    System.Windows.MessageBox.Show("O NIF já existe noutro terceiro!");
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                System.Windows.MessageBox.Show("O NIF não é válido!");
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        System.Windows.MessageBox.Show("O campo: email não pode iniciar ou finalizar com espaços!");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    System.Windows.MessageBox.Show("O campo: email não pode iniciar ou finalizar com espaços!");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                System.Windows.MessageBox.Show("O campo: telefone não pode iniciar ou finalizar com espaços!");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            System.Windows.MessageBox.Show("O campo: localidade é de preenchimento obrigatório e está não está preenchido!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        System.Windows.MessageBox.Show("O campo: localidade não pode iniciar ou finalizar com espaços!");
                                                    }
                                                }
                                                else
                                                {
                                                    System.Windows.MessageBox.Show("O campo: código postal é de preenchimento obrigatório e está não está preenchido!");
                                                }
                                            }
                                            else
                                            {
                                                System.Windows.MessageBox.Show("O campo: código postal não pode iniciar ou finalizar com espaços!");
                                            }
                                        }
                                        else
                                        {
                                            System.Windows.MessageBox.Show("O campo: morada2 não pode iniciar ou finalizar com espaços!");
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("O campo: morada1 é de preenchimento obrigatório e está não está preenchido!");
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("O campo: morada1 não pode iniciar ou finalizar com espaços!");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: tipo é obrigatório, mas não foi selecionado nenhum item!");
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("O campo: descrição é de preenchimento obrigatório e está não está preenchido!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("O campo: descrição não pode iniciar ou finalizar com espaços!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Erro: N1001 - Ocorreu um erro de numeração automática. Se o problema persistir, contacte o administrador de sistema!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
