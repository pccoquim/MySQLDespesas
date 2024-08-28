/*
Cls_030201_DebitosDetLinha.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_030201_DebitosDetLinha
    {
        public int Id { get; set; }
        public int Fatura { get; set; }
        public int Linha { get; set; }
        public string Codigo { get; set; }
        public string Descr { get; set; }
        public string Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Desconto1 { get; set; }
        public decimal Desconto2 { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal PrecoFinalUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public string IVA { get; set; }
        public string TaxaIVA { get; set; }
        public bool Comb { get; set; }
        public string Viatura { get; set; }
        public int KMI { get; set; }
        public int KMF { get; set; }
        public int KME { get; set; }
        public string CodStatus { get; set; }
        public string Status { get; set; }

        public Cls_030201_DebitosDetLinha() { }

        public Cls_030201_DebitosDetLinha(int id, int fatura, int linha, string codigo, string descr, string unidade, decimal quantidade, decimal precoUnitario, decimal desconto1, decimal desconto2, decimal valorDesconto, decimal precoFinalUnitario, decimal valorTotal, string iva, string taxaIVA, bool comb, string viatura, int kmi, int kmf, int kme, string status, string codStatus)
        {
            Id = id;
            Fatura = fatura;
            Linha = linha;
            Codigo = codigo;
            Descr = descr;
            Unidade = unidade;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Desconto1 = desconto1;
            Desconto2 = desconto2;
            ValorDesconto = valorDesconto;
            PrecoFinalUnitario = precoFinalUnitario;
            ValorTotal = valorTotal;
            IVA = iva;
            TaxaIVA = taxaIVA;
            Comb = comb;
            Viatura = viatura;
            KMI = kmi;
            KMF = kmf;
            KME = kme;
            Status = status;
            CodStatus = codStatus;
        }

        public Cls_030201_DebitosDetLinha(int linha, string codigo, string descr, decimal quantidade, decimal precoUnitario, decimal desconto1, decimal desconto2, decimal valorDesconto, decimal precoFinalUnitario, decimal valorTotal, string iva, bool comb, string viatura, int kmi, int kmf, int kme)
        {
            Linha = linha;
            Codigo = codigo;
            Descr = descr;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Desconto1 = desconto1;
            Desconto2 = desconto2;
            ValorDesconto = valorDesconto;
            PrecoFinalUnitario = precoFinalUnitario;
            ValorTotal = valorTotal;
            IVA = iva;
            Comb = comb;
            Viatura = viatura;
            KMI = kmi;
            KMF = kmf;
            KME = kme;
        }
    }
}
