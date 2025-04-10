using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace otomasyon
{
    public partial class Giris : Form
    {
        Kayıt kayit;
        SqlBagla bglG = new SqlBagla();
        public static int KID = 0;
        public static string Ad;

        public Giris()
        {
            InitializeComponent();
        }
        
        private void btnKayit_Click(object sender, EventArgs e)
        {
            kayit = new Kayıt();
            kayit.ShowDialog();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("SELECT KullanıcıID FROM TabloKullanıcı WHERE [Kullanıcı Adı] = @p1 AND Şifre = @p2", bglG.baglanti());
            komut.Parameters.AddWithValue("@p1", txtAd.Text);
            komut.Parameters.AddWithValue("@p2", txtSifre.Text);

            object sonuc = komut.ExecuteScalar();

            // Eğer kullanıcı adı ve şifre eşleşirse
            if (sonuc != null && Convert.ToInt32(sonuc) > 0)
            {
                Ad = txtAd.Text;
                KID = Convert.ToInt32(sonuc);
                this.Hide();
                //MessageBox.Show("Hoşgeldiniz", "Giriş Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 anaEkran = new Form1();
                anaEkran.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
