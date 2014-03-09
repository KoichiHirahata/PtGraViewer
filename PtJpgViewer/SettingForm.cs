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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace PtGraViewer
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();

            if (System.IO.File.Exists(Application.StartupPath+"\\settings.ini"))
            {
                Settings.readSettings();
                if (Settings.imgDir.Length == 0)
                { this.tbSaveDir.Text = "(" + Properties.Resources.NotConfigured + ")"; }
                else
                {
                    this.tbSaveDir.Text = Settings.imgDir;
                    cbBtOpenFolderVisible.Checked = Settings.btOpenFolderVisible;
                }
            }
            else
            { this.tbSaveDir.Text = "(" + Properties.Resources.InitialSetting + ")"; }

            this.ActiveControl = this.btSave;
        }

        private void kettei_Bt_Click(object sender, EventArgs e)
        {
            if ((this.tbSaveDir.Text == "(" + Properties.Resources.NotConfigured + ")") || (this.tbSaveDir.Text == "(" + Properties.Resources.InitialSetting + ")"))
            {
                MessageBox.Show("[Save to:]" + Properties.Resources.NotConfigured, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Settings.imgDir = this.tbSaveDir.Text;
                Settings.saveSettings();
                this.Close();
            }
        }

        private void sansho_Bt_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                this.tbSaveDir.Text = fbd.SelectedPath;
            }
        }

        private void cancel_Bt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sansho_Bt_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog(); //余計なことしない方がよさそう。
            fbd.SelectedPath = Application.StartupPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            { tbSaveDir.Text = fbd.SelectedPath; }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            #region ErrorCheck
            if ((tbSaveDir.Text == "(" + Properties.Resources.InitialSetting + ")") || (tbSaveDir.Text == "(" + Properties.Resources.NotConfigured + ")"))
            {
                MessageBox.Show("[Save to:]" + Properties.Resources.NotConfigured, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(tbSaveDir.Text))
            {
                MessageBox.Show("[Save to:]" + Properties.Resources.FolderNotExist, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            Settings.imgDir = tbSaveDir.Text;
            Settings.btOpenFolderVisible = cbBtOpenFolderVisible.Checked;
            Settings.saveSettings();
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    #region File_control
    //PDFデータの保存先などを記録するsetting.config用のクラス
    public class Settings
    {
        public static string imgDir { get; set; }
        public static Boolean btOpenFolderVisible { get; set; }
        public static Boolean isJP { get; set; } //Property for storing that machine's language is Japanese or not.

        //ファイルから読み込む
        public static void readSettings()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                string text = "";
                #region Read from file
                try
                {
                    using (StreamReader sr = new StreamReader(Application.StartupPath + "\\settings.ini"))
                    { text = sr.ReadToEnd(); }
                }
                catch (Exception e)
                { MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                #endregion

                int index;
                #region Read imgDir
                index = text.IndexOf("Image Directory:");
                if (index == -1)
                {
                    MessageBox.Show("[settings.ini]" + Properties.Resources.UnsupportedFileType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                { Settings.imgDir = getUntilNewLine(text, index + 16); }
                #endregion

                #region Read btOpenFolderVisible
                index = text.IndexOf("Open folder button visible:");
                if (index == -1)
                {
                    MessageBox.Show("[settings.ini]" + Properties.Resources.UnsupportedFileType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    string temp_str = getUntilNewLine(text, index + 27);
                    try { Settings.btOpenFolderVisible = bool.Parse(temp_str); }
                    catch (ArgumentNullException)
                    { Settings.btOpenFolderVisible = false; }
                    catch (FormatException)
                    { Settings.btOpenFolderVisible = false; }
                }
                #endregion
            }
            else
            {
                Settings.imgDir = "";
                Settings.btOpenFolderVisible = false;
            }
        }

        //ファイルに書き込む
        public static void saveSettings()
        {
            string text = "";
            if (System.IO.File.Exists(Application.StartupPath + "\\settings.ini"))
            {
                #region Read from file
                try
                {
                    using (StreamReader sr = new StreamReader(Application.StartupPath + "\\settings.ini"))
                    { text = sr.ReadToEnd(); }
                }
                catch (Exception e)
                { MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                #endregion

                int index;
                #region write imgDir to text
                index = text.IndexOf("Image Directory:");
                if (index == -1)
                {
                    MessageBox.Show("[settings.ini]" + Properties.Resources.UnsupportedFileType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int temp_text_length = text.Length;
                    for (int i = 16; (index + i) <= temp_text_length; i++)
                    {
                        if ((index + i) == temp_text_length)
                        {
                            text = (text.Substring(0, index + 16) + Settings.imgDir);
                            break;
                        }
                        else if (text.Substring(index + 16, 1) != "\r")
                        { text = (text.Substring(0, index + 16) + text.Substring(index + 17)); }
                        else
                        {
                            text = (text.Substring(0, index + 16) + Settings.imgDir + text.Substring(index + 16));
                            break;
                        }
                    }
                }
                #endregion

                #region write btOpenFolderVisible to text
                index = text.IndexOf("Open folder button visible:");
                if (index == -1)
                {
                    MessageBox.Show("[settings.ini]" + Properties.Resources.UnsupportedFileType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int temp_text_length = text.Length;
                    for (int i = 27; (index + i) <= temp_text_length; i++)
                    {
                        if ((index + i) == temp_text_length)
                        {
                            text = (text.Substring(0, index + 27) + Settings.btOpenFolderVisible.ToString());
                            break;
                        }
                        else if (text.Substring(index + 27, 1) != "\r")
                        { text = (text.Substring(0, index + 27) + text.Substring(index + 28)); }
                        else
                        {
                            text = (text.Substring(0, index + 27) + Settings.btOpenFolderVisible.ToString() + text.Substring(index + 27));
                            break;
                        }
                    }
                }
                #endregion
            }
            else
            { text = "Image Directory:" + Settings.imgDir + "\r\n" + "Open folder button visible:" + Settings.btOpenFolderVisible.ToString(); }

            #region Save to settings.ini
            StreamWriter sw = new StreamWriter(Application.StartupPath + @"\settings.ini", false);
            sw.Write(text);
            sw.Close();
            #endregion
        }

        public static string getUntilNewLine(string text, int strPoint)
        {
            string ret = "";
            for (int i = strPoint; i < text.Length; i++)
            {
                if ((text[i].ToString() != "\r") && (text[i].ToString() != "\n"))
                { ret += text[i].ToString(); }
                else
                { break; }
            }

            for (int i = 0; i < ret.Length; i++)
            {
                if (ret.Substring(0, 1) == "\t" || ret.Substring(0, 1) == " " || ret.Substring(0, 1) == "　")
                { ret = ret.Substring(1); }
                else
                { break; }
            }

            for (int i = 0; i < ret.Length; i++)
            {
                if (ret.Substring(ret.Length - 1) == "\t" || ret.Substring(ret.Length - 1) == " " || ret.Substring(ret.Length - 1) == "　")
                { ret = ret.Substring(0, ret.Length - 1); }
                else
                { break; }
            }

            return ret;
        }
    }

    public class file_control
    {
        public static void delFile(string fileName)
        {
            if (System.IO.File.Exists(fileName) == true)
            {
                try
                { System.IO.File.Delete(fileName); }
                catch (System.IO.IOException)
                { MessageBox.Show(Properties.Resources.FileBeingUsed, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                catch (System.UnauthorizedAccessException)
                { MessageBox.Show(Properties.Resources.PermissionDenied, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            { MessageBox.Show(Properties.Resources.FileNotFound, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
    #endregion
}
