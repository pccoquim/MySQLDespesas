/*
frm_020402_UnidadeEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_020402_UnidadeEdit.xaml
    /// </summary>
    public partial class Frm_020402_UnidadeEdit : Window
    {
        private readonly string loginUserId;
        private readonly int selectedUnidadeId;
        string U_Cod = "", U_Descr = "", U_Peso = "", U_Volume = "", U_Status = "";
        public Frm_020402_UnidadeEdit(string loginUserId, int selectedUnidadeId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedUnidadeId = selectedUnidadeId;
            LoadUnidades();
        }

        private void LoadUnidades()
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
                    // Definição de procura de registos na tabela 
                    string query = "SELECT uni_id, uni_cod, uni_descr, uni_peso, uni_volume, uni_status FROM tbl_0204_unidades WHERE uni_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@UniID", selectedUnidadeId);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                U_Cod = reader["uni_cod"].ToString();
                                U_Descr = reader["uni_descr"].ToString();
                                // Recupera os valores decimais
                                decimal peso = Convert.ToDecimal(reader["uni_peso"]);
                                decimal volume = Convert.ToDecimal(reader["uni_Volume"]);

                                // Define a cultura para usar o ponto como separador decimal
                                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

                                // Formata os valores decimais com o ponto como separador decimal
                                U_Peso = peso.ToString("0.0000", culture);
                                U_Volume = volume.ToString("0.0000", culture);
                                U_Status = reader["uni_status"].ToString();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1041)!" + ex.Message);
                }
                txt_Cod.Text = U_Cod;
                txt_Descr.Text = U_Descr;
                txt_Peso.Text = U_Peso;
                txt_Volume.Text = U_Volume;
                cbx_Status.SelectedValue = U_Status;
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
                    string query = "SELECT COUNT(*) FROM tbl_0204_unidades WHERE uni_descr = ? AND uni_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uni_descr", unidade);
                        cmd.Parameters.AddWithValue("@uni_id", selectedUnidadeId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1012)!");

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

        private bool IsTextAllowed(string text)
        {
            // Define uma expressão regular para verificar se o texto contém apenas dígitos e um ponto
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
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
        }

        private void Txt_Peso_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Peso.Text.Trim() != txt_Peso.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("O campo: peso não pode iniciar ou finalizar com espaços!");
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
                System.Windows.MessageBox.Show("O campo: volume não pode iniciar ou finalizar com espaços!");
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
            // Verifica a existência de espaços no inicio ou fim do campo
            if (txt_Descr.Text.Trim() == txt_Descr.Text)
            {
                // Verifica se o campo está preenchido
                if (txt_Descr.Text != "")
                {
                    // Verifica o valor do campo
                    string unidade = txt_Descr.Text;
                    if (!ValidarUnidade(unidade))
                    {
                        if (txt_Descr.Text != U_Descr || txt_Peso.Text != U_Peso || txt_Volume.Text != U_Volume || Convert.ToString(cbx_Status.SelectedValue) != U_Status)
                        {
                            // Obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                // Prevenção de erros
                                try
                                {
                                    conn.Open();
                                    // String para alteração da conta a crédito
                                    string alterar = "UPDATE tbl_0204_unidades SET uni_descr = ?, uni_peso = ?, uni_volume = ?, uni_status = ?, uni_userlastchg = ?, uni_datelastchg = ?, uni_timelastchg = ? WHERE uni_id = ? ";
                                    // ligação com string e comando
                                    using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                    {
                                        // Atribuição de variaveis com valores
                                        cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                        cmd.Parameters.AddWithValue("@Peso", txt_Peso.Text);
                                        cmd.Parameters.AddWithValue("@Volume", txt_Volume.Text);
                                        cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                        // Atribuição de variavel de identificação para execução da alteração
                                        cmd.Parameters.AddWithValue("@uniID", selectedUnidadeId);
                                        // Executa a alteração
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Mensagem de alteração concluida com exito
                                    System.Windows.MessageBox.Show("Unidade alterada com sucesso!");
                                    // Fecha a janela
                                    this.Close();
                                }
                                catch (Exception)
                                {
                                    // Mensagem de erro de ligação
                                    System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar o update (Erro: U1011)!");
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
