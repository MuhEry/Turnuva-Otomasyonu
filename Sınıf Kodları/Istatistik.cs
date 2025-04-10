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
using DevExpress.XtraCharts;

namespace otomasyon
{
    public partial class Istatistik : Form
    {
        SqlBagla bglS = new SqlBagla();

        public Istatistik()
        {
            InitializeComponent();
        }

        private void Istatistik_Load(object sender, EventArgs e)
        {
            chart1.Titles.Add("Maç Sonuç Oranları");
            ListeleG();
            ListeleA();
            ListeleT();
        }

        void ListeleG()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select Ad, Soyad, Skor from TabloOyuncular where TurnuvaID = @p1 and Skor > 0", bglS.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tabloOyuncu.DataSource = dt;
                GridView tblAyar = tabloOyuncu.MainView as GridView;
                if (tblAyar != null)
                {
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 15, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 15, FontStyle.Regular);
                    gridView1.Appearance.FocusedRow.BackColor = Color.FromArgb(180, 250, 250);
                    tblAyar.OptionsView.EnableAppearanceOddRow = true;
                    tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                    tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                    tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(220, 250, 250);
                    tblAyar.Columns["Ad"].Width = 100;
                    tblAyar.Columns["Soyad"].Width = 100;
                    tblAyar.Columns["Skor"].Width = 40;
                    tblAyar.SortInfo.Add(tblAyar.Columns["Skor"], DevExpress.Data.ColumnSortOrder.Descending);
                    tblAyar.Columns["Skor"].Caption = "Gol";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ListeleA()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select Ad, Soyad, Asist from TabloOyuncular where TurnuvaID = @p1 and Asist > 0", bglS.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tabloAS.DataSource = dt;
                GridView tblAyar = tabloAS.MainView as GridView;
                if (tblAyar != null)
                {
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 15, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 15, FontStyle.Regular);
                    gridView1.Appearance.FocusedRow.BackColor = Color.FromArgb(180, 250, 250);
                    tblAyar.OptionsView.EnableAppearanceOddRow = true;
                    tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                    tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                    tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(220, 250, 250);
                    tblAyar.Columns["Ad"].Width = 100;
                    tblAyar.Columns["Soyad"].Width = 100;
                    tblAyar.Columns["Asist"].Width = 50;
                    tblAyar.SortInfo.Add(tblAyar.Columns["Asist"], DevExpress.Data.ColumnSortOrder.Descending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ListeleT()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("Select [Takım Adı], TakımID, Galibiyet, Beraberlik, Yenilgi, Puan, [Atılan Gol], [Yenilen Gol] from TabloTakım where TurnuvaID = @p1", bglS.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tblT.DataSource = dt;
                GridView tblAyar = tblT.MainView as GridView;
                if (tblAyar != null)
                {
                    //tblAyar.Columns["TakımID"].Visible = false;
                    tblAyar.Columns["Galibiyet"].Visible = false;
                    tblAyar.Columns["Beraberlik"].Visible = false;
                    tblAyar.Columns["Yenilgi"].Visible = false;
                    tblAyar.Columns["Puan"].Visible = false;
                    tblAyar.Columns["Atılan Gol"].Visible = false;
                    tblAyar.Columns["Yenilen Gol"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 15, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 15, FontStyle.Regular);
                    gridView1.Appearance.FocusedRow.BackColor = Color.FromArgb(180, 250, 250);
                    tblAyar.OptionsView.EnableAppearanceOddRow = true;
                    tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                    tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                    tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(220, 250, 250);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView3_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            if (dr != null)
            {
                try
                {
                    lblH.Text = "";
                    chart1.Series["s1"].Points.Clear();
                    chart1.Series["s1"].IsValueShownAsLabel = true;

                    int galibiyet = Convert.ToInt32(dr["Galibiyet"]);
                    int beraberlik = Convert.ToInt32(dr["Beraberlik"]);
                    int yenilgi = Convert.ToInt32(dr["Yenilgi"]);
                    int toplamMac = galibiyet + beraberlik + yenilgi;
                    int atlGol = Convert.ToInt32(dr["Atılan Gol"]);
                    int yenGol = Convert.ToInt32(dr["Yenilen Gol"]);
                    int puan = Convert.ToInt32(dr["Puan"]);

                    int kazanmaOrani = Convert.ToInt32((galibiyet * 1.0 / toplamMac) * 100);
                    int beraberlikOrani = Convert.ToInt32((beraberlik * 1.0 / toplamMac) * 100);
                    int yenilgiOrani = Convert.ToInt32((yenilgi * 1.0 / toplamMac) * 100);

                    chart1.Series["s1"].Points.AddXY("Kazanma", kazanmaOrani);
                    chart1.Series["s1"].Points.AddXY("Beraberlik", beraberlikOrani);
                    chart1.Series["s1"].Points.AddXY("Yenilgi", yenilgiOrani);

                    lbl1.Text = "Toplam Maç: " + toplamMac;
                    lbl2.Text = "Atılan Gol: " + atlGol;
                    lbl3.Text = "Yenilen Gol: " + yenGol;
                    lbl4.Text = "Maç Başına Atılan Gol: " + (float)atlGol/toplamMac;
                    lbl5.Text = "Maç başına Yenilen Gol: " + (float)yenGol / toplamMac;
                    lbl6.Text = "Maç başına Puan: " + (float)puan / toplamMac;
                }
                catch (Exception)
                {
                    lblH.Text = "Veri Bulunamadı";
                }
            }
        }
    }
}
