/*
Cls_0005_AccessControl.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace GestaoDom
{
    public class Cls_0005_AccessControl
    {
        public int OptionId { get; set; }
        public string OptionCod { get; set; }
        public string OptionDesig { get; set; }
        public int OptionLevel { get; set; }
        public int IdReg { get; set; }
        public bool OptionAccess { get; set; }
        public bool Access { get; set; }

        public Cls_0005_AccessControl() { }

        public Cls_0005_AccessControl(int optionId, string optionCod, string optionDesig, int optionLevel, int idReg, bool optionAccess)
        {
            OptionId = optionId;
            OptionCod = optionCod;
            OptionDesig = optionDesig;
            OptionLevel = optionLevel;
            IdReg = idReg;
            OptionAccess = optionAccess;
        }

        public Cls_0005_AccessControl(int optionId, string optionCod, int idReg, bool optionAccess)
        {
            OptionId = optionId;
            OptionCod = optionCod;
            IdReg = idReg;
            OptionAccess = optionAccess;
        }

        public static List<Cls_0005_AccessControl> GetAccess(int userid)
        {
            var listAccess = new List<Cls_0005_AccessControl>();
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta
                    var query = "SELECT opm_id optionId, opm_cod optionCod, opm_descr optionDesig, opm_nivel optionLevel, IFNULL(a.acs_id, 0) idReg, IFNULL(a.acs_acesso, 0) optionAccess" +
                    " FROM tbl_0003_opcoesAcesso LEFT JOIN(SELECT acs_userid, acs_cod, acs_id, acs_acesso FROM tbl_0004_acessos WHERE acs_userid = ?) a ON opm_cod = a.acs_cod ORDER BY opm_cod";

                    using (var da = new MySqlDataAdapter(query, conn))
                    {
                        // Atribuição de variavel
                        da.SelectCommand.Parameters.AddWithValue("@acs_userid", userid);
                        // Executa o comando
                        using (var dt = new DataTable())
                        {
                            da.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {
                                int optionId = Convert.ToInt32(row["optionId"]);
                                string optionCod = row["optionCod"] as string;
                                string optionDesig = row["optionDesig"] as string;
                                int optionLevel = Convert.ToInt32(row["optionLevel"]);
                                int idReg = Convert.ToInt32(row["idReg"]);
                                bool optionAccess = row["optionAccess"] != DBNull.Value && Convert.ToBoolean(row["optionAccess"]);

                                listAccess.Add(new Cls_0005_AccessControl(optionId, optionCod, optionDesig, optionLevel, idReg, optionAccess));
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro" + ex.Message);
                }
            }
            return listAccess;
        }

        public string Save(int userId, string loginUserId, List<Cls_0005_AccessControl> listAccess)
        {
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    // abre a ligação
                    conn.Open();

                    foreach (var item in listAccess)
                    {
                        var query = "";
                        if (item.IdReg == 0)
                            query = "INSERT INTO tbl_0004_acessos (acs_cod, acs_userid, acs_acesso, acs_usercreate, acs_datecreate, acs_timecreate, acs_userlastchg, acs_datelastchg, acs_timelastchg) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                        else
                            query = "UPDATE tbl_0004_acessos SET acs_cod = ?, acs_userid = ?, acs_acesso = ?, acs_userlastchg = ?, acs_datelastchg = ?, acs_timelastchg = ? WHERE acs_id = ?";


                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@acscod", item.OptionCod);
                            cmd.Parameters.AddWithValue("@acsuserid", userId);
                            cmd.Parameters.AddWithValue("@acsacesso", item.OptionAccess);
                            cmd.Parameters.AddWithValue("@user", loginUserId);
                            cmd.Parameters.AddWithValue("@date", Cls_0002_ActualDateTime.Date);
                            cmd.Parameters.AddWithValue("@time", Cls_0002_ActualDateTime.Time);

                            if (item.IdReg == 0)
                            {
                                cmd.Parameters.AddWithValue("@userlstchg", '0');
                                cmd.Parameters.AddWithValue("@datelstchg", '0');
                                cmd.Parameters.AddWithValue("@timelstchg", '0');
                            }

                            if (item.IdReg > 0)
                                // Atribuição de variavel
                                cmd.Parameters.AddWithValue("@id", item.IdReg);
                            // Executa o comando
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return "Ok";
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    MessageBox.Show(ex.Message);
                    return "No";
                }
            }
        }

        public static bool AccessGranted(string option, List<Cls_0005_AccessControl> userAccess)
        {
            foreach (var item in userAccess)
            {
                if (item.OptionCod == option)
                {
                    if (item.OptionAccess == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}