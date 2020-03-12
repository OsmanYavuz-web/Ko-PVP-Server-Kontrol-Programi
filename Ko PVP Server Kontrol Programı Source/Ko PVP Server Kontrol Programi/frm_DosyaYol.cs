using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Media;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.VisualBasic; 

namespace Ko_PVP_Server_Kontrol_Programi
{
    public partial class frm_DosyaYol : Form
    {
        #region DLL DOSYALARI
        //FindWindow
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //SendMassage
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessageTimeoutW(IntPtr hWnd, uint Msg, uint wParam, IntPtr lParam, uint fuFlags, uint uTimeout, ref uint lpdwResult);

        //PostMassageW
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PostMessageW(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        //GetDlgItem
        [DllImport("user32.dll")]
        private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        //GetPrivateProfileString
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //WritePrivateProfileString
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        #endregion

        public frm_DosyaYol()
        {
            InitializeComponent();
        }

        OleDbDataAdapter veriler;
        DataTable tablo;

        #region ANA FORM KODLARI

        #region Form_LOAD
        private void frm_DosyaYol_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1021, 733);//1021; 733
            iniOku();
            #region log Bilgisi
            listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "] Program açılıyor...");
            listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "] '" + this.Text + "' PROGRAM AKTİF. ");
            //textBox_GuncelVersion
            listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] Mevcut sürüm bilgisi : " + textBox_GuncelVersion.Text);
            listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "] GENEL AYARLARDAN AKTİF OLANLAR YÜKLENİYOR...");

            if (check_Baslangic.Checked == true)
            {
                listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] Bilgisayar açıldığında Ko PVP Server Kontrol Programın'ı çalıştır.");
            }

            if (check_DosyaYokUyari.Checked == true)
            {
                listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] 'Dosya Yok!' Uyarısı olduğu zaman beni sesli uyar.");
            }

            listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] Programların zaman aşım süresi " + textBox_ZamanAsim.Text + " sn olarak belirlendi.");

            if (check_LogTutma.Checked == true)
            {
                listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] Log kayıtları tut.");
            }

            if (check_GuncellemeKontrol.Checked == true)
            {
                listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] Güncellemeleri otomatik tara.");
            }
            listBox_LogKayit.Items.Insert(0, "[AKTIF][" + label_Saat.Text + "] GENEL AYARLAR YÜKLENDİ..");
            listBox_LogKayit.Items.Insert(0, "");

            #endregion
            otoPencereKontrol();
            #region Oto Kayıtları Getirme
            NoticeKayıtGetir();
            SavasKayıtlarıGetir();
            #endregion
            dbBaglantiKontrol();
        }
        #endregion

        #region Form_SHOWN
        private void frm_DosyaYol_Shown(object sender, EventArgs e)
        {
            /*Form1 form1 = new Form1();
            form1.Show();
            frm_DosyaYol frm = new frm_DosyaYol();
            frm.Enabled = false;*/
            

        }
        #endregion

        #endregion

        #region Manuel İni Okuma Buton
        private void button1_Click(object sender, EventArgs e)
        {
            iniOku();
        }
        #endregion

        #region INI İşlemleri

        #region ini Tanımlama
        INIOkuyucu INI = new INIOkuyucu(Application.StartupPath + "\\Ayar.ini");
        #endregion

        #region ini Okuma
        private void iniOku()
        {
            #region Server Files
            //LoginServer
            string iniLoginServer = INI.Oku("Yol", "LoginServer");
            textBox_LoginServer.Text = iniLoginServer;
            if (textBox_LoginServer.TextLength == 0)
            {
                label_LoginServer.ForeColor = Color.DarkRed;
                label_LoginServer.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_LoginServer.Text))
                {
                    label_LoginServer.ForeColor = Color.DarkGreen;
                    label_LoginServer.Text = "Dosya Var.";
                }
                else
                {
                    label_LoginServer.ForeColor = Color.DarkRed;
                    label_LoginServer.Text = "Dosya Yok!";
                }
            }

            //AiServer
            string iniAiServer = INI.Oku("Yol", "AiServer");
            textBox_AiServer.Text = iniAiServer;
            if (textBox_AiServer.TextLength == 0)
            {
                label_AiServer.ForeColor = Color.DarkRed;
                label_AiServer.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_AiServer.Text))
                {
                    label_AiServer.ForeColor = Color.DarkGreen;
                    label_AiServer.Text = "Dosya Var.";
                }
                else
                {
                    label_AiServer.ForeColor = Color.DarkRed;
                    label_AiServer.Text = "Dosya Yok!";
                }
            }

            //Ebenezer
            string iniEbenezer = INI.Oku("Yol", "Ebenezer");
            textBox_Ebenezer.Text = iniEbenezer;
            if (textBox_Ebenezer.TextLength == 0)
            {
                label_Ebenezer.ForeColor = Color.DarkRed;
                label_Ebenezer.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_Ebenezer.Text))
                {
                    label_Ebenezer.ForeColor = Color.DarkGreen;
                    label_Ebenezer.Text = "Dosya Var.";
                }
                else
                {
                    label_Ebenezer.ForeColor = Color.DarkRed;
                    label_Ebenezer.Text = "Dosya Yok!";
                }
            }

            //Aujard
            string iniAujard = INI.Oku("Yol", "Aujard ");
            textBox_Aujard.Text = iniAujard;
            if (textBox_Aujard.TextLength == 0)
            {
                label_Aujard.ForeColor = Color.DarkRed;
                label_Aujard.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_Aujard.Text))
                {
                    label_Aujard.ForeColor = Color.DarkGreen;
                    label_Aujard.Text = "Dosya Var.";
                }
                else
                {
                    label_Aujard.ForeColor = Color.DarkRed;
                    label_Aujard.Text = "Dosya Yok!";
                }
            }
            #endregion

            #region Firewall
            //AutoSetup
            string iniAutoSetup = INI.Oku("Yol", "AutoSetup");
            textBox_AutoSetup.Text = iniAutoSetup;
            if (textBox_AutoSetup.TextLength == 0)
            {
                label_AutoSetup.ForeColor = Color.DarkRed;
                label_AutoSetup.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_AutoSetup.Text))
                {
                    label_AutoSetup.ForeColor = Color.DarkGreen;
                    label_AutoSetup.Text = "Dosya Var.";
                }
                else
                {
                    label_AutoSetup.ForeColor = Color.DarkRed;
                    label_AutoSetup.Text = "Dosya Yok!";
                }
            }

            //loadrules
            string iniloadrules = INI.Oku("Yol", "loadrules");
            textBox_loadrules.Text = iniloadrules;
            if (textBox_loadrules.TextLength == 0)
            {
                label_loadrules.ForeColor = Color.DarkRed;
                label_loadrules.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_loadrules.Text))
                {
                    label_loadrules.ForeColor = Color.DarkGreen;
                    label_loadrules.Text = "Dosya Var.";
                }
                else
                {
                    label_loadrules.ForeColor = Color.DarkRed;
                    label_loadrules.Text = "Dosya Yok!";
                }
            }

            //ipfw
            string iniipfw = INI.Oku("Yol", "ipfw");
            textBox_ipfw.Text = iniipfw;
            if (textBox_ipfw.TextLength == 0)
            {
                label_ipfw.ForeColor = Color.DarkRed;
                label_ipfw.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_ipfw.Text))
                {
                    label_ipfw.ForeColor = Color.DarkGreen;
                    label_ipfw.Text = "Dosya Var.";
                }
                else
                {
                    label_ipfw.ForeColor = Color.DarkRed;
                    label_ipfw.Text = "Dosya Yok!";
                }
            }

            //qtfw
            string iniqtfw = INI.Oku("Yol", "qtfw");
            textBox_qtfw.Text = iniqtfw;
            if (textBox_qtfw.TextLength == 0)
            {
                label_qtfw.ForeColor = Color.DarkRed;
                label_qtfw.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_qtfw.Text))
                {
                    label_qtfw.ForeColor = Color.DarkGreen;
                    label_qtfw.Text = "Dosya Var.";
                }
                else
                {
                    label_qtfw.ForeColor = Color.DarkRed;
                    label_qtfw.Text = "Dosya Yok!";
                }
            }
            #endregion

            #region Üçüncü Parti Yazılımlar

            #region Birinci Yazılım
            //Birinci
            string iniBirinci = INI.Oku("Yol", "Birinci");
            textBox_Birinci.Text = iniBirinci;

            string iniBirinciAD = INI.Oku("Yol", "BirinciAD");
            label_Birinci_AD.Text = iniBirinciAD;

            if (textBox_Birinci.TextLength == 0)
            {
                label_Birinci.ForeColor = Color.DarkRed;
                label_Birinci.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_Birinci.Text))
                {
                    label_Birinci.ForeColor = Color.DarkGreen;
                    label_Birinci.Text = "Dosya Var.";
                }
                else
                {
                    label_Birinci.ForeColor = Color.DarkRed;
                    label_Birinci.Text = "Dosya Yok!";
                }
            }
            #endregion

            #region İkinci Yazılım
            //Ikinci
            string iniIkinci = INI.Oku("Yol", "Ikinci");
            textBox_Ikinci.Text = iniIkinci;
            string iniIkinciAD = INI.Oku("Yol", "IkinciAD");
            label_Ikinci_AD.Text = iniIkinciAD;
            if (textBox_Ikinci.TextLength == 0)
            {
                label_Ikinci.ForeColor = Color.DarkRed;
                label_Ikinci.Text = "Seçilmemiş";
            }
            else
            {
                if (System.IO.File.Exists(textBox_Ikinci.Text))
                {
                    label_Ikinci.ForeColor = Color.DarkGreen;
                    label_Ikinci.Text = "Dosya Var.";
                }
                else
                {
                    label_Ikinci.ForeColor = Color.DarkRed;
                    label_Ikinci.Text = "Dosya Yok!";
                }
            }
            #endregion

            #endregion

            #region CheckBox Seçili Programlar

            #region Server Files

            //LoginServer
            if (label_LoginServer.Text == "Seçilmemiş" || label_LoginServer.Text == "Dosya Yok!")
            {
                check_LoginServer.Checked = false;
            }
            else
            {
                string iniLoginServerCheck = INI.Oku("ProgramBaslatma", "LoginServerCheck");
                string secimDurumu = iniLoginServerCheck;
                if (secimDurumu == "True")
                {
                    check_LoginServer.Checked = true;
                }
                else
                {
                    check_LoginServer.Checked = false;
                }
            }

            //AiServer
            if (label_AiServer.Text == "Seçilmemiş" || label_AiServer.Text == "Dosya Yok!")
            {
                check_AiServer.Checked = false;
            }
            else
            {
                string iniAiServerCheck = INI.Oku("ProgramBaslatma", "AiServerCheck");
                string secimDurumu2 = iniAiServerCheck;
                if (secimDurumu2 == "True")
                {
                    check_AiServer.Checked = true;
                }
                else
                {
                    check_AiServer.Checked = false;
                }
            }

            //Ebenezer
            if (label_Ebenezer.Text == "Seçilmemiş" || label_Ebenezer.Text == "Dosya Yok!")
            {
                check_Ebenezer.Checked = false;
            }
            else
            {
                string iniEbenezerCheck = INI.Oku("ProgramBaslatma", "EbenezerCheck");
                string secimDurumu3 = iniEbenezerCheck;
                if (secimDurumu3 == "True")
                {
                    check_Ebenezer.Checked = true;
                }
                else
                {
                    check_Ebenezer.Checked = false;
                }
            }

            //Aujard
            if (label_Aujard.Text == "Seçilmemiş" || label_Aujard.Text == "Dosya Yok!")
            {
                check_Aujard.Checked = false;
            }
            else
            {
                string iniAujardCheck = INI.Oku("ProgramBaslatma", "AujardCheck");
                string secimDurumu4 = iniAujardCheck;
                if (secimDurumu4 == "True")
                {
                    check_Aujard.Checked = true;
                }
                else
                {
                    check_Aujard.Checked = false;
                }
            }
            #endregion

            #region Firewall
            //AutoSetup
            if (label_AutoSetup.Text == "Seçilmemiş" || label_AutoSetup.Text == "Dosya Yok!")
            {
                check_AutoSetup.Checked = false;
            }
            else
            {
                string iniAutoSetupCheck = INI.Oku("ProgramBaslatma", "AutoSetupCheck");
                string secimDurumu5 = iniAutoSetupCheck;
                if (secimDurumu5 == "True")
                {
                    check_AutoSetup.Checked = true;
                }
                else
                {
                    check_AutoSetup.Checked = false;
                }
            }

            //loadrules
            if (label_loadrules.Text == "Seçilmemiş" || label_loadrules.Text == "Dosya Yok!")
            {
                check_loadrules.Checked = false;
            }
            else
            {
                string iniloadrulesCheck = INI.Oku("ProgramBaslatma", "loadrulesCheck");
                string secimDurumu6 = iniloadrulesCheck;
                if (secimDurumu6 == "True")
                {
                    check_loadrules.Checked = true;
                }
                else
                {
                    check_loadrules.Checked = false;
                }
            }
            //ipfw
            if (label_ipfw.Text == "Seçilmemiş" || label_ipfw.Text == "Dosya Yok!")
            {
                check_ipfw.Checked = false;
            }
            else
            {
                string iniipfwCheck = INI.Oku("ProgramBaslatma", "ipfwCheck");
                string secimDurumu7 = iniipfwCheck;
                if (secimDurumu7 == "True")
                {
                    check_ipfw.Checked = true;
                }
                else
                {
                    check_ipfw.Checked = false;
                }
            }
            //qtfw
            if (label_qtfw.Text == "Seçilmemiş" || label_qtfw.Text == "Dosya Yok!")
            {
                check_qtfw.Checked = false;
            }
            else
            {
                string iniqtfwCheck = INI.Oku("ProgramBaslatma", "qtfwCheck");
                string secimDurumu8 = iniqtfwCheck;
                if (secimDurumu8 == "True")
                {
                    check_qtfw.Checked = true;
                }
                else
                {
                    check_qtfw.Checked = false;
                }
            }
            #endregion

            #region Üçüncü Parti Yazımlımlar
            //Birinci
            if (label_Birinci.Text == "Seçilmemiş" || label_Birinci.Text == "Dosya Yok!")
            {
                check_Birinci.Checked = false;
            }
            else
            {
                string iniBirinciCheck = INI.Oku("ProgramBaslatma", "BirinciCheck");
                string secimDurumu9 = iniBirinciCheck;
                if (secimDurumu9 == "True")
                {
                    check_Birinci.Checked = true;
                }
                else
                {
                    check_Birinci.Checked = false;
                }
            }

            //Ikinci
            if (label_Ikinci.Text == "Seçilmemiş" || label_Ikinci.Text == "Dosya Yok!")
            {
                check_Ikinci.Checked = false;
            }
            else
            {
                string iniIkinciCheck = INI.Oku("ProgramBaslatma", "IkinciCheck");
                string secimDurumu10 = iniIkinciCheck;
                if (secimDurumu10 == "True")
                {
                    check_Ikinci.Checked = true;
                }
                else
                {
                    check_Ikinci.Checked = false;
                }
            }
            #endregion

            #endregion

            #region Genel Ayarlar

            #region Başlangıçta Programın Açılması
            try
            {
                string iniBaslangic = INI.Oku("GenelAyar", "Baslangic");
                if (iniBaslangic == "True")
                {
                    RegistryKey regKayit = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    regKayit.SetValue("Ko PVP Server Kontrol Programı", (object)"\"" + Application.ExecutablePath + "\"");
                    check_Baslangic.Checked = true;
                }
                else
                {
                    RegistryKey regSil = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    regSil.DeleteValue("Ko PVP Server Kontrol Programı");
                    check_Baslangic.Checked = false;
                }
            }
            catch { }
            #endregion

            #region Dosya Yok! Uyarısı
            string iniDosyaYokUyari = INI.Oku("GenelAyar", "DosyaYokUyari");
            if (iniDosyaYokUyari == "True")
            {
                check_DosyaYokUyari.Checked = true;
                if (label_LoginServer.Text == "Dosya Yok!" ||
                    label_AiServer.Text == "Dosya Yok!" ||
                    label_Ebenezer.Text == "Dosya Yok!" ||
                    label_Aujard.Text == "Dosya Yok!" ||
                    label_AutoSetup.Text == "Dosya Yok!" ||
                    label_loadrules.Text == "Dosya Yok!" ||
                    label_ipfw.Text == "Dosya Yok!" ||
                    label_qtfw.Text == "Dosya Yok!" ||
                    label_Birinci.Text == "Dosya Yok!" ||
                    label_Ikinci.Text == "Dosya Yok!")
                {
                    SystemSounds.Hand.Play();
                    DialogResult hata = MessageBox.Show("Bazı programların dosyaları yok! Uzantıları veya adları değiştirilmiş olabilir.. Lütfen genel ayarlardan kontrol ediniz!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                check_DosyaYokUyari.Checked = false;
            }
            #endregion

            #region Zaman Aşım
            string iniZamanAsim = INI.Oku("GenelAyar", "ZamanAsim");
            textBox_ZamanAsim.Text = iniZamanAsim;
            #endregion

            #region Log Kayıtları
            string iniLogTutma = INI.Oku("GenelAyar", "LogTutma");
            if (iniLogTutma == "True")
            {
                try
                {

                    string yol = Application.StartupPath;
                    string dosyaAdi = "\\LOG\\Log-";
                    DateTime tarih = DateTime.Now;
                    string dosyaAdi2 = tarih.ToString("yyyy.MM.dd HH.mm.ss");
                    string uzanti = ".txt";
                    string logAdi = "Log-" + dosyaAdi2 + uzanti;
                    string tumu = yol + dosyaAdi + dosyaAdi2 + uzanti;
                    StreamWriter Dosya = File.CreateText(tumu);

                    textBox_LogKayitAdi.Text = logAdi;
                    textBox_LogKayitYol.Text = tumu;
                    textBox_Log.Text = tumu;
                    label_LogDurum.ForeColor = Color.DarkGreen;
                    picture_Log.BackColor = Color.DarkGreen;
                    label_LogDurum.Text = "Aktif!";
                    groupBox_LogPenceresi.Enabled = true;
                    check_LogTutma.Checked = true;

                }
                catch { }
            }
            else
            {
                label_LogDurum.ForeColor = Color.DarkRed;
                picture_Log.BackColor = Color.DarkRed;
                label_LogDurum.Text = "Pasif!";
                textBox_LogKayitAdi.Text = "";
                textBox_LogKayitYol.Text = "";
                textBox_Log.Text = "";
                groupBox_LogPenceresi.Enabled = false;
                check_LogTutma.Checked = false;
            }
            #endregion

            #region Güncelleme Kontrol
            string iniGuncellemeKontrol = INI.Oku("GenelAyar", "GuncellemeKontrol");
            if (iniGuncellemeKontrol == "True")
            {
                check_GuncellemeKontrol.Checked = true;
            }
            else
            {
                check_GuncellemeKontrol.Checked = false;
            }
            string iniGuncelVersion = INI.Oku("GenelAyar", "GuncelVersion");
            textBox_GuncelVersion.Text = iniGuncelVersion;
            if (textBox_GuncelVersion.TextLength == 0)
            {
                textBox_GuncelVersion.Text = "Hata";
            }
            #endregion

            #region Process Kontrol Süresi
            string iniProcessSure = INI.Oku("GenelAyar", "ProcessSure");
            textBox_timer_otoPencereKontrolSure.Text = iniProcessSure;
            #endregion

            #endregion

            #region Log Yolları
            string iniChatLOG = INI.Oku("KayitLogYollari","ChatLOG");
            textBox_ChatLog.Text = iniChatLOG;

            string iniItemLOG = INI.Oku("KayitLogYollari", "ItemLOG");
            textBox_ItemLog.Text = iniItemLOG;

            string iniDeathLOG = INI.Oku("KayitLogYollari", "DeathLOG");
            textBox_DeathLog.Text = iniDeathLOG;

            #endregion

            #region Veritabanı bilgileri
            string iniIP = INI.Oku("Database", "IP");
            textBox_IP.Text = iniIP;

            string iniDB = INI.Oku("Database", "DB");
            textBox_Veritabani.Text = iniDB;
            #endregion

        }
        #endregion

        #region ini Ayarları Kaydetme İşlemleri
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult kaydetSoru = MessageBox.Show("Veriler kaydedilsin mi?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (kaydetSoru == DialogResult.Yes)
            {
                #region Server Files
                //LoginServer
                if (label_LoginServer.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "LoginServer", textBox_LoginServer.Text);
                }

                //AiServer
                if (label_AiServer.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "AiServer", textBox_AiServer.Text);
                }

                //Ebenezer
                if (label_Ebenezer.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Ebenezer", textBox_Ebenezer.Text);
                }

                //Aujard
                if (label_Aujard.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Aujard", textBox_Aujard.Text);
                }

                #endregion

                #region Firewall
                //AutoSetup
                if (label_AutoSetup.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "AutoSetup", textBox_AutoSetup.Text);
                }

                //loadrules
                if (label_loadrules.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "loadrules", textBox_loadrules.Text);
                }

                //ipfw
                if (label_ipfw.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "ipfw", textBox_ipfw.Text);
                }

                //qtfw
                if (label_qtfw.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "qtfw", textBox_qtfw.Text);
                }
                #region Üçüncü Parti Yazılımlar
                //Birinci
                if (label_Birinci.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Birinci", textBox_Birinci.Text);
                }

                //Ikinci
                if (label_Ikinci.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Ikinci", textBox_Ikinci.Text);
                }


                #endregion
                #endregion

                #region Üçüncü Parti Yazılımlar
                //Birinci
                if (label_Birinci.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Birinci", textBox_Birinci.Text);
                    //ini Yaz
                    INI.Yaz("Yol", "BirinciAD", label_Birinci_AD.Text);
                }

                //Ikinci
                if (label_Ikinci.Text == "Başarılı")
                {
                    //ini Yaz
                    INI.Yaz("Yol", "Ikinci", textBox_Ikinci.Text);
                    //ini Yaz
                    INI.Yaz("Yol", "IkinciAD", label_Ikinci_AD.Text);
                }


                #endregion

                #region CheckBox Seçili Programlar

                #region Server Files
                //LoginServer
                if (check_LoginServer.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "LoginServerCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "LoginServerCheck", "True");
                }
                //AiServer
                if (check_AiServer.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AiServerCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AiServerCheck", "True");
                }
                //Ebenezer
                if (check_Ebenezer.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "EbenezerCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "EbenezerCheck", "True");
                }
                //Aujard
                if (check_Aujard.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AujardCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AujardCheck", "True");
                }
                #endregion

                #region Firewall
                //AutoSetup
                if (check_AutoSetup.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AutoSetupCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "AutoSetupCheck", "True");
                }

                //loadrules
                if (check_loadrules.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "loadrulesCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "loadrulesCheck", "True");
                }

                //ipfw
                if (check_ipfw.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "ipfwCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "ipfwCheck", "True");
                }

                //qtfw
                if (check_qtfw.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "qtfwCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "qtfwCheck", "True");
                }
                #endregion

                #region Üçüncü Parti Yazımlımlar
                //Birinci
                if (check_Birinci.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "BirinciCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "BirinciCheck", "True");
                }
                //Ikinci
                if (check_Ikinci.Checked == false)
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "IkinciCheck", "False");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("ProgramBaslatma", "IkinciCheck", "True");
                }
                #endregion

                #region Genel Ayarlar

                #region Baslangic
                if (check_Baslangic.Checked == true)
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "Baslangic", "True");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "Baslangic", "False");
                }
                #endregion

                #region Dosya Yok Uyarısı
                if (check_DosyaYokUyari.Checked == true)
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "DosyaYokUyari", "True");
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "DosyaYokUyari", "False");
                }
                #endregion

                #region Zaman Aşım
                //ini Yaz
                INI.Yaz("GenelAyar", "ZamanAsim", textBox_ZamanAsim.Text);

                #endregion

                #region Acilmayan Program Uyarısı
                #endregion

                #region Log Tutma
                //LogTutma
                if (check_LogTutma.Checked == true)
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "LogTutma", "True");
                    string yol = Application.StartupPath;
                    string dosyaAdi = "\\LOG\\";
                    INI.Yaz("GenelAyar", "LogYol", yol + dosyaAdi);
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "LogTutma", "False");
                }
                #endregion

                #region Güncelleme Kontrol
                if (check_GuncellemeKontrol.Checked == true)
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "GuncellemeKontrol", "True");
                    INI.Yaz("GenelAyar", "GuncelVersion", textBox_GuncelVersion.Text);
                }
                else
                {
                    //ini Yaz
                    INI.Yaz("GenelAyar", "GuncellemeKontrol", "False");
                }
                #endregion

                #endregion

                #endregion

                DialogResult soru = MessageBox.Show("Veriler başarıyla kaydedildi.","İşlem",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                
                iniOku();
                LogKayit();
                //ProgramlarıBaslat();
                otoPencereKontrol();

            }
            else if (kaydetSoru == DialogResult.No)
            {
                DialogResult soru = MessageBox.Show("Verileri kaydetmeyi iptal ettiniz.", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region Veritabanı Bilgileri Kaydetme
        private void değiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //("Veritabanı IP adresiniz:", "IP değiştirme", textBox_IP.Text);

           // INI.Yaz("Database", "IP", IPdegistirme);
            string value = textBox_IP.Text;
            if (InputBox("Veritabanı IP adresiniz:", "IP değiştirme", ref value) == DialogResult.OK)
            {
                textBox_IP.Text = value;
                INI.Yaz("Database", "IP", value);
                DialogResult mesaj = MessageBox.Show("Bağlantıyı yenilemeyi unutmayın!", "Bildirim;", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            
        }
        private void değiştirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string value = textBox_Veritabani.Text;
            if (InputBox("Veritabanı adınız:", "Veritabanı adı değiştirme", ref value) == DialogResult.OK)
            {
                textBox_Veritabani.Text = value;
                INI.Yaz("Database", "DB", value);
                DialogResult mesaj = MessageBox.Show("Bağlantıyı yenilemeyi unutmayın!", "Bildirim;", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        #endregion

        #endregion

        #endregion

        #region Server Files Dosya Yolları

        #region LoginServer
        private void button_LoginServer_Click(object sender, EventArgs e)
        {
            if (open_LoginServer.ShowDialog() == DialogResult.OK)
            {
                string yolLoginServer = open_LoginServer.FileName;
                textBox_LoginServer.Text = yolLoginServer;
                if (textBox_LoginServer.Text.IndexOf("Version_Manager.exe") != -1)
                {
                    if (System.IO.File.Exists(yolLoginServer))
                    {
                        label_LoginServer.ForeColor = Color.DarkGreen;
                        label_LoginServer.Text = "Başarılı";

                    }
                }
                else
                {
                    textBox_LoginServer.Text = "";
                    label_LoginServer.ForeColor = Color.DarkRed;
                    label_LoginServer.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_LoginServer.TextLength == 0)
                {
                    label_LoginServer.ForeColor = Color.Black;
                    button_LoginServer.Text = "Bekleniyor..";
                    DialogResult soru = MessageBox.Show("LoginServer Seçilmedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region AiServer
        private void button_AiServer_Click(object sender, EventArgs e)
        {

            if (open_AiServer.ShowDialog() == DialogResult.OK)
            {
                string yolAiServer = open_AiServer.FileName;
                textBox_AiServer.Text = yolAiServer;
                if (textBox_AiServer.Text.IndexOf("AiServer.exe") != -1)
                {
                    if (System.IO.File.Exists(yolAiServer))
                    {
                        label_AiServer.ForeColor = Color.DarkGreen;
                        label_AiServer.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_AiServer.Text = "";
                    label_AiServer.ForeColor = Color.DarkRed;
                    label_AiServer.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_AiServer.TextLength == 0)
                {
                    label_AiServer.ForeColor = Color.Black;
                    button_AiServer.Text = "Bekleniyor..";
                    DialogResult soru = MessageBox.Show("AiServer Seçilmedi!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        #endregion

        #region Ebenezer
        private void button_Ebenezer_Click(object sender, EventArgs e)
        {
            if (open_Ebenezer.ShowDialog() == DialogResult.OK)
            {
                string yolEbenezer = open_Ebenezer.FileName;
                textBox_Ebenezer.Text = yolEbenezer;
                if (textBox_Ebenezer.Text.IndexOf("Ebenezer.exe") != -1)
                {
                    if (System.IO.File.Exists(yolEbenezer))
                    {
                        label_Ebenezer.ForeColor = Color.DarkGreen;
                        label_Ebenezer.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_Ebenezer.Text = "";
                    label_Ebenezer.ForeColor = Color.DarkRed;
                    label_Ebenezer.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_Ebenezer.TextLength == 0)
                {
                    label_Ebenezer.ForeColor = Color.Black;
                    button_Ebenezer.Text = "Bekleniyor..";
                    DialogResult soru = MessageBox.Show("Ebenezer Seçilmedi!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        #endregion

        #region Aujard
        private void button_Aujard_Click(object sender, EventArgs e)
        {
            if (open_Aujard.ShowDialog() == DialogResult.OK)
            {
                string yolAujard = open_Aujard.FileName;
                textBox_Aujard.Text = yolAujard;
                if (textBox_Aujard.Text.IndexOf("Aujard.exe") != -1)
                {
                    if (System.IO.File.Exists(yolAujard))
                    {
                        label_Aujard.ForeColor = Color.DarkGreen;
                        label_Aujard.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_Aujard.Text = "";
                    label_Aujard.ForeColor = Color.DarkRed;
                    label_Aujard.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_Aujard.TextLength == 0)
                {
                    label_Aujard.ForeColor = Color.Black;
                    button_Aujard.Text = "Bekleniyor..";
                    DialogResult soru = MessageBox.Show("Aujard Seçilmedi!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        #endregion

        #endregion

        #region Firewall Dosya Yolları

        #region AutoSetup
        private void button_AutoSetup_Click(object sender, EventArgs e)
        {
            if (open_AutoSetup.ShowDialog() == DialogResult.OK)
            {
                string yolAutoSetup = open_AutoSetup.FileName;
                textBox_AutoSetup.Text = yolAutoSetup;
                if (textBox_AutoSetup.Text.IndexOf("AutoSetup.bat") != -1)
                {
                    if (System.IO.File.Exists(yolAutoSetup))
                    {
                        label_AutoSetup.ForeColor = Color.DarkGreen;
                        label_AutoSetup.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_AutoSetup.Text = "";
                    label_AutoSetup.ForeColor = Color.DarkRed;
                    label_AutoSetup.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_AutoSetup.TextLength == 0)
                {
                    label_AutoSetup.ForeColor = Color.Black;
                    button_AutoSetup.Text = "Bekleniyor..";
                    MessageBox.Show("AutoSetup Seçilmedi!", "Hata;");
                }
            }
        }
        #endregion

        #region loadrules
        private void button_loadrules_Click(object sender, EventArgs e)
        {
            if (open_loadrules.ShowDialog() == DialogResult.OK)
            {
                string yolloadrules = open_loadrules.FileName;
                textBox_loadrules.Text = yolloadrules;
                if (textBox_loadrules.Text.IndexOf("loadrules.cmd") != -1)
                {
                    if (System.IO.File.Exists(yolloadrules))
                    {
                        label_loadrules.ForeColor = Color.DarkGreen;
                        label_loadrules.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_loadrules.Text = "";
                    label_loadrules.ForeColor = Color.DarkRed;
                    label_loadrules.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_loadrules.TextLength == 0)
                {
                    label_loadrules.ForeColor = Color.Black;
                    button_loadrules.Text = "Bekleniyor..";
                    MessageBox.Show("loadrules Seçilmedi!", "Hata;");
                }
            }
        }
        #endregion

        #region ipfw
        private void button_ipfw_Click(object sender, EventArgs e)
        {
            if (open_ipfw.ShowDialog() == DialogResult.OK)
            {
                string yolipfw = open_ipfw.FileName;
                textBox_ipfw.Text = yolipfw;
                if (textBox_ipfw.Text.IndexOf("ipfw.exe") != -1)
                {
                    if (System.IO.File.Exists(yolipfw))
                    {
                        label_ipfw.ForeColor = Color.DarkGreen;
                        label_ipfw.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_ipfw.Text = "";
                    label_ipfw.ForeColor = Color.DarkRed;
                    label_ipfw.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_ipfw.TextLength == 0)
                {
                    label_ipfw.ForeColor = Color.Black;
                    button_ipfw.Text = "Bekleniyor..";
                    MessageBox.Show("ipfw Seçilmedi!", "Hata;");
                }
            }
        }
        #endregion

        #region qtfw
        private void button_qtfw_Click(object sender, EventArgs e)
        {
            if (open_qtfw.ShowDialog() == DialogResult.OK)
            {
                string yolqtfw = open_qtfw.FileName;
                textBox_qtfw.Text = yolqtfw;
                if (textBox_qtfw.Text.IndexOf("qtfw.exe") != -1)
                {
                    if (System.IO.File.Exists(yolqtfw))
                    {
                        label_qtfw.ForeColor = Color.DarkGreen;
                        label_qtfw.Text = "Başarılı";
                    }
                }
                else
                {
                    textBox_qtfw.Text = "";
                    label_qtfw.ForeColor = Color.DarkRed;
                    label_qtfw.Text = "Başarısız";
                }
            }
            else
            {
                if (textBox_qtfw.TextLength == 0)
                {
                    label_qtfw.ForeColor = Color.Black;
                    button_qtfw.Text = "Bekleniyor..";
                    MessageBox.Show("qtfw Seçilmedi!", "Hata;");
                }
            }
        }
        #endregion

        #endregion

        #region Üçüncü Parti Yazılım Dosya Yolları

        #region Birinci Yazılım
        private void button_Birinci_Click(object sender, EventArgs e)
        {
            if (open_Birinci.ShowDialog() == DialogResult.OK)
            {
                string yolBirinci = open_Birinci.FileName;
                textBox_Birinci.Text = yolBirinci;
                label_Birinci_AD.Text = open_Birinci.SafeFileName;
                label_Birinci.ForeColor = Color.DarkGreen;
                label_Birinci.Text = "Başarılı";
            }
            else
            {
                DialogResult soru = MessageBox.Show("Dosya Seçmediniz!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion

        #region İkinci Yazılım
        private void button_Ikinci_Click(object sender, EventArgs e)
        {
            if (open_Ikinci.ShowDialog() == DialogResult.OK)
            {
                string yolIkinci = open_Ikinci.FileName;
                textBox_Ikinci.Text = yolIkinci;
                label_Ikinci_AD.Text = open_Ikinci.SafeFileName;
                label_Ikinci.ForeColor = Color.DarkGreen;
                label_Ikinci.Text = "Başarılı";
            }
            else
            {
                DialogResult soru = MessageBox.Show("Dosya Seçmediniz!", "Hata;", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion


        #endregion

        #region Pencere Kontrolleri

        #region otoPencereKontrol
        private void otoPencereKontrol()
        {
            LoginServerProcessKontrol();
            AiServerProcessKontrol();
            EbenezerProcessKontrol();
            AujardProcessKontrol();
            BirinciProcessKontrol();
            IkinciProcessKontrol();
            FirewallProcessKontrol();
            ChatLogKontrol();
            ItemLogKontrol();
            DeathLogKontrol();
        }
        #endregion


        /*
         listBox_LogKayit.Items.Insert(0,"[HATA]["+zaman.ToString("yyyy.MM.dd HH:mm:ss")+"] LoginServer (Version_Manager.exe) çalışmıyor.");
        */

        #region AçıkProgramlarıListeleme
        private void timer_otoPencereKontrol_Tick(object sender, EventArgs e)
        {
            try
            {
                Process[] Memory = Process.GetProcesses();
                listBox_ProcessAdi.Items.Clear();
                foreach (Process prc in Memory)
                    listBox_ProcessAdi.Items.Add(String.Format(prc.ProcessName));
                otoPencereKontrol();
                ProcessAlma();
                //ProgramlarıBaslat();
                label_Listbox_AktifPencere.Text = "Aktif Pencere : " + listBox_ProcessAdi.Items.Count.ToString();
                label_LogİslemToplam.Text = listBox_LogKayit.Items.Count.ToString();
            }
            catch
            {
                listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "] Oto Pencere Kontrol Sisteminde Hata Oluştu.");
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            otoPencereKontrol();
        }
        private void button_timer_otoPencereKontrolSure_Click(object sender, EventArgs e)
        {
            #region Process Süre Kaydetme
            //ini Yaz
            INI.Yaz("GenelAyar", "ProcessSure", textBox_timer_otoPencereKontrolSure.Text);
            #endregion

            timer_otoPencereKontrol.Interval = int.Parse(textBox_timer_otoPencereKontrolSure.Text);
        }
        #endregion

        #region Server Files
        //LoginServer
        private void LoginServerProcessKontrol()
        {
            if (check_LoginServer.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf("Version_Manager") != -1)
                {
                    picture_LoginServer.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] LoginServer (Version_Manager.exe) aktif.");
                }
                else
                {
                    picture_LoginServer.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] LoginServer (Version_Manager.exe) çalışmıyor.");

                }
            }
            else
            {
                picture_LoginServer.BackColor = Color.Yellow;
            }

        }
        //AiServer
        private void AiServerProcessKontrol()
        {
            if (check_AiServer.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf("AiServer") != -1)
                {
                    picture_AiServer.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] AiServer (AiServer.exe) aktif.");
                }
                else
                {
                    picture_AiServer.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] AiServer (AiServer.exe) çalışmıyor.");
                }
            }
            else
            {
                picture_AiServer.BackColor = Color.Yellow;
            }
        }
        //Ebenezer
        private void EbenezerProcessKontrol()
        {
            if (check_Ebenezer.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf("Ebenezer") != -1)
                {
                    picture_Ebenezer.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] Ebenezer (Ebenezer.exe) aktif.");
                    EbenezerKomutKontrol();
                }
                else
                {
                    picture_Ebenezer.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] Ebenezer (Ebenezer.exe) çalışmıyor.");
                    //Ebenezer Komut Kontrol
                    picture_Ebenezer2.BackColor = Color.DarkRed;
                    label_EbenezerDurum.Text = "Bağlanamadı";
                    groupBox_EbenezerSender.Enabled = false;

                }
            }
            else
            {
                picture_Ebenezer.BackColor = Color.Yellow;
            }
        }
        //Aujard
        private void AujardProcessKontrol()
        {
            if (check_Aujard.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf("Aujard") != -1)
                {
                    picture_Aujard.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] Aujard (Aujard.exe) aktif.");

                }
                else
                {
                    picture_Aujard.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] Aujard (Aujard.exe) çalışmıyor.");

                }
            }
            else
            {
                picture_Aujard.BackColor = Color.Yellow;
            }
        }
        #endregion

        #region Firewall
        //AutoSetup
        string AutoSetupID = "AutoSetup";
        string ipfwID = "ipfw";
        string qtfwID = "qrfw";
        string autosetupDURUM;
        string ipfwDURUM;
        string qtfwDURUM;
        private void FirewallProcessKontrol()
        {
            if (check_AutoSetup.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf(AutoSetupID) != -1)
                {
                    autosetupDURUM = "var";
                }
                else
                {
                    autosetupDURUM = "yok";
                }

            }

            if (check_ipfw.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf(ipfwID) != -1)
                {
                    ipfwDURUM = "var";
                }
                else
                {
                    ipfwDURUM = "yok";
                }
            }

            if (check_ipfw.Checked == true)
            {
                if (listBox_ProcessAdi.Items.IndexOf(qtfwID) != -1)
                {
                    qtfwDURUM = "var";
                }
                else
                {
                    qtfwDURUM = "yok";
                }
            }
            if (check_ipfw.Checked == true && check_qtfw.Checked == true)
            {
                if (ipfwDURUM == "var" && qtfwDURUM == "var")
                {
                    picture_Firewall.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] Firewall aktif.");

                }
                else
                {
                    picture_Firewall.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] Firewall çalışmıyor.");

                }
            }
            else
            {
                picture_Firewall.BackColor = Color.Yellow;

            }
        }
        #endregion

        #region Üçüncü Parti Yazılımları
        //Birinci
        private void BirinciProcessKontrol()
        {
            if (check_Birinci.Checked == true)
            {
                string birinciPencereID = label_Birinci_AD.Text.Replace(".exe", "");
                if (listBox_ProcessAdi.Items.IndexOf(birinciPencereID) != -1)
                {
                    picture_Birinci.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] " + birinciPencereID + "(" + birinciPencereID + ".exe) aktif.");

                }
                else
                {
                    picture_Birinci.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] " + birinciPencereID + "(" + birinciPencereID + ".exe) çalışmıyor.");

                }

            }
            else
            {
                picture_Birinci.BackColor = Color.Yellow;
            }
        }
        //Ikinci
        private void IkinciProcessKontrol()
        {
            if (check_Ikinci.Checked == true)
            {
                string ikinciPencereID = label_Ikinci_AD.Text.Replace(".exe", "");

                if (listBox_ProcessAdi.Items.IndexOf(ikinciPencereID) != -1)
                {
                    picture_Ikinci.BackColor = Color.DarkGreen;
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] " + ikinciPencereID + "(" + ikinciPencereID + ".exe) aktif.");

                }
                else
                {
                    picture_Ikinci.BackColor = Color.DarkRed;
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] " + ikinciPencereID + "(" + ikinciPencereID + ".exe) çalışmıyor.");

                }
            }
            else
            {
                picture_Ikinci.BackColor = Color.Yellow;
            }
        }
        #endregion

        #region Kayıt Konumları

        #region ChatLog
        private void ChatLogKontrol()
        {
            string ChatLogID = textBox_ChatLog.Text;

            if (Directory.Exists(ChatLogID))
            {
                picture_ChatLog.BackColor = Color.DarkGreen;
                listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] [" + ChatLogID + "] ChatLog konumu bulundu.");

            }
            else
            {
                picture_ChatLog.BackColor = Color.DarkRed;
                listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] [" + ChatLogID + "] ChatLog konumu bulunamadı!");

            }
        }
        #endregion

        #region ItemLog
        private void ItemLogKontrol()
        {
            string ItemLogID = textBox_ItemLog.Text;

            if (Directory.Exists(ItemLogID))
            {
                picture_ItemLog.BackColor = Color.DarkGreen;
                listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] [" + ItemLogID + "] ItemLog konumu bulundu.");

            }
            else
            {
                picture_ItemLog.BackColor = Color.DarkRed;
                listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] [" + ItemLogID + "] ItemLog konumu bulunamadı!");

            }
        }
        #endregion

        #region DeathLog
        private void DeathLogKontrol()
        {
            string DeathLogID = textBox_DeathLog.Text;

            if (Directory.Exists(DeathLogID))
            {
                picture_DeathLog.BackColor = Color.DarkGreen;
                listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] [" + DeathLogID + "] DeathLog konumu bulundu.");

            }
            else
            {
                picture_DeathLog.BackColor = Color.DarkRed;
                listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] [" + DeathLogID + "] DeathLog konumu bulunamadı!");

            }
        }
        #endregion

        #endregion

        #endregion

        #region Log İşlemleri
        #region Log Kaydı
        private void LogKayit()
        {
            try
            {
                if (label_LogDurum.Text == "Aktif!")
                {
                    String[] sitesDizi = new String[listBox_LogKayit.Items.Count];
                    listBox_LogKayit.Items.CopyTo(sitesDizi, 0);
                    File.WriteAllLines(textBox_LogKayitYol.Text, sitesDizi);
                    listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "]" + textBox_LogKayitAdi.Text + " isimli .txt dosyasına loglar kaydedildi.");
                }
            }
            catch
            {
                listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "]" + textBox_LogKayitAdi.Text + "isimli .txt log dosyası kaydedilemedi.");

            }

        }



        #endregion
        #region Log Klasörünü Aç
        private void button_LogDosyasiniAc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\LOG");
        }
        #endregion
        #endregion

        #region Form Çıkış
        private void frm_DosyaYol_FormClosing(object sender, FormClosingEventArgs e)
        {
            listBox_LogKayit.Items.Insert(0, "[BILGI][" + label_Saat.Text + "]" + " programdan çıkış yapıldı.");
            listBox_LogKayit.Items.Insert(0, "");
            LogKayit();
            Application.Exit();
        }
        #endregion

        #region Program Zaman Aşım Süre
        private void ZamanAsım()
        {
            System.Threading.Thread.Sleep(int.Parse(textBox_ZamanAsim.Text + "000"));
        }
        #endregion

        #region Process İşlemleri

        #region Process Başlık Alma
        private void ProcessAlma()
        {

            Process[] Memory = Process.GetProcesses();
            listBox_ProcessPID.Items.Clear();
            //listBox_ProcessAdi.Items.Clear();
            listBox_ProcessBaslik.Items.Clear();
            foreach (Process prc in Memory)
            {
                listBox_ProcessPID.Items.Add(String.Format(prc.Id.ToString()));
                //listBox_ProcessAdi.Items.Add(String.Format(prc.ProcessName));
                listBox_ProcessBaslik.Items.Add(String.Format(prc.MainWindowTitle));
            }
        }
        #endregion

        #region Process ListBox İşlemleri
        private void listBox_ProcessPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_ProcessAdi.SelectedIndex = listBox_ProcessPID.SelectedIndex;
            listBox_ProcessBaslik.SelectedIndex = listBox_ProcessPID.SelectedIndex;

        }
        private void listBox_ProcessAdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_ProcessPID.SelectedIndex = listBox_ProcessAdi.SelectedIndex;
            listBox_ProcessBaslik.SelectedIndex = listBox_ProcessAdi.SelectedIndex;
        }

        private void listBox_ProcessBaslik_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_ProcessPID.SelectedIndex = listBox_ProcessBaslik.SelectedIndex;
            listBox_ProcessAdi.SelectedIndex = listBox_ProcessBaslik.SelectedIndex;
        }
        #endregion

        private void button7_Click_1(object sender, EventArgs e)
        {
            ProcessAlma();
        }

        #endregion

        #region Program Başlatma İşlemleri

        #region Program Başlatma Sırası
        private void ProgramlarıBaslat()
        {
            LoginServerBaslatma();
            ZamanAsım();
            AiServerBaslatma();
        }
        #endregion

        #region Server Files
        //LoginServer
        private void LoginServerBaslatma()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "Version_Manager")
                {
                    MessageBox.Show("aktif");
                }
                else
                {

                    string LoginServerID = "VersionManager";
                    Process.Start(textBox_LoginServer.Text);
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] " + LoginServerID + "(" + LoginServerID + ".exe) Program Başlatılıyor...");

                }
            }
        }
        //AiServer
        private void AiServerBaslatma()
        {
            string AiServerID = "AiServer";
            if (check_AiServer.Checked == true)
            {
                if (listBox_ProcessBaslik.Items.IndexOf("VersionManager") != -1)
                {
                    Process.Start(textBox_AiServer.Text);
                    listBox_LogKayit.Items.Insert(0, "[DURUM][" + label_Saat.Text + "] " + AiServerID + "(" + AiServerID + ".exe) Program Başlatılıyor...");
                }
                else
                {
                    listBox_LogKayit.Items.Insert(0, "[HATA][" + label_Saat.Text + "] " + AiServerID + "(" + AiServerID + ".exe) Program Başlatılamadı!...");

                }
            }
        }
        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            ProgramlarıBaslat();
            //Process.Start(textBox_LoginServer.Text);

        }

        #endregion

        #region Sistem Saati Anlık
        private void timer_AnlikZaman_Tick(object sender, EventArgs e)
        {
            DateTime zaman = DateTime.Now;
            label_Saat.Text = zaman.ToString("yyyy.MM.dd HH:mm:ss");

            //Zaman
            DateTime now = DateTime.Now;
            textBox_Bugun.Text = now.DayOfWeek.ToString();

            //Saat
            DateTime now2 = DateTime.Now;
            string saat = now2.ToString("HH:mm:ss");
            textBox2.Text = saat;
        }
        #endregion

        #region Ebenezer Uzaktan Erişim İşlemleri
        //Komut Kullanım: EbenezerKomutGonder("Komutİçeriği");
        private void EbenezerKomutGonder(string Komut)
        {
            IntPtr ebenezerFW = FindWindow(null, textBox_EbenezerBaslik.Text);
            if (ebenezerFW != IntPtr.Zero)
            {
                IntPtr dlgItem = GetDlgItem(ebenezerFW, 0x3ea);
                if (dlgItem != IntPtr.Zero)
                {
                    IntPtr lParam = Marshal.StringToBSTR(Komut);
                    uint lpdwResult = 0;
                    SendMessageTimeoutW(dlgItem, 12, 0, lParam, 0, 100, ref lpdwResult);
                    Marshal.FreeBSTR(lParam);
                    Thread.Sleep(100);
                    PostMessageW(ebenezerFW, 0x100, 13, 0);
                    Thread.Sleep(0x19);
                }
            }
        }
        #endregion

        #region Ebenezer Sender
        private void EbenezerKomutKontrol()
        {
            if (textBox_EbenezerBaslik.TextLength == 0)
            {
                textBox_EbenezerBaslik.Text = "GameServer Ver - 20041111";
            }
            else
            {
                picture_Ebenezer2.BackColor = Color.DarkGreen;
                textBox_EbenezerBaslik2.Text = textBox_EbenezerBaslik.Text;
                label_EbenezerDurum.Text = "Bağlandı";
                groupBox_EbenezerSender.Enabled = true;
            }
        }

        private void button_KomutGonder_Click(object sender, EventArgs e)
        {
            EbenezerKomutGonder(textBox_Komutİcerik.Text);
            listBox_KomutGecmisi.Items.Insert(0, "[KOMUT][" + label_Saat.Text + "] " + textBox_Komutİcerik.Text);

        }

        #endregion

        #region Oto Notice

        #region Notice Kayıt Aktar
        private void NoticeKayıtGetir()
        {
            OleDbDataAdapter adaptor = new OleDbDataAdapter("SELECT * FROM Notice", db.baglanti);
            DataSet ds = new DataSet();
            ds.Clear();
            adaptor.Fill(ds, "Notice");
            data_NoticeKayitTablosu.DataSource = ds.Tables["Notice"];
            data_NoticeKayitTablosu2.DataSource = ds.Tables["Notice"];

            adaptor.Dispose();

            label_NoticeToplamVKayit.Text = "Toplam Kayıt " + data_NoticeKayitTablosu.RowCount.ToString();
        
        }
        #endregion

        #region Notice Kayıt Ekle
        private void button_NoticeKaydet_Click(object sender, EventArgs e)
        {
            
            if (textBox_NoticeMesaj.Text == "")
            {
                DialogResult soru = MessageBox.Show("Notice kutucuğunu boş bırakamazsınız.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                timer_SavasBaslangicZaman.Enabled = false;
                timer_SavasBitisZaman.Enabled = false;
                timer_NoticeGönder.Enabled = false;
                string notice = textBox_NoticeMesaj.Text;
                string zaman = textBox_NoticeZaman.Text;

                #region Seçilen Günler
                string SectigimizVeriler = "";
                for (int i = 0; i < checked_NoticeGunler.CheckedItems.Count; i++)
                {
                    SectigimizVeriler += checked_NoticeGunler.CheckedItems[i].ToString() + " ";
                }
                #endregion
                string pazartesi = null;
                string sali = null;
                string carsamba = null;
                string persembe = null;
                string cuma = null;
                string cumartesi = null;
                string pazar = null;
                #region Günler
                //Pazartesi Seçili ise
                if (SectigimizVeriler.IndexOf("Pazartesi") != -1)
                {
                    pazartesi = "#";
                }
                else
                {
                    pazartesi = "-";
                }

                //Salı Seçili ise
                if (SectigimizVeriler.IndexOf("Salı") != -1)
                {
                    sali = "#";
                }
                else
                {
                    sali = "-";
                }

                //Çarşamba Seçili ise
                if (SectigimizVeriler.IndexOf("Çarşamba") != -1)
                {
                    carsamba = "#";
                }
                else
                {
                    carsamba = "-";
                }

                //Perşembe Seçili ise
                if (SectigimizVeriler.IndexOf("Perşembe") != -1)
                {
                    persembe = "#";
                }
                else
                {
                    persembe = "-";
                }

                //Cuma Seçili ise
                if (SectigimizVeriler.IndexOf("Cuma ") != -1)
                {
                    cuma = "#";
                }
                else
                {
                    cuma = "-";
                }

                //Cumartesi Seçili ise
                if (SectigimizVeriler.IndexOf("Cumartesi") != -1)
                {
                    cumartesi = "#";
                }
                else
                {
                    cumartesi = "-";
                }

                //Pazar Seçili ise
                if (SectigimizVeriler.IndexOf("Pazarr") != -1)
                {
                    pazar = "#";
                }
                else
                {
                    pazar = "-";
                }
                #endregion

                db.baglan();
                string Yenikayıt = string.Format("INSERT INTO Notice(Notice,Zaman,Pazartesi,Sali,Carsamba,Persembe,Cuma,Cumartesi,Pazar)values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", notice, zaman, pazartesi, sali, carsamba, persembe, cuma, cumartesi, pazar);
                DialogResult soru = MessageBox.Show("Notice kayıt edilsin mi?", "İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (soru == DialogResult.Yes)
                {
                    new OleDbCommand(Yenikayıt, db.baglanti).ExecuteNonQuery();
                    NoticeKayıtGetir();
                    timer_SavasBaslangicZaman.Enabled = true;
                    timer_SavasBitisZaman.Enabled = true;
                    timer_NoticeGönder.Enabled = true;

                }
                db.kes();
                
            }


        }
        #endregion

        #region Notice Kayıt Silme
        private void button_NoticeSil_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Seçili kayıt silinsin mi?", "İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                try
                {
                    string sorgu = "DELETE FROM Notice WHERE ID=" + data_NoticeKayitTablosu.CurrentRow.Cells[0].Value.ToString() + "";
                    OleDbCommand komut = new OleDbCommand(sorgu, db.baglanti);
                    db.baglan();
                    komut.ExecuteNonQuery();
                    db.kes();
                    NoticeKayıtGetir();
                }
                catch
                {
                    DialogResult soru2 = MessageBox.Show("Silme İşlemi Başarısız", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }
            }
        }
        #endregion

        #region Notice Zaman Arama ve Aktarma
        string noticeAlma = null;
        private void timer_NoticeGönder_Tick(object sender, EventArgs e)
        {
            try
            {
                //Saat
                DateTime now2 = DateTime.Now;
                string saat = now2.ToString("HH:mm:ss");

                string sorgu = "SELECT * FROM Notice WHERE ";
                sorgu += string.Format("Zaman='{0}'", saat);
                db.baglan();
                veriler = new OleDbDataAdapter(sorgu, db.baglanti);
                tablo = new DataTable();
                veriler.Fill(tablo);
                data_NoticeKayitTablosu2.DataSource = tablo;
                db.kes();

                textBox_Notice.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[1].Value.ToString();
                noticeAlma = textBox_Notice.Text;
                textBox_Zaman.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[2].Value.ToString();
                textBox_Pazartesi.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[3].Value.ToString();
                textBox_Sali.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[4].Value.ToString();
                textBox_Carsamba.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[5].Value.ToString();
                textBox_Persembe.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[6].Value.ToString();
                textBox_Cuma.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[7].Value.ToString();
                textBox_Cumartesi.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[8].Value.ToString();
                textBox_Pazar.Text = data_NoticeKayitTablosu2.CurrentRow.Cells[9].Value.ToString();
                
                NoticeYakalama();


            }
            catch { }
        }

       
        #endregion

        #region Notice Yakalama
        private void NoticeYakalama()
        {

            #region Pazartesi
            //Pazartesi
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Monday" == textBox_Bugun.Text)
                {
                    if (textBox_Pazartesi.Text == "#")
                    {
                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";
                

                    }
                }
            }
            #endregion

            #region Salı
            //Salı
            if (textBox_Zaman.Text == "") { }
            else 
            {
                if ("Tuesday" == textBox_Bugun.Text)
                {
                    if (textBox_Sali.Text == "#") 
                    {
                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";
                       // 
                    }
                }
            }
            #endregion

            #region Çarşamba
            //Çarşamba
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Wednesday" == textBox_Bugun.Text)
                {
                    if (textBox_Carsamba.Text == "#")
                    {

                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";

                    }
                }
            }
            #endregion

            #region Perşembe
            //Perşembe
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Thursday" == textBox_Bugun.Text)
                {
                    if (textBox_Persembe.Text == "#")
                    {
                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";

                    }
                }
            }
            #endregion

            #region Cuma
            //Cuma
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Friday" == textBox_Bugun.Text)
                {
                    if (textBox_Cuma.Text == "#")
                    {
                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";
                    }
                }
            }
            #endregion

            #region Cumartesi
            //Cumartesi
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Saturday" == textBox_Bugun.Text)
                {
                    if (textBox_Cumartesi.Text == "#")
                    {

                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";

                    }
                }
            }
            #endregion

            #region Pazar
            //Pazar
            if (textBox_Zaman.Text == "") { }
            else
            {
                if ("Sunday" == textBox_Bugun.Text)
                {
                    if (textBox_Pazar.Text == "#")
                    {
                        NoticeDongu();
                        textBox_Notice.Text = "";
                        textBox_Zaman.Text = "";
                        textBox_Pazartesi.Text = "";
                        textBox_Sali.Text = "";
                        textBox_Carsamba.Text = "";
                        textBox_Persembe.Text = "";
                        textBox_Cuma.Text = "";
                        textBox_Cumartesi.Text = "";
                        textBox_Pazar.Text = "";

                    }
                }
            }
            #endregion
        }
        #endregion

        private void NoticeDongu() {
            timer_NoticeGeriSayim.Enabled = true;
        }
        private void timer_NoticeGeriSayim_Tick(object sender, EventArgs e)
        {
            int sayi1 = Convert.ToInt32(label_NoticeSayim.Text);
            int sayi2 = Convert.ToInt32(1);
            int sonuc = sayi1 - sayi2;
            label_NoticeSayim.Text = sonuc.ToString();
            if (label_NoticeSayim.Text == "2")
            {
                EbenezerKomutGonder(noticeAlma);
                listBox_NoticeGecmis.Items.Insert(0, noticeAlma);
            }
            else if (label_NoticeSayim.Text == "0")
            {
                label_NoticeSayim.Text = "5";
                timer_NoticeGeriSayim.Enabled = false;
            }
        }

        #endregion

        #region Oto Savaş

        #region Savas Kayıt Listesi Okuma
        private void SavasKayıtlarıGetir() 
        {
            OleDbDataAdapter adaptor = new OleDbDataAdapter("SELECT * FROM Savas", db.baglanti);
            DataSet ds = new DataSet();
            ds.Clear();
            adaptor.Fill(ds, "Savas");
            data_SavasKayitTablosu.DataSource = ds.Tables["Savas"];
            data_SavasKayitTablosu2.DataSource = ds.Tables["Savas"];
            adaptor.Dispose();

            label_SavasToplamKayit.Text ="Toplam Kayıt " + data_SavasKayitTablosu.RowCount.ToString();
        }
        #endregion

        #region SavasKaydet
        private void button_SavasKaydet_Click(object sender, EventArgs e)
        {
            
            db.kes();
            string savasTuru = comboBox_SavasTuru.Text;
            string komut = null;
            #region komutlar
            if (comboBox_SavasTuru.Text == "Lunar War") 
            {
                komut = "+open";
            }
            else if (comboBox_SavasTuru.Text == "Lunar War2")
            {
                komut = "+open2";
            }
            else if (comboBox_SavasTuru.Text == "Dark Lunar War") 
            {
                komut = "+open3";
            }
            else if (comboBox_SavasTuru.Text == "Snow War") 
            {
                komut = "+snowopen";
            }
            #endregion
            string baslangicZaman = TextBox_SavasBaslangicZaman.Text;
            string bitisZaman = TextBox_SavasBitisZaman.Text;

            #region Seçilen Günler
            string SectigimizVeriler = "";
            for (int i = 0; i < checked_SavasGunler.CheckedItems.Count; i++)
            {
                SectigimizVeriler += checked_SavasGunler.CheckedItems[i].ToString() + " ";
            }
            #endregion
            string pazartesi = null;
            string sali = null;
            string carsamba = null;
            string persembe = null;
            string cuma = null;
            string cumartesi = null;
            string pazar = null;
            #region Günler
            //Pazartesi Seçili ise
            if (SectigimizVeriler.IndexOf("Pazartesi") != -1)
            {
                pazartesi = "#";
            }
            else
            {
                pazartesi = "-";
            }

            //Salı Seçili ise
            if (SectigimizVeriler.IndexOf("Salı") != -1)
            {
                sali = "#";
            }
            else
            {
                sali = "-";
            }

            //Çarşamba Seçili ise
            if (SectigimizVeriler.IndexOf("Çarşamba") != -1)
            {
                carsamba = "#";
            }
            else
            {
                carsamba = "-";
            }

            //Perşembe Seçili ise
            if (SectigimizVeriler.IndexOf("Perşembe") != -1)
            {
                persembe = "#";
            }
            else
            {
                persembe = "-";
            }

            //Cuma Seçili ise
            if (SectigimizVeriler.IndexOf("Cuma ") != -1)
            {
                cuma = "#";
            }
            else
            {
                cuma = "-";
            }

            //Cumartesi Seçili ise
            if (SectigimizVeriler.IndexOf("Cumartesi") != -1)
            {
                cumartesi = "#";
            }
            else
            {
                cumartesi = "-";
            }

            //Pazar Seçili ise
            if (SectigimizVeriler.IndexOf("Pazarr") != -1)
            {
                pazar = "#";
            }
            else
            {
                pazar = "-";
            }
            #endregion

            string gecilecekNotice = textBox_GecilecekNotice.Text;
            if (comboBox_SavasTuru.Text == "" || comboBox_SavasTuru.Text == "Savaş Seç") 
            {
                DialogResult soru = MessageBox.Show("Savaş türünü seçmelisiniz!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                timer_SavasBaslangicZaman.Enabled = false;
                timer_SavasBitisZaman.Enabled = false;
                timer_NoticeGönder.Enabled = false;

                db.baglan();
                string Yenikayıt = string.Format("INSERT INTO Savas(SavasTuru,Komut,BaslangicZaman,BitisZaman,Pazartesi,Sali,Carsamba,Persembe,Cuma,Cumartesi,Pazar,GecilecekNotice)values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", savasTuru, komut, baslangicZaman, bitisZaman, pazartesi, sali, carsamba, persembe, cuma, cumartesi, pazar, gecilecekNotice);
                DialogResult soru = MessageBox.Show("Savaş kayıt edilsin mi?", "İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (soru == DialogResult.Yes)
                {
                    new OleDbCommand(Yenikayıt, db.baglanti).ExecuteNonQuery();
                    SavasKayıtlarıGetir();
                    timer_SavasBaslangicZaman.Enabled = true;
                    timer_SavasBitisZaman.Enabled = true;
                    timer_NoticeGönder.Enabled = true;
                }
                db.kes();
            }
        }
        #endregion

        #region Savas Kaydı Silme
        private void button_SavasSil_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Seçili kayıt silinsin mi?", "İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                try
                {
                    string sorgu = "DELETE FROM Savas WHERE ID=" + data_SavasKayitTablosu.CurrentRow.Cells[0].Value.ToString() + "";
                    OleDbCommand komut = new OleDbCommand(sorgu, db.baglanti);
                    db.baglan();
                    komut.ExecuteNonQuery();
                    db.kes();
                    SavasKayıtlarıGetir();
                }
                catch
                {
                    DialogResult soru2 = MessageBox.Show("Silme işlemi başarısız!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        #endregion

        #region Savas Verileri Arama ve Aktarma 

        #region Savaş Başlangıç Zaman
        string komut = null;
        string notice = null;
        private void timer_SavasBaslangicZaman_Tick(object sender, EventArgs e)
        {
            
            try
            {
                //Saat
                DateTime now2 = DateTime.Now;
                string saat = now2.ToString("HH:mm:ss");

                string sorgu = "SELECT * FROM Savas WHERE ";
                sorgu += string.Format("BaslangicZaman='{0}'", saat);
                db.baglan();
                veriler = new OleDbDataAdapter(sorgu, db.baglanti);
                tablo = new DataTable();
                veriler.Fill(tablo);
                data_SavasKayitTablosu2.DataSource = tablo;
                db.kes();

               
                    textBox_SavasKomut.Text = data_SavasKayitTablosu2.CurrentRow.Cells[2].Value.ToString();
                    komut = textBox_SavasKomut.Text;
                    textBox_SavasBaslangicZaman2.Text = data_SavasKayitTablosu2.CurrentRow.Cells[3].Value.ToString();
                    textBox_SavasBitisZaman2.Text = data_SavasKayitTablosu2.CurrentRow.Cells[4].Value.ToString();
                    textBox_SavasPazartesi.Text = data_SavasKayitTablosu2.CurrentRow.Cells[5].Value.ToString();
                    textBox_SavasSali.Text = data_SavasKayitTablosu2.CurrentRow.Cells[6].Value.ToString();
                    textBox_SavasCarsamba.Text = data_SavasKayitTablosu2.CurrentRow.Cells[7].Value.ToString();
                    textBox_SavasPersembe.Text = data_SavasKayitTablosu2.CurrentRow.Cells[8].Value.ToString();
                    textBox_SavasCuma.Text = data_SavasKayitTablosu2.CurrentRow.Cells[9].Value.ToString();
                    textBox_SavasCumartesi.Text = data_SavasKayitTablosu2.CurrentRow.Cells[10].Value.ToString();
                    textBox_SavasPazar.Text = data_SavasKayitTablosu2.CurrentRow.Cells[11].Value.ToString();
                    textBox_SavasNotice.Text = data_SavasKayitTablosu2.CurrentRow.Cells[12].Value.ToString();
                    notice = textBox_SavasNotice.Text;
               

            }
            catch { }
            
            SavasBaslangicSaatGünYakalama();
        }
        #endregion
        
        #region Savaş Bitiş Zaman
        private void timer_SavasBitisZaman_Tick(object sender, EventArgs e)
        {
            try
            {
                //Saat
                DateTime now2 = DateTime.Now;
                string saat = now2.ToString("HH:mm:ss");

                string sorgu = "SELECT * FROM Savas WHERE ";
                sorgu += string.Format("BitisZaman='{0}'", saat);
                db.baglan();
                veriler = new OleDbDataAdapter(sorgu, db.baglanti);
                tablo = new DataTable();
                veriler.Fill(tablo);
                data_SavasKayitTablosu2.DataSource = tablo;
                db.kes();


                textBox_SavasKomut.Text = data_SavasKayitTablosu2.CurrentRow.Cells[2].Value.ToString();
                textBox_SavasBaslangicZaman2.Text = data_SavasKayitTablosu2.CurrentRow.Cells[3].Value.ToString();
                textBox_SavasBitisZaman2.Text = data_SavasKayitTablosu2.CurrentRow.Cells[4].Value.ToString();
                textBox_SavasPazartesi.Text = data_SavasKayitTablosu2.CurrentRow.Cells[5].Value.ToString();
                textBox_SavasSali.Text = data_SavasKayitTablosu2.CurrentRow.Cells[6].Value.ToString();
                textBox_SavasCarsamba.Text = data_SavasKayitTablosu2.CurrentRow.Cells[7].Value.ToString();
                textBox_SavasPersembe.Text = data_SavasKayitTablosu2.CurrentRow.Cells[8].Value.ToString();
                textBox_SavasCuma.Text = data_SavasKayitTablosu2.CurrentRow.Cells[9].Value.ToString();
                textBox_SavasCumartesi.Text = data_SavasKayitTablosu2.CurrentRow.Cells[10].Value.ToString();
                textBox_SavasPazar.Text = data_SavasKayitTablosu2.CurrentRow.Cells[11].Value.ToString();
                textBox_SavasNotice.Text = data_SavasKayitTablosu2.CurrentRow.Cells[12].Value.ToString();

                SavasBitisSaatGünYakalama();
            }
            catch { }
        }
        #endregion


        #endregion

        #region Savaş Başlangıç Günü Saati Yakalama
        private void SavasBaslangicSaatGünYakalama()
        {
            #region Pazartesi
            //Pazartesi
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Monday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPazartesi.Text == "#")
                    {
                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Salı
            //Salı
            if (textBox_SavasBitisZaman2.Text == ""){}
            else 
            {
                if ("Tuesday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasSali.Text == "#") 
                    {
                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";
                       // 
                    }
                }
            }
            #endregion

            #region Çarşamba
            //Çarşamba
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Wednesday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCarsamba.Text == "#")
                    {

                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Perşembe
            //Perşembe
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Thursday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPersembe.Text == "#")
                    {

                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Cuma
            //Cuma
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Friday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCuma.Text == "#")
                    {

                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";
                    }
                }
            }
            #endregion

            #region Cumartesi
            //Cumartesi
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Saturday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCumartesi.Text == "#")
                    {

                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Pazar
            //Pazar
            if (textBox_SavasBaslangicZaman2.Text == "") { }
            else
            {
                if ("Sunday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPazar.Text == "#")
                    {

                        SavasBasglangicBeklemeDöngüsüEbenezerKomut();
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion
        }
        #endregion

        #region Savaş Başlangic Geri Sayim
        private void SavasBasglangicBeklemeDöngüsüEbenezerKomut() 
        {
            timer_SavasGeriSayim.Enabled = true;
        }

        private void timer_SavasGeriSayim_Tick(object sender, EventArgs e)
        {
            
            int sayi1 = Convert.ToInt32(label_SavasBaslangicGeriSayim.Text);
            int sayi2 = Convert.ToInt32(1);
            int sonuc = sayi1 - sayi2;
            label_SavasBaslangicGeriSayim.Text = sonuc.ToString();
            if (label_SavasBaslangicGeriSayim.Text == "299") 
            {
                EbenezerKomutGonder("Savaşın Başlamasına Son 5 DK");
            }
            else if (label_SavasBaslangicGeriSayim.Text == "240")
            {
                EbenezerKomutGonder("Savaşın Başlamasına Son 4 DK");

            }
            else if (label_SavasBaslangicGeriSayim.Text == "180")
            {
                EbenezerKomutGonder("Savaşın Başlamasına Son 3 DK");

            }
            else if (label_SavasBaslangicGeriSayim.Text == "120")
            {
                EbenezerKomutGonder("Savaşın Başlamasına Son 2 DK");

            }
            else if (label_SavasBaslangicGeriSayim.Text == "60")
            {
                EbenezerKomutGonder("Savaşın Başlamasına Son 1 DK");

            }
            else if (label_SavasBaslangicGeriSayim.Text == "2")
            {
                EbenezerKomutGonder("Savaş Kapaıları Açıldı.");
                EbenezerKomutGonder(notice);

            }
            else if (label_SavasBaslangicGeriSayim.Text == "1")
            {
                EbenezerKomutGonder(komut);
            }
            else if (label_SavasBaslangicGeriSayim.Text == "0")
            {
                label_SavasBaslangicGeriSayim.Text = "300";
                timer_SavasGeriSayim.Enabled = false;
            }

        }
        #endregion

        #region Savaş Bitiş Günü Saati Yakalama
        private void SavasBitisSaatGünYakalama()
        {
            #region Pazartesi
            //Pazartesi
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Monday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPazartesi.Text == "#")
                    {
                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Salı
            //Salı
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Tuesday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasSali.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Çarşamba
            //Çarşamba
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Wednesday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCarsamba.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Perşembe
            //Perşembe
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Thursday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPersembe.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Cuma
            //Cuma
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Friday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCuma.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";
                    }
                }
            }
            #endregion

            #region Cumartesi
            //Cumartesi
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Saturday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasCumartesi.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion

            #region Pazar
            //Pazar
            if (textBox_SavasBitisZaman2.Text == "") { }
            else
            {
                if ("Sunday" == textBox_Bugun.Text)
                {
                    if (textBox_SavasPazar.Text == "#")
                    {

                        SavasBitisBeklemeDongusuEbenezerKomut();
                        textBox_SavasBitisZaman2.Text = "";
                        textBox_SavasBaslangicZaman2.Text = "";
                        textBox_SavasPazartesi.Text = "";
                        textBox_SavasSali.Text = "";
                        textBox_SavasCarsamba.Text = "";
                        textBox_SavasPersembe.Text = "";
                        textBox_SavasCuma.Text = "";
                        textBox_SavasCumartesi.Text = "";
                        textBox_SavasPazar.Text = "";
                        textBox_SavasKomut.Text = "";
                        textBox_SavasNotice.Text = "";

                    }
                }
            }
            #endregion
        }
        #endregion

        #region Savas Bitiş Geri Sayim
        private void SavasBitisBeklemeDongusuEbenezerKomut() 
        {
            timer_SavasBitisGeriSayim.Enabled = true;
        }


        private void timer_SavasBitisGeriSayim_Tick(object sender, EventArgs e)
        {
            int sayi1 = Convert.ToInt32(label_SavasBitisGeriSayim.Text);
            int sayi2 = Convert.ToInt32(1);
            int sonuc = sayi1 - sayi2;
            label_SavasBitisGeriSayim.Text = sonuc.ToString();
            if (label_SavasBitisGeriSayim.Text == "299")
            {
                EbenezerKomutGonder("Savaşın Bitmesine Son 5 DK");
            }
            else if (label_SavasBitisGeriSayim.Text == "240")
            {
                EbenezerKomutGonder("Savaşın Bitmesine Son 4 DK");

            }
            else if (label_SavasBitisGeriSayim.Text == "180")
            {
                EbenezerKomutGonder("Savaşın Bitmesine Son 3 DK");

            }
            else if (label_SavasBitisGeriSayim.Text == "120")
            {
                EbenezerKomutGonder("Savaşın Bitmesine Son 2 DK");

            }
            else if (label_SavasBitisGeriSayim.Text == "60")
            {
                EbenezerKomutGonder("Savaşın Bitmesine Son 1 DK");

            }
            else if (label_SavasBitisGeriSayim.Text == "2")
            {
                EbenezerKomutGonder("Savaş Kapıları Kapandı. Herkes Kendi Bölgesine Işınlanıyor..");
            }
            else if (label_SavasBitisGeriSayim.Text == "1")
            {
                EbenezerKomutGonder("/close");//savaş kapatma
            }
            else if (label_SavasBitisGeriSayim.Text == "0")
            {
                label_SavasBitisGeriSayim.Text = "300";
                timer_SavasBitisGeriSayim.Enabled = false;
            }
        }
        #endregion

        #endregion

        #region Veritabanı İşlemleri

        #region Veritabanı Bağlantısı ve Durumu
        private void dbBaglantiKontrol()
        {
            SqlConnection baglanti = new SqlConnection("server=" + textBox_IP.Text + "; Initial Catalog=" + textBox_Veritabani.Text + "; Integrated Security=true;");
            string baglantiDurum = baglanti.State.ToString();

            if (baglantiDurum == "Closed")
            {
                try
                {
                    baglanti.Open();
                    picture_Veritabani.BackColor = Color.DarkGreen;
                    DBverileriCek();
                    if (label_EbenezerDurum.Text == "Bağlandı")
                    {
                        tab_GMIslemleri.Enabled = true;
                    }
                    else
                    {
                        DialogResult mesaj = MessageBox.Show("Veritabanına bağlanıldı fakat 'Ebenezer' aktif olmadığı için bazı özellikleri kullanamayaksınız.", "İşlem;", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tab_GMIslemleri.Enabled = false;
                    }
                }
                catch
                {
                    DialogResult hata = MessageBox.Show("Böyle bir veritabanı bulunamadı! Veritabanı ve ServerIP adresini kontrol ediniz veya 'Service Manager' programının aktif olduğundan emin olunuz. Kontrol ettikten sonra 'Bağlantıyı Yenile' yazısına tıklayıp tekrar deneyiniz..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    picture_Veritabani.BackColor = Color.DarkRed;
                }
            }
            else
            {
                picture_Veritabani.BackColor = Color.DarkRed;
            }


        }
        #endregion

        #region Tekran Bağlantı Deneme
        private void linkLabel_DBBaglantiYenile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dbBaglantiKontrol();
        }
        #endregion

        #endregion

        #region Tüm Verileri Aktar
        private void DBverileriCek()
        {
            onlineUserCekme();
        }
        #endregion

        #region GM İşlemleri

        #region Online Kullanıcı Listesi

        #region Online User Çekme
        private void onlineUserCekme()
        {
            //SqlConnection baglanti = new SqlConnection("Server=" + textBox_IP.Text + ";Database="+ textBox_Veritabani.Text + ";User=" + textBox_KullaniciAdi.Text + ";Pwd=" + textBox_Parola.Text + ";Integrated Security=true");
            SqlConnection baglanti = new SqlConnection("server=" + textBox_IP.Text + "; Initial Catalog=" + textBox_Veritabani.Text + "; Integrated Security=true;");
            DataTable tablo = new DataTable();

            string sorgu = "SELECT * FROM CURRENTUSER";
            try
            {
                baglanti.Open();

                SqlDataAdapter oku = new SqlDataAdapter(sorgu, baglanti);
                oku.Fill(tablo);
                data_OnlineTablosu.DataSource = tablo;
                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
                baglanti.Close();
            }
            catch 
            { 
                MessageBox.Show("Online Kullanıcılar Tablosu Çekilemedi!", "Hata;"); 
            }
        }

        #endregion

        #region Online Liste Yenile
        private void button_OnlineListYenile_Click(object sender, EventArgs e)
        {
            onlineUserCekme();
        }
        #endregion

        #region Seçili Kullanıcı
        private void data_OnlineTablosu_SelectionChanged(object sender, EventArgs e)
        {
            textBox_OnlineSeciliUSER.Text = data_OnlineTablosu.CurrentRow.Cells[3].Value.ToString();
        }
        #endregion

        #region DC Ettirme
        private void button_DCEttirme_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show(textBox_OnlineSeciliUSER.Text + " isimli useri DC etmek istiyormusun?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                EbenezerKomutGonder("+kill" + textBox_OnlineSeciliUSER.Text);
                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
            }
        }
        #endregion

        #region Karakter Öldür
        private void button_KarakterOldur_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show(textBox_OnlineSeciliUSER.Text + " isimli useri öldürmek istiyormusun?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                EbenezerKomutGonder("+smite" + textBox_OnlineSeciliUSER.Text);

                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
            }
        }
        #endregion

        #region Kullaniciyi Banla
        private void button_KullaniciyiBanla_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show(textBox_OnlineSeciliUSER.Text + " isimli useri banlamak istiyormusun?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                EbenezerKomutGonder("+kill" + textBox_OnlineSeciliUSER.Text);
                EbenezerKomutGonder("+ban_char" + textBox_OnlineSeciliUSER.Text);
                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
            }
        }
        #endregion

        #region Kullaniciyi Mutele
        private void button_KullaniciMute_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show(textBox_OnlineSeciliUSER.Text + " isimli useri mutelemek istiyormusun?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                EbenezerKomutGonder("+mute" + textBox_OnlineSeciliUSER.Text);
                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
            }
        }
        #endregion

        #region Herkesi Oyundan At
        private void button_HerkesDC_Click(object sender, EventArgs e)
        {
            DialogResult soru = MessageBox.Show("Oyundaki herkesi DC etmek istiyormusun?", "İşlem;", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (soru == DialogResult.Yes)
            {
                EbenezerKomutGonder("+alldiscount" + textBox_OnlineSeciliUSER.Text);
                label_ToplamOnline.Text = "Toplam Online " + data_OnlineTablosu.RowCount.ToString();
            }
        }
        #endregion

        #endregion

        #endregion

        #region ChatLog

        #region ChatLog Yolu Seç
        private void button_ChatLog_Click(object sender, EventArgs e)
        {
            if (folder_ChatLog.ShowDialog() == DialogResult.OK) 
            {
                string yol = folder_ChatLog.SelectedPath.ToString();
                textBox_ChatLog.Text = yol;
                picture_ChatLog.BackColor = Color.DarkGreen;
                INI.Yaz("KayitLogYollari", "ChatLOG", yol);
                DialogResult mesaj = MessageBox.Show("ChatLog Konumu başarılı bir şekilde seçildi.","İşlem",MessageBoxButtons.OK,MessageBoxIcon.Information);
            } 
            else 
            { 
                picture_ChatLog.BackColor = Color.DarkRed;
                DialogResult mesaj = MessageBox.Show("ChatLog yolu seçilmedi!", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }

        }

        #endregion

        #region ItemLog Yolu Seç
        private void button_ItemLog_Click(object sender, EventArgs e)
        {
            if (folder_ItemLog.ShowDialog() == DialogResult.OK)
            {
                string yol = folder_ItemLog.SelectedPath.ToString();
                textBox_ItemLog.Text = yol;
                picture_ItemLog.BackColor = Color.DarkGreen;
                INI.Yaz("KayitLogYollari", "ItemLOG", yol);
                DialogResult mesaj = MessageBox.Show("ItemLog Konumu başarılı bir şekilde seçildi.", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                picture_ItemLog.BackColor = Color.DarkRed;
                DialogResult mesaj = MessageBox.Show("ItemLog yolu seçilmedi!", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        #endregion

        #region DeathLog Yolu Seç
        private void button_DeathLog_Click(object sender, EventArgs e)
        {
            if (folder_DeathLog.ShowDialog() == DialogResult.OK)
            {
                string yol = folder_DeathLog.SelectedPath.ToString();
                textBox_DeathLog.Text = yol;
                picture_DeathLog.BackColor = Color.DarkGreen;
                INI.Yaz("KayitLogYollari", "DeathLOG", yol);
                DialogResult mesaj = MessageBox.Show("DeathLog Konumu başarılı bir şekilde seçildi.", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                picture_DeathLog.BackColor = Color.DarkRed;
                DialogResult mesaj = MessageBox.Show("DeathLog yolu seçilmedi!", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        

        #endregion

        #region dialogResult
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
          Form form = new Form();
          Label label = new Label();
          TextBox textBox = new TextBox();
          Button buttonOk = new Button();
          Button buttonCancel = new Button();

          form.Text = title;
          label.Text = promptText;
          textBox.Text = value;

          buttonOk.Text = "Kaydet";
          buttonCancel.Text = "İptal";
          buttonOk.DialogResult = DialogResult.OK;
          buttonCancel.DialogResult = DialogResult.Cancel;

          label.SetBounds(9, 20, 372, 13);
          textBox.SetBounds(12, 36, 372, 20);
          buttonOk.SetBounds(228, 72, 75, 23);
          buttonCancel.SetBounds(309, 72, 75, 23);

          label.AutoSize = true;
          textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
          buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
          buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

          form.ClientSize = new Size(396, 107);
          form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
          form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
          form.FormBorderStyle = FormBorderStyle.FixedDialog;
          form.StartPosition = FormStartPosition.CenterScreen;
          form.MinimizeBox = false;
          form.MaximizeBox = false;
          form.AcceptButton = buttonOk;
          form.CancelButton = buttonCancel;

          DialogResult dialogResult = form.ShowDialog();
          value = textBox.Text;
          return dialogResult;
        }
        #endregion

    }


}