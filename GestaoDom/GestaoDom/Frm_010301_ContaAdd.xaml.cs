/*
Frm_010301_ContaAdd.xaml.cs
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
    /// Interaction logic for Frm_010301_ContasAdd.xaml
    /// </summary>
    public partial class Frm_010301_ContaAdd : Window
    {
        private readonly string loginUserId;
        public Frm_010301_ContaAdd(string loginUserId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            LoadNumeracao();
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
                    string query = "SELECT MAX(cntcred_cod) FROM tbl_0103_contascred";
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
                    System.Windows.MessageBox.Show("Erro ao carregar contas (Erro: N1002)!");
                }
                txt_Cod.Text = string.Format("{0:000}", ultimoId + 1);
            }
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
                System.Windows.MessageBox.Show("O campo: número não é de preenchimento obrigatório, mas quando preenchido não pode iniciar ou finalizar com espaços em branco!");
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
            // Verifica se a descrição está preenchida
            if (txt_Descr.Text != "")
            {
                // Verifica os espaços no inicio e fim do campo
                if (txt_Descr.Text.Trim() == txt_Descr.Text)
                {
                    // Verifica os espaços em branco no inocio e fim  do campo
                    if (txt_Nr.Text.Trim() == txt_Nr.Text)
                    {
                        string conta = txt_Descr.Text;
                        // Verifica se já existe a conta
                        if (!ValidarConta(conta))
                        {
                            string contaNr = txt_Nr.Text;
                            // Verifica se já existe o número de conta
                            if (!ValidarNrConta(contaNr))
                            {
                                // Obtem ligação
                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                {
                                    // Controlo de erros
                                    try
                                    {
                                        conn.Open();
                                        // Definição do insert contas credito
                                        string queryCntCred = "INSERT INTO tbl_0103_contascred(cntcred_cod, cntcred_descr, cntcred_nr, cntcred_status, cntcred_usercreate, cntcred_datecreate, cntcred_timecreate, cntcred_userlastchg, cntcred_datelastchg, cntcred_timelastchg) " +
                                            "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                        // Definição de query e ligação
                                        using (MySqlCommand cmdCntCred = new MySqlCommand(queryCntCred, conn))
                                        {
                                            // Atribuição de variaveis com valores
                                            cmdCntCred.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                            cmdCntCred.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                            cmdCntCred.Parameters.AddWithValue("@Nr", txt_Nr.Text);
                                            cmdCntCred.Parameters.AddWithValue("@status", 1);
                                            cmdCntCred.Parameters.AddWithValue("@User", loginUserId);
                                            cmdCntCred.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                            cmdCntCred.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                            cmdCntCred.Parameters.AddWithValue("@UserLastChg", 0);
                                            cmdCntCred.Parameters.AddWithValue("@DateLastChg", 0);
                                            cmdCntCred.Parameters.AddWithValue("@TimeLastChg", 0);
                                            // execução do comando
                                            cmdCntCred.ExecuteNonQuery();
                                        }

                                        // Definição do insert contas débito
                                        string queryCntDeb = "INSERT INTO tbl_0103_contasdeb(cntdeb_cod, cntdeb_descr, cntdeb_nr, cntdeb_status, cntdeb_usercreate, cntdeb_datecreate, cntdeb_timecreate, cntdeb_userlastchg, cntdeb_datelastchg, cntdeb_timelastchg) " +
                                            "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                        // Definição de query e ligação
                                        using (MySqlCommand cmdCntDeb = new MySqlCommand(queryCntDeb, conn))
                                        {
                                            // Atribuição de variaveis
                                            cmdCntDeb.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                            cmdCntDeb.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                            cmdCntDeb.Parameters.AddWithValue("@Nr", txt_Nr.Text);
                                            cmdCntDeb.Parameters.AddWithValue("@status", 01);
                                            cmdCntDeb.Parameters.AddWithValue("@User", loginUserId);
                                            cmdCntDeb.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                            cmdCntDeb.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                            cmdCntDeb.Parameters.AddWithValue("@UserLastChg", 0);
                                            cmdCntDeb.Parameters.AddWithValue("@DateLastChg", 0);
                                            cmdCntDeb.Parameters.AddWithValue("@TimeLastChg", 0);
                                            // execução do comando
                                            cmdCntDeb.ExecuteNonQuery();
                                        }
                                        // Fecha o formulário                            
                                        this.Close();
                                        System.Windows.MessageBox.Show("Conta inserida com exito!");
                                    }
                                    catch (Exception)
                                    {
                                        // mensagem de erro da ligação
                                        System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1003)!");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: número não é de preenchimento obrigatório, mas quando preenchido não pode ser igual a um já existente!");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Já existe uma conta com descrição igual, escolha uma nova descrição!");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("O campo: número não é de preenchimento obrigatório, mas quando preenchido não pode iniciar ou finalizar com espaços em branco!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim da descrição, não são permitidos espaços em branco nestes locais!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O campo: descrição é de preenchimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    string query = "SELECT COUNT(*) FROM tbl_0103_contascred WHERE cntcred_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cntcred_descr", conta);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1001)!");

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
                    string query = "SELECT COUNT(*) FROM tbl_0103_contascred WHERE cntcred_nr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cntcred_nr", contaNr);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1002)!");

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
    }
}
