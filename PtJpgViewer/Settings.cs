using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Npgsql;

namespace PtGraViewer
{
    //PDFデータの保存先などを記録するsetting.config用のクラス
    public class Settings
    {
        public static string imgDir { get; set; }
        public static bool openFolderButtonVisible { get; set; }
        public static Boolean useFeDB { get; set; }
        public static string DBSrvIP { get; set; } //IP address of DB server
        public static string DBSrvPort { get; set; } //Port number of DB server
        public static string DBconnectID { get; set; } //ID of DB user
        public static string DBconnectPw { get; set; } //Pw of DB user
        public static string settingFile_location { get; set; } //Config file path
        public static string lang { get; set; } //language
        public static string sslSetting { get; set; } //SSL setting string
        public static bool usePlugin { get; set; }
        public static string ptInfoPlugin { get; set; } //File location of the plug-in to get patient information

        public static void initiateSettings()
        {
            settingFile_location = Application.StartupPath + "\\settings.config";
            readSettings();
            lang = Application.CurrentCulture.TwoLetterISOLanguageName;
            //Settings.sslSetting = ""; //Use this when you want to connect without using SSL
            sslSetting = "SSL Mode=Require;Trust Server Certificate=true"; //Use this when you want to connect using SSL
            ptInfoPlugin = checkPtInfoPlugin();

            if (Settings.useFeDB)
            {
                if (!testConnect())
                {
                    MessageBox.Show(Properties.Resources.CouldntOpenConn, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings.useFeDB = false;
                }
            }
        }

        public static string retConnStr()
        {
            return "Host=" + Settings.DBSrvIP + ";Port=" + Settings.DBSrvPort + ";Username=" +
                    Settings.DBconnectID + ";Password=" + Settings.DBconnectPw + ";Database=endoDB;" + Settings.sslSetting;
        }

        public static void saveSettings()
        {
            Settings4file st = new Settings4file();
            st.imgDir = Settings.imgDir;
            st.openFolderButtonVisible = Settings.openFolderButtonVisible;
            st.useDB = Settings.useFeDB;
            if (Settings.useFeDB)
            {
                st.DBSrvIP = Settings.DBSrvIP;
                st.DBSrvPort = Settings.DBSrvPort;
                st.DBconnectID = Settings.DBconnectID;
                st.DBconnectPw = PasswordEncoder.Encrypt(Settings.DBconnectPw);
            }
            else
            {
                st.DBSrvIP = "";
                st.DBSrvPort = "";
                st.DBconnectID = "";
                st.DBconnectPw = "";
            }

            XmlSerializer xserializer = new XmlSerializer(typeof(Settings4file));
            //Open file
            System.IO.FileStream fs1 =
                new System.IO.FileStream(Settings.settingFile_location, System.IO.FileMode.Create);
            xserializer.Serialize(fs1, st);
            fs1.Close();
        }

        //Read from file
        public static void readSettings()
        {
            if (System.IO.File.Exists(Settings.settingFile_location))
            {
                Settings4file st = new Settings4file();

                XmlSerializer xserializer = new XmlSerializer(typeof(Settings4file));
                System.IO.FileStream fs2 =
                    new System.IO.FileStream(Settings.settingFile_location, System.IO.FileMode.Open);
                try
                {
                    st = (Settings4file)xserializer.Deserialize(fs2);
                    fs2.Close();
                }
                catch (InvalidOperationException)
                {
                    DialogResult ret;
                    ret = MessageBox.Show(Properties.Resources.SettingFileBroken, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    fs2.Close();
                    if (ret == DialogResult.Yes)
                    { file_control.delFile(Settings.settingFile_location); }
                }

                Settings.imgDir = st.imgDir;
                Settings.openFolderButtonVisible = st.openFolderButtonVisible;
                Settings.useFeDB = st.useDB;
                Settings.usePlugin = st.usePlugin;
                Settings.DBSrvIP = st.DBSrvIP;
                Settings.DBSrvPort = st.DBSrvPort;
                Settings.DBconnectID = st.DBconnectID;
                Settings.DBconnectPw = PasswordEncoder.Decrypt(st.DBconnectPw);
            }
        }

        public static string checkPtInfoPlugin()
        {
            if (File.Exists(Application.StartupPath + "\\plugins.ini"))
            {
                string text = file_control.readFromFile(Application.StartupPath + "\\plugins.ini");
                string plugin_location = file_control.readItemSettingFromText(text, "Patient information=");
                if (File.Exists(plugin_location))
                { return plugin_location; }
                else
                { return ""; }
            }
            else
            { return ""; }
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

        public static Boolean testConnect()
        {
            try
            {
                using (var conn = new NpgsqlConnection(retConnStr()))
                {
                    try
                    { conn.Open(); }
                    catch (NpgsqlException ex)
                    {
                        MessageBox.Show(Properties.Resources.CouldntOpenConn + "\r\n" + ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                        return false;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(Properties.Resources.ConnClosed + "\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("[testConnect] " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }

                    return true;
                }
            }
            #region catch
            catch (Exception ex)
            {
                MessageBox.Show("[testConnect] " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            #endregion
        }
    }

    [Serializable()]
    public class Settings4file
    {
        public string imgDir { get; set; }
        public Boolean openFolderButtonVisible { get; set; } //Property for PtGraViewer
        public bool useDB { get; set; }
        public bool usePlugin { get; set; }
        public string DBSrvIP { get; set; } //IP address of DB server
        public string DBSrvPort { get; set; } //Port number of DB server
        public string DBconnectID { get; set; } //ID of DB user
        public string DBconnectPw { get; set; } //Pw of DB user
    }
}
