using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Schema;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Reflection;
using System.Resources;

namespace GameSupPr.BackEnd {

    public class SQLException : Exception {
        
    }
    public class OSQL {
        public Dictionary<String, String> param { get; protected set; }
        public DataTable ds;
        protected JArray jArray;
        public bool isDoneQuery = false;
        public string query { get; private set; }
        public OSQL() {
            param = new Dictionary<string, string>();
            jArray = new JArray();
            isDoneQuery = false;
        }
        public void setQuery() {

        }
        public void AddParam(string key, string value) {
            param.Add(key, value);
        }
        private void ReplaceParam() {
            if (String.IsNullOrEmpty(query))
                return;
            if (param.Count <= 0)
                return;
            foreach (KeyValuePair<string, string> pair in param) {
                query = query.Replace("#" + pair.Key + "#", "'" + pair.Value.Replace("'", "\\'") + "'");
                query = query.Replace("@" + pair.Key, "'" + pair.Value.Replace("'", "\\'") + "'");
            }
        }
        private string CallDB() {
            return Properties.Resources.DB;
        }
        public DataTable Go() {
            using (MySqlConnection con = new MySqlConnection(CallDB())) {
                try {
                    ds = new DataTable();
                    ds.Clear();
                    con.Open();
                    Console.WriteLine("QUERY: " + query);
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader table = cmd.ExecuteReader();
                    Console.WriteLine("Read Complete");

                    while (table.Read()) {
                        JObject mObj = new JObject();

                        for (int i = 0; i < table.FieldCount; i++) {
                            Console.WriteLine(table.GetName(i) + ": " + table[i].ToString());
                            mObj.Add(table.GetName(i).ToString(), table[i].ToString());
                        }
                        jArray.Add(mObj);
                    }
                    table.Close();
                    ds = JsonConvert.DeserializeObject<DataTable>(jArray.ToString());
                    isDoneQuery = true;
                    return ds;
                }
                catch (Exception e) {
                    Console.WriteLine("Fail Error: " + e.Message);
                    throw new SQLException();
                }
                finally {
                }
            }
            
        }
    }
}
