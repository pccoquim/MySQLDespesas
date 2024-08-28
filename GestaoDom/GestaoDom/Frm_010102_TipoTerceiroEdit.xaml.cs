/*
Frm_010102_TipoTerceiroEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_010102_TipoTerceiroEdit.xaml
    /// </summary>
    public partial class Frm_010102_TipoTerceiroEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedTipoTercId;
        private string Cod = "", Descr = "", Status = "";

        public Frm_010102_TipoTerceiroEdit(string loginUserId, int selectedTipoTercId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.selectedTipoTercId = selectedTipoTercId;
            LoadTipoTerceiros();
        }

        private void LoadTipoTerceiros()
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
                        cbx_Status.ItemsSource = dtStatus.DefaultView;
                        cbx_Status.DisplayMemberPath = "status_descr";
                        cbx_Status.SelectedValuePath = "status_cod";
                        cbx_Status.SelectedIndex = -1;
                        cbx_Status.IsEditable = false;
                    }

                    string query = "SELECT tipoterc_id, tipoterc_cod, tipoterc_descr, tipoterc_status, status_descr FROM tbl_0101_tipoterceiro, tbl_0001_status WHERE tipoterc_status = status_cod AND tipoterc_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@tipotercID", selectedTipoTercId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Cod = reader["tipoterc_cod"].ToString();
                                Descr = reader["tipoterc_descr"].ToString();
                                Status = reader["tipoterc_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar tipo de terceiros (Erro: C1007)!");
                }

                txt_Cod.Text = Cod;
                txt_Descr.Text = Descr;
                cbx_Status.SelectedValue = Status;
            }
        }

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                MessageBox.Show("Verifique os espaços em branco no inicio ou fim da descrição, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Descr.Text != "")
            {
                if (txt_Descr.Text != Descr || Convert.ToString(cbx_Status.SelectedValue) != Status)
                {
                    // obtem ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            // update do tipo de terceiro
                            string query = "UPDATE tbl_0101_tipoterceiro SET tipoterc_descr = ?, tipoterc_status = ?, tipoterc_userlastchg =  ?, tipoterc_datelastchg = ?, tipoterc_timelastchg = ? WHERE tipoterc_id = ?";
                            // definição de query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@desig", txt_Descr.Text);
                                cmd.Parameters.AddWithValue("@status", cbx_Status.SelectedValue);
                                cmd.Parameters.AddWithValue("@userlstchg", loginUserId);
                                cmd.Parameters.AddWithValue("@datelstchg", Cls_0002_ActualDateTime.Date);
                                cmd.Parameters.AddWithValue("@timelstchg", Cls_0002_ActualDateTime.Time);
                                cmd.Parameters.AddWithValue("@tipotercID", selectedTipoTercId);
                                // execução do comando
                                cmd.ExecuteNonQuery();
                            }
                            // Mensagem de execução bem sucedida
                            System.Windows.MessageBox.Show("Tipo de terceiro alterado com sucesso!");
                            // Fecha o formulário após gravar
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            // mensagem de erro da ligação
                            System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: U1001)!" + ex.Message);
                            return;
                        }
                    }
                }
                else
                {
                    if (ShowConfirmation("Não foram efetuadas alterações para gravar! Pretende sair?"))
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("O campo: Descrição é de preencimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }
    }
}
