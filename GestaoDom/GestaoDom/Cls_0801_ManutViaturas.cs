/*
Cls_0801_ManutViarias.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_0801_ManutViaturas
    {
        public int Id { get; set; }
        public int Ref { get; set; }
        public string CodViatura { get; set; }
        public string MatViatura { get; set; }
        public string DescrViatura { get; set; }
        public string Descr { get; set; }
        public string DataManut { get; set; }
        public string Km { get; set; }
        public bool Oleo { get; set; }
        public bool FiltroOleo { get; set; }
        public bool FiltroAr { get; set; }
        public string Efetuado { get; set; }
        public string KmProximo { get; set; }
        public string Status { get; set; }
    }
}
