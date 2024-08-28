/*
Frm_200003_RelatorioSubFamAno.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_200003_RelatorioSubFamAno.xaml
    /// </summary>
    public partial class Frm_200003_RelatorioSubFamAno : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private string ano;
        readonly DateTime dataPedido = DateTime.Now;
        public Frm_200003_RelatorioSubFamAno(int loginUserSequence, string loginUserId, string loginUserType)
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
                if (Cls_0005_AccessControl.AccessGranted("M28A0103", user.MenuAccess))
                {
                    Btn_PrintValue.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_PrintValue.Visibility = Visibility.Collapsed;
                }

            }
        }

        private void Txt_Ano_KeyUp(object sender, KeyEventArgs e)
        {
            if (Txt_Ano.Text.Length == 4)
            {
                Btn_PrintValue.IsEnabled = true;
                Btn_PrintQuantity.IsEnabled = true;
            }
            else
            {
                Btn_PrintValue.IsEnabled = false;
                Btn_PrintQuantity.IsEnabled = false;
            }
        }

        private void Btn_PrintValue_Click(object sender, RoutedEventArgs e)
        {
            QueryValue();
        }

        private void Btn_PrintQuantity_Click(object sender, RoutedEventArgs e)
        {
            QueryQuantity();
        }

        private void QueryValue()
        {
            ano = Txt_Ano.Text;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string create = "CREATE TABLE tmp " +
                        "SELECT LEFT(md_codartigo, 5) AS cod, fd_datadoc, md_quantidade, md_precofinal " +
                        "FROM tbl_0302_movimentosdebito_det " +
                        "LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura " +
                        "WHERE LEFT(fd_datadoc, 4) = ? ";
                    using (MySqlCommand cmd = new MySqlCommand(create, conn))
                    {
                        // Atribuição de variaveis
                        cmd.Parameters.AddWithValue("@ano", ano);
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                }
            }

            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyyMMddHHmmss") + ".txt";
            string filePath = @"c:\Despesas\Relatórios\SfmAnoMesValor_" + ano + "_" + formattedDate;
            using (StreamWriter outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine($"{"Relatório de gastos em valor por subfamilia/ano/mês",-100}{" " + dataPedido.ToString("dd/MM/yyyy"),146}");
                outStream.WriteLine($"{" ",-100}{" " + dataPedido.ToString("HH:mm:ss"),146}");
                outStream.WriteLine($"{"Ano: " + ano,-100}{" " + loginUserId,146}");

                var columns = new[] {
                new { Width = 50, Header = "Subfamilia" },
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
                        string query = "SELECT sfm_descr, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '01' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM01, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '02' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM02, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '03' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM03, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '04' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM04, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '05' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM05, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '06' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM06, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '07' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM07, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '08' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM08, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '09' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM09, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '10' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM10, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '11' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM11, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '12' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VM12, " +
                            "FORMAT(SUM(md_precofinal * md_quantidade), 2, 12) AS VMTOTAL " +
                            "FROM tmp " +
                            "LEFT JOIN tbl_0202_subfamilias ON tmp.cod = tbl_0202_subfamilias.sfm_codigo " +
                            "GROUP BY sfm_descr " +
                            "ORDER BY sfm_descr";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string md_sfmdescr = reader.GetString("sfm_descr");
                                    string md_vm01 = reader.GetString("VM01");
                                    string md_vm02 = reader.GetString("VM02");
                                    string md_vm03 = reader.GetString("VM03");
                                    string md_vm04 = reader.GetString("VM04");
                                    string md_vm05 = reader.GetString("VM05");
                                    string md_vm06 = reader.GetString("VM06");
                                    string md_vm07 = reader.GetString("VM07");
                                    string md_vm08 = reader.GetString("VM08");
                                    string md_vm09 = reader.GetString("VM09");
                                    string md_vm10 = reader.GetString("VM10");
                                    string md_vm11 = reader.GetString("VM11");
                                    string md_vm12 = reader.GetString("VM12");
                                    string md_vtotal = reader.GetString("VMTOTAL");

                                    string formatted_vm01 = md_vm01 + " €";
                                    string formatted_vm02 = md_vm02 + " €";
                                    string formatted_vm03 = md_vm03 + " €";
                                    string formatted_vm04 = md_vm04 + " €";
                                    string formatted_vm05 = md_vm05 + " €";
                                    string formatted_vm06 = md_vm06 + " €";
                                    string formatted_vm07 = md_vm07 + " €";
                                    string formatted_vm08 = md_vm08 + " €";
                                    string formatted_vm09 = md_vm09 + " €";
                                    string formatted_vm10 = md_vm10 + " €";
                                    string formatted_vm11 = md_vm11 + " €";
                                    string formatted_vm12 = md_vm12 + " €";
                                    string formatted_vtotal = md_vtotal + " €";

                                    // Formatação para saída no arquivo
                                    outStream.Write(md_sfmdescr.PadRight(columns[0].Width) + " | ");
                                    outStream.Write(formatted_vm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(formatted_vm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(formatted_vm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(formatted_vm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(formatted_vm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(formatted_vm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(formatted_vm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(formatted_vm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(formatted_vm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(formatted_vm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(formatted_vm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(formatted_vm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(formatted_vtotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                    }
                }

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
                        string query = "SELECT FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '01' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM01, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '02' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM02, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '03' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM03, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '04' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM04, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '05' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM05, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '06' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM06, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '07' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM07, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '08' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM08, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '09' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM09, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '10' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM10, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '11' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM11, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '12' THEN md_precofinal * md_quantidade ELSE 0 END), 2, 12) AS VTM12, " +
                                                "FORMAT(SUM(md_precofinal * md_quantidade), 2, 12) AS VTMTOTAL " +
                                                "FROM tmp " +
                                                "LEFT JOIN tbl_0202_subfamilias ON tmp.cod = tbl_0202_subfamilias.sfm_codigo ";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string md_vtm01 = reader.GetString("VTM01");
                                    string md_vtm02 = reader.GetString("VTM02");
                                    string md_vtm03 = reader.GetString("VTM03");
                                    string md_vtm04 = reader.GetString("VTM04");
                                    string md_vtm05 = reader.GetString("VTM05");
                                    string md_vtm06 = reader.GetString("VTM06");
                                    string md_vtm07 = reader.GetString("VTM07");
                                    string md_vtm08 = reader.GetString("VTM08");
                                    string md_vtm09 = reader.GetString("VTM09");
                                    string md_vtm10 = reader.GetString("VTM10");
                                    string md_vtm11 = reader.GetString("VTM11");
                                    string md_vtm12 = reader.GetString("VTM12");
                                    string md_vtmtotal = reader.GetString("VTMTOTAL");

                                    string formatted_vtm01 = md_vtm01 + " €";
                                    string formatted_vtm02 = md_vtm02 + " €";
                                    string formatted_vtm03 = md_vtm03 + " €";
                                    string formatted_vtm04 = md_vtm04 + " €";
                                    string formatted_vtm05 = md_vtm05 + " €";
                                    string formatted_vtm06 = md_vtm06 + " €";
                                    string formatted_vtm07 = md_vtm07 + " €";
                                    string formatted_vtm08 = md_vtm08 + " €";
                                    string formatted_vtm09 = md_vtm09 + " €";
                                    string formatted_vtm10 = md_vtm10 + " €";
                                    string formatted_vtm11 = md_vtm11 + " €";
                                    string formatted_vtm12 = md_vtm12 + " €";
                                    string formatted_vtmtotal = md_vtmtotal + " €";

                                    // Formatação para saída no arquivo
                                    outStream.Write("Total".PadRight(columns[0].Width) + " | ");
                                    outStream.Write(formatted_vtm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(formatted_vtm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(formatted_vtm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(formatted_vtm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(formatted_vtm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(formatted_vtm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(formatted_vtm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(formatted_vtm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(formatted_vtm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(formatted_vtm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(formatted_vtm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(formatted_vtm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(formatted_vtmtotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                    }
                    foreach (var col in columns)
                    {
                        outStream.Write(new string('-', col.Width) + "-+-");
                    }
                    outStream.WriteLine();
                }
            }

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string drop = "DROP TABLE tmp ";
                    using (MySqlCommand cmd = new MySqlCommand(drop, conn))
                    {
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao eliminar dados temporários" + ex.Message);
                }
            }

            System.Windows.MessageBox.Show("Relatório concluido com sucesso!");
            this.Close();
        }


        private void QueryQuantity()
        {
            ano = Txt_Ano.Text;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string create = "CREATE TABLE tmp " +
                         "SELECT LEFT(md_codartigo, 5) AS cod, fd_datadoc, md_quantidade " +
                         "FROM tbl_0302_movimentosdebito_det " +
                         "LEFT JOIN tbl_0301_movimentosdebito ON tbl_0302_movimentosdebito_det.md_id_fatura = tbl_0301_movimentosdebito.fd_id_fatura " +
                         "WHERE LEFT(fd_datadoc, 4) = ? ";
                    using (MySqlCommand cmd = new MySqlCommand(create, conn))
                    {
                        // Atribuição de variaveis
                        cmd.Parameters.AddWithValue("@ano", ano);
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                }
            }

            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyyMMddHHmmss") + ".txt";
            string filePath = @"c:\Despesas\Relatórios\SfmAnoMesQtd_" + ano + "_" + formattedDate;
            using (StreamWriter outStream = new StreamWriter(filePath))
            {
                outStream.WriteLine($"{"Relatório de gastos em quantidade por subfamilia/ano/mês",-100}{" " + dataPedido.ToString("dd/MM/yyyy"),146}");
                outStream.WriteLine($"{" ",-100}{" " + dataPedido.ToString("HH:mm:ss"),146}");
                outStream.WriteLine($"{"Ano: " + ano,-100}{" " + loginUserId,146}");

                var columns = new[] {
                new { Width = 50, Header = "Subfamilia" },
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
                        string query = "SELECT sfm_descr, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '01' THEN md_quantidade ELSE 0 END), 2, 12) AS QM01, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '02' THEN md_quantidade ELSE 0 END), 2, 12) AS QM02, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '03' THEN md_quantidade ELSE 0 END), 2, 12) AS QM03, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '04' THEN md_quantidade ELSE 0 END), 2, 12) AS QM04, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '05' THEN md_quantidade ELSE 0 END), 2, 12) AS QM05, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '06' THEN md_quantidade ELSE 0 END), 2, 12) AS QM06, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '07' THEN md_quantidade ELSE 0 END), 2, 12) AS QM07, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '08' THEN md_quantidade ELSE 0 END), 2, 12) AS QM08, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '09' THEN md_quantidade ELSE 0 END), 2, 12) AS QM09, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '10' THEN md_quantidade ELSE 0 END), 2, 12) AS QM10, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '11' THEN md_quantidade ELSE 0 END), 2, 12) AS QM11, " +
                            "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '12' THEN md_quantidade ELSE 0 END), 2, 12) AS QM12, " +
                            "FORMAT(SUM(md_quantidade), 2, 12) AS QMTOTAL " +
                            "FROM tmp " +
                            "LEFT JOIN tbl_0202_subfamilias ON tmp.cod = tbl_0202_subfamilias.sfm_codigo " +
                            "GROUP BY sfm_descr " +
                            "ORDER BY sfm_descr";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string md_sfmdescr = reader.GetString("sfm_descr");
                                    string md_qm01 = reader.GetString("QM01");
                                    string md_qm02 = reader.GetString("QM02");
                                    string md_qm03 = reader.GetString("QM03");
                                    string md_qm04 = reader.GetString("QM04");
                                    string md_qm05 = reader.GetString("QM05");
                                    string md_qm06 = reader.GetString("QM06");
                                    string md_qm07 = reader.GetString("QM07");
                                    string md_qm08 = reader.GetString("QM08");
                                    string md_qm09 = reader.GetString("QM09");
                                    string md_qm10 = reader.GetString("QM10");
                                    string md_qm11 = reader.GetString("QM11");
                                    string md_qm12 = reader.GetString("QM12");
                                    string md_qtotal = reader.GetString("QMTOTAL");

                                    // Formatação para saída no arquivo
                                    outStream.Write(md_sfmdescr.PadRight(columns[0].Width) + " | ");
                                    outStream.Write(md_qm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(md_qm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(md_qm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(md_qm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(md_qm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(md_qm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(md_qm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(md_qm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(md_qm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(md_qm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(md_qm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(md_qm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(md_qtotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                    }
                }

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
                        string query = "SELECT FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '01' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM01, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '02' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM02, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '03' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM03, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '04' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM04, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '05' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM05, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '06' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM06, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '07' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM07, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '08' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM08, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '09' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM09, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '10' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM10, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '11' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM11, " +
                                                "FORMAT(SUM(CASE WHEN SUBSTRING(fd_datadoc, 5, CHAR_LENGTH(fd_datadoc) - 6) = '12' THEN md_quantidade ELSE 0 END), 2, 12) AS QTM12, " +
                                                "FORMAT(SUM(md_quantidade), 2, 12) AS QTMTOTAL " +
                                                "FROM tmp " +
                                                "LEFT JOIN tbl_0202_subfamilias ON tmp.cod = tbl_0202_subfamilias.sfm_codigo ";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string md_qm01 = reader.GetString("QTM01");
                                    string md_qm02 = reader.GetString("QTM02");
                                    string md_qm03 = reader.GetString("QTM03");
                                    string md_qm04 = reader.GetString("QTM04");
                                    string md_qm05 = reader.GetString("QTM05");
                                    string md_qm06 = reader.GetString("QTM06");
                                    string md_qm07 = reader.GetString("QTM07");
                                    string md_qm08 = reader.GetString("QTM08");
                                    string md_qm09 = reader.GetString("QTM09");
                                    string md_qm10 = reader.GetString("QTM10");
                                    string md_qm11 = reader.GetString("QTM11");
                                    string md_qm12 = reader.GetString("QTM12");
                                    string md_qtotal = reader.GetString("QTMTOTAL");

                                    // Formatação para saída no arquivo
                                    outStream.Write("Total".PadRight(columns[0].Width) + " | ");
                                    outStream.Write(md_qm01.PadLeft(columns[1].Width) + " | ");
                                    outStream.Write(md_qm02.PadLeft(columns[2].Width) + " | ");
                                    outStream.Write(md_qm03.PadLeft(columns[3].Width) + " | ");
                                    outStream.Write(md_qm04.PadLeft(columns[4].Width) + " | ");
                                    outStream.Write(md_qm05.PadLeft(columns[5].Width) + " | ");
                                    outStream.Write(md_qm06.PadLeft(columns[6].Width) + " | ");
                                    outStream.Write(md_qm07.PadLeft(columns[7].Width) + " | ");
                                    outStream.Write(md_qm08.PadLeft(columns[8].Width) + " | ");
                                    outStream.Write(md_qm09.PadLeft(columns[9].Width) + " | ");
                                    outStream.Write(md_qm10.PadLeft(columns[10].Width) + " | ");
                                    outStream.Write(md_qm11.PadLeft(columns[11].Width) + " | ");
                                    outStream.Write(md_qm12.PadLeft(columns[12].Width) + " | ");
                                    outStream.Write(md_qtotal.PadLeft(columns[13].Width) + " | ");
                                    outStream.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar dados (C10) " + ex.Message);
                    }
                    foreach (var col in columns)
                    {
                        outStream.Write(new string('-', col.Width) + "-+-");
                    }
                    outStream.WriteLine();
                }
            }

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string drop = "DROP TABLE tmp ";
                    using (MySqlCommand cmd = new MySqlCommand(drop, conn))
                    {
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao eliminar dados temporários" + ex.Message);
                }
            }

            System.Windows.MessageBox.Show("Relatório concluido com sucesso!");
            this.Close();
        }
    }
}
