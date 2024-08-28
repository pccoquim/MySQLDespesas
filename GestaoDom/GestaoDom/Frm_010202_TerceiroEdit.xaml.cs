/*
Frm_010202_TerceiroEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
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
    /// Interaction logic for Frm_010202_TerceiroEdit.xaml
    /// </summary>
    public partial class Frm_010202_TerceiroEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedTerceiroId;
        private string cod = "", desig = "", tipo = "", morada1 = "", morada2 = "", cp = "", localidade = "", nif = "", tlf = "", email = "", status = "";

        public Frm_010202_TerceiroEdit(string loginUserId, int selectedTerceiroId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.selectedTerceiroId = selectedTerceiroId;
            LoadTerceiros();
        }

        private void LoadTerceiros()
        {

            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // ComboBox tipo
                    // defenição da consulta
                    string queryTipo = "SELECT tipoterc_cod, tipoterc_descr FROM tbl_0101_tipoterceiro ORDER BY tipoterc_descr";
                    using (MySqlCommand cmdTipo = new MySqlCommand(queryTipo, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapterTipo = new MySqlDataAdapter(cmdTipo);
                        DataTable dtTipo = new DataTable();
                        adapterTipo.Fill(dtTipo);
                        // Limpa a combobox antes de adicionar os itens
                        cbx_Tipo.Items.Clear();
                        cbx_Tipo.ItemsSource = dtTipo.DefaultView;
                        cbx_Tipo.DisplayMemberPath = "tipoterc_descr";
                        cbx_Tipo.SelectedValuePath = "tipoterc_cod";
                        cbx_Tipo.SelectedIndex = -1;
                        cbx_Tipo.IsEditable = false;
                    }

                    // ComboBox status
                    // defenição da consulta
                    string queryStatus = "SELECT status_cod, status_descr FROM tbl_0001_status ORDER BY status_descr";
                    using (MySqlCommand cmdStatus = new MySqlCommand(queryStatus, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapterStatus = new MySqlDataAdapter(cmdStatus);
                        DataTable dtStatus = new DataTable();
                        adapterStatus.Fill(dtStatus);
                        // Limpa a combobox antes de adicionar os itens
                        cbx_Status.Items.Clear();
                        cbx_Status.ItemsSource = dtStatus.DefaultView;
                        cbx_Status.DisplayMemberPath = "status_descr";
                        cbx_Status.SelectedValuePath = "status_cod";
                        cbx_Status.SelectedIndex = -1;
                        cbx_Status.IsEditable = false;
                    }


                    // Dados dos acesso

                    string query = "SELECT terc_id, terc_cod, terc_descr, terc_codtipo, terc_morada1, terc_morada2, terc_cp, terc_localidade, terc_nif, terc_tlf, terc_email, terc_status, status_descr FROM tbl_0102_terceiros, tbl_0101_tipoterceiro, tbl_0001_status WHERE terc_codtipo = tipoterc_cod AND terc_status = status_cod AND terc_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["terc_cod"].ToString();
                                desig = reader["terc_descr"].ToString();
                                tipo = reader["terc_codtipo"].ToString();
                                morada1 = reader["terc_morada1"].ToString();
                                morada2 = reader["terc_morada2"].ToString();
                                cp = reader["terc_cp"].ToString();
                                localidade = reader["terc_localidade"].ToString();
                                nif = reader["terc_nif"].ToString();
                                tlf = reader["terc_tlf"].ToString();
                                email = reader["terc_email"].ToString();
                                status = reader["terc_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar tipo de terceiros (Erro: C1010)!");
                }

                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                cbx_Tipo.SelectedValue = tipo;
                txt_Morada1.Text = morada1;
                txt_Morada2.Text = morada2;
                txt_CP.Text = cp;
                txt_Localidade.Text = localidade;
                txt_NIF.Text = nif;
                txt_Tlf.Text = tlf;
                txt_Email.Text = email;
                cbx_Status.SelectedValue = status;
            }

            // Verifica se o NIF é o utilizado por defeito (por defeito pode ser alterado, se for o número do terceiro não pode ser alterado
            if (nif == "999999990")
            {
                txt_NIF.IsEnabled = true;
            }
            else
            {
                txt_NIF.IsEnabled = false;
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
            return digitoControlo == (int)Char.GetNumericValue(nif[8]);
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

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Txt_Email_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Email_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Email.Text.Trim() != txt_Email.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: email não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Tlf_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Tlf_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Tlf.Text.Trim() != txt_Tlf.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: telefone não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_NIF_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_NIF_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_NIF.Text.Trim() != txt_NIF.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: NIF não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Localidade_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Localidade_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Localidade.Text.Trim() != txt_Localidade.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: localidade não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_CP_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_CP_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_CP.Text.Trim() != txt_CP.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: código postal não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Morada2_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Morada2_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Morada2.Text.Trim() != txt_Morada2.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: morada2 não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Morada1_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Morada1_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Morada1.Text.Trim() != txt_Morada1.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: morada1 não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Descr_KeyDown(object sender, KeyEventArgs e)
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Variáveis para a verificação de alterações
            string defaultnif = "999999990";
            // Verifica o preenchimento do tipo - seleção da cbx_Tipo
            if (cbx_Tipo.SelectedIndex != -1)
            {
                // Verifica a existência de espaços no inicio ou fim da morada1
                if (txt_Morada1.Text.Trim() == txt_Morada1.Text)
                {
                    // Verifica se a morada1 está preenchida
                    if (txt_Morada1.Text != "")
                    {
                        // Verifica a existência de espaços no inicio ou fim da morada2
                        if (txt_Morada2.Text.Trim() == txt_Morada2.Text)
                        {
                            // Verifica a existência de espaços no inicio ou fim do código postal
                            if (txt_CP.Text.Trim() == txt_CP.Text)
                            {
                                // Verifica se o código postal está preenchido
                                if (txt_CP.Text != "")
                                {
                                    // Verifica a existência de espaços no inicio ou fim da localidade
                                    if (txt_Localidade.Text.Trim() == txt_Localidade.Text)
                                    {
                                        // Verifica se a localidade está preenchida
                                        if (txt_Localidade.Text != "")
                                        {
                                            // Verifica a existência de espaços no inicio ou fim do telefone
                                            if (txt_Tlf.Text.Trim() == txt_Tlf.Text)
                                            {
                                                // Verifica a existência de espaços no inicio ou fim do email
                                                if (txt_Email.Text.Trim() == txt_Email.Text)
                                                {
                                                    // Verifica a existência de espaços n inicio ou fim da descrição
                                                    if (txt_Descr.Text.Trim() == txt_Descr.Text)
                                                    {
                                                        string nif;
                                                        // Verifica se a descrição está preenchida e se é igual à armazenada
                                                        if (txt_Descr.Text != "" && txt_Descr.Text == desig)
                                                        {
                                                            // Verifica se o NIF está ativo para efetuar alterações
                                                            if (txt_NIF.IsEnabled)
                                                            {
                                                                if (txt_NIF.Text.Trim() != txt_NIF.Text)
                                                                {
                                                                    nif = txt_NIF.Text;
                                                                    // Verifica se o NIF está em branco ou se é o por defeito
                                                                    if (txt_NIF.Text == "" || txt_NIF.Text == defaultnif)
                                                                    {
                                                                        if (ShowConfirmation("O campo: NIF não está preenchido ou é o NIF por defeito. Deseja utilizar o NIF por defeito (999999990)?"))
                                                                        {
                                                                            // Verificar alterações em todos os campos, excepto no nif que está desativado e descrição já verificado
                                                                            if (Convert.ToString(cbx_Tipo.SelectedValue) != tipo || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade
                                                                                || txt_NIF.Text != nif || txt_Tlf.Text != tlf || txt_Email.Text != email || Convert.ToString(cbx_Status.SelectedValue) != status)
                                                                            {
                                                                                // Obtem ligação
                                                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                {
                                                                                    // Prevenção de erros
                                                                                    try
                                                                                    {
                                                                                        conn.Open();
                                                                                        // String para alteração do terceiro
                                                                                        string alterar = "UPDATE tbl_0102_terceiros SET terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?, txt_cp = ?, terc_localidade = ?, terc_nif = ?, terc_tlf = ?, terc_email = ?, terc_status, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                                        // ligação com string e comando
                                                                                        using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                                        {
                                                                                            // Atribuição de variaveis com valores
                                                                                            cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                            cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                            cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                            cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                            cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                            cmd.Parameters.AddWithValue("@NIF", defaultnif);
                                                                                            cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                            cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                            cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                            // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                            cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                            // Executa a alteração
                                                                                            cmd.ExecuteNonQuery();
                                                                                        }
                                                                                        // Mensagem de alteração concluida com exito
                                                                                        System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                                        // Fecha a janela
                                                                                        this.Close();
                                                                                    }
                                                                                    catch (Exception)
                                                                                    {
                                                                                        // Mensagem de erro de ligação
                                                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1004)!");
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
                                                                    }
                                                                    // Verifica se o nif está preenchido
                                                                    else if (txt_NIF.Text != "")
                                                                    {
                                                                        // Verifica se o nif  é válido
                                                                        if (ValidarNIF(nif))
                                                                        {
                                                                            // Verifica se o nif já existe noutro terceiro
                                                                            if (!ExistsNIF(nif))
                                                                            {
                                                                                // Verificar alterações em todos os campos, excepto no nif que está desativado e existem alterações
                                                                                if (Convert.ToString(cbx_Tipo.SelectedValue) != tipo || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade
                                                                                    || txt_NIF.Text != nif || txt_Tlf.Text != tlf || txt_Email.Text != email || Convert.ToString(cbx_Status.SelectedValue) != status)
                                                                                {
                                                                                    // Obtem ligação
                                                                                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                    {
                                                                                        // Prevenção de erros
                                                                                        try
                                                                                        {
                                                                                            conn.Open();
                                                                                            // String para alteração do terceiro
                                                                                            string alterar = "UPDATE tbl_0102_terceiros SET terc_descr = ?, terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?, txt_cp = ?, terc_localidade = ?, terc_nif = ?, terc_tlf = ?, terc_email = ?, terc_status, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                                            // ligação com string e comando
                                                                                            using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                                            {
                                                                                                // Atribuição de variaveis com valores
                                                                                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                                cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                                cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                                cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                                cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                                cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                                cmd.Parameters.AddWithValue("@NIF", nif);
                                                                                                cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                                cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                                // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                                cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                                // Executa a alteração
                                                                                                cmd.ExecuteNonQuery();
                                                                                            }
                                                                                            // Mensagem de alteração concluida com exito
                                                                                            System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                                            // Fecha a janela
                                                                                            this.Close();
                                                                                        }
                                                                                        catch (Exception)
                                                                                        {
                                                                                            // Mensagem de erro de ligação
                                                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1003)!");
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
                                                                            // Mensagem se nif já existir noutro terceiro
                                                                            else
                                                                            {
                                                                                System.Windows.MessageBox.Show("O NIF indicado já existe noutro terceiro!");
                                                                            }
                                                                        }
                                                                        // Mensagem se nif for inválido
                                                                        else
                                                                        {
                                                                            System.Windows.MessageBox.Show("O NIF não é válido!");
                                                                        }
                                                                    }
                                                                }
                                                                // Mensagem se existirem espaços em branco no inicio ou fim do nif
                                                                else
                                                                {
                                                                    System.Windows.MessageBox.Show("O campo: nif não pode iniciar ou finalizar com espaços!");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToString(cbx_Tipo) != tipo || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade || txt_Tlf.Text != tlf || txt_Email.Text != email)
                                                                {
                                                                    // Obtem ligação
                                                                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                    {
                                                                        // Prevenção de erros
                                                                        try
                                                                        {
                                                                            conn.Open();
                                                                            // String para alteração do terceiro
                                                                            string alterar = "UPDATE tbl_0102_terceiros SET terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?,terc_cp = ?, terc_localidade = ?, terc_nif = ?, terc_tlf = ?, terc_email = ?, terc_status = ?, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                            // ligação com string e comando
                                                                            using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                            {
                                                                                // Atribuição de variaveis com valores
                                                                                cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                cmd.Parameters.AddWithValue("@NIF", defaultnif);
                                                                                cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                // Executa a alteração
                                                                                cmd.ExecuteNonQuery();
                                                                            }
                                                                            // Mensagem de alteração concluida com exito
                                                                            System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                            // Fecha a janela
                                                                            this.Close();
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            // Mensagem de erro de ligação
                                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1005)!" + ex.Message);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        // Verifica se a descrição está preenchida e é diferente da armazenada e valida o terceiro se não for igual
                                                        else if (txt_Descr.Text != "" && txt_Descr.Text != desig && !ValidarTerceiro(desig))
                                                        {
                                                            // Verifica se o NIF está ativo para efetuar alterações
                                                            if (txt_NIF.IsEnabled)
                                                            {
                                                                if (txt_NIF.Text.Trim() != txt_NIF.Text)
                                                                {
                                                                    nif = txt_NIF.Text;
                                                                    // Verifica se o NIF está em branco ou se é o por defeito
                                                                    if (txt_NIF.Text == "" || txt_NIF.Text == defaultnif)
                                                                    {
                                                                        if (ShowConfirmation("O campo: NIF não está preenchido ou é o NIF por defeito. Deseja utilizar o NIF por defeito (999999990)?"))
                                                                        {
                                                                            // Verificar alterações em todos os campos, excepto no nif que está desativado e descrição já verificado
                                                                            if (Convert.ToString(cbx_Tipo.SelectedValue) != tipo || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade
                                                                                || txt_NIF.Text != nif || txt_Tlf.Text != tlf || txt_Email.Text != email || Convert.ToString(cbx_Status.SelectedValue) != status)
                                                                            {
                                                                                // Obtem ligação
                                                                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                {
                                                                                    // Prevenção de erros
                                                                                    try
                                                                                    {
                                                                                        conn.Open();
                                                                                        // String para alteração do terceiro
                                                                                        string alterar = "UPDATE tbl_0102_terceiros SET terc_descr = ?, terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?, txt_cp = ?, terc_localidade = ?, terc_nif = ?, terc_tlf = ?, terc_email = ?, terc_status = ?, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                                        // ligação com string e comando
                                                                                        using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                                        {
                                                                                            // Atribuição de variaveis com valores
                                                                                            cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                            cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                            cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                            cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                            cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                            cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                            cmd.Parameters.AddWithValue("@NIF", defaultnif);
                                                                                            cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                            cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                            cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                            // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                            cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                            // Executa a alteração
                                                                                            cmd.ExecuteNonQuery();
                                                                                        }
                                                                                        // Mensagem de alteração concluida com exito
                                                                                        System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                                        // Fecha a janela
                                                                                        this.Close();
                                                                                    }
                                                                                    catch (Exception)
                                                                                    {
                                                                                        // Mensagem de erro de ligação
                                                                                        System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1004)!");
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
                                                                    }
                                                                    // Verifica se o nif está preenchido
                                                                    else if (txt_NIF.Text != "")
                                                                    {
                                                                        // Verifica se o nif  é válido
                                                                        if (ValidarNIF(nif))
                                                                        {
                                                                            // Verifica se o nif já existe noutro terceiro
                                                                            if (!ExistsNIF(nif))
                                                                            {
                                                                                // Verificar alterações em todos os campos, excepto no nif que está desativado e existem alterações
                                                                                if (txt_Descr.Text != desig || Convert.ToString(cbx_Tipo.SelectedValue) != tipo || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade
                                                                                    || txt_NIF.Text != nif || txt_Tlf.Text != tlf || txt_Email.Text != email || Convert.ToString(cbx_Status.SelectedValue) != status)
                                                                                {
                                                                                    // Obtem ligação
                                                                                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                                    {
                                                                                        // Prevenção de erros
                                                                                        try
                                                                                        {
                                                                                            conn.Open();
                                                                                            // String para alteração do terceiro
                                                                                            string alterar = "UPDATE tbl_0102_terceiros SET terc_descr = ?, terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?, txt_cp = ?, terc_localidade = ?, terc_nif = ?, terc_tlf = ?, terc_email = ?, terc_status = ?, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                                            // ligação com string e comando
                                                                                            using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                                            {
                                                                                                // Atribuição de variaveis com valores
                                                                                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                                cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                                cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                                cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                                cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                                cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                                cmd.Parameters.AddWithValue("@NIF", txt_NIF.Text);
                                                                                                cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                                cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                                // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                                cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                                // Executa a alteração
                                                                                                cmd.ExecuteNonQuery();
                                                                                            }
                                                                                            // Mensagem de alteração concluida com exito
                                                                                            System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                                            // Fecha a janela
                                                                                            this.Close();
                                                                                        }
                                                                                        catch (Exception)
                                                                                        {
                                                                                            // Mensagem de erro de ligação
                                                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1003)!");
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
                                                                            // Mensagem se nif já existir noutro terceiro
                                                                            else
                                                                            {
                                                                                System.Windows.MessageBox.Show("O NIF indicado já existe noutro terceiro!");
                                                                            }
                                                                        }
                                                                        // Mensagem se nif for inválido
                                                                        else
                                                                        {
                                                                            System.Windows.MessageBox.Show("O NIF não é válido!");
                                                                        }
                                                                    }
                                                                }
                                                                // Mensagem se existirem espaços em branco no inicio ou fim do nif
                                                                else
                                                                {
                                                                    System.Windows.MessageBox.Show("O campo: nif não pode iniciar ou finalizar com espaços!");
                                                                }

                                                            }
                                                            // Se o campo nif está desativado
                                                            else
                                                            {
                                                                // Verificar alterações em todos os campos, excepto no nif que está desativado e existem alterações
                                                                if (txt_Descr.Text != tipo || Convert.ToString(cbx_Tipo.SelectedValue) != morada1 || txt_Morada1.Text != morada1 || txt_Morada2.Text != morada2 || txt_CP.Text != cp || txt_Localidade.Text != localidade || txt_Tlf.Text != tlf || txt_Email.Text != email || Convert.ToString(cbx_Status.SelectedValue) != status)
                                                                {
                                                                    // Obtem ligação
                                                                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                                                    {
                                                                        // Prevenção de erros
                                                                        try
                                                                        {
                                                                            conn.Open();
                                                                            // String para alteração do terceiro
                                                                            string alterar = "UPDATE tbl_0102_terceiros SET terc_descr = ?, terc_codtipo = ?, terc_morada1 = ?, terc_morada2 = ?, txt_cp = ?, terc_localidade = ?, terc_tlf = ?, terc_email = ?, terc_status = ?, terc_userlastchg = ?, terc_datelastchg = ?, terc_timelastchg = ? WHERE terc_id = ? ";
                                                                            // ligação com string e comando
                                                                            using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                                            {
                                                                                // Atribuição de variaveis com valores
                                                                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                                                cmd.Parameters.AddWithValue("@Tipo", cbx_Tipo.SelectedValue);
                                                                                cmd.Parameters.AddWithValue("@Morada1", txt_Morada1.Text);
                                                                                cmd.Parameters.AddWithValue("@Morada2", txt_Morada2.Text);
                                                                                cmd.Parameters.AddWithValue("@CP", txt_CP.Text);
                                                                                cmd.Parameters.AddWithValue("@Local", txt_Localidade.Text);
                                                                                cmd.Parameters.AddWithValue("@Tlf", txt_Tlf.Text);
                                                                                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                                                                                cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                                                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                                                // Atribuição de variavel de identificação do terceiro para execução da alteração
                                                                                cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                                                                // Executa a alteração
                                                                                cmd.ExecuteNonQuery();
                                                                            }
                                                                            // Mensagem de alteração concluida com exito
                                                                            System.Windows.MessageBox.Show("Terceiro alterado com sucesso!");
                                                                            // Fecha a janela
                                                                            this.Close();
                                                                        }
                                                                        catch (Exception)
                                                                        {
                                                                            // Mensagem de erro de ligação
                                                                            System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update do terceiro (Erro: U1002)!");
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
                                                        }
                                                        else if (txt_Descr.Text == "")
                                                        {
                                                            System.Windows.MessageBox.Show("O campo: descrição é de preenchimento obrigatório e está não está preenchido!");
                                                        }
                                                    }
                                                    // Mensagem se existirem espaços em branco no inicio ou fim da descrição
                                                    else
                                                    {
                                                        System.Windows.MessageBox.Show("O campo: nome não pode iniciar ou finalizar com espaços!");
                                                    }
                                                }
                                                // Mensagem se existirem espaços em branco no inicio ou fim do email
                                                else
                                                {
                                                    System.Windows.MessageBox.Show("O campo: email não pode iniciar ou finalizar com espaços!");
                                                }
                                            }
                                            // Mensagem se existirem espaços em branco no inicio ou fim do telefone
                                            else
                                            {
                                                System.Windows.MessageBox.Show("O campo: telefone não pode iniciar ou finalizar com espaços!");
                                            }
                                        }
                                        // Mensagem de falta de preenchimento da localidade
                                        else
                                        {
                                            System.Windows.MessageBox.Show("O campo: localidade é de preenchimento obrigatório e está não está preenchido!");
                                        }
                                    }
                                    // Mensagem se existirem espaços em branco no inicio ou fim da localidade
                                    else
                                    {
                                        System.Windows.MessageBox.Show("O campo: localidade não pode iniciar ou finalizar com espaços!");
                                    }
                                }
                                // Mensagem de falta de preenchimento do código postal
                                else
                                {
                                    System.Windows.MessageBox.Show("O campo: código postal é de preenchimento obrigatório e está não está preenchido!");
                                }
                            }
                            // Mensagem se existirem espaços em branco no inicio ou fim do código postal
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: código postal não pode iniciar ou finalizar com espaços!");
                            }
                        }
                        // Mensagem se existirem espaços em branco no inicio ou fim da morada2
                        else
                        {
                            System.Windows.MessageBox.Show("O campo: morada2 não pode iniciar ou finalizar com espaços!");
                        }
                    }
                    // Mensagem de falta de preenchimento da morada1
                    else
                    {
                        System.Windows.MessageBox.Show("O campo: morada1 é de preenchimento obrigatório e está não está preenchido!");
                    }
                }
                // Mensagem se existirem espaços em branco no inicio ou fim da morada1
                else
                {
                    System.Windows.MessageBox.Show("O campo: morada1 não pode iniciar ou finalizar com espaços!");
                }
            }
            // Mensagem de falta de seleção de tipo
            else
            {
                System.Windows.MessageBox.Show("O campo: tipo é obrigatório, mas não foi selecionado nenhum item!");
            }

        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
