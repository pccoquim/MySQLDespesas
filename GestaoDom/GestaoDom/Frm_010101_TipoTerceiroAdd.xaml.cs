/*
Frm_010101_TipoTerceiroAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_010101_TipoTerceiroAdd.xaml
    /// </summary>
    public partial class Frm_010101_TipoTerceiroAdd : Window
    {
        private readonly string loginUserId;
        public Frm_010101_TipoTerceiroAdd(string loginUserId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            LoadTipoTerceiroAdd();
            this.loginUserId = loginUserId;
        }

        private void LoadTipoTerceiroAdd()
        {
            int ultimoId = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT MAX(tipoterc_cod) FROM tbl_0101_tipoterceiro";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            ultimoId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar tipos de terceiros: " + ex.Message);
                }

                txt_Cod.Text = string.Format("{0:00}", ultimoId + 1);
            }
        }

        private void Txt_Descr_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "")
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Descr.Text != "")
            {
                // Verifica se já existe o tipo de terceiro
                string tipoTerceiro = txt_Descr.Text;
                if (!ValidarTipoTerceiro(tipoTerceiro))
                {
                    // obtem ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            // insert do novo utilizador
                            string query = "INSERT INTO tbl_0101_tipoterceiro(tipoterc_cod, tipoterc_descr, tipoterc_status, tipoterc_usercreate, tipoterc_datecreate, tipoterc_timecreate, tipoterc_userlastchg, tipoterc_datelastchg, tipoterc_timelastchg) " +
                                "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";
                            // definição de query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Atribuição de variaveis
                                cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                cmd.Parameters.AddWithValue("@status", 1);
                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                cmd.Parameters.AddWithValue("@Userlastchg", 0);
                                cmd.Parameters.AddWithValue("@Datalastchg", 0);
                                cmd.Parameters.AddWithValue("@Horalastchg", 0);
                                // execução do comando
                                cmd.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Acesso inserido com exito!");

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
                    System.Windows.MessageBox.Show("Já existe uma descrição do tipo de terceiro igual, escolha uma nova descrição!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O campo: descrição do tipo de terceiro é de preenchimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim da descrição, não são permitidos espaços em branco nestes locais!");
            }
        }

        public bool ValidarTipoTerceiro(string tipoTerceiro)
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
                    string query = "SELECT COUNT(*) FROM tbl_0101_tipoterceiro WHERE tipoterc_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipoterc_descr", tipoTerceiro);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados: " + ex.Message);
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
    }
}
