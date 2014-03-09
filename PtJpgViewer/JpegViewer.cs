using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PtGraViewer
{
    public partial class JpegViewer : Form
    {
        private ArrayList jpgFiles = new ArrayList();
        private string selectedJpgFile;

        public JpegViewer(string[] _jpgFiles, string jpgFile = "")
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;//Maximize form

            #region initialize dgv
            //Add ImageColumn
            DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
            imgColumn.Name = "Image";
            imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;//イメージを縦横の比率を維持して拡大、縮小表示する
            imgColumn.Description = "イメージ"; //イメージの説明。セルをクリップボードにコピーした時に使用される
            dgv.Columns.Add(imgColumn);//DataGridViewに追加する
            dgv.Columns["Image"].Width = 120;
            dgv.RowTemplate.Height = 90;

            //Add CheckBoxColumn
            DataGridViewCheckBoxColumn cbColumn = new DataGridViewCheckBoxColumn();
            cbColumn.Name = "cbColumn";
            dgv.Columns.Add(cbColumn);
            cbColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //Add TextBoxColumn
            DataGridViewTextBoxColumn tbColumn = new DataGridViewTextBoxColumn();
            tbColumn.Name = "tbColumn";
            tbColumn.ReadOnly = true;
            dgv.Columns.Add(tbColumn);
            tbColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgv.RowHeadersVisible = false;//Make selector invisible
            dgv.ColumnHeadersVisible = false;//Make column header invisible
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            #endregion

            #region dgv image procedure
            jpgFiles.AddRange(_jpgFiles);
            jpgFiles.Sort();

            for (int i = 0; i < jpgFiles.Count; i++)
            {
                try
                {
                    dgv.Rows.Add();
                    dgv["Image", i].Value = new Bitmap(jpgFiles[i].ToString());
                }
                catch (ArgumentOutOfRangeException)
                { MessageBox.Show("[ArgumentOutOfRangeException]" + Properties.Resources.HasOccurred, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            #endregion

            #region pictureBox
            pbMain.SizeMode = PictureBoxSizeMode.Zoom;
            selectedJpgFile = jpgFile;
            #endregion
        }

        #region cbColumn
        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if ((bool)dgv[e.ColumnIndex, e.RowIndex].Value)
                { numberTb(e.RowIndex); }
                else
                { dgv.Rows[e.RowIndex].Cells["tbColumn"].Value = ""; }
            }
        }

        private void numberTb(int rowIndex)
        {
            Boolean numCheck = true;
            int i;
            for (i = 1; (i <= 8) && (numCheck == true); i++)
            {
                numCheck = false;
                foreach (var dr in dgv.Rows.Cast<DataGridViewRow>())
                {
                    if ((dr.Cells["tbColumn"].Value != null) && (dr.Cells["tbColumn"].Value != DBNull.Value))
                    {
                        if (dr.Cells["tbColumn"].Value.ToString() == i.ToString())
                        { numCheck = true; }
                    }
                }
            }
            i = i - 1;

            if ((i == 8) && (numCheck == true))
            {
                MessageBox.Show(Properties.Resources.UpperLimitIs8, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgv.Rows[rowIndex].Cells["cbColumn"].Value = false;
            }
            else
            {
                dgv.Rows[rowIndex].Cells["tbColumn"].Value = i.ToString();
            }
            return;
        }

        private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)//これがないとフォーカスを移さないと反応しない。
        {
            if (dgv.CurrentCellAddress.X == 1 && dgv.IsCurrentCellDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);  //Commit          
            }
        }
        #endregion

        #region Print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ppDialog.Document = this.pDoc;
            this.ppDialog.Height = 600;
            this.ppDialog.ShowDialog();
        }

        private void pDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            #region Settings
            //A4を探して設定。
            int index;
            for (index = 0; index < pDoc.PrinterSettings.PaperSizes.Count; index++)
            {
                if (pDoc.PrinterSettings.PaperSizes[index].PaperName.Contains("A4") == true)
                {
                    pDoc.DefaultPageSettings.PaperSize = pDoc.PrinterSettings.PaperSizes[index];
                    break;
                }
            }

            //A4が設定できなかったら通知。
            if (index == pDoc.PrinterSettings.PaperSizes.Count)
            { MessageBox.Show(Properties.Resources.A4NotExist, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            //画像の縮小を高品質に設定
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //MessageBox.Show(e.Graphics.PageUnit.ToString());
            #endregion

            Bitmap bmp;
            Bitmap bmpOriginal;
            Bitmap bmpNew;
            float resol = 72.0F;//dpi設定
            int margin = 50;//余白設定
            int boxWidth = 360;
            int boxHeight = 270;
            Graphics g;
            int[] gSize = new int[2];
            foreach (var dr in dgv.Rows.Cast<DataGridViewRow>())
            {
                if ((dr.Cells["tbColumn"].Value != null) && (dr.Cells["tbColumn"].Value != DBNull.Value))
                {
                    switch (dr.Cells["tbColumn"].Value.ToString())
                    {
                        case "1":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin, margin, gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "2":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin + boxWidth, margin, gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "3":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin, margin + boxHeight, gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "4":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin + boxWidth, margin + boxHeight, gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "5":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin, margin + (boxHeight * 2), gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "6":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin + boxWidth, margin + (boxHeight * 2), gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "7":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin, margin + (boxHeight * 3), gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                        case "8":
                            bmpOriginal = Bitmap.FromFile(jpgFiles[dr.Index].ToString()) as Bitmap;
                            if (bmpOriginal.HorizontalResolution != resol || bmpOriginal.VerticalResolution != resol)
                            {
                                bmpNew = new Bitmap(bmpOriginal.Width, bmpOriginal.Height);
                                bmpNew.SetResolution(resol, resol);
                                g = Graphics.FromImage(bmpNew);
                                g.DrawImage(bmpOriginal, 0, 0, bmpOriginal.Width, bmpOriginal.Height);
                                g.Dispose();
                                bmp = new Bitmap(bmpNew);

                                bmpNew.Dispose();
                            }
                            else
                            { bmp = new Bitmap(bmpOriginal); }

                            gSize = determinSize(bmp);
                            e.Graphics.DrawImage(bmp, margin + boxWidth, margin + (boxHeight * 3), gSize[0], gSize[1]);

                            bmpOriginal.Dispose();
                            bmp.Dispose();
                            break;
                    }
                }
            }
        }

        private int[] determinSize(Bitmap bmp)
        {
            int[] ret = new int[2];
            int boxWidth = 360;
            int boxHeight = 270;

            if (((double)bmp.Width / bmp.Height) > ((double)boxWidth / boxHeight))
            {
                ret[0] = boxWidth;
                ret[1] = (int)Math.Round((double)boxWidth * bmp.Height / bmp.Width);
            }
            else
            {
                ret[0] = (int)Math.Round((double)boxHeight * bmp.Width / bmp.Height);
                ret[1] = boxHeight;
            }

            return ret;
        }
        #endregion

        private void dgv_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            pbMain.ImageLocation = jpgFiles[e.RowIndex].ToString();
            this.Text = jpgFiles[e.RowIndex].ToString();
        }

        private void JpegViewer_Shown(object sender, EventArgs e)
        {
            pbMain.ImageLocation = selectedJpgFile;
            this.Text = selectedJpgFile;
            dgv.CurrentCell = dgv.Rows[jpgFiles.IndexOf(selectedJpgFile)].Cells["Image"];
        }
    }
}
