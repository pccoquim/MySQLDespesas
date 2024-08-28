/*
Frm_000401_AcessosAdd.xaml.cs
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
    /// Interaction logic for Frm_000401_AcessosAdd.xaml
    /// </summary>
    public partial class Frm_000401_AcessosAdd : Window
    {
        private readonly string loginUserId;
        public Frm_000401_AcessosAdd(string loginUserId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
        }

        public bool ValidarCodigo(string accessCod)
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
                    string query = "SELECT COUNT(*) FROM tbl_0003_opcoesAcesso WHERE opm_cod = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@vtr_descr", accessCod);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1019)!");
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
        }

        private void Txt_Cod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && txt_Nivel.Text != "")
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

        private void Txt_Descr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && txt_Nivel.Text != "")
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

        private void Txt_Nivel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txt_Cod.Text != "" && txt_Descr.Text != "" && txt_Nivel.Text != "")
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
            if (txt_Cod.Text != "" && txt_Descr.Text != "" && txt_Nivel.Text != "")
            {
                string accessCod = txt_Cod.Text;
                if (!ValidarCodigo(accessCod))
                {
                    // obtem ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            // insert da nova opção
                            string query = "INSERT INTO tbl_0003_opcoesAcesso(opm_cod, opm_descr, opm_nivel, opm_status, opm_usercreate, opm_datecreate, opm_timecreate, opm_userlastchg, opm_datelastchg, opm_timelastchg) " +
                                "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                            // definição de query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Atribuição de variaveis com valores
                                cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                cmd.Parameters.AddWithValue("@Nr", txt_Nivel.Text);
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
                            // Fecha o formulário                            
                            this.Close();
                            MessageBox.Show("Acesso inserido com exito!");
                        }
                        catch (Exception)
                        {
                            // mensagem de erro da ligação
                            MessageBox.Show("Ocorreu um erro ao ligar à base de dados (I1011)");
                            return;
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Já existe um acesso com este código, indique um novo código!");
                }
            }
            else
            {
                MessageBox.Show("Todos os campos são de preenchimento obrigatório!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
