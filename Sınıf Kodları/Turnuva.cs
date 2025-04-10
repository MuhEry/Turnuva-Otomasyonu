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
    public partial class Turnuva : Form
    {
        SqlBagla bglT = new SqlBagla();
        public Turnuva()
        {
            InitializeComponent();
        }

        private void Turnuva_Load(object sender, EventArgs e)
        {
            Cmb();
        }

        private void BtnTO_Click(object sender, EventArgs e)
        {
            if (cmbMT.SelectedIndex == -1 || cmbTT.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtTAd.Text))
            {
                MessageBox.Show("Tüm alanları doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand komut = new SqlCommand("insert into TabloTurnuva (KullanıcıID, [Turnuva Adı], MaçTürü, TurnuvaTürü) values (@p1,@p2,@p3,@p4)", bglT.baglanti());
                komut.Parameters.AddWithValue("@p1", Giris.KID);
                komut.Parameters.AddWithValue("@p2", txtTAd.Text);
                komut.Parameters.AddWithValue("@p3", cmbMT.SelectedIndex);
                komut.Parameters.AddWithValue("@p4", cmbTT.SelectedIndex);
                komut.ExecuteNonQuery();
                bglT.baglanti().Close();
                this.Close();
                MessageBox.Show("Turnuva oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void Cmb()
        {
            cmbTT.Items.Add("Lig");
            cmbTT.Items.Add("Turnuva");
            cmbTT.Items.Add("Karma");
            cmbMT.Items.Add("Tek Devreli");
            cmbMT.Items.Add("Çift Devreli");
        }

        private void btnBM_Click(object sender, EventArgs e)
        {
            Bilgi.Text = "Maç usulü bir takımın \nrakip takımlarla bir maç mı \nyoksa iki maçlı (rövanşlı) mı \noynayacağını belirler";
        }

        private void btnBT_Click(object sender, EventArgs e)
        {
            Bilgi.Text = "Karma Sistem \ntakımların önce lig usulünde \nmaçları oynadığı, sonrasında \npuan durumuna göre bazı \ntakımların tekrar eleme usulü \nmaçları oynadığı sistemdir";
        }
    }
}
