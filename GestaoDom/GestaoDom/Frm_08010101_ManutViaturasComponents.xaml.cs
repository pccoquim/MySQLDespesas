/*
Frm_08010101_ManutViariasComponents.xaml.cs
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
    /// Interaction logic for Frm_08010101_ManutViaturasComponents.xaml
    /// </summary>
    public partial class Frm_08010101_ManutViaturasComponents : Window
    {
        // Define a cultura desejada para exibição (cultura europeia)
        readonly CultureInfo euroCulture = new CultureInfo("pt-PT");
        // Variável para identificação do utilizador
        private readonly string loginUserId, viatura, data, descr, efetuado;
        private readonly int km, kmProxima;
        private readonly bool oleo, filtroOleo, filtroAr;
        private string viatDescr;
        private decimal valor = 0;

        public Frm_08010101_ManutViaturasComponents(string loginUserId, string viatura, string data, string descr, int km, bool oleo, bool filtroOleo, bool filtroAr, string efetuado, int kmProxima)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.viatura = viatura;
            this.data = data;
            this.descr = descr;
            this.km = km;
            this.oleo = oleo;
            this.filtroOleo = filtroOleo;
            this.filtroAr = filtroAr;
            this.efetuado = efetuado;
            this.kmProxima = kmProxima;
            LoadData();
            LoadComponents();
        }

        private void LoadData()
        {
            // Consulta de código da viatura para obtenção da descrição
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT vtr_cod, vtr_matricula, vtr_descr FROM tbl_0205_viaturas WHERE vtr_cod = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Definição da variavel para consulta
                        cmd.Parameters.AddWithValue("@Cod", viatura);
                        // Executa o comando e obtém o resultado
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                viatDescr = reader["vtr_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Viatura (Erro: C1081)!" + ex.Message);
                }
            }
            // Passar dados para respetivos destinos
            Txt_Viatura.Text = viatDescr;
            Txt_Data.Text = data;
            Txt_Descr.Text = descr;
            Txt_Km.Text = km.ToString();
            if (oleo == true)
            {
                Ckb_Oleo.IsChecked = true;
            }
            else
            {
                Ckb_Oleo.IsChecked = false;
            }
            if (filtroOleo == true)
            {
                Ckb_FiltroOleo.IsChecked = true;
            }
            else
            {
                Ckb_FiltroOleo.IsChecked = false;
            }
            if (filtroAr == true)
            {
                Ckb_FiltroAr.IsChecked = true;
            }
            else
            {
                Ckb_FiltroAr.IsChecked = false;
            }
            Txt_Efetuado.Text = efetuado;
            Txt_KmProxima.Text = kmProxima.ToString();
            // Formato personalizado para exibir o símbolo do euro
            string euroFormat = "C"; // "C" é o formato de moeda que inclui o símbolo do euro.

            // Formate o valor com a cultura e o formato personalizado
            string valorFormatado = valor.ToString(euroFormat, euroCulture);

            // Atribua o valor formatado à TextBox
            Txt_Valor.Text = valorFormatado;
        }

        private void LoadComponents()
        {
            // Lista para preenchimento da view
            ObservableCollection<Cls_080101_ManutViaturasComponents> components = new ObservableCollection<Cls_080101_ManutViaturasComponents>();
            // 
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    // Abre a ligação
                    conn.Open();
                    // Consulta
                    string query = "SELECT md_id, md_id_fatura, md_codartigo, md_quantidade,md_precofinal, art_descr, uni_descr " +
                        "FROM tbl_0302_movimentosdebito_det " +
                        "LEFT JOIN tbl_0207_artigos ON tbl_0302_movimentosdebito_det.md_codartigo = tbl_0207_artigos.art_codigo " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                        "WHERE md_status = '1' " +
                        "ORDER BY md_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                components.Add(new Cls_080101_ManutViaturasComponents
                                {
                                    Ref = Convert.ToInt32(reader["md_id"]),
                                    CodArtigo = reader["md_codartigo"].ToString(),
                                    Descr = reader["art_descr"].ToString(),
                                    Unidade = reader["uni_descr"].ToString(),
                                    Quant = Convert.ToDecimal(reader["md_quantidade"]),
                                    PrcUnit = Convert.ToDecimal(reader["md_precofinal"]),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados " + ex.Message);
                }
            }
            Lst_Artigos.ItemsSource = components;
        }
        /* Conversão da data
        string dataTextBox = Txt_Data.Text;
        string dataDB = "";

            if (DateTime.TryParseExact(dataTextBox, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
            {
                dataDB = dataFormatada.ToString("yyyyMMdd");
            }
            else
            {
                System.Windows.MessageBox.Show("A data está num formato incorreto!");
            }*/



        private void Lst_Manutencao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cbx_Terceiro_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void LstManutencao_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Lst_Components_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Finalize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
