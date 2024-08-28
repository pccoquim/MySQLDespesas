/*
Frm_020102_FamiliaEdit.xaml.cs
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
    /// Interaction logic for Frm_020102_FamiliaEdit.xaml
    /// </summary>
    public partial class Frm_020102_FamiliaEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedFamId;
        private string cod = "", desig = "", status = "";
        public Frm_020102_FamiliaEdit(string loginUserId, int selectedFamId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedFamId = selectedFamId;
            LoadFamilias();
        }

        private void LoadFamilias()
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

                    // Dados familias
                    string query = "SELECT fam_codigo, fam_descr, fam_status FROM tbl_0201_familias WHERE fam_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@famID", selectedFamId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["fam_codigo"].ToString();
                                desig = reader["fam_descr"].ToString();
                                status = reader["fam_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar dados (Erro: C1023)!");
                }

                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                cbx_Status.SelectedValue = status;
            }
        }

        public bool ValidarFamilia(string familia)
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
                    string query = "SELECT COUNT(*) FROM tbl_0201_familias WHERE fam_descr = ? AND fam_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fam_descr", familia);
                        cmd.Parameters.AddWithValue("@fam_id", selectedFamId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1008)!");

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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Verifica a existência de espaços no inicio ou fim do campo
            if (txt_Descr.Text.Trim() == txt_Descr.Text)
            {
                // Verifica se o campo está preenchido
                if (txt_Descr.Text != "")
                {
                    // Verifica o valor do campo
                    string familia = txt_Descr.Text;
                    if (!ValidarFamilia(familia))
                    {
                        if (txt_Descr.Text != desig || Convert.ToString(cbx_Status.SelectedValue) != status)
                        {
                            // Obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                // Prevenção de erros
                                try
                                {
                                    conn.Open();
                                    // String para alteração da conta a crédito
                                    string alterar = "UPDATE tbl_0201_familias SET fam_descr = ?, fam_status = ?, fam_userlastchg = ?, fam_datelastchg = ?, fam_timelastchg = ? WHERE fam_id = ? ";
                                    // ligação com string e comando
                                    using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                    {
                                        // Atribuição de variaveis com valores
                                        cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                        cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                        // Atribuição de variavel de identificação para execução da alteração
                                        cmd.Parameters.AddWithValue("@famID", selectedFamId);
                                        // Executa a alteração
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Mensagem de alteração concluida com exito
                                    System.Windows.MessageBox.Show("Familia alterada com sucesso!");
                                    // Fecha a janela
                                    this.Close();
                                }
                                catch (Exception)
                                {
                                    // Mensagem de erro de ligação
                                    System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update (Erro: U1008)!");
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
                    // Mensagem se já existir
                    else
                    {
                        System.Windows.MessageBox.Show("O nome indicado já existe!");
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
