using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

//using System.Net.Sockets;
//using System.Threading;
//using System.Globalization;

namespace TextToRTFConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string constr = "Data Source=127.0.0.1; Initial Catalog=DB;User=sa;Password=Password;";        

            using (SqlConnection selectConnection = new SqlConnection(constr))
            {
                selectConnection.Open();
                string selectSql = "SELECT * FROM Employees Where ID = @ID ORDER BY FirstName ASC; ";
                using (SqlCommand selectCommand = new SqlCommand(selectSql, selectConnection))
                {
                    selectCommand.Parameters.AddWithValue("@ID", 5476);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        System.Windows.Forms.RichTextBox rtb = new System.Windows.Forms.RichTextBox();
                        
                        while (reader.Read())
                        {
                            using (SqlConnection updateConnection = new SqlConnection(constr))
                            {
                                updateConnection.Open();
                                rtb.Text = reader[1].ToString();                                
                                string updateSql = $"UPDATE Employees SET Notes = @RTF WHERE ID = @ID";
                                using (var updateCommand = new SqlCommand(updateSql, updateConnection))
                                {
                                    updateCommand.Parameters.AddWithValue("@RTF", rtb.Rtf);
                                    updateCommand.Parameters.AddWithValue("@ID", reader[0]);
                                    updateCommand.ExecuteNonQuery();
                                    Console.WriteLine($"Güncellendi : {reader[0]}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Bitti.");
            Console.ReadKey();
        }
    }
}
