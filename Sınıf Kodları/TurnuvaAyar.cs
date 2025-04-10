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
    public partial class TurnuvaAyar : Form
    {
        SqlBagla bglT = new SqlBagla();
        Turnuva turnuva;
        bool sutunAyar = false;
        int trnvID;

        public TurnuvaAyar()
        {
            InitializeComponent();
        }

        private void TurnuvaAyar_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select  TurnuvaID,[Turnuva Adı], TurnuvaTürü, MaçTürü from TabloTurnuva Where KullanıcıID=@p1 ", bglT.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", Giris.KID);
            da.Fill(dt);
            tabloTurnuva.DataSource = dt;
            GridView tblAyar = tabloTurnuva.MainView as GridView;
            if (!sutunAyar && tblAyar != null)
            {
                tblAyar.OptionsBehavior.Editable = false;
                tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 11, FontStyle.Bold);
                tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 11, FontStyle.Regular);
                tblAyar.Columns["TurnuvaID"].Caption = "ID";
                tblAyar.Columns["TurnuvaID"].Width = 10;
                tblAyar.Columns["Turnuva Adı"].Width = 100;
                tblAyar.Columns["TurnuvaTürü"].Width = 60;
                tblAyar.Columns["MaçTürü"].Width = 60;
                sutunAyar = true;
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Delete from TabloTurnuva where TurnuvaID=@p1", bglT.baglanti());
            komut.Parameters.AddWithValue("@p1", trnvID);
            komut.ExecuteNonQuery();
            bglT.baglanti().Close();
            MessageBox.Show("Turnuva Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Listele();
        }

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                txtTAd.Text = ("Seçilen turnuva: " + dr["Turnuva Adı"].ToString());
                trnvID = Convert.ToInt32(dr["TurnuvaID"]);
            }
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void BtnOEkle_Click(object sender, EventArgs e)
        {
            turnuva = new Turnuva();
            turnuva.ShowDialog();
            Listele();
        }

        private void btnGnclle_Click(object sender, EventArgs e)
        {
            if (txtTAAd.Text == "")
            {
                MessageBox.Show("Turnuva Adı Giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SqlCommand tkmkontrol = new SqlCommand("SELECT COUNT(*) FROM TabloTurnuva WHERE KullanıcıID = @p1 AND [Turnuva Adı] = @p2", bglT.baglanti());
            tkmkontrol.Parameters.AddWithValue("@p1", Giris.KID);
            tkmkontrol.Parameters.AddWithValue("@p2", txtTAAd.Text);
            int count = Convert.ToInt32(tkmkontrol.ExecuteScalar());
            if (count > 0)
            {
                MessageBox.Show(txtTAAd.Text + " Adında bir turnuva zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand komut = new SqlCommand("Update TabloTurnuva set [Turnuva Adı]=@p1 where KullanıcıID = @p2 AND [Turnuva Adı] = @p3", bglT.baglanti());
                komut.Parameters.AddWithValue("@p1", txtTAAd.Text);
                komut.Parameters.AddWithValue("@p2", Giris.KID);
                komut.Parameters.AddWithValue("@p3", txtTAd.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Turnuva Adı Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }
            bglT.baglanti().Close();
        }
    }
}
