/*
Frm_000402_AcessosEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_000402_AcessosEdit.xaml
    /// </summary>
    public partial class Frm_000402_AcessosEdit : System.Windows.Window
    {
        private readonly Regex numericRegex = new Regex(@"^\d+$");
        private readonly string loginUserId;
        private readonly int accessId;
        private int Nivel = 0;
        private string Cod = "", Descr = "", Status = "";

        public Frm_000402_AcessosEdit(string loginUserId, int accessId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.accessId = accessId;
            LoadAcessos();
        }

        private void Txt_Nivel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !numericRegex.IsMatch(e.Text);
        }

        private void Txt_Nivel_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;
            string text = textBox.Text;

            if (!string.IsNullOrEmpty(text) && !numericRegex.IsMatch(text))
            {
                int caretIndex = textBox.CaretIndex;
                textBox.Text = Regex.Replace(text, @"[^\d]", "");
                textBox.CaretIndex = caretIndex > 0 ? caretIndex - 1 : 0;
            }
        }

        private void LoadAcessos()
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

                    // Dados dos acessos
                    string query = "SELECT opm_id, opm_cod, opm_descr, opm_nivel, opm_status, status_descr FROM tbl_0003_opcoesAcesso, tbl_0001_status WHERE opm_status = status_cod AND opm_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@AcessosID", accessId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Cod = reader["opm_cod"].ToString();
                                Descr = reader["opm_descr"].ToString();
                                Nivel = Convert.ToInt32(reader["opm_nivel"]);
                                Status = reader["opm_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error while loading users: " + ex.Message);
                }

                txt_Cod.Text = Cod;
                txt_Descr.Text = Descr;
                txt_Nivel.Text = Nivel.ToString();
                cbx_Status.SelectedValue = Status;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Descr.Text != "")
            {
                if (txt_Nivel.Text != "")
                {
                    if (txt_Descr.Text != Descr || txt_Nivel.Text != Convert.ToString(Nivel) || Convert.ToString(cbx_Status.SelectedValue) != Status)
                    {
                        // obtem ligação
                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                        {
                            try
                            {
                                conn.Open();
                                // update do utilizador
                                string query = "UPDATE tbl_0003_opcoesAcesso SET opm_descr = ?, opm_nivel = ?, opm_status = ?, opm_userlastchg = ?, opm_datelastchg = ?, opm_timelastchg = ? WHERE opm_id = ?";
                                // definição de query e ligação
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    // Atribuição de variaveis com valores
                                    cmd.Parameters.AddWithValue("@Cod", txt_Descr.Text);
                                    cmd.Parameters.AddWithValue("@Nr", txt_Nivel.Text);
                                    cmd.Parameters.AddWithValue("@status", cbx_Status.SelectedValue);
                                    cmd.Parameters.AddWithValue("@User", loginUserId);
                                    cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                    cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                    cmd.Parameters.AddWithValue("@opcaoID", accessId);
                                    // execução do comando
                                    cmd.ExecuteNonQuery();
                                }
                                // Mensagem de execução bem sucedida
                                System.Windows.MessageBox.Show("Acesso alterado com sucesso!");
                                // Fecha o formulário após gravar
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                // mensagem de erro da ligação
                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados: " + ex.Message);
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
                    MessageBox.Show("O campo: Nivel é de preencimento obrigatório e não está preenchido!");
                }
            }
            else
            {
                MessageBox.Show("O campo: Descrição é de preencimento obrigatório e não está preenchido!");
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

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }
    }
}