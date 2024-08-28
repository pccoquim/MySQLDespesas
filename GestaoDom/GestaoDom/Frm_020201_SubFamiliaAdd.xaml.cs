/*
Frm_020201_SubFamiliaAdd.xaml.cs
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
    /// Interaction logic for Frm_020201_SubFamiliaAdd.xaml
    /// </summary>
    public partial class Frm_020201_SubFamiliaAdd : Window
    {
        private readonly string loginUserId, selectedSFCodFam;
        public Frm_020201_SubFamiliaAdd(string loginUserId, string selectedSFCodFam)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.selectedSFCodFam = selectedSFCodFam;
            LoadFamilias();
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
                    string query = "SELECT MAX(sfm_cod) FROM tbl_0202_subfamilias WHERE sfm_codfam = ?";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sfm_codfam", selectedSFCodFam);
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
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1005)!");
                }
                txt_Cod.Text = string.Format("{0:000}", ultimoId + 1);
            }
        }

        private void LoadFamilias()
        {
            string Cod = "", Descr = "";
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT fam_id, fam_codigo, fam_descr FROM tbl_0201_familias WHERE fam_codigo = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fam_cod", selectedSFCodFam);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                Cod = reader["fam_codigo"].ToString();
                                Descr = reader["fam_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1029)!" + ex.Message);
                }
                txt_CodFam.Text = Cod;
                txt_DescrFam.Text = Descr;
            }
        }

        public bool ValidarSubFamilia(string subFamilia)
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
                    string query = "SELECT COUNT(*) FROM tbl_0202_subfamilias WHERE sfm_codfam = ? AND sfm_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sfm_codfam", selectedSFCodFam);
                        cmd.Parameters.AddWithValue("@sfm_descr", subFamilia);
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se a descrição está preenchida
            if (txt_Descr.Text != "")
            {
                // Verifica os espaços no inicio e fim do campo
                if (txt_Descr.Text.Trim() == txt_Descr.Text)
                {
                    string subFamilia = txt_Descr.Text;
                    // Verifica se já existe o número de conta
                    if (!ValidarSubFamilia(subFamilia))
                    {
                        // Obtem ligação
                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                        {
                            // Controlo de erros
                            try
                            {
                                conn.Open();
                                // Definição do insert 
                                string query = "INSERT INTO tbl_0202_subfamilias(sfm_codfam, sfm_cod, sfm_codigo, sfm_descr, sfm_status, sfm_usercreate, sfm_datecreate, sfm_timecreate, sfm_userlastchg, sfm_datelastchg, sfm_timelastchg) " +
                                    "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                // Definição de query e ligação
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    // Atribuição de variaveis
                                    cmd.Parameters.AddWithValue("@CodFam", txt_CodFam.Text);
                                    cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                    cmd.Parameters.AddWithValue("@Codigo", txt_CodFam.Text + txt_Cod.Text);
                                    cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                    cmd.Parameters.AddWithValue("@status", 1);
                                    cmd.Parameters.AddWithValue("@User", loginUserId);
                                    cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                    cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                    cmd.Parameters.AddWithValue("@UserLastChg", 0);
                                    cmd.Parameters.AddWithValue("@DateLastChg", 0);
                                    cmd.Parameters.AddWithValue("@TimeLastChg", 0);
                                    // execução do comando
                                    cmd.ExecuteNonQuery();
                                }
                                System.Windows.MessageBox.Show("SubFamilia inserida com exito!");
                                // Fecha o formulário                            
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                // mensagem de erro da ligação
                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1006)!" + ex.Message);
                                return;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe uma subfamilia com este nome, escolha uma nova descrição!");
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
