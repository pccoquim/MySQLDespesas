/*
Frm_020301_GrupoAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
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
    /// Interaction logic for Frm_020301_GrupoAdd.xaml
    /// </summary>
    public partial class Frm_020301_GrupoAdd : Window
    {
        private readonly string loginUserId;
        private string selectedGrupoCodFam, selectedGrupoCodSF;
        public Frm_020301_GrupoAdd(string loginUserId, string selectedGrupoCodFam, string selectedGrupoCodSF)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedGrupoCodFam = selectedGrupoCodFam;
            this.selectedGrupoCodSF = selectedGrupoCodSF;
            LoadCbxFamilia();
        }

        private void LoadCbxFamilia()
        {
            // ComboBox familia
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT fam_codigo, fam_descr FROM tbl_0201_familias ORDER BY fam_descr";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        //cbx_Familia.Items.Clear();

                        DataRow allRow = dt.NewRow();
                        allRow["fam_codigo"] = 00; // Defina um valor que represente "Todos"
                        allRow["fam_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);
                        // Limpa a combobox antes de adicionar os itens

                        cbx_Familia.ItemsSource = dt.DefaultView;
                        cbx_Familia.DisplayMemberPath = "fam_descr";
                        cbx_Familia.SelectedValuePath = "fam_codigo";
                        //cbx_Familia.SelectedIndex = 0;
                        cbx_Familia.SelectedValue = selectedGrupoCodFam;
                        cbx_Familia.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Familia (Erro: C1037)!");
                }
            }
        }

        private void LoadCbx_SubFamilia()
        {
            // ComboBox subfamilia
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT sfm_codigo, sfm_descr FROM tbl_0202_subfamilias WHERE sfm_codfam = ? ORDER BY sfm_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@sfm_codfam", selectedGrupoCodFam);
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow allRow = dt.NewRow();
                        allRow["sfm_codigo"] = 000;
                        allRow["sfm_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_SubFamilia.ItemsSource = dt.DefaultView;
                        cbx_SubFamilia.DisplayMemberPath = "sfm_descr";
                        cbx_SubFamilia.SelectedValuePath = "sfm_codigo";
                        cbx_SubFamilia.SelectedValue = selectedGrupoCodSF;
                        cbx_SubFamilia.IsEditable = false;
                        //Cls_0000_VariaveisGlobais.GrupoCodSF = cbx_SubFamilia.SelectedValue.ToString();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_SubFamilias (Erro: C1038)!" + ex.Message);
                }
            }
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
                    string query = "SELECT MAX(grp_cod) FROM tbl_0203_grupos WHERE grp_codsubfam = ?";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grp_codfam", selectedGrupoCodSF);
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
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1006)!");
                }
                txt_Cod.Text = string.Format("{0:000}", ultimoId + 1);
            }
        }

        public bool ValidarGrupo(string grupo)
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
                    string query = "SELECT COUNT(*) FROM tbl_0203_grupos WHERE grp_codSubfam = ? AND grp_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grp_codsubfam", selectedGrupoCodSF);
                        cmd.Parameters.AddWithValue("@grp_descr", grupo);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1010)!");

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
                    string grupo = txt_Descr.Text;
                    // Verifica se já existe o número de conta
                    if (!ValidarGrupo(grupo))
                    {
                        // Obtem ligação
                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                        {
                            // Controlo de erros
                            try
                            {
                                conn.Open();
                                // Definição do insert 
                                string query = "INSERT INTO tbl_0203_grupos(grp_codsubfam, grp_cod, grp_codigo, grp_descr, grp_status, grp_usercreate, grp_datecreate, grp_timecreate, grp_userlastchg, grp_datelastchg, grp_timelastchg) " +
                                    "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                // Definição de query e ligação
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    // Atribuição de variaveis
                                    cmd.Parameters.AddWithValue("@CodFam", cbx_SubFamilia.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                    cmd.Parameters.AddWithValue("@Codigo", cbx_SubFamilia.SelectedValue + txt_Cod.Text);
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
                                System.Windows.MessageBox.Show("Grupo inserido com exito!");

                            }
                            catch (Exception ex)
                            {
                                // mensagem de erro da ligação
                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1007)!" + ex.Message);
                                return;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe um grupo com este nome, escolha uma novo nome!");
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

        private void Cbx_Familia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_SubFamilia.IsEnabled = true;
            txt_Descr.IsEnabled = true;
            selectedGrupoCodFam = cbx_Familia.SelectedValue.ToString();
            LoadCbx_SubFamilia();
        }

        private void Cbx_SubFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGrupoCodSF = cbx_SubFamilia.SelectedValue.ToString();
            LoadNumeracao();
        }
    }
}
