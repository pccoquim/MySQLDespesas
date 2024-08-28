/*
Cls_0002_ActualDateTime.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using System;

namespace GestaoDom
{
    public static class Cls_0002_ActualDateTime
    {
        public static string Date { get; } = DateTime.Now.ToString("yyyyMMdd");
        public static string Time { get; } = DateTime.Now.ToString("HHmmss");
    }
}
