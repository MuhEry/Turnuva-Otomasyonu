using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;

namespace otomasyon
{
    public partial class Fikstur : Form
    {
        SqlBagla bglF = new SqlBagla();
        bool sutunayar = false;
        int MacID;
        DateTime tarih;

        public Fikstur()
        {
            InitializeComponent();
        }

        private void Fikstur_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT 
                m.MacID,
                m.Hafta,
                t1.[Takım Adı] AS EvSahibi,
                m.SkorTakim1 AS SkorEv,
                m.SkorTakim2 AS SkorDp,
                t2.[Takım Adı] AS Deplasman,
                m.Tarih
            FROM 
                TabloMaclar m
            JOIN 
                TabloTakım t1 ON m.Takim1ID = t1.TakımID
            JOIN 
                TabloTakım t2 ON m.Takim2ID = t2.TakımID
            WHERE 
                m.TurnuvaID = @p1
            ORDER BY 
                m.Hafta, m.Tarih", bglF.baglanti());

                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tblf.DataSource = dt;
                GridView tblAyar = tblf.MainView as GridView;
                if (!sutunayar && tblAyar != null)
                {
                    tblAyar.Columns["MacID"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.OptionsView.EnableAppearanceOddRow = true;
                    tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                    tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(245, 245, 245);
                    tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 14, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 14, FontStyle.Bold);
                    tblAyar.Columns["SkorEv"].Width = 10;
                    tblAyar.Columns["SkorDp"].Width = 10;
                    tblAyar.Columns["Hafta"].Width = 3;
                    tblAyar.Columns["Tarih"].Width = 55;
                    sutunayar = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                t1.Text = dr["EvSahibi"].ToString();
                t2.Text = dr["Deplasman"].ToString();
                s1.Text = dr["SkorEv"].ToString();
                s2.Text = dr["SkorDp"].ToString();
                MacID = Convert.ToInt32(dr["MacID"]);
                tID.Text = "Maç ID: " + MacID;
                tarih = Convert.ToDateTime(dr["Tarih"]);
                tt.Text = "Tarih: " + tarih;
                SkorListe(MacID);
                AsistListe(MacID);
            }
        }
        void SkorListe(int MacID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT 
                ts.MacID,
                oy.Ad,
                oy.Soyad
            FROM 
                TabloSkor ts
            JOIN 
                TabloOyuncular oy ON ts.OyuncuID = oy.OyuncuID
            WHERE 
                ts.MacID = @p1 and ts.Gol=1", bglF.baglanti());

                da.SelectCommand.Parameters.AddWithValue("@p1", MacID);
                da.Fill(dt);
                tblG.DataSource = dt;
                GridView tblAyar = tblG.MainView as GridView;
                if (tblAyar != null)
                {
                    tblAyar.Columns["MacID"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Bold);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void AsistListe(int MacID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT 
                ts.MacID,
                oy.Ad,
                oy.Soyad
            FROM 
                TabloSkor ts
            JOIN 
                TabloOyuncular oy ON ts.OyuncuID = oy.OyuncuID
            WHERE 
                ts.MacID = @p1 and ts.Asist=1", bglF.baglanti());

                da.SelectCommand.Parameters.AddWithValue("@p1", MacID);
                da.Fill(dt);
                tblS.DataSource = dt;
                GridView tblAyar = tblS.MainView as GridView;

                    tblAyar.Columns["MacID"].Visible = false;
                    tblAyar.OptionsBehavior.Editable = false;
                    tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                    tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 12, FontStyle.Bold);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu. Turnuva seçtiğinizden emin olun: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
