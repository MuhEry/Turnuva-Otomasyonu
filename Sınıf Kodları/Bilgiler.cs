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
    public partial class Bilgiler : Form
    {
        SqlBagla bglBlg = new SqlBagla();

        public Bilgiler()
        {
            InitializeComponent();
        }

        private void Bilgiler_Load(object sender, EventArgs e)
        {
            TakimListele();
        }
        void TakimListele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select TakımID, [Takım Adı] from TabloTakım where TurnuvaID = @p1", bglBlg.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            da.Fill(dt);
            tblBlgTakim.DataSource = dt;

            GridView tblAyar = tblBlgTakim.MainView as GridView; // Sütun genişlikleri ayarlanır
            if (tblAyar != null)
            {
                tblAyar.Columns["TakımID"].Caption = "ID"; // TakımID kısmına sadece ID yazarak kısaltıyoruz
                tblAyar.Columns["TakımID"].Width = 20;
                tblAyar.Columns["Takım Adı"].Width = 200;
                tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Regular);
                tblAyar.OptionsBehavior.Editable = false;
            }
        }
        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view != null)
            {
                // Seçilen takımı alıyoruz
                string takim = Convert.ToString(view.GetFocusedRowCellValue("Takım Adı"));
                OyuncuListele(takim);
            }
        }
        void OyuncuListele(string takim)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * from TabloOyuncular where [Takım]=@p1", bglBlg.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", takim);
            da.Fill(dt);
            tblBlgOyuncu.DataSource = dt;

            GridView tblAyar = tblBlgOyuncu.MainView as GridView; // Sütun ayarları
            if (tblAyar != null)
            {
                tblAyar.Columns["TakımID"].Visible = false;
                tblAyar.Columns["OyuncuID"].Caption = "ID"; // OyuncuID kısmına sadece ID yazarak kısaltıyoruz
                tblAyar.Columns["OyuncuID"].Width = 23;
                tblAyar.Columns["Ad"].Width = 100;
                tblAyar.Columns["Soyad"].Width = 100;
                tblAyar.Columns["Ülke"].Width = 100;
                tblAyar.Columns["Yaş"].Width = 25;
                tblAyar.Columns["Takım"].Width = 100;
                tblAyar.Columns["Maç"].Width = 30;
                tblAyar.Columns["Skor"].Width = 30;
                tblAyar.Columns["Asist"].Width = 30;
                tblAyar.OptionsView.EnableAppearanceOddRow = true; // Satırları sıralı renklendirme
                tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(220, 220, 220);
                tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(250, 250, 250);
                tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Regular);
                tblAyar.OptionsBehavior.Editable = false;
            }
        }
    }
}
