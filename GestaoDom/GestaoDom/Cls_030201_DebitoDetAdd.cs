/*
Cls_030201_DebitoDetAdd.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_030201_DebitoDetAdd
    {
        public string Code { get; set; }
        public string Designation { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal FinalPriceUnit { get; set; }
        public decimal TotalValue { get; set; }
        public string Iva { get; set; }
        public string IvaTax { get; set; }
        public bool Combustivel { get; set; }
        public string Vehicle { get; set; }
        public int InitialKm { get; set; }
        public int FinalKm { get; set; }
        public int CompletedKm { get; set; }
    }
}
