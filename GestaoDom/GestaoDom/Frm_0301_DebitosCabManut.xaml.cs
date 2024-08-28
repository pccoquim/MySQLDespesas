/*
Frm_0301_DebitosCabManut.xaml.cs
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
    /// Interaction logic for Frm_0301_DebitosCabManut.xaml
    /// </summary>
    public partial class Frm_0301_DebitosCabManut : Window
    {
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private int selectedDebCabId, selectedDebCabIndex, selectedDebCabRef;
        public Frm_0301_DebitosCabManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();

            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadDebitosCab();
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar 
                if (Cls_0005_AccessControl.AccessGranted("M25D0101", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar 
                if (Cls_0005_AccessControl.AccessGranted("M25D0102", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar 
                if (Cls_0005_AccessControl.AccessGranted("M25D0103", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadDebitosCab()
        {
            ObservableCollection<Cls_0301_DebitosCab> debitosCab = new ObservableCollection<Cls_0301_DebitosCab>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT fd_id, fd_id_fatura, fd_codterc, fd_numdoc, fd_datadoc, fd_conta, fd_valor, fd_status, terc_descr, cntcred_descr, status_descr " +
                        "FROM tbl_0301_movimentosdebito " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0301_movimentosdebito.fd_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0103_contascred ON tbl_0301_movimentosdebito.fd_conta = tbl_0103_contascred.cntcred_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0301_movimentosdebito.fd_status = tbl_0001_status.status_cod " +
                        "ORDER BY fd_id_fatura DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dataDB = reader["fd_datadoc"].ToString();

                                if (DateTime.TryParseExact(dataDB, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                                {
                                    string dataFormatadaParaUI = dataFormatada.ToString("dd.MM.yyyy");

                                    debitosCab.Add(new Cls_0301_DebitosCab
                                    {
                                        Id = Convert.ToInt32(reader["fd_id"]),
                                        Ref = Convert.ToInt32(reader["fd_id_fatura"]),
                                        DescrTerc = reader["terc_descr"].ToString(),
                                        NumDoc = reader["fd_numdoc"].ToString(),
                                        Data = dataFormatadaParaUI, // Aqui, definimos a data formatada para a propriedade "Data".
                                        DescrConta = reader["cntcred_descr"].ToString(),
                                        Valor = Math.Round(Convert.ToDecimal(reader["fd_valor"]), 2),
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
                    MessageBox.Show("Erro ao carregar dados (C1074) " + ex.Message);
                }
            }
            ListDebitosCab.ItemsSource = debitosCab;
        }

        private void LoadDebitosDet()
        {
            ObservableCollection<Cls_030201_DebitosDetLinha> debitosDet = new ObservableCollection<Cls_030201_DebitosDetLinha>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT md_id, md_id_fatura, md_id_linha, md_codartigo, md_codiva, md_quantidade, md_precobase, md_desconto1, md_desconto2, md_valordesconto, md_precofinal, md_status, art_descr, uni_descr, iva_taxa, status_descr " +
                        "FROM tbl_0302_movimentosdebito_det " +
                        "LEFT JOIN tbl_0207_artigos ON tbl_0302_movimentosdebito_det.md_codartigo = tbl_0207_artigos.art_codigo " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                        "LEFT JOIN tbl_0206_taxasiva ON tbl_0302_movimentosdebito_det.md_codiva = tbl_0206_taxasiva.iva_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0302_movimentosdebito_det.md_status = tbl_0001_status.status_cod " +
                        "WHERE md_id_fatura = ? " +
                        "ORDER BY md_id_linha";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idfatura", selectedDebCabRef);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                debitosDet.Add(new Cls_030201_DebitosDetLinha
                                {
                                    Linha = Convert.ToInt32(reader["md_id_linha"]),
                                    Codigo = reader["md_codartigo"].ToString(),
                                    Descr = reader["art_descr"].ToString(),
                                    Unidade = reader["uni_descr"].ToString(),
                                    Quantidade = Convert.ToDecimal(reader["md_quantidade"]),
                                    PrecoUnitario = Convert.ToDecimal(reader["md_precobase"]),
                                    Desconto1 = Convert.ToDecimal(reader["md_desconto1"]),
                                    Desconto2 = Convert.ToDecimal(reader["md_desconto2"]),
                                    ValorDesconto = Convert.ToDecimal(reader["md_valordesconto"]),
                                    PrecoFinalUnitario = Convert.ToDecimal(reader["md_precofinal"]),
                                    ValorTotal = Math.Round(Convert.ToDecimal(reader["md_quantidade"]) * Convert.ToDecimal(reader["md_precofinal"]), 2),
                                    TaxaIVA = reader["iva_taxa"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1082) " + ex.Message);
                }
            }
            ListDebitosDet.ItemsSource = debitosDet;
        }

        private void ListDebitosCab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListDebitosCab.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0301_DebitosCab)ListDebitosCab.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedDebCabId = selectedItem.Id;
                selectedDebCabRef = selectedItem.Ref;
                selectedDebCabIndex = ListDebitosCab.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
                LoadDebitosDet();
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedDebCabId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListDebitosCab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListDebitosCab.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M25D0102", user.MenuAccess))
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
            Frm_030101_DebitoCabAdd debitoCabAdd = new Frm_030101_DebitoCabAdd(loginUserSequence, loginUserId, loginUserType)
            {
                Owner = this
            };
            debitoCabAdd.ShowDialog();
            LoadDebitosCab();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_030102_DebitoCabEdit debitosCabEdit = new Frm_030102_DebitoCabEdit(selectedDebCabRef, loginUserId, selectedDebCabId)
            {
                Owner = this
            };
            debitosCabEdit.ShowDialog();
            LoadDebitosCab();
            ListDebitosCab.SelectedIndex = selectedDebCabIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_030103_DebitoCabDelete frm_030103_DebitoCabDelete = new Frm_030103_DebitoCabDelete(selectedDebCabRef, selectedDebCabId)
            {
                Owner = this
            };
            frm_030103_DebitoCabDelete.ShowDialog();
            LoadDebitosCab();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
