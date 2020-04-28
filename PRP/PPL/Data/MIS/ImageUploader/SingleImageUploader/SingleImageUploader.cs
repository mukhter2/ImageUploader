using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRP.PPL.Data.MIS.UserManagement.ImageSelectorSingle
{
    public partial class SingleImageUploader : Form
    {
        public SingleImageUploader()
        {
            InitializeComponent();
        }
        DataRow dr;
        string updateOld = "";
        string updateNew = "";
        int updateRow = -1;
        string mainFolderLocation = "";
        private void ImageSelector_Load(object sender, EventArgs e)
        {
            string p = Directory.GetCurrentDirectory();
            string projectDirectory = Directory.GetParent(p).Parent.FullName;
            var foldersFound = Directory.GetDirectories(projectDirectory, "ImageUploader", SearchOption.AllDirectories);
            mainFolderLocation = foldersFound[0] + "\\Images";
            InitializeOpenFileDialog();
            AddDgvforUnit();
            DgvLoader();
            
        }
        private void AddDgvforUnit()
        {
            dgvFileInfo.DataSource = null;
            dgvFileInfo.Columns.Clear();
            dgvFileInfo.Refresh();

            DataTable tdt = new DataTable();
            tdt.Columns.Add(new DataColumn("Name", typeof(string)));
            tdt.Columns.Add(new DataColumn("Image", typeof(Image)));
            tdt.Columns.Add(new DataColumn("Size", typeof(string)));
            tdt.Columns.Add(new DataColumn("Source", typeof(string)));
            //tdt.Columns.Add(new DataColumn("Update", typeof(Button)));
            //tdt.Columns.Add(new DataColumn("Delete", typeof(string)));
            //tdt.Columns.Add(new DataColumn("Download", typeof(string)));



            dgvFileInfo.DataSource = tdt;
            dgvFileInfo.AllowUserToAddRows = false;  //---DataGrid Additional Row Hide---//
                                                     //dgvFileInfo.Columns[0].Width = 250;  //---Set 1st Column Width---//
                                                     // dgvFileInfo.Columns[5].Width = 190;
                                                     //dgvFileInfo.Columns[0].Width = 150;
                                                     //dgvFileInfo.Columns[3].Width = 200;

            dgvFileInfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvFileInfo.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvFileInfo.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvFileInfo.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvFileInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            dgvFileInfo.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFileInfo.Columns["Size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFileInfo.Columns["Source"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
           // dgvFileInfo.Columns["Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFileInfo.Columns["Delete"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFileInfo.Columns["Download"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dgvFileInfo.Columns[0].DividerWidth = 2;
            dgvFileInfo.Columns[1].DividerWidth = 2;
            dgvFileInfo.Columns[2].DividerWidth = 2;
            dgvFileInfo.Columns[3].DividerWidth = 2;
           //dgvFileInfo.Columns[4].DividerWidth = 2;
           //dgvFileInfo.Columns[5].DividerWidth = 2;



            //dgvFileInfo.Columns[4].Width = 80;
            //dgvFileInfo.Columns[5].Width = 80;
            //dgvFileInfo.Columns[6].Width = 80;

        }

        private void DgvLoader()
        {
            
            DataTable dt = new DataTable();
            string sSelectedPath = "";
            if (rdoBrowse.Checked)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Custom Description";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sSelectedPath = fbd.SelectedPath;
                }
            }
            else
            {
               

                sSelectedPath = mainFolderLocation;





            }
            if (sSelectedPath == "") return;
            String[] s1;
            s1 = Directory.GetFiles(@""+sSelectedPath,
                   "*",
                   SearchOption.AllDirectories);
            for (int i = 0; i < s1.Length; i++)
            {

                FileInfo f = new FileInfo(s1[i]);

                if(!(f.FullName.EndsWith(".jpg") || f.Name.EndsWith(".png")))
                {
                    continue;
                }
                FileSystemInfo f1 = new FileInfo(s1[i]);
                
                dt = dgvFileInfo.DataSource as DataTable;
                dr = dt.NewRow();
                //Get File name of each file name
                string mad = f1.Name;
                for (int task = 20; task < mad.Length; task += 20)
                {
                    mad = mad.Insert(task, Environment.NewLine);
                    task += 4;
                }

                dr["Name"] = mad;
                //Get File Type/Extension of each file 

                Image loadedImage = resizeImage(Image.FromFile(f1.FullName), new Size(100, 100));


                dr["Image"] = loadedImage;
                //Get File Size of each file in KB format
                dr["Size"] = (f.Length / 1024).ToString() + " KB";
                //Get file Create Date and Time 
                mad = f1.FullName;
                for (int task = 20; task < mad.Length; task += 20)
                {
                    mad = mad.Insert(task, Environment.NewLine);
                    task += 4;
                }
                dr["Source"] = mad;
                
                
                //dr["Update"] = btnUpdateGrid;
                //dr["Delete"] = "Delete";
                //dr["Download"] = "Download";
                dt.Rows.Add(dr);

            }
            if (dt.Rows.Count > 0)
            {
                //Finally Add DataTable into DataGridView
                dgvFileInfo.DataSource = dt;
                DataGridViewButtonColumn btnUpdateGrid = new DataGridViewButtonColumn();
                //Padding newPadding = new Padding(1);
                //dgvFileInfo.RowTemplate.DefaultCellStyle.Padding = newPadding;
                btnUpdateGrid.Text = "Update";
                btnUpdateGrid.Name = "Update";
                //btnUpdateGrid.DefaultCellStyle.BackColor = Color.AliceBlue;
                btnUpdateGrid.FlatStyle = FlatStyle.Standard;
                //btnUpdateGrid.DefaultCellStyle.BackColor = Color.Aqua;
                btnUpdateGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 14F,FontStyle.Bold, GraphicsUnit.Pixel);

                btnUpdateGrid.UseColumnTextForButtonValue = true;
                dgvFileInfo.Columns.Insert(4, btnUpdateGrid);

                //----delete 
                DataGridViewButtonColumn btnDeleteGrid = new DataGridViewButtonColumn();
                btnDeleteGrid.Text = "Delete";
                btnDeleteGrid.Name = "Delete";
                btnDeleteGrid.FlatStyle = FlatStyle.Standard;
                //btnDeleteGrid.DefaultCellStyle.BackColor = Color.Aqua;
                btnDeleteGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
                btnDeleteGrid.UseColumnTextForButtonValue = true;
                dgvFileInfo.Columns.Insert(5, btnDeleteGrid);

                //--- download
                DataGridViewButtonColumn btnDownloadGrid = new DataGridViewButtonColumn();
                btnDownloadGrid.Text = "Download";
                btnDownloadGrid.Name = "Download";
                btnDownloadGrid.FlatStyle = FlatStyle.Standard;
                //btnDownloadGrid.DefaultCellStyle.BackColor = Color.Aqua;
                btnDownloadGrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
                btnDownloadGrid.UseColumnTextForButtonValue = true;
                dgvFileInfo.Columns.Insert(6, btnDownloadGrid);
                dgvFileInfo.Columns[4].DividerWidth = 2;
                dgvFileInfo.Columns[5].DividerWidth = 2;
                dgvFileInfo.Columns[6].DividerWidth = 2;
                dgvFileInfo.Columns["Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFileInfo.Columns["Delete"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFileInfo.Columns["Download"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvFileInfo.Columns["Update"].DefaultCellStyle.Padding = new Padding(10, 25, 10, 25);
                dgvFileInfo.Columns["Delete"].DefaultCellStyle.Padding = new Padding(10, 25, 10, 25);
                dgvFileInfo.Columns["Download"].DefaultCellStyle.Padding = new Padding(8, 25, 8, 25);
            }


        }
        private void InitializeOpenFileDialog()
        {
            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Filter =
              "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
              "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "My Image Browser";
        }
        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
        private void selectFilesButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<Control> listControls = flowLayoutPanel1.Controls.Cast<Control>().ToList();

                foreach (Control control in listControls)
                {
                    flowLayoutPanel1.Controls.Remove(control);
                    control.Dispose();
                }
                // Read the files
                string file = openFileDialog1.FileName;
                
                    // Create a PictureBox.
                    try
                    {

                        
                        Panel panel = new Panel();
                        PictureBox pb = new PictureBox();
                        TextBox tb = new TextBox();
                        TextBox tb2 = new TextBox();
                        Button btnNew = new Button();
                        btnNew.Text = "Remove";
                        
                        btnNew.ForeColor = Color.Green;
                        
                        tb.Name = "Link";
                        tb2.Name = "Rename";
                        Image loadedImage = resizeImage(Image.FromFile(file),new Size(200,200));
                        
                        pb.Height = loadedImage.Height;
                        pb.Width = loadedImage.Width;
                        pb.Image = loadedImage;
                        tb.Text = file;
                        panel.Controls.Add(pb);
                        tb2.Text = DateTime.Now.ToString("hh-mm-ss-ffffff"); 
                        panel.Height = 300;
                        panel.Width = 200;
                        tb.Location = new Point(0,200);
                        tb.Size =new Size(200,20);

                        tb2.Location = new Point(0, 225);
                        tb2.Size = new Size(200, 20);
                        btnNew.Location = new Point(75, 275);
                        btnNew.Height = 20;
                        btnNew.Width = 60;
                        panel.BackColor = Color.AliceBlue;
                        btnNew.Click += btnNew_Click;
                        panel.Controls.Add(tb);
                        panel.Controls.Add(tb2);
                        panel.Controls.Add(btnNew);
                        flowLayoutPanel1.Controls.Add(panel);
                        
                    }
                    
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                
                
            }

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Panel x=(Panel) clickedButton.Parent;
            if (flowLayoutPanel1.Controls.Contains(x))
            {
                flowLayoutPanel1.Controls.Remove(x);
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sSelectedPath = "";
            string source = "";
            string rename = "";
            if (chkSave.Checked)
            {
                sSelectedPath = mainFolderLocation;
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = mainFolderLocation;

                fbd.Description = "Custom Description";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sSelectedPath = fbd.SelectedPath;

                }
                if (sSelectedPath.Length == 0)
                {
                    MessageBox.Show("No Path for saving is selected.");
                    return;
                }
            }
            foreach (Panel c in flowLayoutPanel1.Controls)
            {

                foreach (Control cd in c.Controls)
                {
                    if (cd.GetType() == typeof(TextBox))
                    {
                        
                        TextBox td = (TextBox)cd;
                        if(cd.Name == "Link")
                        {
                            source = td.Text;
                        }else if (cd.Name == "Rename")
                        {
                            rename = td.Text;
                        }
                    }



                }
                string s = Path.GetExtension(source);
                string p = sSelectedPath + "\\" + rename+""+s;
                System.IO.File.Copy(source, p);
                
            }
            MessageBox.Show("Saved Successfully.");
            List<Control> listControls = flowLayoutPanel1.Controls.Cast<Control>().ToList();

            foreach (Control control in listControls)
            {
                flowLayoutPanel1.Controls.Remove(control);
                control.Dispose();
            }
            AddDgvforUnit();
            DgvLoader();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (Panel c in flowLayoutPanel1.Controls)
            {
                string source = "";
                foreach (Control cd in c.Controls)
                {
                    if (cd.GetType() == typeof(TextBox))
                    {

                        TextBox td = (TextBox)cd;
                        if (cd.Name == "Link")
                        {
                            source = td.Text;
                            break;
                        }
                        
                    }



                }
                flowLayoutPanel1.Controls.Remove(c);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(source);

            }
            MessageBox.Show("successfully deleted");
            return;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void rdoBrowse_CheckedChanged(object sender, EventArgs e)
        {
            AddDgvforUnit();
            DgvLoader();
        }

        private void deleteSource()
        {

        }
        private void updateSource()
        {

        }
        private void dgvFileInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grpUpdate.Visible = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult myResult;
            myResult = MessageBox.Show("Do you really want to Update the image?", "Update Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (myResult == DialogResult.Cancel)
            {
                return;
            }
            
            string naming = dgvFileInfo.Rows[updateRow].Cells[0].Value.ToString();
            naming = naming.Replace(System.Environment.NewLine, "");
            string p = updateOld.Replace(naming,"");
            string s = Path.GetExtension(updateNew);
            
            p +=  txtUpdateNewName.Text;
            p += (s);
            System.IO.File.Copy(updateNew, p);

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.IO.File.Delete(updateOld);
            AddDgvforUnit();
            DgvLoader();
            MessageBox.Show("Updated Successfully.");
            grpUpdate.Visible = false;
            return;
        }

        private void dgvFileInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dgvFileInfo.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dgvFileInfo.Columns["Delete"].Index)
            {
                DialogResult myResult;
                myResult = MessageBox.Show("Do you really want to Delete the image?", "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (myResult == DialogResult.Cancel)
                {
                    return;
                }
                string source = dgvFileInfo.Rows[e.RowIndex].Cells[e.ColumnIndex - 2].Value.ToString();
                source = source.Replace(System.Environment.NewLine, "");
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(source);
                MessageBox.Show("Deleted successfully");
                AddDgvforUnit();
                DgvLoader();
                return;
            }
            if (e.ColumnIndex == dgvFileInfo.Columns["Update"].Index)
            {


                grpUpdate.Visible = true;

                updateOld = dgvFileInfo.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();
                updateOld = updateOld.Replace(System.Environment.NewLine, "");
                updateRow = e.RowIndex;
                Image loadedImage = resizeImage(Image.FromFile(updateOld), new Size(200, 200));

                pboOld.Image = loadedImage;
                DialogResult dr = this.openFileDialog2.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    updateNew = openFileDialog2.FileName;

                    loadedImage = resizeImage(Image.FromFile(updateNew), new Size(200, 200));
                    pboNew.Image = loadedImage;
                    txtUpdateNewName.Text = DateTime.Now.ToString("hh-mm-ss-ffffff");
                }
                else
                {
                    return;
                }
                return;
            }
            if (e.ColumnIndex == dgvFileInfo.Columns["Download"].Index)
            {
                grpUpdate.Visible = true;

                updateOld = dgvFileInfo.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value.ToString();
                updateOld = updateOld.Replace(System.Environment.NewLine, "");
                string oldName = dgvFileInfo.Rows[e.RowIndex].Cells[0].Value.ToString();
                oldName = oldName.Replace(System.Environment.NewLine, "");
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Custom Description";
                string sSelectedPath = "";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sSelectedPath = fbd.SelectedPath;

                }
                if (sSelectedPath.Length == 0)
                {
                    MessageBox.Show("No Path for saving is selected.");
                    return;
                }

                string p = sSelectedPath + "\\" + oldName;
                System.IO.File.Copy(updateOld, p);
                MessageBox.Show("Successfully Downloaded.");
                return;
            }
        }
    }
}
