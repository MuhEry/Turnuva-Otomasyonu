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
    public partial class AnaSayfa : Form
    {
        SqlBagla bglA = new SqlBagla();
        Turnuva turnuva;
        Giris giris;
        bool sutunAyar = false;
        public static int TID;
        public static byte TTur;
        public static byte MacTur;
        public static byte KHD;

        public AnaSayfa()
        {
            InitializeComponent();
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            Listele();
        }
        void Listele()
        {
            labelControl3.Text = ("Merhaba: " + Giris.Ad);
            // Turnuva listesini doldur
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select  TurnuvaID, [Turnuva Adı], TurnuvaTürü, MaçTürü from TabloTurnuva Where KullanıcıID=@p1 ", bglA.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", Giris.KID);
            da.Fill(dt);
            tabloTurnuva.DataSource = dt;
            GridView tblAyar = tabloTurnuva.MainView as GridView;
            if (!sutunAyar && tblAyar != null)
            {
                tblAyar.Columns["TurnuvaTürü"].Visible = false;
                tblAyar.Columns["MaçTürü"].Visible = false;
                tblAyar.OptionsBehavior.Editable = false;
                tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 13, FontStyle.Bold);
                tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 13, FontStyle.Bold);
                tblAyar.Columns["TurnuvaID"].Caption = "ID";
                tblAyar.Columns["TurnuvaID"].Width = 10;
                tblAyar.Columns["Turnuva Adı"].Width = 200;
                sutunAyar = true;
            }
        }

        private void BtnOEkle_Click(object sender, EventArgs e)
        {
            turnuva = new Turnuva();
            turnuva.ShowDialog();
            Listele();
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                txtTAd.Text = ("Aktif Turnuva: " + dr["Turnuva Adı"].ToString());
                TID = Convert.ToInt32(dr["TurnuvaID"]);
                TTur = Convert.ToByte(dr["TurnuvaTürü"]);
                MacTur = Convert.ToByte(dr["MaçTürü"]);
            }
        }

        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            giris = new Giris();
            KHD = 1;
            Form form1 = Application.OpenForms["Form1"];
            if (form1 != null)
            {
                form1.Close();
            }
            KHD = 0;
            giris.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
