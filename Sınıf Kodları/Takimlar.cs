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
    public partial class Takimlar : Form
    {
        SqlBagla bglT = new SqlBagla();
        bool sutunayar = false; // Sütun genişliklerini sadece 1 kez ayarlamak için değişken tanımlıyoruz
        string tAd;
        public Takimlar()
        {
            InitializeComponent();
        }

        void Listele()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select * from TabloTakım where TurnuvaID = @p1", bglT.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tabloTA.DataSource = dt;
                GridView tblAyar = tabloTA.MainView as GridView;
                if (!sutunayar && tblAyar != null)
                {
                    tblAyar.Columns["TurnuvaID"].Visible = false;
                    tblAyar.Columns["Devam"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Regular);
                    tblAyar.Columns["TakımID"].Caption = "ID"; // TakımID kısmına sadece ID yazarak kısaltıyoruz
                    tblAyar.Columns["TakımID"].Width = 25;
                    tblAyar.Columns["GrupID"].Width = 30;
                    tblAyar.Columns["Takım Adı"].Width = 200;
                    sutunayar = true; // GridView ayarları tamamlandı
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Takimlar_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnTAEkle_Click(object sender, EventArgs e)
        {
            SqlCommand tkmkontrol = new SqlCommand("SELECT COUNT(*) FROM TabloTakım WHERE TurnuvaID = @p1 AND [Takım Adı] = @p2", bglT.baglanti());
            tkmkontrol.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            tkmkontrol.Parameters.AddWithValue("@p2", txtTAAd.Text);
            int count = Convert.ToInt32(tkmkontrol.ExecuteScalar());
            bglT.baglanti().Close();
            if (count > 0)
            {
                MessageBox.Show(txtTAAd.Text+" Adında bir takım zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                // Ekle butonuna basıldığında girilen bilgilerle yeni takım ekle
                SqlCommand komut = new SqlCommand("insert into TabloTakım ([Takım Adı], GrupID, TurnuvaID) values (@p1, @p2, @p3)", bglT.baglanti());
                komut.Parameters.AddWithValue("@p1", txtTAAd.Text);
                komut.Parameters.AddWithValue("@p2", txtGID.Text);
                komut.Parameters.AddWithValue("@p3", AnaSayfa.TID);
                komut.ExecuteNonQuery();
                bglT.baglanti().Close();
                MessageBox.Show("Takım Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu, Turnuva seçtiğinizden ve GrupID değerine sayı girdiğinizden emin olun. " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTAGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand tkmkontrol = new SqlCommand("SELECT COUNT(*) FROM TabloTakım WHERE TurnuvaID = @p1 AND [Takım Adı] = @p2", bglT.baglanti());
            tkmkontrol.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            tkmkontrol.Parameters.AddWithValue("@p2", txtTAAd.Text);
            int count = Convert.ToInt32(tkmkontrol.ExecuteScalar());
            bglT.baglanti().Close();
            if (count > 0 && tAd != txtTAAd.Text)
            {
                MessageBox.Show(txtTAAd.Text + " Adında bir takım zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Girilen bilgilerle seçilen takımı güncelle
                SqlCommand komut = new SqlCommand("Update TabloTakım set [Takım Adı]=@p1, GrupID=@p2 where TakımID=@p3", bglT.baglanti());
                komut.Parameters.AddWithValue("@p1", txtTAAd.Text);
                komut.Parameters.AddWithValue("@p2", txtGID.Text);
                komut.Parameters.AddWithValue("@p3", txtTAID.Text);
                komut.ExecuteNonQuery();
                bglT.baglanti().Close();
                MessageBox.Show("Takım Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }
        }

        private void btnTASil_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Delete from TabloTakım where TakımID=@p1", bglT.baglanti());
            komut.Parameters.AddWithValue("@p1", txtTAID.Text);
            komut.ExecuteNonQuery();
            bglT.baglanti().Close();
            MessageBox.Show("Takım Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Listele();
        }

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            // Tablo üzerinden tıklanan takımın adını text kutusuna yaz
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                tAd = dr["Takım Adı"].ToString();
                txtTAAd.Text = tAd;
                txtGID.Text = dr["GrupID"].ToString();
                txtTAID.Text = dr["TakımID"].ToString();
            }
        }
    }
}
