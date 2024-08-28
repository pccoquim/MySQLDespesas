/*
Frm_010401_TipoReceitaAdd.xaml.cs
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
    /// Interaction logic for Frm_0104_TipoReceitaAdd.xaml
    /// </summary>
    public partial class Frm_010401_TipoReceitaAdd : Window
    {
        private readonly string loginUserId;
        public Frm_010401_TipoReceitaAdd(string loginUserId)
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
                    string query = "SELECT MAX(tr_cod) FROM tbl_0104_tiporeceita";
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
                    System.Windows.MessageBox.Show("Erro ao carregar contas (Erro: N1003)!");
                }

                txt_Cod.Text = string.Format("{0:000}", ultimoId + 1);
            }
        }

        public bool ValidarTipoReceita(string tipoReceita)
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
                    string query = "SELECT COUNT(*) FROM tbl_0104_tiporeceita WHERE tr_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tr_descr", tipoReceita);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1005)!");

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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se a descrição está preenchida
            if (txt_Descr.Text != "")
            {
                // Verifica os espaços no inicio e fim do campo
                if (txt_Descr.Text.Trim() == txt_Descr.Text)
                {
                    string tipoReceita = txt_Descr.Text;
                    // Verifica se já existe o número de conta
                    if (!ValidarTipoReceita(tipoReceita))
                    {
                        // Obtem ligação
                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                        {
                            // Controlo de erros
                            try
                            {
                                conn.Open();
                                // Definição do insert 
                                string query = "INSERT INTO tbl_0104_tiporeceita(tr_cod, tr_descr, tr_status, tr_usercreate, tr_datecreate, tr_timecreate, tr_userlastchg, tr_datelastchg, tr_timelastchg) " +
                                    "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                // Definição de query e ligação
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
                                    cmd.Parameters.AddWithValue("@Datelastchg", 0);
                                    cmd.Parameters.AddWithValue("@Timelastchg", 0);
                                    // execução do comando
                                    cmd.ExecuteNonQuery();
                                }
                                // Fecha o formulário                            
                                this.Close();
                                System.Windows.MessageBox.Show("Tipo de receita inserida com exito!");
                            }
                            catch (Exception)
                            {
                                // mensagem de erro da ligação
                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1004)!");
                                return;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe um tipo de receita com descrição igual, escolha uma nova descrição!");
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
    }
}
