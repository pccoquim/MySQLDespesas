/*
Frm_020401_UnidadeAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_020401_UnidadeAdd.xaml
    /// </summary>
    public partial class Frm_020401_UnidadeAdd : Window
    {
        private readonly string loginUserId;
        public Frm_020401_UnidadeAdd(string loginUserId)
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
                    string query = "SELECT MAX(uni_cod) FROM tbl_0204_unidades";
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
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1007)!");
                }
                txt_Cod.Text = string.Format("{0:00}", ultimoId + 1);
            }
        }

        public bool ValidarUnidade(string unidade)
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
                    string query = "SELECT COUNT(*) FROM tbl_0204_unidades WHERE uni_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uni_descr", unidade);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1011)!");

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

        private bool IsTextAllowed(string text)
        {
            // Define uma expressão regular para verificar se o texto contém apenas dígitos e um ponto
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome, não são permitidos espaços em branco nestes locais!");
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

        private void Txt_Peso_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Peso.Text.Trim() != txt_Peso.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do volume, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_Peso_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Peso_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Txt_Volume_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Volume.Text.Trim() != txt_Volume.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do volume, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_Volume_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Volume_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido contém apenas caracteres permitidos (0-9 e ponto)
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
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
                    // Verifica se já existe 
                    string unidade = txt_Descr.Text;
                    if (!ValidarUnidade(unidade))
                    {
                        // Verifica se a descrição está preenchida
                        if (txt_Peso.Text != "")
                        {
                            // Verifica os espaços no inicio e fim do campo
                            if (txt_Peso.Text.Trim() == txt_Peso.Text)
                            {
                                // Verifica se a descrição está preenchida
                                if (txt_Volume.Text != "")
                                {
                                    // Verifica os espaços no inicio e fim do campo
                                    if (txt_Volume.Text.Trim() == txt_Volume.Text)
                                    {
                                        // Obtem ligação
                                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                        {
                                            // Controlo de erros
                                            try
                                            {
                                                conn.Open();
                                                // Definição do insert 
                                                string query = "INSERT INTO tbl_0204_unidades(uni_cod, uni_descr, uni_peso, uni_volume, uni_status, uni_usercreate, uni_datecreate, uni_timecreate, uni_userlastchg, uni_datelastchg, uni_timelastchg) " +
                                                    "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                                // Definição de query e ligação
                                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                                {
                                                    // Atribuição de variaveis
                                                    cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                                    cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                                    cmd.Parameters.AddWithValue("@Peso", txt_Peso.Text);
                                                    cmd.Parameters.AddWithValue("@Volume", txt_Volume.Text);
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
                                                this.Close();
                                                System.Windows.MessageBox.Show("Unidade inserida com exito!");

                                            }
                                            catch (Exception ex)
                                            {
                                                // mensagem de erro da ligação
                                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1009)!" + ex.Message);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do volume, não são permitidos espaços em branco nestes locais!");
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("O campo: volume é de preenchimento obrigatório e não está preenchido!");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do peso, não são permitidos espaços em branco nestes locais!");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("O campo: peso é de preenchimento obrigatório e não está preenchido!");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe uma unidade com este nome, escolha uma nova descrição!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome, não são permitidos espaços em branco nestes locais!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O campo: nome é de preenchimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
