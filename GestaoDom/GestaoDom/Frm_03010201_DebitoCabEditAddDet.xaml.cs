/*
Frm_03010201_DebitoCabEditAddDet.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_03010201_DebitoCabEditAddDet.xaml
    /// </summary>
    public partial class Frm_03010201_DebitoCabEditAddDet : Window
    {
        private readonly string loginUserId, linhaDebCabCodTerc;
        private readonly int selectedDebCabRef, selectedDebCabId;
        private string codigo = "", descr = "", unidade = "";
        private int idIndex = -1;
        private int debDetId;
        public Frm_03010201_DebitoCabEditAddDet(string loginUserId, string linhaDebCabCodTerc, int selectedDebCabRef, int selectedDebCabId)
        {
            InitializeComponent();
            Btn_AddLinha.Visibility = Visibility.Collapsed;
            LoadArtigos();
            this.linhaDebCabCodTerc = linhaDebCabCodTerc;
            this.loginUserId = loginUserId;
            this.selectedDebCabRef = selectedDebCabRef;
            this.selectedDebCabId = selectedDebCabId;
        }

        private void LoadArtigos()
        {
            // ListView
            ObservableCollection<Cls_0302_DebitosDet> debitosDet = new ObservableCollection<Cls_0302_DebitosDet>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT art_id, art_codigo, art_descr, art_status, uni_descr, status_descr " +
                        "FROM tbl_0207_artigos " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                        "WHERE art_codterc = ? AND art_status = 1 " +
                        "ORDER BY art_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codterc", linhaDebCabCodTerc);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                debitosDet.Add(new Cls_0302_DebitosDet
                                {
                                    Id = Convert.ToInt32(reader["art_id"]),
                                    Cod = reader["art_codigo"].ToString(),
                                    Artigo = reader["art_descr"].ToString(),
                                    Unidade = reader["uni_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1079) " + ex.Message);
                }
            }
            ListArtigos.ItemsSource = debitosDet;
        }

        private void ListArtigos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListArtigos.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0302_DebitosDet)ListArtigos.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                codigo = selectedItem.Cod;
                descr = selectedItem.Artigo;
                unidade = selectedItem.Unidade;
                idIndex = ListArtigos.Items.IndexOf(selectedItem);
                debDetId = selectedItem.Id;
                Btn_AddLinha.Visibility = Visibility.Visible;
            }
            else
            {
                Btn_AddLinha.Visibility = Visibility.Collapsed;
                // No item selected, reset the user_ID variable
                idIndex = -1;
            }
        }

        private void ListArtigos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListArtigos.SelectedItem != null)
            {
                Btn_AddLinha_Click(sender, e);
            }
        }

        private void Btn_AddLinha_Click(object sender, RoutedEventArgs e)
        {
            Frm_0301020101_DebitoCabEditAddDet frm_0301020101_DebitoCabEditAddDet = new Frm_0301020101_DebitoCabEditAddDet(loginUserId, selectedDebCabRef, selectedDebCabId, codigo, descr, unidade)
            {
                Owner = this
            };
            frm_0301020101_DebitoCabEditAddDet.ShowDialog();
            this.Close();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
