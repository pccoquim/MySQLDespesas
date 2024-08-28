/*
Frm_040003_CreditoDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Globalization;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_040003_CreditoDelete.xaml
    /// </summary>
    public partial class Frm_040003_CreditoDelete : Window
    {
        private readonly int selectedCredId;
        private string CredID = "", NDoc = "", Data = "", CodTerc = "", DescrTerc = "", CodTipoReceita = "", DescrTipoReceita = "", CodContaDebito = "", DescrContaDebito = "", CodContaCredito = "", DescrContaCredito = "", Status = "", Valor = "";
        private bool Transf;
        public Frm_040003_CreditoDelete(int selectedCredId)
        {
            InitializeComponent();
            this.selectedCredId = selectedCredId;
            LoadCredito();
        }

        private void LoadCredito()
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT mc_id, mc_cred_id, mc_codterc, mc_numerodoc, mc_datamov, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status, terc_descr, tr_descr, cntcred_descr, cntdeb_descr, status_descr " +
                        "FROM tbl_0402_movimentoscredito " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0402_movimentoscredito.mc_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0104_tiporeceita ON tbl_0402_movimentoscredito.mc_codtiporeceita = tbl_0104_tiporeceita.tr_cod " +
                        "LEFT JOIN tbl_0103_contascred ON tbl_0402_movimentoscredito.mc_contacredito = tbl_0103_contascred.cntcred_cod " +
                        "LEFT JOIN tbl_0103_contasdeb ON tbl_0402_movimentoscredito.mc_contadebito = tbl_0103_contasdeb.cntdeb_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0402_movimentoscredito.mc_status = tbl_0001_status.status_cod " +
                        "WHERE mc_id = ? " +
                        "ORDER BY mc_cred_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@mcID", selectedCredId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                CredID = reader["mc_cred_id"].ToString();
                                NDoc = reader["mc_numerodoc"].ToString();
                                Data = reader["mc_datamov"].ToString();
                                CodTerc = reader["mc_codterc"].ToString();
                                DescrTerc = reader["terc_descr"].ToString();
                                CodTipoReceita = reader["mc_codtiporeceita"].ToString();
                                DescrTipoReceita = reader["tr_descr"].ToString();
                                CodContaDebito = reader["mc_contadebito"].ToString();
                                DescrContaDebito = reader["cntdeb_descr"].ToString();
                                CodContaCredito = reader["mc_contacredito"].ToString();
                                DescrContaCredito = reader["cntcred_descr"].ToString();
                                Valor = reader["mc_valor"].ToString();
                                Transf = Convert.ToBoolean(reader["mc_transf"]);
                                Status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1073)!" + ex.Message);
                }
                string dataDB = Data.ToString();

                if (DateTime.TryParseExact(dataDB, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                {
                    string dataFormatadaParaUI = dataFormatada.ToString("dd.MM.yyyy");

                    Txt_Data.Text = dataFormatadaParaUI; // Aqui, definimos a data formatada para a propriedade "Data".
                }
                else
                {
                    System.Windows.MessageBox.Show("Data inválida!");
                }
                Txt_Ref.Text = CredID;
                Txt_NDoc.Text = NDoc;
                Txt_CodTerc.Text = CodTerc;
                Txt_DescrTerc.Text = DescrTerc;
                Txt_CodTipoReceita.Text = CodTipoReceita;
                Txt_DescrTipoReceita.Text = DescrTipoReceita;
                Txt_CodContaDebito.Text = CodContaDebito;
                Txt_DescrContaDebito.Text = DescrContaDebito;
                Txt_CodContaCredito.Text = CodContaCredito;
                Txt_DescrContaCredito.Text += DescrContaCredito;
                if (Transf == true)
                {
                    this.Ckb_Transf.IsChecked = true;
                }
                else
                {
                    this.Ckb_Transf.IsChecked = false;
                }
                Txt_Valor.Text = Valor;
                Txt_Status.Text = Status;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        public decimal ValidarDebito(string debito)
        {
            decimal mov = Convert.ToDecimal(debito);
            decimal Saldo = 0;
            decimal valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT cntcred_saldo FROM tbl_0103_contascred WHERE cntcred_cod = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cntcred_cod", Txt_CodContaCredito.Text);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                Saldo = Convert.ToDecimal(reader["cntcred_saldo"]);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1024)!");

                }
                valor = Saldo - mov;

                return valor;
            }
        }

        private void Delete()
        {
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros update valor do saldo da conta débito
                try
                {
                    conn.Open();
                    // Definição do insert 
                    string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo + ? WHERE cntcred_cod = ?";
                    // Definição de query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variaveis
                        cmd.Parameters.AddWithValue("@Valor", Convert.ToDecimal(Txt_Valor.Text));
                        cmd.Parameters.AddWithValue("@ContaCredito", Txt_CodContaDebito.Text);
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1024)!" + ex.Message);
                    return;
                }
            }

            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros update valor do saldo da conta a crédito
                try
                {
                    conn.Open();
                    // Definição do insert 
                    string query = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
                    // Definição de query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variaveis
                        cmd.Parameters.AddWithValue("@Valor", Convert.ToDecimal(Txt_Valor.Text));
                        cmd.Parameters.AddWithValue("@ContaCredito", Txt_CodContaCredito.Text);
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro ao atualizar saldos (Erro: U1025)!" + ex.Message);
                    return;
                }
            }

            // Eliminação
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela terceiros
                    string query = "DELETE FROM tbl_0402_movimentoscredito WHERE mc_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@mcID", selectedCredId);
                        // Executa a consulta
                        cmd.ExecuteNonQuery();
                    }
                    // Fecha o formulário                            
                    this.Close();
                    System.Windows.MessageBox.Show("Movimento eliminado com sucesso!");
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1010)!");
                }
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDebito(Txt_Valor.Text) >= 0)
            {
                if (ShowConfirmation("Tem certeza que deseja eliminar este movimento?"))
                {
                    Delete();
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (ShowConfirmation("Anular este movimento provoca um saldo negativo de " + Convert.ToDecimal(ValidarDebito(Txt_Valor.Text)) + " na conta " + Txt_CodContaCredito + "-" + Txt_DescrContaCredito.Text + "! Deseja continuar?"))
                {
                    Delete();
                }
                else
                {
                    return;
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
