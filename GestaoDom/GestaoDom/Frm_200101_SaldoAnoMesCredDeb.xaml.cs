/*
Frm_200101_SaldoAnoMesCredDeb.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.IO;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_200101_SaldoAnoMesCredDeb.xaml
    /// </summary>
    public partial class Frm_200101_SaldoAnoMesCredDeb : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private string ano;
        readonly DateTime dataPedido = DateTime.Now;

        public Frm_200101_SaldoAnoMesCredDeb(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();

            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M28A0201", user.MenuAccess))
                {
                    Btn_Print.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Print.Visibility = Visibility.Collapsed;
                }

            }
        }

        private void Txt_Ano_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Txt_Ano.Text.Length == 4)
            {
                Btn_Print.IsEnabled = true;
            }
            else
            {
                Btn_Print.IsEnabled = false;
            }
        }


        private void Btn_Print_Click(object sender, RoutedEventArgs e)
        {
            QuerySaldo();
        }

        private void QuerySaldo()
        {
            ano = Txt_Ano.Text;
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyyMMddHHmmss") + ".txt";
            string filePath = @"c:\Despesas\Relatórios\SaldoAnoMes_" + ano + "_" + formattedDate;
            //string symbol = "€";
            using (StreamWriter outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine($"{"Relatório de saldos em valor por ano/mês",-100}{" " + dataPedido.ToString("dd/MM/yyyy"),146}");
                outStream.WriteLine($"{" ",-100}{" " + dataPedido.ToString("HH:mm:ss"),146}");
                outStream.WriteLine($"{"Ano: " + ano,-100}{" " + loginUserId,146}");

                var columns = new[] {
                new { Width = 15, Header = "Movimentos" },
                new { Width = 12, Header = "    JAN" },
                new { Width = 12, Header = "    FEV" },
                new { Width = 12, Header = "    MAR" },
                new { Width = 12, Header = "    ABR" },
                new { Width = 12, Header = "    MAI" },
                new { Width = 12, Header = "    JUN" },
                new { Width = 12, Header = "    JUL" },
                new { Width = 12, Header = "    AGO" },
                new { Width = 12, Header = "    SET" },
                new { Width = 12, Header = "    OUT" },
                new { Width = 12, Header = "    NOV" },
                new { Width = 12, Header = "    DEZ" },
                new { Width = 12, Header = "   TOTAL" }
            };

                foreach (var col in columns)
                {
                    outStream.Write(new string('-', col.Width) + "-+-");
                }
                outStream.WriteLine();

                foreach (var col in columns)
                {
                    outStream.Write(col.Header.PadRight(col.Width) + " | ");
                }
                outStream.WriteLine();

                foreach (var col in columns)
                {
                    outStream.Write(new string('-', col.Width) + "-+-");
                }
                outStream.WriteLine();

                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '01' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita<> 0) AS CM01, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '01' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM01, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '02' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM02, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '02' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM02, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '03' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM03, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '03' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM03, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '04' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM04, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '04' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM04, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '05' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM05, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '05' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM05, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '06' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM06, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '06' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM06, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '07' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM07, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '07' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM07, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '08' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM08, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '08' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM08, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '09' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM09, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '09' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM09, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '10' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM10, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '10' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM10, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '11' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM11, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '11' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM11, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) = '12' THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CM12, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '12' THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DM12, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(mc_datamov, 5, CHAR_LENGTH(mc_datamov) - 6) THEN mc_valor ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0402_movimentoscredito WHERE LEFT(mc_datamov, 4) = '" + ano + "' AND mc_codtiporeceita <> 0) AS CMTOTAL, " +
                                             "(SELECT CAST(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) THEN md_quantidade * md_precofinal ELSE 0 END) AS DECIMAL(12, 2)) FROM tbl_0302_movimentosdebito_det LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura WHERE LEFT(fd_datadoc, 4) = '" + ano + "') AS DMTOTAL ";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string cm01 = reader.GetString("CM01");
                                    string dm01 = reader.GetString("DM01");
                                    string s01 = Convert.ToString(reader.GetDecimal("CM01") - reader.GetDecimal("DM01"));
                                    string cm02 = reader.GetString("CM02");
                                    string dm02 = reader.GetString("DM02");
                                    string s02 = Convert.ToString(reader.GetDecimal("CM02") - reader.GetDecimal("DM02"));
                                    string cm03 = reader.GetString("CM03");
                                    string dm03 = reader.GetString("DM03");
                                    string s03 = Convert.ToString(reader.GetDecimal("CM03") - reader.GetDecimal("DM03"));
                                    string cm04 = reader.GetString("CM04");
                                    string dm04 = reader.GetString("DM04");
                                    string s04 = Convert.ToString(reader.GetDecimal("CM04") - reader.GetDecimal("DM04"));
                                    string cm05 = reader.GetString("CM05");
                                    string dm05 = reader.GetString("DM05");
                                    string s05 = Convert.ToString(reader.GetDecimal("CM05") - reader.GetDecimal("DM05"));
                                    string cm06 = reader.GetString("CM06");
                                    string dm06 = reader.GetString("DM06");
                                    string s06 = Convert.ToString(reader.GetDecimal("CM06") - reader.GetDecimal("DM06"));
                                    string cm07 = reader.GetString("CM07");
                                    string dm07 = reader.GetString("DM07");
                                    string s07 = Convert.ToString(reader.GetDecimal("CM07") - reader.GetDecimal("DM07"));
                                    string cm08 = reader.GetString("CM08");
                                    string dm08 = reader.GetString("DM08");
                                    string s08 = Convert.ToString(reader.GetDecimal("CM08") - reader.GetDecimal("DM08"));
                                    string cm09 = reader.GetString("CM09");
                                    string dm09 = reader.GetString("DM09");
                                    string s09 = Convert.ToString(reader.GetDecimal("CM09") - reader.GetDecimal("DM09"));
                                    string cm10 = reader.GetString("CM10");
                                    string dm10 = reader.GetString("DM10");
                                    string s10 = Convert.ToString(reader.GetDecimal("CM10") - reader.GetDecimal("DM10"));
                                    string cm11 = reader.GetString("CM11");
                                    string dm11 = reader.GetString("DM11");
                                    string s11 = Convert.ToString(reader.GetDecimal("CM11") - reader.GetDecimal("DM11"));
                                    string cm12 = reader.GetString("CM12");
                                    string dm12 = reader.GetString("DM12");
                                    string s12 = Convert.ToString(reader.GetDecimal("CM12") - reader.GetDecimal("DM12"));
                                    string ctotal = reader.GetString("CMTOTAL");
                                    string dtotal = reader.GetString("DMTOTAL");
                                    string stotal = Convert.ToString(reader.GetDecimal("CMTOTAL") - reader.GetDecimal("DMTOTAL"));

                                    string fcm01 = cm01 + " €";
                                    string fdm01 = dm01 + " €";
                                    string fs01 = s01 + " €";
                                    string fcm02 = cm02 + " €";
                                    string fdm02 = dm02 + " €";
                                    string fs02 = s02 + " €";
                                    string fcm03 = cm03 + " €";
                                    string fdm03 = dm03 + " €";
                                    string fs03 = s03 + " €";
                                    string fcm04 = cm04 + " €";
                                    string fdm04 = dm04 + " €";
                                    string fs04 = s04 + " €";
                                    string fcm05 = cm05 + " €";
                                    string fdm05 = dm05 + " €";
                                    string fs05 = s05 + " €";
                                    string fcm06 = cm06 + " €";
                                    string fdm06 = dm06 + " €";
                                    string fs06 = s06 + " €";
                                    string fcm07 = cm07 + " €";
                                    string fdm07 = dm07 + " €";
                                    string fs07 = s07 + " €";
                                    string fcm08 = cm08 + " €";
                                    string fdm08 = dm08 + " €";
                                    string fs08 = s08 + " €";
                                    string fcm09 = cm09 + " €";
                                    string fdm09 = dm09 + " €";
                                    string fs09 = s09 + " €";
                                    string fcm10 = cm10 + " €";
                                    string fdm10 = dm10 + " €";
                                    string fs10 = s10 + " €";
                                    string fcm11 = cm11 + " €";
                                    string fdm11 = dm11 + " €";
                                    string fs11 = s11 + " €";
                                    string fcm12 = cm12 + " €";
                                    string fdm12 = dm12 + " €";
                                    string fs12 = s12 + " €";
                                    string fctotal = ctotal + " €";
                                    string fdtotal = dtotal + " €";
                                    string fstotal = stotal + " €";

                                    // Formatação para saída no arquivo
                                    outStream.Write("Crédito".PadRight(columns[0].Width) + " | ");
                                    outStream.Write(fcm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(fcm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(fcm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(fcm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(fcm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(fcm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(fcm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(fcm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(fcm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(fcm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(fcm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(fcm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(fctotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();

                                    outStream.Write("Débito".PadRight(columns[0].Width) + " | ");
                                    outStream.Write(fdm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(fdm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(fdm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(fdm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(fdm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(fdm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(fdm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(fdm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(fdm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(fdm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(fdm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(fdm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(fdtotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();

                                    foreach (var col in columns)
                                    {
                                        outStream.Write(new string('-', col.Width) + "-+-");
                                    }
                                    outStream.WriteLine();

                                    outStream.Write("Saldo".PadRight(columns[0].Width) + " | ");
                                    outStream.Write(fs01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(fs02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(fs03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(fs04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(fs05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(fs06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(fs07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(fs08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(fs09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(fs10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(fs11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(fs12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(fstotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                    }
                    foreach (var col in columns)
                    {
                        outStream.Write(new string('-', col.Width) + "-+-");
                    }
                    outStream.WriteLine();
                }
            }
            System.Windows.MessageBox.Show("Relatório concluido com sucesso!");
            this.Close();
        }
    }
}
