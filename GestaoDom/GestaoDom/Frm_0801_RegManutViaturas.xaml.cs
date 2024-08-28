/*
Frm_0801_RegManutViarias.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0801_RegManutViaturas.xaml
    /// </summary>
    public partial class Frm_0801_RegManutViaturas : Window
    {
        public static Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedManutId, selectedManutRef, selectedManutIndex;
        public Frm_0801_RegManutViaturas(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();

            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadManut();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar 
                if (Cls_0005_AccessControl.AccessGranted("M25V0201", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar 
                if (Cls_0005_AccessControl.AccessGranted("M25V0202", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar 
                if (Cls_0005_AccessControl.AccessGranted("M25V0203", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void LoadManut()
        {
            ObservableCollection<Cls_0801_ManutViaturas> manutViaturas = new ObservableCollection<Cls_0801_ManutViaturas>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT vtm_id, vtm_mid, vtm_codviatura, vtm_descr, vtm_datamanut, vtm_km, vtm_oleo, vtm_filtrooleo, vtm_filtroar, vtm_efetuado, vtm_kmproxima, vtm_status, vtr_matricula, vtr_descr, status_descr " +
                        "FROM tbl_0901_viaturasmanutencao " +
                        "LEFT JOIN tbl_0205_viaturas ON tbl_0901_viaturasmanutencao.vtm_codviatura = tbl_0205_viaturas.vtr_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0901_viaturasmanutencao.vtm_status = tbl_0001_status.status_cod " +
                        "ORDER BY vtm_mid DESC";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dataDB = reader["vtm_datamanut"].ToString();

                                if (DateTime.TryParseExact(dataDB, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                                {
                                    string dataFormatadaParaUI = dataFormatada.ToString("dd.MM.yyyy");

                                    manutViaturas.Add(new Cls_0801_ManutViaturas
                                    {
                                        Id = Convert.ToInt32(reader["vtm_id"]),
                                        Ref = Convert.ToInt32(reader["vtm_mid"]),
                                        CodViatura = reader["vtm_codviatura"].ToString(),
                                        MatViatura = reader["vtr_matricula"].ToString(),
                                        DescrViatura = reader["vtr_descr"].ToString(),
                                        Descr = reader["vtm_descr"].ToString(),
                                        DataManut = dataFormatadaParaUI,
                                        Km = reader["vtm_km"].ToString(),
                                        Oleo = Convert.ToBoolean(reader["vtm_oleo"]),
                                        FiltroOleo = Convert.ToBoolean(reader["vtm_filtrooleo"]),
                                        FiltroAr = Convert.ToBoolean(reader["vtm_filtroar"]),
                                        Efetuado = reader["vtm_efetuado"].ToString(),
                                        KmProximo = reader["vtm_kmproxima"].ToString(),
                                        Status = reader["status_descr"].ToString(),
                                    });
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Data inválida!");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                }
            }
            ListManutViaturas.ItemsSource = manutViaturas;
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void LoadDet()
        {
            ObservableCollection<Cls_0801_ManutViaturasComponents> manutViaturasComponents = new ObservableCollection<Cls_0801_ManutViaturasComponents>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT vmm_id, vmm_mid, vmm_refmanut, vmm_id_linha, vmm_id_debdet, vmm_quantidade, vmm_status, md_codartigo, md_precofinal, art_descr " +
                        "FROM tbl_0902_viaturasmatmanut " +
                        "LEFT JOIN tbl_0302_movimentosdebito_det ON tbl_0902_viaturasmatmanut.vmm_id_debdet = tbl_0302_movimentosdebito_det.md_id " +
                        "LEFT JOIN tbl_0207_artigos ON tbl_0302_movimentosdebito_det.md_codartigo = tbl_0207_artigos.art_codigo " +
                        "LEFT JOIN tbl_0001_status ON tbl_0902_viaturasmatmanut.vmm_status = tbl_0001_status.status_cod " +
                        "WHERE vmm_refmanut = ? " +
                        "ORDER BY vmm_id_linha";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@RefManut", selectedManutRef);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                manutViaturasComponents.Add(new Cls_0801_ManutViaturasComponents
                                {
                                    Id = Convert.ToInt32(reader["vmm_id"]),
                                    Ref = Convert.ToInt32(reader["vmm_mid"]),
                                    CodArtigo = reader["md_codartigo"].ToString(),
                                    Descr = reader["art_descr"].ToString(),
                                    Quant = Convert.ToDecimal(reader["vmm_quantidade"]),
                                    PrcUnit = Convert.ToDecimal(reader["md_precofinal"]),
                                    PrcTotal = Math.Round(Convert.ToDecimal(reader["vmm_quantidade"]) * Convert.ToDecimal(reader["md_precofinal"]), 2),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                }
            }
            Lst_ManutViaturasDet.ItemsSource = manutViaturasComponents;
        }

        private void ListManutViaturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListManutViaturas.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0801_ManutViaturas)ListManutViaturas.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedManutId = selectedItem.Id;
                selectedManutRef = selectedItem.Ref;
                selectedManutIndex = ListManutViaturas.Items.IndexOf(selectedItem);
                LoadDet();
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedManutId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListManutViaturas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListManutViaturas.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M25V0202", user.MenuAccess))
                    {
                        Btn_Edit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_Edit_Click(sender, e);
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_080101_RegManutViaturasAdd reg = new Frm_080101_RegManutViaturasAdd(loginUserId)
            {
                Owner = this
            };
            reg.ShowDialog();
            LoadManut();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_080102_RegManutViaturasEdit reg = new Frm_080102_RegManutViaturasEdit(loginUserId, selectedManutId)
            {
                Owner = this
            };
            reg.ShowDialog();
            LoadManut();
            ListManutViaturas.SelectedIndex = selectedManutIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_080103_RegManutViaturasDelete reg = new Frm_080103_RegManutViaturasDelete(selectedManutId)
            {
                Owner = this,
            };
            reg.ShowDialog();
            LoadManut();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
