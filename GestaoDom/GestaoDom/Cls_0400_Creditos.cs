/*
Cls_0400_Creditos.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
namespace GestaoDom
{
    public class Cls_0400_Creditos
    {
        public int Id { get; set; }
        public int Ref { get; set; }
        public string CodTerc { get; set; }
        public string DescrTerc { get; set; }
        public string NumDoc { get; set; }
        public string Data { get; set; }
        public string CodTipoReceita { get; set; }
        public string DescrTipoReceita { get; set; }
        public string CodContaO { get; set; }
        public string DescrContaO { get; set; }
        public string CodContaD { get; set; }
        public string DescrContaD { get; set; }
        public decimal Valor { get; set; }
        public bool Transf { get; set; }
        public string Status { get; set; }
    }
}
