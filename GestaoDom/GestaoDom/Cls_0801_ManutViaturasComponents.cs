/*
Cls_0801_ManutViariasComponents.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_0801_ManutViaturasComponents
    {
        public int Id { get; set; }
        public int Ref { get; set; }
        public string CodArtigo { get; set; }
        public string Descr { get; set; }
        public decimal Quant { get; set; }
        public decimal PrcUnit { get; set; }
        public decimal PrcTotal { get; set; }
        public string Status { get; set; }
    }
}
