using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace PtGraViewer
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();

            if (System.IO.File.Exists(Settings.settingFile_location))
            {
                if (String.IsNullOrWhiteSpace(Settings.imgDir))
                { this.tbSaveDir.Text = "(" + Properties.Resources.NotConfigured + ")"; }
                else
                {
                    this.tbSaveDir.Text = Settings.imgDir;
                    cbBtOpenFolderVisible.Checked = Settings.openFolderButtonVisible;
                }
            }
            else
            { this.tbSaveDir.Text = "(" + Properties.Resources.InitialSetting + ")"; }

            this.ActiveControl = this.btSave;
        }

        #region Buttons
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
            Settings.openFolderButtonVisible = cbBtOpenFolderVisible.Checked;
            Settings.saveSettings();
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }

    #region file_control
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

        public static string readFromFile(string fileName)
        {
            string text = "";
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                { text = sr.ReadToEnd(); }
            }
            catch (Exception e)
            { MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return text;
        }

        public static string readItemSettingFromText(string text, string itemName)
        {
            int index;
            index = text.IndexOf(itemName);
            if (index == -1)
            {
                MessageBox.Show("[settings.config]" + Properties.Resources.UnsupportedFileType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            else
            { return getUntilNewLine(text, index + itemName.Length); }
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
    #endregion

    #region password
    public class PasswordEncoder
    {
        private PasswordEncoder() { }

        private const string AesIV = @"&%jqiIurtmslLE58";
        private const string AesKey = @"3uJi<9!$kM0lkxme";

        public static string Encrypt(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            { return ""; }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = Encoding.Unicode.GetBytes(text);

            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                return Convert.ToBase64String(dest);
            }
        }

        public static string Decrypt(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            { return ""; }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = System.Convert.FromBase64String(text);
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }
    }
    #endregion
}
