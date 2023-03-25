using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using Org.BouncyCastle.Asn1.Crmf;

namespace project_1
{
    public partial class Form1 : Form
    {
        string path = "users.db";
        string cs = @"URI=file:" + Application.StartupPath + "//users.db";
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataReader reader;
        bool oe_mode ;
        string[] files_path ;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            //ToBin("osama");
            label6.Text = "Welcome  " + Environment.UserName;
            Creat_db();
            data_show();
           
          
         
         
        }
        private void Creat_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source = "+path))
                {
                    sqlite.Open();
                    string sql = "CREATE TABLE users(id int , name varchar(255) , level varchar(255) , gpa double)";
                    cmd = new SQLiteCommand(sql, sqlite);
                    cmd.ExecuteNonQuery();
                }
            }            
        }

        private void add_Click(object sender, EventArgs e)
        {

            if(oe_mode == true)
            {
                add_oe(id.Text + '☭', name.Text + '☭', level.Text + '☭', gpa.Text );
                Thread.Sleep(1000);
                show_oe();
            }
            else
            {
                var conn = new SQLiteConnection(cs);
                conn.Open();

                var cmd = new SQLiteCommand(conn);

                cmd.CommandText = "INSERT INTO users(id , name , level , gpa) VALUES(@id , @name , @level , @gpa) ";

                int id_1 = Int32.Parse(id.Text);
                string NAME = name.Text.ToString();
                string LEVEL = level.Text.ToString();
                double GPA = double.Parse(gpa.Text);
                cmd.Parameters.AddWithValue("@id", id_1);
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@level", LEVEL);
                cmd.Parameters.AddWithValue("@gpa", GPA);
                try
                {
                    cmd.ExecuteNonQuery();
                    dataGridView2.Rows.Clear();
                    name.Text = "";
                    id.Text = "";
                    level.Text = "";
                    gpa.Text = "";
                    search.Text = "";

                    data_show();

                }
                catch (Exception ex)
                {
                    Alert alert = new Alert();
                    alert.msg = ex.ToString();
                    alert.ShowDialog();
                }
            }

          

        }
        private void data_show()
        {

            var conn = new SQLiteConnection(cs);
            conn.Open();
            string commned = "SELECT * FROM users";
            var cmd = new SQLiteCommand(commned, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    dataGridView2.Rows.Add(reader["id"].ToString(), reader["name"].ToString(), reader["level"].ToString(), reader["gpa"].ToString());
                }
                catch (Exception ex)
                {
                    Alert alert = new Alert();
                    alert.msg = ex.ToString();
                    alert.ShowDialog();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (oe_mode == true)
            {
                char o = '☭';
                StreamWriter sw = new StreamWriter("users.oe");
                bool deleted = false;
                foreach (string line in files_path)
                {
                    string[] data = line.Split(o);
                    // MessageBox.Show(data[1]);
                    if (data[0] != id.Text)
                    {

                        sw.WriteLine(data[0] + "☭" + data[1] + "☭" + data[2] + "☭" + data[3]);

                        deleted= true;
                    }

                }
                if(deleted == true)
                {
                    sw.Close();
                    Alert alert= new Alert();
                    alert.msg = "Data Saved successfully";
                    alert.ShowDialog();
                    show_oe();
                }
            }
            else
            {
                var conn = new SQLiteConnection(cs);
                conn.Open();

                var cmd = new SQLiteCommand(conn);
                try
                {
                    int iD = Int32.Parse(id.Text);

                    cmd.CommandText = "DELETE  FROM users WHERE id =@id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", iD);
                    cmd.ExecuteNonQuery();
                    dataGridView2.Rows.Clear();

                    data_show();
                }
                catch (Exception ex)
                {
                    Alert alert = new Alert();
                    alert.msg = ex.ToString();
                    alert.ShowDialog();
                }

            }


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            id.Text = dataGridView2.CurrentRow.Cells["id_col"].Value.ToString();
            level.Text = dataGridView2.CurrentRow.Cells["level_col"].Value.ToString();
            gpa.Text = dataGridView2.CurrentRow.Cells["gpa_col"].Value.ToString();
            name.Text = dataGridView2.CurrentRow.Cells["name_col"].Value.ToString().Trim();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        //    id.Text    = dataGridView2.CurrentRow.Cells["id_col"].Value.ToString();
        //    level.Text = dataGridView2.CurrentRow.Cells["level_col"].Value.ToString();
        //    gpa.Text   = dataGridView2.CurrentRow.Cells["gpa_col"].Value.ToString();
        //    name.Text  = dataGridView2.CurrentRow.Cells["name_col"].Value.ToString().Trim();

        }

        private void search_Click(object sender, EventArgs e)
        {
            if(oe_mode == true)
            {
               
                dataGridView2.Rows.Clear();
                char o = '☭';
                string[] search_oe;
                foreach (string line in files_path)
                {
                    string[] data = line.Split(o);
                    // MessageBox.Show(data[1]);
                    if (data[0] == search.Text)
                    {
                        dataGridView2.Rows.Add(data[0], data[1], data[2], data[3]);
                    }
                }
            }
            try
            {
                var conn = new SQLiteConnection(cs);
                conn.Open();
                int search_id = Int32.Parse(search.Text);

                string commned = "SELECT * FROM users WHERE id = " + search_id +"";
                var cmd = new SQLiteCommand(commned, conn);
                reader = cmd.ExecuteReader();
                dataGridView2.Rows.Clear();

                while (reader.Read())
                {
                    try
                    {
                        dataGridView2.Rows.Add(reader["id"].ToString(), reader["name"].ToString(), reader["level"].ToString(), reader["gpa"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Alert alert = new Alert();
                        alert.msg = ex.ToString();
                        alert.Show();
                    }
                }
            }
            catch (Exception )
            {
                var conn = new SQLiteConnection(cs);
                conn.Open();

                string commned = "SELECT * FROM users WHERE name='"+search.Text.ToString().Trim()+"'";
                var cmd = new SQLiteCommand(commned, conn);
                reader = cmd.ExecuteReader();
                dataGridView2.Rows.Clear();
                while (reader.Read())
                {
                    try
                    {
                        dataGridView2.Rows.Add(reader["id"].ToString(), reader["name"].ToString(), reader["level"].ToString(), reader["gpa"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Alert alert = new Alert();
                        alert.msg = ex.ToString();
                        alert.ShowDialog();
                    }
                }
            }

        

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (oe_mode == true)
            {
                char o = '☭';
                StreamWriter su = new StreamWriter("users.oe");
                bool updated = false;
                foreach (string line in files_path)
                {
                    string[] data = line.Split(o);
                    // MessageBox.Show(data[1]);
                    if (data[0] == id.Text)
                    {

                        data[1] = name.Text;
                        data[2] = level.Text;
                        data[3] = gpa.Text;
                       
                    }

                    su.WriteLine(data[0] + "☭" + data[1] + "☭" + data[2] + "☭" + data[3]);
                    updated = true;
              

                }

                if (updated == true)
                {
                    su.Close();
                    Alert alert = new Alert();
                    alert.msg = "Data saved successfully";
                    alert.ShowDialog();
                    show_oe();
                }
            }
            else
            {
                var conn = new SQLiteConnection(cs);
                conn.Open();
                var cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE users SET name =@name , level = @level , gpa = @gpa WHERE id =@id";

                int id_1 = Int32.Parse(id.Text);
                string NAME = name.Text.ToString();
                string LEVEL = level.Text.ToString();
                double GPA = double.Parse(gpa.Text);
                cmd.Parameters.AddWithValue("@id", id_1);
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@level", LEVEL);
                cmd.Parameters.AddWithValue("@gpa", GPA);
                try
                {
                    cmd.ExecuteNonQuery();
                    dataGridView2.Rows.Clear();
                    data_show();

                }
                catch (Exception ex)
                {
                    Alert alert = new Alert();
                    alert.msg = ex.ToString();
                    alert.ShowDialog();
                }
            }
          
        }

        private void export_Click(object sender, EventArgs e)
        {

            if (dataGridView2.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
               
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {

                            ErrorMessage = true;
                            Alert alert = new Alert();
                            alert.msg = "Unable to wride data in disk" + ex.ToString();
                            alert.ShowDialog();
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView2.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.DefaultCell.BorderColor = new BaseColor(241, 91, 181);
                            pTable.DefaultCell.BackgroundColor = new BaseColor(255, 214, 10);
                            
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_CENTER;
                           
                            foreach (DataGridViewColumn col in dataGridView2.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pCell.BackgroundColor = new BaseColor(6, 214, 160);
                                pCell.BorderColorLeft = new BaseColor(Color.White);
                                
                                pTable.AddCell(pCell);
                                //Console.WriteLine(col);

                            }

                            foreach (DataGridViewRow viewRow in dataGridView2.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {

                                    pTable.AddCell(dcell.Value.ToString());
                               //     Console.WriteLine(dcell.Value.ToString());
                                }
                              




                            }
                            pTable.AddCell("Created ");
                            pTable.AddCell("By ");
                            pTable.AddCell("osama ");
                            pTable.AddCell("elkassaby ");


                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 8f, 8f, 8f);
                                document.AddTitle("Created By OsamaElkassaby");
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            Alert alert = new Alert();
                            alert.msg = "Data Export Successfully";
                            alert.ShowDialog();
                        }

                        catch (Exception ex)
                        {

                            Alert alert = new Alert();
                            alert.msg = "Error while exporting Data" + ex.ToString();
                            alert.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record Found", "Info");

            }
        }

      
        private void name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                name.Text = "";
                id.Text = "";
                level.Text = "";
                gpa.Text = "";
                search.Text = "";

            }
        }

    

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chosess file .oe";
            ofd.Filter = "oe file |*.oe";
            ofd.ShowDialog();
            files_path = File.ReadAllLines(ofd.FileName);
            string[] file = files_path;
            dataGridView2.Rows.Clear();
            char o = '☭';
            foreach (string line in file)
            {
                string[] data = line.Split(o);
                // MessageBox.Show(data[1]);
                if (data[0] == search.Text)
                {
                    dataGridView2.Rows.Add(data[0], data[1], data[2], data[3]);
                }
                dataGridView2.Rows.Add(data[0], data[1], data[2], data[3]);
            }
        }
        private void add_oe( string id ,string name ,string level , string gpa )
        {
            StreamReader sr = new StreamReader("users.oe");
            string sr_data = sr.ReadToEnd();
            sr.Close();
            StreamWriter sw = new StreamWriter("users.oe");
            sw.WriteLine( sr_data + id+ name  + level  + gpa);
            sw.Close();
            Alert alert = new Alert();
            alert.msg = "data saved sucsessfuly";
            alert.ShowDialog();
        }


//        name = Request.QueryString["name"];
//id = Request.QueryString["id"];

//try
//{
//    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//        string filePath = Path.Combine(desktopPath, "data.txt");
//    using (System.IO.StreamWriter sw = new StreamWriter(filePath, true))
//    {
//        sw.WriteLine(name + "||" + id);
//        sw.Close();
//    }



//}
//catch (Exception ex)
//{
//    Label label = new Label();
//    label.Text = ex.Message;
//    Controls.Add(label);
//}
private void show_oe()
        {
            if(files_path != null)
            {
                dataGridView2.Rows.Clear();
                char o = '☭';
                string[] lines = files_path;
                foreach (string line in lines)
                {
                    string[] data = line.Split(o);
                    
                    dataGridView2.Rows.Add(data[0], data[1], data[2], data[3]);
                }
            }
            else
            {
                Alert alert = new Alert();
                alert.msg = "chosess path first";
                alert.ShowDialog();
            }
           
        }

        private void oe_CheckedChanged(object sender, EventArgs e)
        {
            if(oe_mode == true)
            {
                oe.Checked = false;
                oe_mode = false;

            }
            else
            {
                oe_mode = true;

                oe.Checked = true;
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
         Application.Exit();
        }
        private void ToBin(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in s.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));

            }
           
            Alert alert = new Alert();
            alert.msg = sb.ToString();
            alert.ShowDialog();
        }
    }
}
