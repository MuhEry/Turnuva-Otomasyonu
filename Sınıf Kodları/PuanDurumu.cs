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
    public partial class PuanDurumu : Form
    {
        SqlBagla bglP = new SqlBagla();

        public PuanDurumu()
        {
            InitializeComponent();
        }
        private void PuanDurumu_Load(object sender, EventArgs e)
        {
            Listele();
            GrupListe();
        }

        void GrupListe()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT GrupID FROM TabloTakım WHERE TurnuvaID = @p1", bglP.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);

                cmbGrup.DataSource = dt;
                cmbGrup.DisplayMember = "GrupID";
                cmbGrup.ValueMember = "GrupID";
                cmbGrup.SelectedIndex = -1;
                cmbGrup.Text = "Grup";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gruplar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * from TabloTakım where TurnuvaID = @p1", bglP.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
            da.Fill(dt);
            tabloPuan.DataSource = dt;

            GridView tblAyar = tabloPuan.MainView as GridView;
            if (tblAyar != null)
            {
                tblAyar.Columns["TurnuvaID"].Visible = false;
                tblAyar.Columns["TakımID"].Visible = false;
                tblAyar.Columns["Devam"].Visible = false;
                tblAyar.Columns["GrupID"].Width = 30;
                tblAyar.Columns["Takım Adı"].Width = 150;
                tblAyar.Columns["Puan"].Width = 50;
                tblAyar.Columns["Galibiyet"].Width = 50;
                tblAyar.Columns["Yenilgi"].Width = 50;
                tblAyar.Columns["Beraberlik"].Width = 50;
                tblAyar.Columns["Atılan Gol"].Width = 50;
                tblAyar.Columns["Yenilen Gol"].Width = 50;
                tblAyar.OptionsView.EnableAppearanceOddRow = true; // Satırları sıralı renklendirme
                tblAyar.OptionsView.EnableAppearanceEvenRow = true;
                tblAyar.Appearance.EvenRow.BackColor = Color.FromArgb(245, 245, 245);
                tblAyar.Appearance.OddRow.BackColor = Color.FromArgb(150, 240, 240);
                tblAyar.Appearance.HeaderPanel.Font = new Font("Bahnschrift", 14, FontStyle.Bold);
                tblAyar.Appearance.Row.Font = new Font("Bahnschrift", 13, FontStyle.Bold);
                tblAyar.OptionsBehavior.Editable = false;
                if (AnaSayfa.TTur == 1) // Lig Türü 0-Lig  1-Turnuva  2-Karma
                {
                    tblAyar.Columns["Puan"].Visible = false;
                    tblAyar.Columns["Galibiyet"].Visible = false;
                    tblAyar.Columns["Yenilgi"].Visible = false;
                    tblAyar.Columns["Beraberlik"].Visible = false;
                }
                else
                {
                tblAyar.SortInfo.Add(tblAyar.Columns["Puan"], DevExpress.Data.ColumnSortOrder.Descending);
                }
            }
        }

        private void cmbGrup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrup.SelectedValue != null && !string.IsNullOrEmpty(cmbGrup.SelectedValue.ToString()))
            {
                int GID;
                if (int.TryParse(cmbGrup.SelectedValue.ToString(), out GID))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter("SELECT GrupID, [Takım Adı], Puan, Galibiyet, Yenilgi, Beraberlik, [Atılan Gol], [Yenilen Gol] FROM TabloTakım WHERE TurnuvaID = @p1 AND GrupID = @p2", bglP.baglanti());
                        da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                        da.SelectCommand.Parameters.AddWithValue("@p2", GID);
                        da.Fill(dt);
                        GrupListe();
                        tabloPuan.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Takımlar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}

