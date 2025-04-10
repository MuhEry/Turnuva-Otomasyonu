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
    public partial class Kayıt : Form
    {
        SqlBagla bglK = new SqlBagla();
        public Kayıt()
        {
            InitializeComponent();
        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            SqlCommand tkmkontrol = new SqlCommand("SELECT COUNT(*) FROM TabloKullanıcı WHERE [Kullanıcı Adı] = @p1", bglK.baglanti());
            tkmkontrol.Parameters.AddWithValue("@p1", txtKAd.Text);
            int count = Convert.ToInt32(tkmkontrol.ExecuteScalar());
            bglK.baglanti().Close();
            if (count > 0)
            {
                MessageBox.Show(txtKAd.Text + " Kullanıcı adı zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtKAd.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
                {
                    MessageBox.Show("Lütfen Zorunlu Alanları(*) Doldurun", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand komut = new SqlCommand("insert into TabloKullanıcı ([Kullanıcı Adı], Şifre, Ad, Soyad, [E posta], Telefon) values (@p1,@p2,@p3,@p4,@p5,@p6)", bglK.baglanti());
                    komut.Parameters.AddWithValue("@p1", txtKAd.Text);
                    komut.Parameters.AddWithValue("@p2", txtSifre.Text);
                    komut.Parameters.AddWithValue("@p3", txtAd.Text);
                    komut.Parameters.AddWithValue("@p4", txtSoyad.Text);
                    komut.Parameters.AddWithValue("@p5", txtEPosta.Text);
                    komut.Parameters.AddWithValue("@p6", txtTel.Text);
                    komut.ExecuteNonQuery();
                    bglK.baglanti().Close();
                    MessageBox.Show("Kayıt Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }
    }
}
