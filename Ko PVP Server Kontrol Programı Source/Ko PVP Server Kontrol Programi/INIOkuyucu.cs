using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Ko_PVP_Server_Kontrol_Programi
{
    class INIOkuyucu
    {
        // Windows API'lerini çağır
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIOkuyucu(string dosyaYolu)
        {
            // sınıf oluşturulurken alınan bilgiyi gerçek değişkene aktar
            DosyaYolu = dosyaYolu;
        }

        private string DosyaYolu = String.Empty;
        public string Varsayilan { get; set; }

        public string Oku(string bolum, string anahtar)
        {
            // dosya okunamama durumunda dönecek varsayılan değer
            Varsayilan = Varsayilan ?? String.Empty;

            StringBuilder builder = new StringBuilder(256);
            GetPrivateProfileString(bolum, anahtar, Varsayilan, builder, 255, DosyaYolu);

            return builder.ToString();
        }

        public long Yaz(string bolum, string anahtar, string deger)
        {
            // alınan bilgiler api ile dosyaya yazılıyor
            return WritePrivateProfileString(bolum, anahtar, deger, DosyaYolu);
        }
    }
}
