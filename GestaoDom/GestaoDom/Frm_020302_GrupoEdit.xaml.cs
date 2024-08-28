/*
Frm_020302_GrupoEdit.xaml.cs
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
    /// Interaction logic for Frm_0203_GrupoEdit.xaml
    /// </summary>
    public partial class Frm_020302_GrupoEdit : Window
    {
        private readonly string loginUserId;
        private readonly string selectedGrupoCodSF;
        private readonly string selectedGrupoCod;
        private readonly int selectedGrupoId;
        private string GRPFamCod = "", GRPFamDescr = "", GRPSFCod = "", GRPSFDescr = "", GRPCod = "", GRPDescr = "", GRPStatus = "";
        public Frm_020302_GrupoEdit(string loginUserId, string selectedGrupoCodSF, string selectedGrupoCod, int selectedGrupoId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedGrupoCodSF = selectedGrupoCodSF;
            this.selectedGrupoCod = selectedGrupoCod;
            this.selectedGrupoId = selectedGrupoId;
            LoadGrupos();
        }

        private void LoadGrupos()
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
                    // Ligação à base de dados
                    string query = "SELECT grp_cod, grp_descr, grp_status, sfm_cod, sfm_descr, fam_codigo, fam_descr FROM tbl_0203_grupos LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo" +
                        " LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo WHERE grp_codigo = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grp_cod", selectedGrupoCod);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                GRPCod = reader["grp_cod"].ToString();
                                GRPDescr = reader["grp_descr"].ToString();
                                GRPStatus = reader["grp_status"].ToString();
                                GRPSFCod = reader["sfm_cod"].ToString();
                                GRPSFDescr = reader["sfm_descr"].ToString();
                                GRPFamCod = reader["fam_codigo"].ToString();
                                GRPFamDescr = reader["fam_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1039)!" + ex.Message);
                }
                // Preenchimento dos campos
                txt_CodFam.Text = GRPFamCod;
                txt_DescrFam.Text = GRPFamDescr;
                txt_CodSubFam.Text = GRPSFCod;
                txt_DescrSubFam.Text = GRPSFDescr;
                txt_Cod.Text = GRPCod;
                txt_Descr.Text = GRPDescr;
                cbx_Status.SelectedValue = GRPStatus;
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
                    string query = "SELECT COUNT(*) FROM tbl_0203_grupos WHERE grp_codsubfam = ? AND grp_descr = ? AND grp_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grp_codsubfam", selectedGrupoCodSF);
                        cmd.Parameters.AddWithValue("@grp_descr", grupo);
                        cmd.Parameters.AddWithValue("@grp_id", selectedGrupoId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1010)!" + ex.Message);

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

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
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
                        if (txt_Descr.Text != GRPDescr || Convert.ToString(cbx_Status.SelectedValue) != GRPStatus)
                        {
                            // Obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                // Controlo de erros
                                try
                                {
                                    conn.Open();
                                    // Definição do insert 
                                    string query = "UPDATE tbl_0203_grupos SET grp_descr = ?, grp_status = ?, grp_userlastchg = ?, grp_datelastchg = ?, grp_timelastchg = ? WHERE grp_id = ?";
                                    // Definição de query e ligação
                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                    {
                                        // Atribuição de variaveis
                                        cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                        cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                        cmd.Parameters.AddWithValue("@id", selectedGrupoId);
                                        // execução do comando
                                        cmd.ExecuteNonQuery();
                                    }
                                    System.Windows.MessageBox.Show("Grupo alterado com exito!");
                                    // Fecha o formulário                            
                                    this.Close();


                                }
                                catch (Exception ex)
                                {
                                    // mensagem de erro da ligação
                                    System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: U1010)!" + ex.Message);
                                    return;
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
    }
}
