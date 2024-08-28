/*
#################################################################
#   Projeto:        GestaoDom                                   #
#   Arquivo:        Cls_0000_Cryptography.cs                    #
#   Autor:          Paulo da Cruz Coquim                        #
#   Data:           07.06.2024                                  #
#   Data alteração: 28.08.2024                                  #
#   Versão:         1.0.1                                       #
#################################################################
*/
using System.Text;

namespace GestaoDom
{
    internal class Cls_0000_Cryptography
    {
        private static readonly string Key = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";

        public static string Encrypt(string password)
        {
            StringBuilder encryptedPassword = new StringBuilder();
            foreach (char c in password)
            {
                int index = Key.IndexOf(c);
                if (index != -1)
                {
                    char encryptedChar = Key[(index + 9) % Key.Length];
                    encryptedPassword.Append(encryptedChar);
                }
                else
                {
                    encryptedPassword.Append(c);
                }
            }
            return encryptedPassword.ToString();
        }

        public static string Decrypt(string encryptedPassword)
        {
            StringBuilder decryptedPassword = new StringBuilder();
            foreach (char c in encryptedPassword)
            {
                int index = Key.IndexOf(c);
                if (index != -1)
                {
                    char decryptedChar = Key[(index - 9 + Key.Length) % Key.Length];
                    decryptedPassword.Append(decryptedChar);
                }
                else
                {
                    decryptedPassword.Append(c);
                }
            }
            return decryptedPassword.ToString();
        }
    }
}
