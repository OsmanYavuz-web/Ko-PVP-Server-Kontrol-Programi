namespace Ko_PVP_Server_Kontrol_Programi
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_SiteGirisURL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.web_SiteOnizleme = new System.Windows.Forms.WebBrowser();
            this.richTextBox_SiteKaynakKod = new System.Windows.Forms.RichTextBox();
            this.textBox_DownloadURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Giris = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_KullaniciAdi = new System.Windows.Forms.TextBox();
            this.textBox_Parola = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_Durum = new System.Windows.Forms.Label();
            this.textBox_SiteAdresi = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox_Dosyalar = new System.Windows.Forms.ListBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.label_MecvutSurum = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.deger2 = new System.Windows.Forms.RichTextBox();
            this.deger1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 300);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // textBox_SiteGirisURL
            // 
            this.textBox_SiteGirisURL.Location = new System.Drawing.Point(181, 388);
            this.textBox_SiteGirisURL.Name = "textBox_SiteGirisURL";
            this.textBox_SiteGirisURL.Size = new System.Drawing.Size(266, 23);
            this.textBox_SiteGirisURL.TabIndex = 17;
            this.textBox_SiteGirisURL.Text = "http://192.168.60.2/giris";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 392);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Giris Yapılmamışsa";
            // 
            // web_SiteOnizleme
            // 
            this.web_SiteOnizleme.Location = new System.Drawing.Point(241, 421);
            this.web_SiteOnizleme.MinimumSize = new System.Drawing.Size(23, 23);
            this.web_SiteOnizleme.Name = "web_SiteOnizleme";
            this.web_SiteOnizleme.Size = new System.Drawing.Size(206, 105);
            this.web_SiteOnizleme.TabIndex = 15;
            this.web_SiteOnizleme.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web_SiteOnizleme_DocumentCompleted);
            // 
            // richTextBox_SiteKaynakKod
            // 
            this.richTextBox_SiteKaynakKod.Location = new System.Drawing.Point(22, 421);
            this.richTextBox_SiteKaynakKod.Name = "richTextBox_SiteKaynakKod";
            this.richTextBox_SiteKaynakKod.Size = new System.Drawing.Size(212, 105);
            this.richTextBox_SiteKaynakKod.TabIndex = 14;
            this.richTextBox_SiteKaynakKod.Text = "";
            this.richTextBox_SiteKaynakKod.TextChanged += new System.EventHandler(this.richTextBox_SiteKaynakKod_TextChanged);
            // 
            // textBox_DownloadURL
            // 
            this.textBox_DownloadURL.Location = new System.Drawing.Point(181, 356);
            this.textBox_DownloadURL.Name = "textBox_DownloadURL";
            this.textBox_DownloadURL.Size = new System.Drawing.Size(266, 23);
            this.textBox_DownloadURL.TabIndex = 13;
            this.textBox_DownloadURL.Text = "http://192.168.60.2/program";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 359);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Download Adresi";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(49, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 19);
            this.label2.TabIndex = 18;
            this.label2.Text = "Kullanıcı Adı";
            // 
            // button_Giris
            // 
            this.button_Giris.Enabled = false;
            this.button_Giris.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button_Giris.Location = new System.Drawing.Point(298, 149);
            this.button_Giris.Name = "button_Giris";
            this.button_Giris.Size = new System.Drawing.Size(87, 27);
            this.button_Giris.TabIndex = 19;
            this.button_Giris.Text = "Giriş Yap";
            this.button_Giris.UseVisualStyleBackColor = true;
            this.button_Giris.Click += new System.EventHandler(this.button_Giris_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(112, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 19);
            this.label3.TabIndex = 20;
            this.label3.Text = "Parola";
            // 
            // textBox_KullaniciAdi
            // 
            this.textBox_KullaniciAdi.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox_KullaniciAdi.Location = new System.Drawing.Point(181, 85);
            this.textBox_KullaniciAdi.Name = "textBox_KullaniciAdi";
            this.textBox_KullaniciAdi.Size = new System.Drawing.Size(204, 26);
            this.textBox_KullaniciAdi.TabIndex = 21;
            this.textBox_KullaniciAdi.Text = "dangerousman32";
            // 
            // textBox_Parola
            // 
            this.textBox_Parola.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox_Parola.Location = new System.Drawing.Point(181, 117);
            this.textBox_Parola.Name = "textBox_Parola";
            this.textBox_Parola.Size = new System.Drawing.Size(204, 26);
            this.textBox_Parola.TabIndex = 22;
            this.textBox_Parola.Text = "holocaust32";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.Location = new System.Drawing.Point(12, 239);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(473, 23);
            this.progressBar1.TabIndex = 23;
            // 
            // label_Durum
            // 
            this.label_Durum.AutoSize = true;
            this.label_Durum.Image = ((System.Drawing.Image)(resources.GetObject("label_Durum.Image")));
            this.label_Durum.Location = new System.Drawing.Point(9, 221);
            this.label_Durum.Name = "label_Durum";
            this.label_Durum.Size = new System.Drawing.Size(98, 15);
            this.label_Durum.TabIndex = 24;
            this.label_Durum.Text = "Bekleniyor...";
            // 
            // textBox_SiteAdresi
            // 
            this.textBox_SiteAdresi.Location = new System.Drawing.Point(181, 327);
            this.textBox_SiteAdresi.Name = "textBox_SiteAdresi";
            this.textBox_SiteAdresi.Size = new System.Drawing.Size(266, 23);
            this.textBox_SiteAdresi.TabIndex = 26;
            this.textBox_SiteAdresi.Text = "http://192.168.60.2/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 330);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Site Adresi";
            // 
            // listBox_Dosyalar
            // 
            this.listBox_Dosyalar.FormattingEnabled = true;
            this.listBox_Dosyalar.ItemHeight = 15;
            this.listBox_Dosyalar.Location = new System.Drawing.Point(453, 327);
            this.listBox_Dosyalar.Name = "listBox_Dosyalar";
            this.listBox_Dosyalar.Size = new System.Drawing.Size(186, 199);
            this.listBox_Dosyalar.TabIndex = 27;
            // 
            // timer2
            // 
            this.timer2.Interval = 200;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 70;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // label_MecvutSurum
            // 
            this.label_MecvutSurum.AutoSize = true;
            this.label_MecvutSurum.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label_MecvutSurum.Image = ((System.Drawing.Image)(resources.GetObject("label_MecvutSurum.Image")));
            this.label_MecvutSurum.Location = new System.Drawing.Point(9, 181);
            this.label_MecvutSurum.Name = "label_MecvutSurum";
            this.label_MecvutSurum.Size = new System.Drawing.Size(133, 13);
            this.label_MecvutSurum.TabIndex = 28;
            this.label_MecvutSurum.Text = "Mevcut Sürüm: 1.0.0.0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(645, 474);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Version Kaç";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(645, 503);
            this.textBox1.MaxLength = 4;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 32;
            // 
            // deger2
            // 
            this.deger2.Location = new System.Drawing.Point(645, 404);
            this.deger2.Name = "deger2";
            this.deger2.Size = new System.Drawing.Size(100, 64);
            this.deger2.TabIndex = 33;
            this.deger2.Text = "</TD></TR>\n<TR>\n<TD height=32 vAlign=middle align=left>Download Linki:&nbsp;&nbsp" +
    ";</TD>\n";
            // 
            // deger1
            // 
            this.deger1.Location = new System.Drawing.Point(645, 327);
            this.deger1.Name = "deger1";
            this.deger1.Size = new System.Drawing.Size(100, 71);
            this.deger1.TabIndex = 34;
            this.deger1.Text = "<TR>\n<TD height=32 vAlign=middle align=left>Version:&nbsp;&nbsp;</TD>\n<TD>";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 551);
            this.Controls.Add(this.deger1);
            this.Controls.Add(this.deger2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_MecvutSurum);
            this.Controls.Add(this.listBox_Dosyalar);
            this.Controls.Add(this.textBox_SiteAdresi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_Durum);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBox_Parola);
            this.Controls.Add(this.textBox_KullaniciAdi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_Giris);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_SiteGirisURL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.web_SiteOnizleme);
            this.Controls.Add(this.richTextBox_SiteKaynakKod);
            this.Controls.Add(this.textBox_DownloadURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ko PVP Server Kontrol Programı";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_SiteGirisURL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.WebBrowser web_SiteOnizleme;
        private System.Windows.Forms.RichTextBox richTextBox_SiteKaynakKod;
        private System.Windows.Forms.TextBox textBox_DownloadURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Giris;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_KullaniciAdi;
        private System.Windows.Forms.TextBox textBox_Parola;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_Durum;
        private System.Windows.Forms.TextBox textBox_SiteAdresi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox_Dosyalar;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Label label_MecvutSurum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox deger2;
        private System.Windows.Forms.RichTextBox deger1;
    }
}