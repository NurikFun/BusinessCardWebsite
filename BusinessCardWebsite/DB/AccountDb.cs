using BusinessCardWebsite.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BusinessCardWebsite.DB
{
    public class AccountDb
    {
        private MySqlConnection con;
        private MySqlCommand cmd { get; set; }

        public AccountDb(string connectionString)
        {
            con = new MySqlConnection(connectionString);
        }


        public QueryResult checkUser(UserModel user)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("select count(*) from nii_test.site_users where login=@login and password=@password;"))
                {
                    cmd.Parameters.AddWithValue("@login", user.login);
                    cmd.Parameters.AddWithValue("@password", user.password);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();

                    qr.resulCode = false;
                    MySqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    qr.resulCode = Convert.ToInt32(reader[0]) > 0;
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }


        public QueryResult updateSession(UserModel user)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("update nii_test.site_users set session_value=@session_value where login=@login;"))
                {
                    cmd.Parameters.AddWithValue("@session_value", user.session_value);
                    cmd.Parameters.AddWithValue("@login", user.login);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }



        private T ObjectResultFalse<T>(Exception e, T t) where T : QueryResult
        {
            t.resulCode = false;
            t.resultMessage = e.Message;
            return t;
        }


    }
}