using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GameSupPr.BackEnd.SQLView {
    public enum LogInState {
        Complete =0 ,
        SameIDexist = 1,
        LoginFailed = 2,
        NoIDExist = 3
    }
    public class LogConnData {
        public string id;
        private string pwd;
        public void setpwd(string data) {
            pwd = data;
        }
        public string SHA256Hash(string data) {

            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash) {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public string name;
        public LogInState LogIn() {
            try {
                OSQL sql = new OSQL();
                sql.setQuery("" +
                    "SELECT count(*) FROM USER_TB " +
                    "WHERE USER_ID = #user_id# AND PWD = #pwd# ");
                sql.AddParam("user_id", id);
                sql.AddParam("pwd", SHA256Hash(pwd));
                sql.Go();
                if(sql.ds.Rows.Count > 0) {
                    return LogInState.Complete;
                } else {
                    return LogInState.NoIDExist;
                }

            }
            catch (Exception e){
                Console.WriteLine("Debug Error: "+e.Message +"\n"+ e.StackTrace);
                return LogInState.LoginFailed;
            }
        }
        public LogInState SignUP() {
            try{
                OSQL idCheck = new OSQL();
                idCheck.setQuery("" +
                    "SELECT count(*) FROM USER_TB " +
                    "WHERE USER_ID = #user_id#");
                idCheck.AddParam("user_id", id);
                //idCheck.AddParam("pwd", SHA256Hash(pwd));
                idCheck.Go();
                if (idCheck.ds.Rows.Count >= 1) {
                    return LogInState.SameIDexist;
                } else {

                    OSQL signUpsql = new OSQL();
                    signUpsql.setQuery("" +
                        "INSERT INTO USER_TB (USER_ID, PWD, USER_NM)" +
                        "values ('#user_id#,#pwd#,#user_nm#");
                    signUpsql.AddParam("user_id", id);
                    signUpsql.AddParam("pwd", pwd);
                    signUpsql.AddParam("user_nm", name);


                    signUpsql.Go();
                    

                    return LogInState.Complete;
                }
            }
            catch(Exception e) {
                Console.WriteLine("Debug Error: " + e.Message + "\n" + e.StackTrace);
                return LogInState.LoginFailed;
            }
        }
    }
}
