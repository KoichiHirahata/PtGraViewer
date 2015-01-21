using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace PtGraViewer
{
    public partial class MainForm : Form
    {
        private string ptID;
        private string IdDateNoExt;//ID_date_sequence number_.ext

        public MainForm()
        {
            InitializeComponent();
            Settings.readSettings();
            Settings.isJP = (Application.CurrentCulture.TwoLetterISOLanguageName == "ja");

            left_load_label.Visible = false;
            right_load_label.Visible = false;

            btOpenFolder.Visible = Settings.btOpenFolderVisible;

            string[] args = System.Environment.GetCommandLineArgs();
            if (args.Count() > 1)
            {
                if (isWrongFolderName(args[1]))
                { MessageBox.Show(Properties.Resources.WrongText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                { tbPtID.Text = args[1]; }
            }
        }

        #region Menu
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm sf = new SettingForm();
            sf.ShowDialog(this);
            btOpenFolder.Visible = Settings.btOpenFolderVisible;
        }
        #endregion

        #region btView
        private void BtView_Click(object sender, EventArgs e)
        { viewFiles(); }

        private void tbPtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            { viewFiles(); }
        }

        private void viewFiles()
        {
            #region Error check
            if (tbPtID.Text.Length == 0)
            {
                MessageBox.Show(Properties.Resources.NoID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Settings.imgDir))
            {
                MessageBox.Show("[" + Properties.Resources.InitialSetting + "]" + Properties.Resources.NotConfigured, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(Settings.imgDir))
            {
                MessageBox.Show("[" + Properties.Resources.InitialSetting + "]" + Properties.Resources.FolderNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isWrongFolderName(tbPtID.Text))
            {
                MessageBox.Show(Properties.Resources.WrongText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            ptID = tbPtID.Text;
            IdDateNoExt = null;

            LvParent.Items.Clear();
            ilParent.Images.Clear();

            LvItems.Items.Clear();
            ilItems.Images.Clear();

            string ptImgDir = Settings.imgDir + @"\" + ptID;
            if (!Directory.Exists(ptImgDir))
            {
                MessageBox.Show(Properties.Resources.FolderNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            left_load_label.Visible = true;
            this.Update();

            string[] jpgFiles1 = Directory.GetFiles(ptImgDir, "*_???.???", SearchOption.TopDirectoryOnly);//*_???.*だと-001.*と一部だぶる。何故か良く分からない。
            string[] jpgFiles2 = Directory.GetFiles(ptImgDir, "*-001.???", SearchOption.AllDirectories);
            string[] jpgFiles = jpgFiles1.Concat(jpgFiles2).ToArray();

            int width = 48;
            int height = 48;
            ilParent.ImageSize = new Size(width, height);
            LvParent.LargeImageList = ilParent;
            LvParent.Sorting = SortOrder.Descending;

            string ext1;//ファイル拡張子を格納
            Image original;
            Image thumnail;
            for (int i = 0; jpgFiles.Length > i; i++)
            {
                ext1 = jpgFiles[i].Substring(jpgFiles[i].Length - 3, 3);
                switch (ext1)
                {
                    case "pdf":
                    case "PDF":
                        if (!File.Exists(Application.StartupPath + @"\pdf48.png"))
                        {
                            MessageBox.Show("[pdf48.png]" + Properties.Resources.FileNotFound, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        original = Bitmap.FromFile(Application.StartupPath + @"\pdf48.png");
                        ilParent.Images.Add(original);
                        original.Dispose();

                        LvParent.Items.Add(trimFileName(jpgFiles[i]) + "(PDF)", i);
                        break;
                    case "jpg":
                    case "JPG":
                        try
                        { original = Bitmap.FromFile(jpgFiles[i]); }
                        catch (OutOfMemoryException)
                        {
                            MessageBox.Show(Properties.Resources.OutOfMemory, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                        thumnail = createThumnail(Bitmap.FromFile(jpgFiles[i]), width, height);
                        ilParent.Images.Add(thumnail);

                        original.Dispose();
                        thumnail.Dispose();

                        LvParent.Items.Add(trimFileName(jpgFiles[i]), i);
                        break;
                }
                left_load_label.Text = "Now loading... [" + (i + 1).ToString() + "/" + jpgFiles.Length.ToString() + "]";
                this.Update();
            }

            left_load_label.Visible = false;

            if (LvParent.Items.Count == 0)
            { MessageBox.Show(Properties.Resources.FileNotFound, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                LvParent.Focus();
                LvParent.Items[0].Selected = true;
            }
        }
        #endregion

        #region btOpenFolder
        private void btOpenFolder_Click(object sender, EventArgs e)
        {
            #region Error check
            if (isWrongFolderName(tbPtID.Text))
            {
                MessageBox.Show(Properties.Resources.WrongText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            if (Directory.Exists(Settings.imgDir + @"\" + tbPtID.Text))
            { System.Diagnostics.Process.Start(Settings.imgDir + @"\" + tbPtID.Text); }
            else
            { MessageBox.Show(Properties.Resources.FolderNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region LvParent
        private void LvParentItemProcedure()
        {
            right_load_label.Visible = true;
            this.Update();

            LvItems.Items.Clear();
            ilItems.Images.Clear();
            IdDateNoExt = null;

            string selectedText;
            Boolean isPDF;
            selectedText = LvParent.SelectedItems[0].Text;
            if (selectedText.Substring(selectedText.Length - 5) == "(PDF)")
            {
                isPDF = true;
                selectedText = selectedText.Substring(0, selectedText.Length - 5);
            }
            else
            { isPDF = false; }

            //textから元のファイルを探す手がかりをゲット。
            string searchText = ptID + "_";
            if (Settings.isJP)
            { searchText += selectedText.Substring(0, 4) + selectedText.Substring(5, 2) + selectedText.Substring(8, 2) + "_" + selectedText.Substring(11, 3); }
            else
            { searchText += selectedText.Substring(0, 4) + selectedText.Substring(5, 2) + selectedText.Substring(8); }

            string searchFolderName = searchText;

            if (isPDF)
            {
                IdDateNoExt = searchText + ".pdf";
                searchText = searchText + "*.pdf";
            }
            else
            {
                IdDateNoExt = searchText + ".jpg";
                searchText = searchText + "*.jpg";
            }

            string[] gFilesTopDir = Directory.GetFiles(Settings.imgDir + @"\" + ptID, searchText, SearchOption.TopDirectoryOnly);
            string[] gFiles;
            if (Directory.Exists(Settings.imgDir + @"\" + ptID + @"\" + searchFolderName))
            {
                string[] gFilesSubDir = Directory.GetFiles(Settings.imgDir + @"\" + ptID + @"\" + searchFolderName, searchText, SearchOption.TopDirectoryOnly);
                gFiles = new string[gFilesTopDir.Length + gFilesSubDir.Length];
                gFilesTopDir.CopyTo(gFiles, 0);
                gFilesSubDir.CopyTo(gFiles, gFilesTopDir.Length);
            }
            else
            {
                gFiles = new string[gFilesTopDir.Length];
                gFilesTopDir.CopyTo(gFiles, 0);
            }

            int width = 48;
            int height = 48;
            ilItems.ImageSize = new Size(width, height);
            LvItems.LargeImageList = ilItems;
            LvItems.Sorting = SortOrder.Ascending;

            string ext1;//ファイル拡張子を格納
            Image original;
            Image thumnail;
            for (int i = 0; gFiles.Length > i; i++)
            {
                ext1 = gFiles[i].Substring(gFiles[i].Length - 3, 3);
                switch (ext1)
                {
                    case "pdf":
                        if (!File.Exists(Application.StartupPath + @"\pdf48.png"))
                        {
                            MessageBox.Show("[pdf48.png]" + Properties.Resources.FileNotFound, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        original = Bitmap.FromFile(Application.StartupPath + @"\pdf48.png");
                        ilItems.Images.Add(original);
                        original.Dispose();

                        LvItems.Items.Add(trim2No(gFiles[i]), i);
                        break;
                    case "jpg":
                        try
                        { original = Bitmap.FromFile(gFiles[i]); }
                        catch (OutOfMemoryException)
                        {
                            MessageBox.Show(Properties.Resources.OutOfMemory, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                        thumnail = createThumnail(Bitmap.FromFile(gFiles[i]), width, height);
                        ilItems.Images.Add(thumnail);

                        original.Dispose();
                        thumnail.Dispose();

                        LvItems.Items.Add(trim2No(gFiles[i]), i);
                        break;
                }
                right_load_label.Text = "Now loading... [" + (i + 1).ToString() + "/" + gFiles.Length.ToString() + "]";
                this.Update();
            }

            right_load_label.Visible = false;

            //If only one file selceted, open the file. If not, move focus to LvItems.
            if (gFiles.Length == 1)
            { openFile(trim2No(gFiles[0])); }
            else
            {
                LvItems.Focus();
                LvItems.Items[0].Selected = true;
            }
        }

        private void LvParent_DoubleClick(object sender, EventArgs e)
        { LvParentItemProcedure(); }

        private void LvParent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { LvParentItemProcedure(); }
        }
        #endregion

        #region LvItems
        private void LvItems_MouseDoubleClick(object sender, MouseEventArgs e)
        { openFile(LvItems.SelectedItems[0].Text); }

        private void LvItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { openFile(LvItems.SelectedItems[0].Text); }
            else if (e.KeyCode == Keys.Escape)
            { LvParent.Focus(); }
        }
        #endregion

        #region Trim functions
        private string trimFileName(string fileName)//ファイル名を日付+連番までに削る。（lvParent用）
        {
            string ret = Path.GetFileName(fileName); //パス削除

            //最初のID部分削除
            while (ret.Substring(0, 1) != "_")
            {
                ret = ret.Substring(1);
            }
            ret = ret.Substring(1);

            ret = ret.Substring(0, ret.Length - 4); //拡張子削除

            //-001を削除
            if (ret.Substring(ret.Length - 4) == "-001")
            { ret = ret.Substring(0, ret.Length - 4); }

            if (Settings.isJP)
            {
                //通し番号前の_を削除
                ret = ret.Substring(0, ret.Length - 4) + ret.Substring(ret.Length - 3);

                //年月日挿入
                ret = ret.Substring(0, 4) + "年" + ret.Substring(4, 2) + "月" + ret.Substring(6, 2) + "日" + ret.Substring(8);
            }
            else
            { ret = ret.Substring(0, 4) + "/" + ret.Substring(4, 2) + "/" + ret.Substring(6); }

            return ret;
        }

        private string trim2No(string fileName) //ファイル名を連番+[-???]までトリムする。
        {
            string ret = Path.GetFileName(fileName); //Delete path string
            //最初のID部分削除
            while (ret.Substring(0, 1) != "_")
            {
                ret = ret.Substring(1);
            }
            ret = ret.Substring(1);

            //Delete date string
            while (ret.Substring(0, 1) != "_")
            {
                ret = ret.Substring(1);
            }
            ret = ret.Substring(1);

            ret = ret.Substring(0, ret.Length - 4); //拡張子削除

            return ret;
        }
        #endregion

        private Image createThumnail(Image original, int w, int h)
        {
            Bitmap canvas = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
            double fw = (double)w / (double)(original.Width);
            double fh = (double)h / (double)(original.Height);
            double scale = Math.Min(fw, fh);

            int w2 = (int)(original.Width * scale);
            int h2 = (int)(original.Height * scale);

            g.DrawImage(original, (w - w2) / 2, (h - h2) / 2, w2, h2);
            g.Dispose();

            return canvas;
        }

        private void openFile(string gNumber)//PDF files will open with associated software. The others will send to viewer.
        {
            //Search file from ID folder and ID_Date_No folder
            string searchFileName = IdDateNoExt.Substring(0, IdDateNoExt.Length - 4) + gNumber.Substring(3) + IdDateNoExt.Substring(IdDateNoExt.Length - 4);
            string searchFolderName = IdDateNoExt.Substring(0, IdDateNoExt.Length - 4);
            string[] gFilesTopDir = Directory.GetFiles(Settings.imgDir + @"\" + ptID, searchFileName, SearchOption.TopDirectoryOnly);
            string[] gFile;
            if (Directory.Exists(Settings.imgDir + @"\" + ptID + @"\" + searchFolderName))
            {
                string[] gFilesSubDir = Directory.GetFiles(Settings.imgDir + @"\" + ptID + @"\" + searchFolderName, searchFileName, SearchOption.TopDirectoryOnly);
                gFile = new string[gFilesTopDir.Length + gFilesSubDir.Length];
                gFilesTopDir.CopyTo(gFile, 0);
                gFilesSubDir.CopyTo(gFile, gFilesTopDir.Length);
            }
            else
            {
                gFile = new string[gFilesTopDir.Length];
                gFilesTopDir.CopyTo(gFile, 0);
            }

            if (gFile.Length > 1)
            { MessageBox.Show(Properties.Resources.FileNameDuplicated, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            string gFileName = gFile[0].ToString();

            if (gFileName.Substring(gFileName.Length - 3) == "pdf")
            { System.Diagnostics.Process.Start(gFileName); }
            else
            {
                string searchText = IdDateNoExt.Substring(0, IdDateNoExt.Length - 4) + "*" + IdDateNoExt.Substring(IdDateNoExt.Length - 4);
                string[] FilesOfSameFolder = Directory.GetFiles(Settings.imgDir + @"\" + ptID, searchText, SearchOption.AllDirectories);
                JpegViewer jv = new JpegViewer(FilesOfSameFolder, gFileName);
                jv.ShowDialog(this);
            }
        }

        private Boolean isWrongFolderName(string str)
        {
            string[] checkStr = { "/", @"\", ":", "*", "?", "\"", "<", ">", "|", "#", "{", "}", "%", "&", "~", ".." };
            for (int i = 0; i < checkStr.Count(); i++)
            {
                if (0 <= str.IndexOf(checkStr[i]))
                    return true;
            }

            if (str.Substring(0, 1) == ".")
            { return true; }

            if (str.Substring(str.Length - 1, 1) == ".")
            { return true; }

            if (str.Substring(0, 1) == "　")
            { return true; }

            if (str.Substring(str.Length - 1, 1) == "　")
            { return true; }

            return false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (tbPtID.Text.Length > 0)
            { viewFiles(); }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        { System.Diagnostics.Process.Start("http://www.madeinclinic.jp/%E3%82%BD%E3%83%95%E3%83%88%E3%82%A6%E3%82%A7%E3%82%A2/pt_graphic/ptgraviewer/"); }
    }
}
