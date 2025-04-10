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
    public partial class Sonuc : Form
    {
        SqlBagla bglS = new SqlBagla();
        bool sutunayar = false;
        int MacID;
        DateTime tarih;
        int tID1;
        int tID2;

        public Sonuc()
        {
            InitializeComponent();
        }

        private void Sonuc_Load(object sender, EventArgs e)
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
                m.Tarih,
                m.Takim1ID,
                m.Takim2ID
            FROM 
                TabloMaclar m
            JOIN 
                TabloTakım t1 ON m.Takim1ID = t1.TakımID
            JOIN 
                TabloTakım t2 ON m.Takim2ID = t2.TakımID
            WHERE 
                m.TurnuvaID = @p1
            ORDER BY 
                m.Hafta, m.Tarih", bglS.baglanti());

                da.SelectCommand.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                da.Fill(dt);
                tblf.DataSource = dt;
                GridView tblAyar = tblf.MainView as GridView;
                if (!sutunayar && tblAyar != null)
                {
                    tblAyar.Columns["MacID"].Visible = false;
                    tblAyar.Columns["Takim1ID"].Visible = false;
                    tblAyar.Columns["Takim2ID"].Visible = false;
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

        private void gridView1_FocusedRowObjectChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
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
                dTime.Value = tarih;
                tID1 = Convert.ToInt32(dr["Takim1ID"]);
                tID2 = Convert.ToInt32(dr["Takim2ID"]);
                oyuncuG(tID1, tID2);
                oyuncuA(tID1, tID2);
            }
        }

        private void btnTAEkle_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand komut = new SqlCommand("Update TabloMaclar set SkorTakim1=@p1, SkorTakim2=@p2, Tarih=@p3 where MacID=@p4", bglS.baglanti());
                komut.Parameters.AddWithValue("@p1", s1.Text);
                komut.Parameters.AddWithValue("@p2", s2.Text);
                komut.Parameters.AddWithValue("@p3", dTime.Value);
                komut.Parameters.AddWithValue("@p4", MacID);
                komut.ExecuteNonQuery();
                int skorEv = int.Parse(s1.Text);
                int skorDp = int.Parse(s2.Text);


                SqlCommand kontrolKomut = new SqlCommand("SELECT Puan FROM TabloMaclar WHERE MacID = @p1",bglS.baglanti());
                kontrolKomut.Parameters.AddWithValue("@p1", MacID);
                int puan = Convert.ToInt32(kontrolKomut.ExecuteScalar());

                if (puan == 1) // Puan durumunu etkiliyorsa
                {
                    if (skorEv > skorDp)
                    {
                        SqlCommand updateEv = new SqlCommand("UPDATE TabloTakım SET Puan = Puan + 3, Galibiyet = Galibiyet + 1 WHERE TakımID = @p1; UPDATE TabloTakım SET Yenilgi = Yenilgi + 1 WHERE TakımID = @p2",bglS.baglanti());
                        updateEv.Parameters.AddWithValue("@p1", tID1);
                        updateEv.Parameters.AddWithValue("@p2", tID2);
                        updateEv.ExecuteNonQuery();
                    }
                    else if (skorEv < skorDp)
                    {
                        SqlCommand updateDp = new SqlCommand("UPDATE TabloTakım SET Puan = Puan + 3, Galibiyet = Galibiyet + 1 WHERE TakımID = @p1; UPDATE TabloTakım SET Yenilgi = Yenilgi + 1 WHERE TakımID = @p2",bglS.baglanti());
                        updateDp.Parameters.AddWithValue("@p1", tID2);
                        updateDp.Parameters.AddWithValue("@p2", tID1);
                        updateDp.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand updateBoth = new SqlCommand("UPDATE TabloTakım SET Puan = Puan + 1, Beraberlik = Beraberlik + 1 WHERE TakımID = @p1 OR TakımID = @p2",bglS.baglanti());
                        updateBoth.Parameters.AddWithValue("@p1", tID1);
                        updateBoth.Parameters.AddWithValue("@p2", tID2);
                        updateBoth.ExecuteNonQuery();
                    }
                }
                else if(AnaSayfa.MacTur == 0)   // Maç Türü: 0-Tek Devreli  1-Çift Devreli
                {
                    if (skorEv > skorDp)
                    {
                        SqlCommand updateEv = new SqlCommand("UPDATE TabloTakım SET Devam = 0 WHERE TakımID = @p1", bglS.baglanti());
                        updateEv.Parameters.AddWithValue("@p1", tID2);
                        updateEv.ExecuteNonQuery();
                    }
                    else if (skorEv < skorDp)
                    {
                        SqlCommand updateEv = new SqlCommand("UPDATE TabloTakım SET Devam = 0 WHERE TakımID = @p1", bglS.baglanti());
                        updateEv.Parameters.AddWithValue("@p1", tID1);
                        updateEv.ExecuteNonQuery();
                    }
                }
                else
                {
                    ElemeKontrol(tID1, tID2);
                }
                SqlCommand updateEvGol = new SqlCommand("UPDATE TabloTakım SET [Atılan Gol] = [Atılan Gol] + @agol, [Yenilen Gol] = [Yenilen Gol] + @ygol WHERE TakımID = @p1", bglS.baglanti());
                updateEvGol.Parameters.AddWithValue("@agol", skorEv);
                updateEvGol.Parameters.AddWithValue("@ygol", skorDp);
                updateEvGol.Parameters.AddWithValue("@p1", tID1);
                updateEvGol.ExecuteNonQuery();

                SqlCommand updateDpGol = new SqlCommand("UPDATE TabloTakım SET [Atılan Gol] = [Atılan Gol] + @agol, [Yenilen Gol] = [Yenilen Gol] + @ygol WHERE TakımID = @p1", bglS.baglanti());
                updateDpGol.Parameters.AddWithValue("@agol", skorDp);
                updateDpGol.Parameters.AddWithValue("@ygol", skorEv);
                updateDpGol.Parameters.AddWithValue("@p1", tID2);
                updateDpGol.ExecuteNonQuery();
                bglS.baglanti().Close();
                MessageBox.Show("Maç ve Puanlar Hesaplandı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ElemeKontrol(int tID1, int tID2)
        {
            try
            {
                SqlCommand cmdEle = new SqlCommand(@"
            SELECT 
                SUM(CASE WHEN Takim1ID = @p1 THEN SkorTakim1 ELSE SkorTakim2 END) AS Takim1Toplam,
                SUM(CASE WHEN Takim1ID = @p2 THEN SkorTakim1 ELSE SkorTakim2 END) AS Takim2Toplam
            FROM TabloMaclar
            WHERE 
                (Takim1ID = @p1 AND Takim2ID = @p2) OR (Takim1ID = @p2 AND Takim2ID = @p1)", bglS.baglanti());
                cmdEle.Parameters.AddWithValue("@p1", tID1);
                cmdEle.Parameters.AddWithValue("@p2", tID2);

                SqlDataReader dr = cmdEle.ExecuteReader();
                if (dr.Read())
                {
                    int takim1Toplam = Convert.ToInt32(dr["Takim1Toplam"]);
                    int takim2Toplam = Convert.ToInt32(dr["Takim2Toplam"]);

                    if (takim1Toplam > takim2Toplam)
                    {
                        SqlCommand cmdE = new SqlCommand("UPDATE TabloTakım SET Devam = 0 WHERE TakımID = @p1", bglS.baglanti());
                        cmdE.Parameters.AddWithValue("@p1", tID2);
                        cmdE.ExecuteNonQuery();
                        bglS.baglanti().Close();
                        SqlCommand cmdE1 = new SqlCommand("UPDATE TabloTakım SET Devam = 1 WHERE TakımID = @p1", bglS.baglanti());
                        cmdE1.Parameters.AddWithValue("@p1", tID1);
                        cmdE1.ExecuteNonQuery();
                        bglS.baglanti().Close();
                    }
                    else if (takim1Toplam < takim2Toplam)
                    {
                        SqlCommand cmdE2 = new SqlCommand("UPDATE TabloTakım SET Devam = 0 WHERE TakımID = @p1", bglS.baglanti());
                        cmdE2.Parameters.AddWithValue("@p1", tID1);
                        cmdE2.ExecuteNonQuery();
                        bglS.baglanti().Close();
                        SqlCommand cmdE3 = new SqlCommand("UPDATE TabloTakım SET Devam = 1 WHERE TakımID = @p1", bglS.baglanti());
                        cmdE3.Parameters.AddWithValue("@p1", tID2);
                        cmdE3.ExecuteNonQuery();
                        bglS.baglanti().Close();
                    }
                }
                dr.Close();
                bglS.baglanti().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void oyuncuG(int a, int b)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT OyuncuID, Ad FROM TabloOyuncular WHERE TakımID=@p1 or TakımID=@p2", bglS.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", a);
            da.SelectCommand.Parameters.AddWithValue("@p2", b);
            da.Fill(dt);
            cmbG.DataSource = dt;
            cmbG.DisplayMember = "Ad";
            cmbG.ValueMember = "OyuncuID";
            cmbG.SelectedIndex = -1;
        }
        void oyuncuA(int a, int b)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT OyuncuID, Ad FROM TabloOyuncular WHERE TakımID=@p1 or TakımID=@p2", bglS.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@p1", a);
            da.SelectCommand.Parameters.AddWithValue("@p2", b);
            da.Fill(dt);
            cmbA.DataSource = dt;
            cmbA.DisplayMember = "Ad";
            cmbA.ValueMember = "OyuncuID";
            cmbA.SelectedIndex = -1;
        }

        private void btnG_Click(object sender, EventArgs e)
        {
            if (cmbG.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir Oyuncu seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oID = Convert.ToInt32(cmbG.SelectedValue);
            SqlCommand komut = new SqlCommand("insert into TabloSkor (OyuncuID, MacID, Gol) values (@p1,@p2,@p3)", bglS.baglanti());
            komut.Parameters.AddWithValue("@p1", oID);
            komut.Parameters.AddWithValue("@p2", MacID);
            komut.Parameters.AddWithValue("@p3", true);
            komut.ExecuteNonQuery();
            bglS.baglanti().Close();

            SqlCommand updateEv = new SqlCommand("UPDATE TabloOyuncular SET Skor = Skor + 1, Maç = Maç + 1 WHERE OyuncuID = @p1", bglS.baglanti());
            updateEv.Parameters.AddWithValue("@p1", oID);
            updateEv.ExecuteNonQuery();
            bglS.baglanti().Close();

            MessageBox.Show("Oyuncu Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            if (cmbA.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir Oyuncu seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int oID = Convert.ToInt32(cmbA.SelectedValue);
            SqlCommand komut = new SqlCommand("insert into TabloSkor (OyuncuID, MacID, Asist) values (@p1,@p2,@p3)", bglS.baglanti());
            komut.Parameters.AddWithValue("@p1", oID);
            komut.Parameters.AddWithValue("@p2", MacID);
            komut.Parameters.AddWithValue("@p3", true);
            komut.ExecuteNonQuery();
            bglS.baglanti().Close();

            SqlCommand updateEv = new SqlCommand("UPDATE TabloOyuncular SET Asist = Asist + 1, Maç = Maç + 1 WHERE OyuncuID = @p1", bglS.baglanti());
            updateEv.Parameters.AddWithValue("@p1", oID);
            updateEv.ExecuteNonQuery();
            bglS.baglanti().Close();

            MessageBox.Show("Oyuncu Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
