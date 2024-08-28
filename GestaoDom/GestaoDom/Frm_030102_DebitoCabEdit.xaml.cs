/*
Frm_030102_DebitoCabEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_030102_DebitoCabEdit.xaml
    /// </summary>
    public partial class Frm_030102_DebitoCabEdit : Window
    {
        private readonly int selectedDebCabRef, selectedDebCabId;
        private readonly string loginUserId;
        private string linhaDebCabCodTerc;
        // Define a cultura desejada para exibição (cultura europeia)
        readonly CultureInfo euroCulture = new CultureInfo("pt-PT");

        private string nDoc = "", data = "", conta = "", status = "";
        private decimal valor = 0;
        private int id = 0;
        private int linhaId = 0, faturaId = 0, linhaNum = 0, kmi = 0, kmf = 0, kme = 0;
        private decimal quantidade = 0, precoBase = 0, desconto1 = 0, desconto2 = 0, valorDesconto = 0, valorUnitarioFinal = 0, valorTotal = 0;
        private bool combustivel = false;
        private string codArtigo = "", descrArtigo = "", descrUnidade = "", codIVA = "", viatura = "", codStatus;
        public Frm_030102_DebitoCabEdit(int selectedDebCabRef, string loginUserId, int selectedDebCabId)
        {
            InitializeComponent();
            this.selectedDebCabRef = selectedDebCabRef;
            this.loginUserId = loginUserId;
            this.selectedDebCabId = selectedDebCabId;
            LoadCbx_Conta();
            LoadCbx_Status();
            LoadCab();
            LoadDet();
            Btn_EditItems.Visibility = Visibility.Collapsed;
            Btn_DeleteItems.Visibility = Visibility.Collapsed;
        }

        private void LoadCbx_Conta()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT cntcred_cod, cntcred_descr FROM tbl_0103_contascred WHERE cntcred_status = 1 AND cntcred_cod != 0 ORDER BY cntcred_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_Conta.ItemsSource = dt.DefaultView;
                        Cbx_Conta.DisplayMemberPath = "cntcred_descr";
                        Cbx_Conta.SelectedValuePath = "cntcred_cod";
                        Cbx_Conta.SelectedIndex = -1;
                        Cbx_Conta.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da Cbx_Conta (Erro: C1083)!");
                }
            }
        }

        private void LoadCbx_Status()
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT status_cod, status_descr, status_cor FROM tbl_0001_status ORDER BY status_descr";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        // Limpa a combobox antes de adicionar os itens
                        Cbx_Status.Items.Clear();
                        Cbx_Status.ItemsSource = dt.DefaultView;
                        Cbx_Status.DisplayMemberPath = "status_descr";
                        Cbx_Status.SelectedValuePath = "status_cod";
                        Cbx_Status.SelectedIndex = -1;
                        Cbx_Status.IsEditable = false;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Status (Erro: C1084)!");
                }
            }
        }

        private void Cbx_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbx_Status.SelectedIndex == 0)
            {
                Cbx_Status.Foreground = Brushes.Green;
            }
            else if (Cbx_Status.SelectedIndex == 1)
            {
                Cbx_Status.Foreground = Brushes.Red;
            }
            else
            {
                Cbx_Status.Foreground = Brushes.Blue;
            }
        }

        private void LoadCab()
        {
            ObservableCollection<Cls_0301_DebitosCab> debitosCab = new ObservableCollection<Cls_0301_DebitosCab>();
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT fd_id, fd_id_fatura, fd_codterc, fd_numdoc, fd_datadoc, fd_conta, fd_valor, fd_status, terc_descr, cntcred_descr " +
                        "FROM tbl_0301_movimentosdebito " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0301_movimentosdebito.fd_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0103_contascred ON tbl_0301_movimentosdebito.fd_conta = tbl_0103_contascred.cntcred_cod " +
                        "WHERE fd_id_fatura = ? " +
                        "ORDER BY fd_id_fatura DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idfatura", selectedDebCabRef);
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
                                        CodTerc = reader["fd_codterc"].ToString(),
                                        DescrTerc = reader["terc_descr"].ToString(),
                                        NumDoc = reader["fd_numdoc"].ToString(),
                                        Data = dataFormatadaParaUI, // Aqui, definimos a data formatada para a propriedade "Data".
                                        CodConta = reader["fd_conta"].ToString(),
                                        Valor = Math.Round(Convert.ToDecimal(reader["fd_valor"]), 2),
                                        Status = reader["fd_status"].ToString(),
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
                    MessageBox.Show("Erro ao carregar dados (C1085)" + ex.Message);
                }
            }
            // Preenchimento dos campos
            if (debitosCab.Count > 0)
            {
                var linha = debitosCab[0]; // preencher com o primeiro item da lista.

                id = linha.Id;
                Txt_Ref.Text = linha.Ref.ToString();
                linhaDebCabCodTerc = linha.CodTerc;
                Txt_Terceiro.Text = linha.DescrTerc;
                Txt_NDoc.Text = linha.NumDoc;
                nDoc = linha.NumDoc;
                Txt_Data.Text = linha.Data;
                data = linha.Data;
                Cbx_Conta.SelectedValue = linha.CodConta;
                conta = linha.CodConta;
                // Valor a ser exibido
                valor = linha.Valor;

                // Formato personalizado para exibir o símbolo do euro
                string euroFormat = "C"; // "C" é o formato de moeda que inclui o símbolo do euro.

                // Formate o valor com a cultura e o formato personalizado
                string valorFormatado = valor.ToString(euroFormat, euroCulture);

                // Atribua o valor formatado à TextBox
                Txt_Valor.Text = valorFormatado;
                Cbx_Status.SelectedValue = linha.Status;
                status = linha.Status;
            }
        }

        private void LoadDet()
        {
            ObservableCollection<Cls_030201_DebitosDetLinha> debitosDet = new ObservableCollection<Cls_030201_DebitosDetLinha>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT md_id, md_id_fatura, md_id_linha, md_codartigo, md_codiva, md_quantidade, md_precobase, md_desconto1, md_desconto2, md_valordesconto, md_precofinal, md_combustivel, md_codviatura, md_kmi, md_kmf, md_kmefetuados, md_status, art_descr, uni_descr, iva_taxa, status_descr " +
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
                                    Id = Convert.ToInt32(reader["md_id"]),
                                    Fatura = Convert.ToInt32(reader["md_id_fatura"]),
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
                                    IVA = reader["md_codiva"].ToString(),
                                    TaxaIVA = reader["iva_taxa"].ToString(),
                                    Comb = Convert.ToBoolean(reader["md_combustivel"]),
                                    Viatura = reader["md_codviatura"].ToString(),
                                    KMI = Convert.ToInt32(reader["md_kmi"]),
                                    KMF = Convert.ToInt32(reader["md_kmf"]),
                                    KME = Convert.ToInt32(reader["md_kmefetuados"]),
                                    CodStatus = reader["md_status"].ToString(),
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

        private bool IsValidDate(string date)
        {
            // Verifica se a data é válida usando DateTime.TryParseExact.
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }

        public bool ValidarNDoc(string nDoc)
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
                    string query = "SELECT COUNT(*) FROM tbl_0301_movimentosdebito WHERE fd_codterc = ? AND fd_numdoc = ? AND fd_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codterc", linhaDebCabCodTerc);
                        cmd.Parameters.AddWithValue("@nDoc", nDoc);
                        cmd.Parameters.AddWithValue("@id", id);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1025)!" + ex.Message);

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

        private void ListDebitosDet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListDebitosDet.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_030201_DebitosDetLinha)ListDebitosDet.SelectedItem;
                // Obtem o valor (ID column) e guarda em variável
                linhaId = selectedItem.Id;
                faturaId = selectedItem.Fatura;
                linhaNum = selectedItem.Linha;
                codArtigo = selectedItem.Codigo;
                descrArtigo = selectedItem.Descr;
                descrUnidade = selectedItem.Unidade;
                quantidade = selectedItem.Quantidade;
                precoBase = selectedItem.PrecoUnitario;
                desconto1 = selectedItem.Desconto1;
                desconto2 = selectedItem.Desconto2;
                valorDesconto = selectedItem.ValorDesconto;
                valorUnitarioFinal = selectedItem.PrecoFinalUnitario;
                valorTotal = selectedItem.ValorTotal;
                codIVA = selectedItem.IVA;
                combustivel = selectedItem.Comb;
                viatura = selectedItem.Viatura;
                kmi = selectedItem.KMI;
                kmf = selectedItem.KMF;
                kme = selectedItem.KME;
                codStatus = selectedItem.CodStatus;
                status = selectedItem.Status;

                Btn_EditItems.Visibility = Visibility.Visible;
                Btn_DeleteItems.Visibility = Visibility.Visible;
            }
            else
            {
                Btn_EditItems.Visibility = Visibility.Collapsed;
                Btn_DeleteItems.Visibility = Visibility.Collapsed;
                linhaId = 0;
            }
        }

        private void ListDebitosDet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListDebitosDet.SelectedItem != null)
            {
                Btn_EditItems_Click(sender, e);
            }
        }

        private void Txt_NDoc_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_NDoc.Text.Trim() != Txt_NDoc.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do número de documento, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_NDoc_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValidDate(Txt_Data.Text))
            {
                MessageBox.Show("A data inserida não é válida. Use o formato dd/mm/yyyy.", "Erro de formato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Txt_Data_KeyDown(object sender, KeyEventArgs e)
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

        private void Txt_Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var text = textBox.Text.Replace(".", "");
                if (text.Length >= 2 && text.Length < 4)
                {
                    textBox.Text = text.Insert(2, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
                else if (text.Length >= 4)
                {
                    textBox.Text = text.Insert(2, ".").Insert(5, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
            }
        }

        private void Btn_AddItems_Click(object sender, RoutedEventArgs e)
        {
            Frm_03010201_DebitoCabEditAddDet debitosCabEditAddDet = new Frm_03010201_DebitoCabEditAddDet(loginUserId, linhaDebCabCodTerc, selectedDebCabRef, selectedDebCabId)
            {
                Owner = this
            };
            debitosCabEditAddDet.ShowDialog();
            LoadCab();
            LoadDet();
        }

        private void Btn_EditItems_Click(object sender, RoutedEventArgs e)
        {
            Frm_03010202_DebitoCabEditEditDet frm_03010202_DebitoCabEditEditDet = new Frm_03010202_DebitoCabEditEditDet(linhaId, faturaId, linhaNum, codArtigo, descrArtigo, descrUnidade, quantidade, precoBase, desconto1, desconto2, valorDesconto, valorUnitarioFinal, valorTotal, codIVA, combustivel, viatura, kmi, kmf, kme, codStatus, loginUserId, selectedDebCabId, selectedDebCabRef)
            {
                Owner = this
            };
            frm_03010202_DebitoCabEditEditDet.ShowDialog();
            LoadCab();
            LoadDet();
        }

        private void Btn_DeleteItems_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_NDoc.Text != "")
            {
                if (Txt_Data.Text != "")
                {
                    if (Txt_NDoc.Text != nDoc || Txt_Data.Text != data || Convert.ToString(Cbx_Conta.SelectedValue) != conta || Convert.ToString(Cbx_Status.SelectedValue) != status)
                    {
                        if (!ValidarNDoc(Txt_NDoc.Text))
                        {
                            string dataTextBox = Txt_Data.Text;
                            string dataDB = "";

                            if (DateTime.TryParseExact(dataTextBox, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                            {
                                dataDB = dataFormatada.ToString("yyyyMMdd");
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("A data está num formato incorreto!");
                            }
                            // Obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                // Creditar a conta anterior
                                try
                                {
                                    if (Convert.ToString(Cbx_Conta.SelectedValue) != conta)
                                    {
                                        string queryCreditarSaldoConta = " UPDATE tbl_0103_contascred SET cntcred_saldo += ? WHERE cntcred_cod = ?";
                                        using (MySqlCommand cmdCreditarSaldoConta = new MySqlCommand(queryCreditarSaldoConta, conn))
                                        {
                                            cmdCreditarSaldoConta.Parameters.AddWithValue("@saldo", valor);
                                            cmdCreditarSaldoConta.Parameters.AddWithValue("@conta", conta);
                                            cmdCreditarSaldoConta.ExecuteNonQuery();
                                        }
                                    }
                                    // Debitar a conta atual
                                    string queryDebitarSaldoConta = " UPDATE tbl_0103_contascred SET cntcred_saldo -= ? WHERE cntcred_cod = ?";
                                    using (MySqlCommand cmdDebitarSaldoConta = new MySqlCommand(queryDebitarSaldoConta, conn))
                                    {
                                        cmdDebitarSaldoConta.Parameters.AddWithValue("@saldo", valor);
                                        cmdDebitarSaldoConta.Parameters.AddWithValue("@conta", Cbx_Conta.SelectedValue);
                                        cmdDebitarSaldoConta.ExecuteNonQuery();
                                    }
                                    // Alteração de estado e atualização de saldos de conta
                                    string estado = Convert.ToString(Cbx_Status.SelectedValue);
                                    if (Convert.ToString(Cbx_Status.SelectedValue) != status)
                                    {
                                        // Estado alterado para ativo, debita da conta
                                        if (estado == "1")
                                        {
                                            string queryAlterarEstadoDet1 = " UPDATE tbl_0302_movimentosdebito_det SET md_status = ? WHERE md_id_fatura = ?";
                                            using (MySqlCommand cmdAlterarEstadoDet1 = new MySqlCommand(queryAlterarEstadoDet1, conn))
                                            {
                                                cmdAlterarEstadoDet1.Parameters.AddWithValue("@estado", estado);
                                                cmdAlterarEstadoDet1.Parameters.AddWithValue("@fatura", Txt_Ref.Text);
                                                cmdAlterarEstadoDet1.ExecuteNonQuery();
                                            }
                                            // Debitar no saldo de conta
                                            string queryAlterarSaldoAtivo1 = " UPDATE tbl_0103_contascred SET cntcred_saldo -= ? WHERE cntcred_cod = ?";
                                            using (MySqlCommand cmdAlterarSaldoAtivo1 = new MySqlCommand(queryAlterarSaldoAtivo1, conn))
                                            {
                                                cmdAlterarSaldoAtivo1.Parameters.AddWithValue("@saldo", valor);
                                                cmdAlterarSaldoAtivo1.Parameters.AddWithValue("@conta", Cbx_Conta.SelectedValue);
                                                cmdAlterarSaldoAtivo1.ExecuteNonQuery();
                                            }
                                        }
                                        // Estado alterado para inativo,credita da conta
                                        else if (estado == "2")
                                        {
                                            string queryAlterarEstadoDet2 = " UPDATE tbl_0302_movimentosdebito_det SET md_status = ? WHERE md_id_fatura = ?";
                                            using (MySqlCommand cmdAlterarEstadoDet2 = new MySqlCommand(queryAlterarEstadoDet2, conn))
                                            {
                                                cmdAlterarEstadoDet2.Parameters.AddWithValue("@estado", estado);
                                                cmdAlterarEstadoDet2.Parameters.AddWithValue("@fatura", Txt_Ref.Text);
                                                cmdAlterarEstadoDet2.ExecuteNonQuery();
                                            }
                                            // Creditar no saldo de conta
                                            string queryAlterarSaldoAtivo2 = " UPDATE tbl_0103_contascred SET cntcred_saldo += ? WHERE cntcred_cod = ?";
                                            using (MySqlCommand cmdAlterarSaldoAtivo2 = new MySqlCommand(queryAlterarSaldoAtivo2, conn))
                                            {
                                                cmdAlterarSaldoAtivo2.Parameters.AddWithValue("@saldo", valor);
                                                cmdAlterarSaldoAtivo2.Parameters.AddWithValue("@conta", Cbx_Conta.SelectedValue);
                                                cmdAlterarSaldoAtivo2.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    // String para alteração 
                                    string alterar = "UPDATE tbl_0301_movimentosdebito SET fd_numdoc = ?, fd_datadoc = ?, fd_conta = ?, fd_status = ?, fd_userlastchg = ?, fd_datelastchg = ?, fd_timelastchg = ? WHERE fd_id = ? ";
                                    // ligação com string e comando
                                    using (MySqlCommand cmd = new MySqlCommand(alterar, conn))
                                    {
                                        // Atribuição de variaveis com valores
                                        cmd.Parameters.AddWithValue("@nDoc", Txt_NDoc.Text);
                                        cmd.Parameters.AddWithValue("@data", dataDB);
                                        cmd.Parameters.AddWithValue("@conta", Cbx_Conta.SelectedValue);
                                        cmd.Parameters.AddWithValue("@status", Cbx_Status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@user", loginUserId);
                                        cmd.Parameters.AddWithValue("@data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@hora", Cls_0002_ActualDateTime.Time);
                                        // Atribuição de variavel de identificação para execução da alteração
                                        cmd.Parameters.AddWithValue("@id", id);
                                        // Executa a alteração
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Mensagem de alteração concluida com exito
                                    System.Windows.MessageBox.Show("Cabeçalho de movimento a débito alterado com sucesso!");
                                    // Fecha a janela
                                    this.Close();
                                }
                                catch (Exception)
                                {
                                    // Mensagem de erro de ligação
                                    System.Windows.MessageBox.Show("Ocorreu um erro ao efetuar a alteração (Erro: U1011)!");
                                }
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Por favor escolha outro número de documento! O número de documento utilizado já foi utilizado noutro registo.");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Não foram encontradas alterações para gravação");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Por favor, preencha o campo data! Este campo é de preenchimento obrigatório e não se encontra preenchido");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Por favor, preencha o campo nº documento! Este campo é de preenchimento obrigatório e não se encontra preenchido");
            }
        }
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}