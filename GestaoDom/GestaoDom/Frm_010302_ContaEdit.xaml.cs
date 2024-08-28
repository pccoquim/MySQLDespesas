/*
Frm_010302_ContaEdit.xaml.cs
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
    /// Interaction logic for Frm_010302_ContaEdit.xaml
    /// </summary>
    public partial class Frm_010302_ContaEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedContaId;
        string Cod = "", Descr = "", Nr = "", Status = "";
        public Frm_010302_ContaEdit(string loginUserId, int selectedContaId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedContaId = selectedContaId;
            LoadContas();
        }

        private void LoadContas()
        {
            // ComboBox status
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
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
                        cmdStatus.Parameters.AddWithValue("@contaID", selectedContaId);
                        cbx_Status.ItemsSource = dtStatus.DefaultView;
                        cbx_Status.DisplayMemberPath = "status_descr";
                        cbx_Status.SelectedValuePath = "status_cod";
                        cbx_Status.SelectedIndex = -1;
                        cbx_Status.IsEditable = false;
                    }
                    // Dados dos acesso
                    string queryConta = "SELECT cntcred_cod, cntcred_descr, cntcred_nr, cntcred_status, status_descr FROM tbl_0103_contascred, tbl_0001_status WHERE cntcred_status = status_cod AND cntcred_id = ?";

                    using (MySqlCommand cmdConta = new MySqlCommand(queryConta, conn))
                    {
                        // Atribuição de variavel
                        cmdConta.Parameters.AddWithValue("@contaID", selectedContaId);
                        using (MySqlDataReader reader = cmdConta.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Cod = reader["cntcred_cod"].ToString();
                                Descr = reader["cntcred_descr"].ToString();
                                Nr = reader["cntcred_nr"].ToString();
                                Status = reader["cntcred_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar contas (Erro: C1014)!");
                }

                txt_Cod.Text = Cod;
                txt_Descr.Text = Descr;
                txt_Nr.Text = Nr;
                cbx_Status.SelectedValue = Status;
            }
        }

        public bool ValidarConta(string conta)
        {
            int existe = 0;
            bool valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT COUNT(*) FROM tbl_0103_contascred WHERE cntcred_descr = ? AND cntcred_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cntcred_descr", conta);
                        cmd.Parameters.AddWithValue("@id", selectedContaId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1003)!");
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

        public bool ValidarNrConta(string contaNr)
        {
            int existe = 0;
            bool valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta
                    string query = "SELECT COUNT(*) FROM tbl_0103_contascred WHERE cntcred_nr = ? AND cntcred_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cntcred_nr", contaNr);
                        cmd.Parameters.AddWithValue("@id", selectedContaId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1004)!");

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

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: descrição não pode iniciar ou finalizar com espaços!");
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

        private void Txt_Nr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Nr.Text.Trim() != txt_Nr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: número não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Txt_Nr_KeyDown(object sender, KeyEventArgs e)
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Verifica a existência de espaços no inicio ou fim do campo
            if (txt_Descr.Text.Trim() == txt_Descr.Text)
            {
                // Verifica se o campo está preenchido
                if (txt_Descr.Text != "")
                {
                    string conta = txt_Descr.Text;
                    // Verifica se a conta  é válida
                    if (!ValidarConta(conta))
                    {
                        // Verifica a existência de espaços no inicio ou fim do campo
                        if (txt_Nr.Text.Trim() == txt_Nr.Text)
                        {
                            // Verifica se o campo está preenchido
                            if (txt_Nr.Text != "")
                            {
                                string contaNr = txt_Nr.Text;
                                // Verifica se o número  é válido
                                if (!ValidarNrConta(contaNr))
                                {
                                    if (txt_Descr.Text != Descr || txt_Nr.Text != Nr || Convert.ToString(cbx_Status.SelectedValue) != Status)
                                    {
                                        // Obtem ligação
                                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                        {
                                            // Prevenção de erros
                                            try
                                            {
                                                conn.Open();
                                                // String para alteração da conta a crédito
                                                string alterar = "UPDATE tbl_0103_contascred SET cntcred_descr = ?, cntcred_nr = ?, cntcred_status = ?, cntcred_userlastchg = ?, cntcred_datelastchg = ?, cntcred_timelastchg = ? WHERE cntcred_id = ? ";
                                                // ligação com string e comando
                                                using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                                {
                                                    // Atribuição de variaveis com valores
                                                    cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                    cmd.Parameters.AddWithValue("@Morada1", txt_Nr.Text);
                                                    cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                    cmd.Parameters.AddWithValue("@User", loginUserId);
                                                    cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                    cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                    // Atribuição de variavel de identificação para execução da alteração
                                                    cmd.Parameters.AddWithValue("@contaID", selectedContaId);
                                                    // Executa a alteração
                                                    cmd.ExecuteNonQuery();
                                                }
                                                // String para alteração da conta adébito
                                                string alterarc = "UPDATE tbl_0103_contasdeb SET cntdeb_descr = ?, cntdeb_nr = ?, cntdeb_status = ?, cntdeb_userlastchg = ?, cntdeb_datelastchg = ?, cntdeb_timelastchg = ? WHERE cntdeb_id = ? ";
                                                // ligação com string e comando
                                                using (MySqlCommand cmdc = new MySqlCommand(alterarc, conn))
                                                {
                                                    // Atribuição de variaveis com valores
                                                    cmdc.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                    cmdc.Parameters.AddWithValue("@Morada1", txt_Nr.Text);
                                                    cmdc.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                                    cmdc.Parameters.AddWithValue("@User", loginUserId);
                                                    cmdc.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                                    cmdc.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                                    // Atribuição de variavel de identificação para execução da alteração
                                                    cmdc.Parameters.AddWithValue("@contaID", selectedContaId);
                                                    // Executa a alteração
                                                    cmdc.ExecuteNonQuery();
                                                }
                                                // Mensagem de alteração concluida com exito
                                                System.Windows.MessageBox.Show("Conta alterada com sucesso!");
                                                // Fecha a janela
                                                this.Close();
                                            }
                                            catch (Exception)
                                            {
                                                // Mensagem de erro de ligação
                                                System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update da conta (Erro: U1006)!");
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
                                // Mensagem se número já existir noutra conta
                                else
                                {
                                    System.Windows.MessageBox.Show("O número indicado já existe noutra conta!");
                                }
                            }
                            // Mensagem de falta de preenchimento da número
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: número é de preenchimento obrigatório e está não está preenchido!");
                            }
                        }
                        // Mensagem se existirem espaços em branco no inicio ou fim do número
                        else
                        {
                            System.Windows.MessageBox.Show("O campo: número não pode iniciar ou finalizar com espaços!");
                        }
                    }
                    // Mensagem se já existir a conta
                    else
                    {
                        System.Windows.MessageBox.Show("O nome indicado já existe noutra conta!");
                    }
                }
                // Mensagem de falta de preenchimento da descrição
                else
                {
                    System.Windows.MessageBox.Show("O campo: nome é de preenchimento obrigatório e está não está preenchido!");
                }
            }
            // Mensagem se existirem espaços em branco no inicio ou fim do descrição
            else
            {
                System.Windows.MessageBox.Show("O campo: nome não pode iniciar ou finalizar com espaços!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
