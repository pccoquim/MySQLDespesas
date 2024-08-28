/*
Cls_0003_Users.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using System.Collections.Generic;

namespace GestaoDom
{
    public class Cls_0003_Users
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public bool ChangePassword { get; set; }
        public int PasswordCount { get; set; }
        public string PasswordStatus
        {
            get { return PasswordCount > 3 ? "Desativada" : "Ativada"; }
        }
        public List<Cls_0005_AccessControl> MenuAccess { get; set; }

        public Cls_0003_Users() { }

        public Cls_0003_Users(int id, string userID, string name, string pw, string type, string status, bool chgpw, int pwcount)
        {
            this.ID = id;
            this.UserID = userID;
            this.Name = name;
            this.Password = pw;
            this.Type = type;
            this.Status = status;
            this.ChangePassword = chgpw;
            this.PasswordCount = pwcount;
        }
    }
}
