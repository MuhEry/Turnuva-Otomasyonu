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
using DevExpress.XtraGrid.Views.Grid;

namespace otomasyon
{
    public partial class Oyuncular : Form
    {
        SqlBagla bglO = new SqlBagla();
        bool sutunayar = false;

        public Oyuncular()
        {
            InitializeComponent();
        }
        private void Oyuncular_Load(object sender, EventArgs e)
        {
            Listele();
            oyuncuEkleTakim();
            oyuncuETakim();
        }

        void Listele()
        {
            try
            {
                // Oyuncu listesini doldur
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select * from TabloOyuncular where TurnuvaID = @p1", bglO.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tabloOyuncu.DataSource = dt;
                GridView tblAyar = tabloOyuncu.MainView as GridView;
                if (!sutunayar && tblAyar != null)
                {
                    tblAyar.Columns["TakımID"].Visible = false;
                    tblAyar.Columns["TurnuvaID"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Regular);
                    gridView1.Appearance.FocusedRow.BackColor = Color.FromArgb(180, 250, 250);
                    tblAyar.OptionsView.EnableAppearanceOddRow = true; // Satırları sıralı renklendirme
                    tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                    tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                    tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(220, 250, 250);
                    tblAyar.Columns["OyuncuID"].Caption = "ID"; // OyuncuID kısmına sadece ID yazarak kısaltıyoruz
                    tblAyar.Columns["Ad"].Width = 255;
                    tblAyar.Columns["Soyad"].Width = 255;
                    tblAyar.Columns["Ülke"].Width = 240;
                    tblAyar.Columns["Takım"].Width = 255;
                    sutunayar = true; // GridView ayarları tamamlandı
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void temizle()
        {
            txtEkleAd.Text = "";
            txtEkleSoyad.Text = "";
            txtEkleUlke.Text = "";
            txtEkleYas.Text = "";
            txtEkleMac.Text = "";
            txtEkleSkor.Text = "";
            txtEkleAsist.Text = "";
            cmbEkleTakim.Text = "";
        }
        private void BtnOEkle_Click(object sender, EventArgs e)
        {
            if (cmbEkleTakim.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir takım seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçilen takımın ID'sini al
            int takimID = Convert.ToInt32(cmbEkleTakim.SelectedValue);
            // Ekle butonuna basıldığında girilen bilgilerle yeni oyuncu ekle
            SqlCommand komut = new SqlCommand("insert into TabloOyuncular (Ad, Soyad, Ülke, Yaş, Takım, Maç, Skor, Asist, TakımID, TurnuvaID) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)" , bglO.baglanti());
            komut.Parameters.AddWithValue("@p1",txtEkleAd.Text);
            komut.Parameters.AddWithValue("@p2", txtEkleSoyad.Text);
            komut.Parameters.AddWithValue("@p3", txtEkleUlke.Text);
            komut.Parameters.AddWithValue("@p4", txtEkleYas.Text);
            komut.Parameters.AddWithValue("@p5", cmbEkleTakim.Text);
            komut.Parameters.AddWithValue("@p6", txtEkleMac.Text);
            komut.Parameters.AddWithValue("@p7", txtEkleSkor.Text);
            komut.Parameters.AddWithValue("@p8", txtEkleAsist.Text);
            komut.Parameters.AddWithValue("@p9", takimID);
            komut.Parameters.AddWithValue("@p10", AnaSayfa.TID);
            komut.ExecuteNonQuery();
            bglO.baglanti().Close();
            MessageBox.Show("Oyuncu Eklendi","Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
            temizle();
        }

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            // Tablo üzerinden tıklanan oyuncunun bilgilerini text kutularına doldur
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                txtEAd.Text = dr["Ad"].ToString();
                txtESoyad.Text = dr["Soyad"].ToString();
                txtEUlke.Text = dr["Ülke"].ToString();
                txtEYas.Text = dr["Yaş"].ToString();
                txtEMac.Text = dr["Maç"].ToString();
                txtESkor.Text = dr["Skor"].ToString();
                txtEAsist.Text = dr["Asist"].ToString();
                cmbETakim.Text = dr["Takım"].ToString();
                txtEID.Text = dr["OyuncuID"].ToString();
            }
        }

        private void btnODuzenle_Click(object sender, EventArgs e)
        {
            if (cmbETakim.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir takım seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Seçilen takımın ID'sini al
            int takimID = Convert.ToInt32(cmbETakim.SelectedValue);
            // Girilen bilgilerle seçilen oyuncuyu güncelle
            SqlCommand komut = new SqlCommand("Update TabloOyuncular set Ad=@p1, Soyad=@p2, Ülke=@p3, Yaş=@p4, Takım=@p5, Maç=@p6, Skor=@p7, Asist=@p8, TakımID=@p9 where OyuncuID=@p10", bglO.baglanti());
            komut.Parameters.AddWithValue("@p1", txtEAd.Text);
            komut.Parameters.AddWithValue("@p2", txtESoyad.Text);
            komut.Parameters.AddWithValue("@p3", txtEUlke.Text);
            komut.Parameters.AddWithValue("@p4", txtEYas.Text);
            komut.Parameters.AddWithValue("@p5", cmbETakim.Text);
            komut.Parameters.AddWithValue("@p6", txtEMac.Text);
            komut.Parameters.AddWithValue("@p7", txtESkor.Text);
            komut.Parameters.AddWithValue("@p8", txtEAsist.Text);
            komut.Parameters.AddWithValue("@p9", takimID);
            komut.Parameters.AddWithValue("@p10", txtEID.Text);
            komut.ExecuteNonQuery();
            bglO.baglanti().Close();
            MessageBox.Show("Oyuncu Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }
        private void btnOSil_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Delete from TabloOyuncular where OyuncuID=@p1", bglO.baglanti());
            komut.Parameters.AddWithValue("@p1", txtEID.Text);
            komut.ExecuteNonQuery();
            bglO.baglanti().Close();
            MessageBox.Show("Oyuncu Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Listele();
        }
        void oyuncuEkleTakim()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT TakımID, [Takım Adı] FROM TabloTakım where TurnuvaID = @p1", bglO.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            da.Fill(dt);

            cmbEkleTakim.DataSource = dt;
            cmbEkleTakim.DisplayMember = "Takım Adı";
            cmbEkleTakim.ValueMember = "TakımID";
            cmbEkleTakim.SelectedIndex = -1;
        }
        void oyuncuETakim()
        {
            // Oyuncu düzenle kısmındaki takım seçme bölümüne tablodaki takımları listele
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT TakımID, [Takım Adı] FROM TabloTakım where TurnuvaID = @p1", bglO.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            da.Fill(dt);

            cmbETakim.DataSource = dt;
            cmbETakim.DisplayMember = "Takım Adı";
            cmbETakim.ValueMember = "TakımID";
            cmbETakim.SelectedIndex = -1;
        }
    }
}
