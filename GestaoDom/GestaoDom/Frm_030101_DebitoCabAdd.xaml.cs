/*
Frm_030101_DebitoCabAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_030101_DebitoCabAdd.xaml
    /// </summary>
    public partial class Frm_030101_DebitoCabAdd : Window
    {
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        public Frm_030101_DebitoCabAdd(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();

            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadCbx_Terceiro();
            LoadCbx_Conta();

        }

        private void LoadCbx_Terceiro()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT terc_cod, terc_descr FROM tbl_0102_terceiros WHERE terc_status = 1 AND terc_cod != 0 ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cbx_Terceiro.ItemsSource = dt.DefaultView;
                        cbx_Terceiro.DisplayMemberPath = "terc_descr";
                        cbx_Terceiro.SelectedValuePath = "terc_cod";
                        cbx_Terceiro.SelectedValue = -1;
                        cbx_Terceiro.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1075)!" + ex.Message);
                }
            }
        }

        private void LoadCbx_Conta()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT cntcred_cod, cntcred_descr FROM tbl_0103_contascred WHERE cntcred_status = 1 AND cntcred_cod != 0 ORDER BY cntcred_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cbx_Conta.ItemsSource = dt.DefaultView;
                        cbx_Conta.DisplayMemberPath = "cntcred_descr";
                        cbx_Conta.SelectedValuePath = "cntcred_cod";
                        cbx_Conta.SelectedIndex = -1;
                        cbx_Conta.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1076)!" + ex.Message);
                }
            }
        }

        public bool ValidarNDoc(string nDoc)
        {
            string document = nDoc;
            int existe = 0;
            bool valor;
            string codTerc = cbx_Terceiro.SelectedValue.ToString();

            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT COUNT(*) FROM tbl_0301_movimentosdebito WHERE fd_codterc = ? AND fd_numdoc = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fd_codterc", codTerc);
                        cmd.Parameters.AddWithValue("@fd_nDoc", document);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1025)!" + ex.Message);
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

        private void Txt_NDoc_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_NDoc.Text.Trim() != txt_NDoc.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_NDoc_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValidDate(txt_Data.Text))
            {
                MessageBox.Show("A data inserida não é válida. Use o formato dd/mm/yyyy.", "Erro de formato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Txt_Data_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var text = textBox.Text.Replace(".", "");
                if (text.Length >= 2 && text.Length < 4)
                {
                    textBox.Text = text.Insert(2, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
                else if (text.Length >= 4)
                {
                    textBox.Text = text.Insert(2, ".").Insert(5, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
            }
        }

        private bool IsValidDate(string date)
        {
            // Verifica se a data é válida usando DateTime.TryParseExact.
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }

        private void Btn_AddItems_Click(object sender, RoutedEventArgs e)
        {
            object terceiro = cbx_Terceiro.SelectedValue;
            object debito = cbx_Conta.SelectedValue;


            if (txt_NDoc.Text != "")
            {
                // Verifica os espaços no inicio e fim do campo
                if (txt_NDoc.Text.Trim() == txt_NDoc.Text)
                {
                    if (!ValidarNDoc(txt_NDoc.Text))
                    {
                        if (debito != null)
                        {
                            if (terceiro != null)
                            {
                                string codTerc = cbx_Terceiro.SelectedValue.ToString();
                                string nDoc = txt_NDoc.Text;
                                string data = txt_Data.Text;
                                string codConta = cbx_Conta.SelectedValue.ToString();
                                Frm_0302_DebitosDetManut frm_0302_DebitosDetManut = new Frm_0302_DebitosDetManut(codTerc, nDoc, data, codConta,loginUserSequence, loginUserId, loginUserType)
                                {
                                    Owner = this
                                };
                                frm_0302_DebitosDetManut.ShowDialog();
                                this.Close();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("O campo: terceiro é de preenchimento obrigatório e não está selecionado nenhum terceiro!");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("O campo: conta a débito é de preenchimento obrigatório e não está selecionada nenhuma conta!");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe este número de documento para este terceiro!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O campo: nº de documento é de preenchimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
