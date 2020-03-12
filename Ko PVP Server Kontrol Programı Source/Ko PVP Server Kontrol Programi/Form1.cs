using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Ko_PVP_Server_Kontrol_Programi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string version = Application.ProductVersion.ToString();
        frm_DosyaYol formAC = new frm_DosyaYol();
        private void Form1_Load(object sender, EventArgs e)
        {
            //515; 338
            //this.Size = new Size(515,338);
            label_MecvutSurum.Text = "Mevcut Sürüm : " + version;
            web_SiteOnizleme.Navigate(textBox_DownloadURL.Text);
            dosyaListele();
        }

        #region Giriş Yapma
        private void button_Giris_Click(object sender, EventArgs e)
        {
            string siteURL = web_SiteOnizleme.Url.ToString();
            if (textBox_SiteGirisURL.Text == siteURL)
            {
                HtmlElement ele = web_SiteOnizleme.Document.GetElementById("kadi"); if (ele != null)
                    ele.InnerText = textBox_KullaniciAdi.Text;

                ele = web_SiteOnizleme.Document.GetElementById("sifre"); if (ele != null)
                    ele.InnerText = textBox_Parola.Text;

                foreach (HtmlElement form in web_SiteOnizleme.Document.Forms)
                    form.InvokeMember("submit");

            }
            else 
            {
                label_Durum.Text = "Giriş yapılamadı. Lütfen tekrar deneyiniz.";
            }
        }
        #endregion

        #region Site Kaynak Kodu
        private void web_SiteOnizleme_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            richTextBox_SiteKaynakKod.Text = web_SiteOnizleme.Document.Body.InnerHtml.ToString();
        }
        #endregion

        #region Site Giriş Durumları
        private void richTextBox_SiteKaynakKod_TextChanged(object sender, EventArgs e)
        {
            //Done
            string siteDurum = web_SiteOnizleme.StatusText.ToString();
            string siteURL = web_SiteOnizleme.Url.ToString();


            if (richTextBox_SiteKaynakKod.Text.IndexOf("Giriş Yapılmış") != -1)
            {
                label_Durum.Text = "Başarılı bir şekilde giriş yapıldı...";
                if (textBox_SiteAdresi.Text == siteURL)
                {
                    if (label_Durum.Text == "Başarılı bir şekilde giriş yapıldı...")
                    {
                        web_SiteOnizleme.Navigate(textBox_DownloadURL.Text);
                    }
                }
            }
            else
            {
                if (textBox_SiteGirisURL.Text == siteURL) { }
                else
                {
                    web_SiteOnizleme.Navigate(textBox_SiteGirisURL.Text);
                    label_Durum.Text = "Giriş yapılmamış. Lütfen bekleyin yönlendiriliyorsunuz...";
                }
            }

            if (textBox_SiteGirisURL.Text == siteURL) 
            {
               // versionBul();
            }

            if (richTextBox_SiteKaynakKod.Text.IndexOf("Kullanıcı Adı:") != -1)
            {
                    label_Durum.Text = "Artık giriş yapabilirsiniz. Kullanıcı Adı ve Parolanızı giriniz...";
                    button_Giris.Enabled = true;
            }

            if (richTextBox_SiteKaynakKod.Text.IndexOf("Tüm alanları doldurmanız gerekiyor..") != -1)
            {
                label_Durum.Text = "Tüm alanları doldurmanız gerekiyor.";

            }

            if (richTextBox_SiteKaynakKod.Text.IndexOf("Böyle bir üye, sistemde kayıtlı gözükmüyor.") != -1)
            {
                label_Durum.Text = "Kullanıcı adı veya Parola yanlış kontrol ediniz.";

            }


        }
        #endregion

        #region ProgressBar Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label_Durum.Text == "Başarılı bir şekilde giriş yapıldı...")
            {
                timer3.Enabled = true;
            }
        }
        #endregion

        #region Dosya Tarama
        private void dosyaTara()
        {
            listBox_Dosyalar.SelectedIndex = listBox_Dosyalar.SelectedIndex + 1;
            label_Durum.Text = "Taranıyor " + listBox_Dosyalar.Text;
            int toplamVeri = listBox_Dosyalar.Items.Count - 1;
            if (listBox_Dosyalar.SelectedIndex == toplamVeri)
            {
                label_Durum.Text = "Tarama işlemi tamamlandı...";
                timer2.Enabled = false;
            }
        }
        #endregion

        #region Dosya Listeleme
        private void dosyaListele() 
        {
            string yol = Application.StartupPath;

            //listBox1'in içinde bulunan Item'ları
            //Items.Clear() methodu ile temizliyoruz.
            listBox_Dosyalar.Items.Clear();
            //Daha sonra DirectoryInfo tipinden bir değişken oluşturup,
            //içindeki dosyaları okumak istediğimiz klasörün dizin bilgisini veriyoruz.
            DirectoryInfo di = new DirectoryInfo(yol);
            //FileInfo tipinden bir değişken oluşturuyoruz.
            //çünkü di.GetFiles methodu, bize FileInfo tipinden bir dizi dönüyor.
            FileInfo[] rgFiles = di.GetFiles();
            //foreach döngümüzle fgFiles içinde dönüyoruz.
            foreach (FileInfo fi in rgFiles)
            {
                //fi.Name bize dosyanın adını dönüyor.
                //fi.FullName ise bize dosyasının dizin bilgisini döner.
                listBox_Dosyalar.Items.Add(fi.Name);
            }
        }
        #endregion

        #region Dosya Tara Timer
        private void timer2_Tick(object sender, EventArgs e)
        {
            dosyaTara();
        }

        #endregion

        #region Version Bulma
        private void versionBul()
        {
            string gelen =
                richTextBox_SiteKaynakKod.Text;

            int titleIndexBaslangici =
                gelen.IndexOf(deger1.Text) + deger1.TextLength; //7

            int titleIndexBitisi =
                gelen.Substring(titleIndexBaslangici).IndexOf(deger2.Text); //8
            //<TD>
            string sonuc = gelen.Substring(titleIndexBaslangici, titleIndexBitisi);//9
            sonuc = sonuc.Replace("<TD>", "").TrimEnd();
            textBox1.Text = sonuc.TrimEnd();

            if (version == sonuc)
            {
                label_Durum.Text = "Program en güncel versionda..";
            }
            else
            {
                label_Durum.Text = "Güncel version bulundu.. Version: "+ sonuc ;
                DialogResult mesaj = MessageBox.Show("Yeni sürüm bulundu! \r\nGüncel Version: " + sonuc + "\r\nSizin Kullandığınız Version: "+ version+ "\r\nGüncelleştirilsin mi?", "İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }

        }
        #endregion

        #region Program Akışı
        private void timer3_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = progressBar1.Value + 1;

            if (progressBar1.Value == 1)
            {
                textBox_KullaniciAdi.Enabled = false;
                textBox_Parola.Enabled = false;
                button_Giris.Enabled = false;
                label_Durum.Text = "Program dosyaları kontrol ediliyor..";
            }
            else if (progressBar1.Value == 20)
            {
                timer2.Enabled = true;
            }
            else if (progressBar1.Value == 60)
            {
                label_Durum.Text = "Güncelleştirmeler taranıyor...";
            }
            else if (progressBar1.Value == 70)
            {
                label_Durum.Text = "Kullandığınız Sürüm: " + Application.ProductVersion;
            }
            else if (progressBar1.Value == 75)
            {
                versionBul();
            }
            else if (progressBar1.Value == 98)
            {
                timer3.Enabled = false;
                label_Durum.Text = "Program açılıyor...";
                
            }
            else if (progressBar1.Value == 100)
            {
               
                this.Hide();
                this.Visible = false;
                formAC.Show();
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            versionBul();
                
        }


    }
}
