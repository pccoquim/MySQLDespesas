/*
Frm_0302_DebitosDetManut.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data de alteração: 24.06.2024
Versão: 1.1.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0302_DebitosDetManut.xaml
    /// </summary>
    public partial class Frm_0302_DebitosDetManut : Window
    {
        private readonly List<Cls_030201_DebitosDetLinha> cls_030201_DebitosDetLinhas = new List<Cls_030201_DebitosDetLinha>();
        private readonly MySqlConnection conn;
        private MySqlTransaction transaction;
        readonly CultureInfo euroCulture = new CultureInfo("pt-PT");
        private readonly string codTerc, nDoc, data, codConta, loginUserId, loginUserType;
        private string selectedDebDetArtCod, selectedDebDetArtDescr, selectedDebDetArtUni;
        private readonly int loginUserSequence;
        private int selectedDebDetArtId;
        private int selectedDebDetArtIndex;
        private string Ref = "";
        private int nLinha = 1, nILinha = 1;
        private decimal valorFatura = 0;
        private bool DDAdd, DDEdit, DDDelete, DDComb;
        private string DDCodigo, DDDescr, DDUnidade, DDIVA, DDTaxaIVA, DDViatura;
        private decimal DDQuantidade, DDPrecoUnitario, DDDesconto1, DDDesconto2, DDValorDesconto, DDPrecoFinalUnitario, DDValorTotal, DDDiferenca;
        private int DDKMI, DDKMF, DDKME, DDIdLinha;


        public Frm_0302_DebitosDetManut(string codTerc, string nDoc, string data, string codConta, int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            conn = Cls_0001_DBConnection.GetConnection();
            this.codTerc = codTerc;
            this.nDoc = nDoc;
            this.data = data;
            this.codConta = codConta;
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            txt_Data.Text = data;
            txt_NDoc.Text = nDoc;
            btn_Add.Visibility = System.Windows.Visibility.Collapsed;
            LoadCab();
        }

        public void LoadCab()
        {
            // Terceiro
            string DescrTerc = "";
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string queryTerc = "SELECT terc_cod, terc_descr FROM tbl_0102_terceiros WHERE terc_cod = ?";

                    using (MySqlCommand cmdTerc = new MySqlCommand(queryTerc, conn))
                    {
                        cmdTerc.Parameters.AddWithValue("@codterc", codTerc.ToString());
                        using (MySqlDataReader readerTerc = cmdTerc.ExecuteReader())
                        {

                            if (readerTerc.Read())
                            {
                                DescrTerc = readerTerc["terc_descr"].ToString();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Erro a carregar dados!");
                            }
                        }
                    }
                    txt_Terceiro.Text = DescrTerc;

                    // Conta
                    string DescrConta = "";
                    // defenição da consulta
                    string queryConta = "SELECT cntcred_cod, cntcred_descr FROM tbl_0103_contascred WHERE cntcred_cod = ?";

                    using (MySqlCommand cmdConta = new MySqlCommand(queryConta, conn))
                    {
                        cmdConta.Parameters.AddWithValue("@codcnt", codConta.ToString());
                        using (MySqlDataReader readerConta = cmdConta.ExecuteReader())
                        {

                            if (readerConta.Read())
                            {
                                DescrConta = readerConta["cntcred_descr"].ToString();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Erro a carregar dados!");
                            }
                        }
                    }
                    txt_Conta.Text = DescrConta;

                    // ListView
                    ObservableCollection<Cls_0302_DebitosDet> debitosDet = new ObservableCollection<Cls_0302_DebitosDet>();

                    string queryArt = "SELECT art_id, art_codigo, art_descr, art_status, uni_descr, status_descr " +
                        "FROM tbl_0207_artigos " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                        "WHERE art_codterc = ? AND art_status = ? " +
                        "ORDER BY art_descr";

                    using (MySqlCommand cmdArt = new MySqlCommand(queryArt, conn))
                    {
                        cmdArt.Parameters.AddWithValue("@codterc", codTerc.ToString());
                        cmdArt.Parameters.AddWithValue("@status", 1);
                        using (MySqlDataReader readerArt = cmdArt.ExecuteReader())
                        {
                            while (readerArt.Read())
                            {
                                debitosDet.Add(new Cls_0302_DebitosDet
                                {
                                    Id = Convert.ToInt32(readerArt["art_id"]),
                                    Cod = readerArt["art_codigo"].ToString(),
                                    Artigo = readerArt["art_descr"].ToString(),
                                    Unidade = readerArt["uni_descr"].ToString(),
                                });
                            }
                        }
                    }
                    ListArtigos.ItemsSource = debitosDet;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (C1079) " + ex.Message);
                }
            }
        }

        private void ListArtigos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListArtigos.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0302_DebitosDet)ListArtigos.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedDebDetArtCod = selectedItem.Cod;
                selectedDebDetArtDescr = selectedItem.Artigo;
                selectedDebDetArtUni = selectedItem.Unidade;
                selectedDebDetArtIndex = ListArtigos.Items.IndexOf(selectedItem);
                selectedDebDetArtId = selectedItem.Id;
                btn_Add.Visibility = Visibility.Visible;

            }
            else
            {
                btn_Add.Visibility = Visibility.Collapsed;

                // No item selected, reset the user_ID variable
                selectedDebDetArtId = -1;
            }
        }

        private void ListArtigos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListArtigos.SelectedItem != null)
            {
                Btn_Add_Click(sender, e);
            }
        }

        private void LoadLinhasDebito()
        {
            ListDebitosDet.ItemsSource = cls_030201_DebitosDetLinhas;
            // Formato personalizado para exibir o símbolo do euro
            string euroFormat = "C"; // "C" é o formato de moeda que inclui o símbolo do euro.

            // Formate o valor com a cultura e o formato personalizado
            string valorFormatado = valorFatura.ToString(euroFormat, euroCulture);

            // Atribua o valor formatado à TextBox
            txt_Valor.Text = valorFormatado;
            //txt_Valor.Text = valorFatura.ToString("0.00");
        }

        private void ListDebitosDet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListArtigos.SelectedItem != null)
            {
                btn_Add.Visibility = Visibility.Visible;
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                btn_Add.Visibility = Visibility.Collapsed;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void Numeracao()
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
                    string query = "SELECT MAX(fd_id_fatura) FROM tbl_0301_movimentosdebito";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1011)!");
                }
                Ref = (ultimoId + 1).ToString();
            }
        }

        private void InsertCab()
        {
            string dataTextBox = data;
            string dataDB = "";

            if (DateTime.TryParseExact(dataTextBox, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
            {
                dataDB = dataFormatada.ToString("yyyyMMdd");
            }
            else
            {
                System.Windows.MessageBox.Show("A data está num formato incorreto!");
            }
            string insertQuery = "INSERT INTO tbl_0301_movimentosdebito (fd_id_fatura, fd_codterc, fd_numdoc, fd_datadoc, fd_datalimitepag, fd_dataemissaodoc, fd_conta, fd_valor, fd_status, fd_usercreate, fd_datecreate, fd_timecreate, fd_userlastchg, fd_datelastchg, fd_timelastchg) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn, transaction);
            insertCommand.Parameters.AddWithValue("@id_fatura", Ref);
            insertCommand.Parameters.AddWithValue("@codterc", codTerc);
            insertCommand.Parameters.AddWithValue("@numdoc", nDoc);
            insertCommand.Parameters.AddWithValue("@datadoc", dataDB);
            insertCommand.Parameters.AddWithValue("@datalimitepag", 0);
            insertCommand.Parameters.AddWithValue("@dataemissaodoc", 0);
            insertCommand.Parameters.AddWithValue("@conta", codConta);
            insertCommand.Parameters.AddWithValue("@valor", valorFatura);
            insertCommand.Parameters.AddWithValue("@status", 1);
            insertCommand.Parameters.AddWithValue("@user", loginUserId);
            insertCommand.Parameters.AddWithValue("@date", Cls_0002_ActualDateTime.Date);
            insertCommand.Parameters.AddWithValue("@time", Cls_0002_ActualDateTime.Time);
            insertCommand.Parameters.AddWithValue("@userlastchg", 0);
            insertCommand.Parameters.AddWithValue("@datelastchg", 0);
            insertCommand.Parameters.AddWithValue("@timelastchg", 0);

            insertCommand.ExecuteNonQuery();
        }

        private void InsertDet(int Ref, Cls_030201_DebitosDetLinha linha)
        {
            string insertQuery = "INSERT INTO tbl_0302_movimentosdebito_det (md_id_fatura, md_id_linha, md_codartigo, md_codiva, md_quantidade, md_precobase, md_desconto1, md_desconto2, md_valordesconto, md_precofinal, md_combustivel, md_codviatura ,md_kmi, md_kmf, md_kmefetuados, md_status, md_usercreate, md_datecreate, md_timecreate, md_userlastchg, md_datelastchg, md_timelastchg) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, conn, transaction);
            insertCommand.Parameters.AddWithValue("@Id_fatura", Ref);
            insertCommand.Parameters.AddWithValue("@Id_linha", nILinha);
            insertCommand.Parameters.AddWithValue("@CodArtigo", linha.Codigo);
            insertCommand.Parameters.AddWithValue("@Codiva", linha.IVA);
            insertCommand.Parameters.AddWithValue("@Quantidade", linha.Quantidade);
            insertCommand.Parameters.AddWithValue("@Precobase", linha.PrecoUnitario);
            insertCommand.Parameters.AddWithValue("@Desconto1", linha.Desconto1);
            insertCommand.Parameters.AddWithValue("@Desconto2", linha.Desconto2);
            insertCommand.Parameters.AddWithValue("@Valordesconto", linha.ValorDesconto);
            insertCommand.Parameters.AddWithValue("@Precofinal", linha.PrecoFinalUnitario);
            insertCommand.Parameters.AddWithValue("@Combustivel", linha.Comb);
            insertCommand.Parameters.AddWithValue("@Viatura", linha.Viatura);
            insertCommand.Parameters.AddWithValue("@KMI", linha.KMF);
            insertCommand.Parameters.AddWithValue("@KMF", 0);
            insertCommand.Parameters.AddWithValue("@KME", 0);
            insertCommand.Parameters.AddWithValue("@status", 1);
            insertCommand.Parameters.AddWithValue("@user", loginUserId);
            insertCommand.Parameters.AddWithValue("@date", Cls_0002_ActualDateTime.Date);
            insertCommand.Parameters.AddWithValue("@time", Cls_0002_ActualDateTime.Time);
            insertCommand.Parameters.AddWithValue("@userlastchg", 0);
            insertCommand.Parameters.AddWithValue("@datelastchg", 0);
            insertCommand.Parameters.AddWithValue("@timelastchg", 0);

            insertCommand.ExecuteNonQuery();
        }

        private void UpdateDet()
        {
            string updateQuery = "UPDATE tbl_0302_movimentosdebito_det SET md_kmf = ?, md_kmefetuados = ? WHERE md_codviatura = ? AND md_kmi = ?";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn, transaction);
            updateCommand.Parameters.AddWithValue("@KMF", DDKMF);
            updateCommand.Parameters.AddWithValue("@KME", DDKME);
            updateCommand.Parameters.AddWithValue("@Viatura", DDViatura);
            updateCommand.Parameters.AddWithValue("@KMI", DDKMI);

            updateCommand.ExecuteNonQuery();
        }

        private void UpdateSaldo()
        {
            string updateQuery = "UPDATE tbl_0103_contascred SET cntcred_saldo = cntcred_saldo - ? WHERE cntcred_cod = ?";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn, transaction);
            updateCommand.Parameters.AddWithValue("@Valor", valorFatura);
            updateCommand.Parameters.AddWithValue("@ContaCredito", codConta);

            updateCommand.ExecuteNonQuery();
        }

        public decimal ValidarDebito(decimal debito)
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
                        cmd.Parameters.AddWithValue("@cntcred_cod", codConta);
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
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1021)!");

                }
                valor = Saldo - mov;

                return valor;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void AddRow()
        {
            Frm_030201_DebitoDetAdd debitoDetAdd = new Frm_030201_DebitoDetAdd(selectedDebDetArtCod, selectedDebDetArtDescr, selectedDebDetArtUni)
            {
                Owner = this
            };
            debitoDetAdd.AddCompleted += (sender, debitoDetRowAdd) =>
            {
                DDCodigo = debitoDetRowAdd.Code;
                DDDescr = debitoDetRowAdd.Designation;
                DDUnidade = debitoDetRowAdd.Unit;
                DDQuantidade = debitoDetRowAdd.Quantity;
                DDPrecoUnitario = debitoDetRowAdd.PriceUnit;
                DDDesconto1 = debitoDetRowAdd.Discount1;
                DDDesconto2 = debitoDetRowAdd.Discount2;
                DDValorDesconto = debitoDetRowAdd.DiscountValue;
                DDPrecoFinalUnitario = debitoDetRowAdd.FinalPriceUnit;
                DDValorTotal = debitoDetRowAdd.TotalValue;
                DDIVA = debitoDetRowAdd.Iva;
                DDTaxaIVA = debitoDetRowAdd.IvaTax;
                DDComb = debitoDetRowAdd.Combustivel;
                DDViatura = debitoDetRowAdd.Vehicle;
                DDKMI = debitoDetRowAdd.InitialKm;
                DDKMF = debitoDetRowAdd.FinalKm;
                DDKME = debitoDetRowAdd.CompletedKm;
                debitoDetAdd.Close();
            };
            debitoDetAdd.Closed += DebitoDetAdd_Closed;
            debitoDetAdd.ShowDialog();
        }

        private void DebitoDetAdd_Closed(object sender, EventArgs e)
        {
            Frm_030201_DebitoDetAdd debitoDetAdd = (Frm_030201_DebitoDetAdd)sender;
            DDAdd = debitoDetAdd.DDAdd;
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
            btn_Finalize.Visibility = Visibility.Visible;
            if (DDAdd == true)
            {
                Cls_030201_DebitosDetLinha linha = new Cls_030201_DebitosDetLinha
                {
                    Linha = nLinha,
                    Codigo = DDCodigo,
                    Descr = DDDescr,
                    Unidade = DDUnidade,
                    Quantidade = DDQuantidade,
                    PrecoUnitario = DDPrecoUnitario,
                    Desconto1 = DDDesconto1,
                    Desconto2 = DDDesconto2,
                    ValorDesconto = DDValorDesconto,
                    PrecoFinalUnitario = DDPrecoFinalUnitario,
                    ValorTotal = DDValorTotal,
                    IVA = DDIVA,
                    TaxaIVA = DDTaxaIVA,
                    Comb = DDComb,
                    Viatura = DDViatura,
                    KMI = DDKMI,
                    KMF = DDKMF,
                    KME = DDKME
                };
                valorFatura += DDValorTotal;
                nLinha++;
                cls_030201_DebitosDetLinhas.Add(linha);
                ListDebitosDet.ItemsSource = null;
                LoadLinhasDebito();
            }
        }

        private void Btn_Add_NewProdut_Click(object sender, RoutedEventArgs e)
        {
            Frm_0205_ArtigosManut artigosManut = new Frm_0205_ArtigosManut(loginUserSequence, loginUserId, loginUserType);
            artigosManut.Owner = this;
            artigosManut.ShowDialog();
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCab();
        }

        private void EditRow()
        {
            Cls_030201_DebitosDetLinha linha = (Cls_030201_DebitosDetLinha)ListDebitosDet.SelectedItem;
            Frm_030202_DebitoDetEdit debitoDetEdit = new Frm_030202_DebitoDetEdit(linha.Linha, linha.Codigo, linha.Descr, linha.Unidade, linha.Quantidade, linha.PrecoUnitario, linha.Desconto1, linha.Desconto2, linha.ValorDesconto, linha.PrecoFinalUnitario, linha.ValorTotal, linha.IVA, linha.Comb, linha.Viatura, linha.KMI, linha.KMF, linha.KME)
            {
                Owner = this
            };
            debitoDetEdit.EditCompleted += (sender, debitoDetRowEdit) =>
            {
                DDIdLinha = debitoDetRowEdit.Line;
                DDCodigo = debitoDetRowEdit.Code;
                DDDescr = debitoDetRowEdit.Designation;
                DDUnidade = debitoDetRowEdit.Unit;
                DDQuantidade = debitoDetRowEdit.Quantity;
                DDPrecoUnitario = debitoDetRowEdit.PriceUnit;
                DDDesconto1 = debitoDetRowEdit.Discount1;
                DDDesconto2 = debitoDetRowEdit.Discount2;
                DDValorDesconto = debitoDetRowEdit.DiscountValue;
                DDPrecoFinalUnitario = debitoDetRowEdit.FinalPriceUnit;
                DDDiferenca = debitoDetRowEdit.Difference;
                DDValorTotal = debitoDetRowEdit.TotalValue;
                DDIVA = debitoDetRowEdit.Iva;
                DDTaxaIVA = debitoDetRowEdit.IvaTax;
                DDComb = debitoDetRowEdit.Combustivel;
                DDViatura = debitoDetRowEdit.Vehicle;
                DDKMI = debitoDetRowEdit.InitialKm;
                DDKMF = debitoDetRowEdit.FinalKm;
                DDKME = debitoDetRowEdit.CompletedKm;
                debitoDetEdit.Close();
            };
            valorFatura += DDDiferenca;
            debitoDetEdit.Closed += DebitoDetEdit_Closed;
            debitoDetEdit.ShowDialog();
        }

        private void DebitoDetEdit_Closed(object sender, EventArgs e)
        {
            Frm_030202_DebitoDetEdit debitoDetEdit = (Frm_030202_DebitoDetEdit)sender;
            DDEdit = debitoDetEdit.DDEdit;
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ListDebitosDet.SelectedItem != null)
            {
                EditRow();
                if (DDEdit == true)
                {
                    Cls_030201_DebitosDetLinha itemAtualizado = new Cls_030201_DebitosDetLinha
                    {
                        Linha = DDIdLinha,
                        Codigo = DDCodigo,
                        Descr = DDDescr,
                        Unidade = DDUnidade,
                        Quantidade = DDQuantidade,
                        PrecoUnitario = DDPrecoUnitario,
                        Desconto1 = DDDesconto1,
                        Desconto2 = DDDesconto2,
                        ValorDesconto = DDValorDesconto,
                        PrecoFinalUnitario = DDPrecoFinalUnitario,
                        ValorTotal = DDValorTotal,
                        IVA = DDIVA,
                        TaxaIVA = DDTaxaIVA,
                        Comb = DDComb,
                        Viatura = DDViatura,
                        KMI = DDKMI,
                        KMF = DDKMF,
                        KME = DDKME
                    };
                    valorFatura += DDDiferenca;
                    int indice = cls_030201_DebitosDetLinhas.FindIndex(item => item.Linha == DDIdLinha);
                    cls_030201_DebitosDetLinhas[indice] = itemAtualizado;
                    ListDebitosDet.ItemsSource = null;
                    LoadLinhasDebito();
                }
            }
        }

        private void DeleteRow()
        {
            Cls_030201_DebitosDetLinha item = (Cls_030201_DebitosDetLinha)ListDebitosDet.SelectedItem;
            Frm_030203_DebitoDetDelete debitoDetDelete = new Frm_030203_DebitoDetDelete(item.Linha, item.Codigo, item.Descr, item.Unidade, item.Quantidade, item.PrecoUnitario, item.Desconto1, item.Desconto2, item.ValorDesconto, item.PrecoFinalUnitario, item.ValorTotal, item.TaxaIVA, item.Comb, item.Viatura, item.KMI, item.KMF, item.KME)
            {
                Owner = this
            };
            debitoDetDelete.DeleteCompleted += (sender, debitoDetRowDelete) =>
            {
                DDIdLinha = debitoDetRowDelete.Line;
                DDValorTotal = debitoDetRowDelete.TotalValue;
                debitoDetDelete.Close();
            };
            debitoDetDelete.Closed += DebitoDetDelete_Closed;
            debitoDetDelete.ShowDialog();
        }

        private void DebitoDetDelete_Closed(object sender, EventArgs e)
        {
            Frm_030203_DebitoDetDelete debitoDetDelete = (Frm_030203_DebitoDetDelete)sender;
            DDDelete = debitoDetDelete.DDDelete;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListDebitosDet.SelectedItem != null)
            {
                DeleteRow();
                if (DDDelete == true)
                {
                    valorFatura -= DDValorTotal;
                    int indice = cls_030201_DebitosDetLinhas.FindIndex(linha => linha.Linha == DDIdLinha);
                    cls_030201_DebitosDetLinhas.RemoveAt(indice);
                    ListDebitosDet.ItemsSource = null;
                    LoadLinhasDebito();
                }
            }
        }

        private void Btn_Finalize_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarDebito(valorFatura) >= 0)
            {
                Numeracao();

                try
                {
                    conn.Open();
                    // Inicie a transação
                    transaction = conn.BeginTransaction();

                    // Insira o cabeçalho da fatura 
                    InsertCab();

                    // Insira todas as linhas da fatura
                    foreach (var linha in cls_030201_DebitosDetLinhas)
                    {
                        InsertDet(Convert.ToInt32(Ref), linha);
                        nILinha++;
                    }
                    // Update para saldo de conta
                    UpdateSaldo();
                    // Update para calculo de quilometros
                    UpdateDet();
                    // Faz commit da transação se tudo estiver bem
                    transaction.Commit();
                    System.Windows.MessageBox.Show("Inclusáo concluida com sucesso! Número do registo: " + Ref);
                }
                catch (MySqlException ex)
                {
                    // Em caso de erro, faz rollback da transação e mostra uma mensagem de erro
                    transaction.Rollback();
                    System.Windows.MessageBox.Show("Erro ao criar a referência: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                this.Close();
            }
            else
            {
                decimal valor = Convert.ToDecimal(ValidarDebito(valorFatura));
                string valorFormatado = valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("pt-PT"));

                // Não existem alterações para gravar
                if (ShowConfirmation("Este movimento provoca um saldo negativo de " + valorFormatado + " na conta a débito. Deseja continuar?"))
                {
                    Numeracao();

                    try
                    {
                        conn.Open();
                        // Inicio da transação
                        transaction = conn.BeginTransaction();

                        // Insere o cabeçalho da fatura (se ainda não inserido)
                        InsertCab();

                        // Insere todas as linhas da fatura
                        foreach (var linha in cls_030201_DebitosDetLinhas)
                        {
                            InsertDet(Convert.ToInt32(Ref), linha);
                            nILinha++;
                        }
                        // Update para saldo de conta
                        UpdateSaldo();
                        // Update para calculo de quilometros
                        UpdateDet();
                        // Faz commit da transação se tudo estiver bem
                        transaction.Commit();
                        System.Windows.MessageBox.Show("Inclusáo concluida com sucesso! Número do registo: " + Ref);
                    }
                    catch (Exception)
                    {
                        // Em caso de erro, faz rollback da transação e mostra uma mensagem de erro
                        transaction.Rollback();
                        System.Windows.MessageBox.Show("Erro ao criar a referência: ");
                    }
                    finally
                    {
                        conn.Close();
                    }
                    this.Close();
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
