/*
Cls_0301_DebitosCab.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_0301_DebitosCab
    {
        public int Id { get; set; }
        public int Ref { get; set; }
        public string CodTerc { get; set; }
        public string DescrTerc { get; set; }
        public string NumDoc { get; set; }
        public string Data { get; set; }
        public string DataLP { get; set; }
        public string DataEm { get; set; }
        public string CodConta { get; set; }
        public string DescrConta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
    }
}
