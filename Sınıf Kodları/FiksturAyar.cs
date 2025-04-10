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
    public partial class FiksturAyar : Form
    {
        SqlBagla bglF = new SqlBagla();
        List<int> TIDListe = new List<int>();
        int hafta;
        int Thafta = 1;
        int saat;
        bool Karma1 = false;

        public FiksturAyar()
        {
            InitializeComponent();
        }

        private void FiksturAyar_Load(object sender, EventArgs e)
        {
            Sayi();
        }

        void Sayi()
        {
            try
            {
                TIDListe.Clear();
                SqlCommand cmd = new SqlCommand("SELECT TakımID FROM TabloTakım WHERE TurnuvaID = @turnuvaID and Devam = 1", bglF.baglanti());
                cmd.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);

                SqlDataReader dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {
                    TIDListe.Add(Convert.ToInt32(dr["TakımID"]));
                }
                dr.Close();
                bglF.baglanti().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Takımlar alınırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void FLig(DateTime tarih)
        {
            Sayi();
            saat = 12;
            hafta = 1;
            DateTime baslangicTarihi = dateTimePicker1.Value;
            try
            {
                for (int i = 0; i < TIDListe.Count; i++)
                {
                    if (i % 2 == 0) { hafta = 1; } else { hafta = TIDListe.Count - 1; }
                    for (int k = i + 1; k < TIDListe.Count; k++)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@t1ID", TIDListe[i]);
                        cmd.Parameters.AddWithValue("@t2ID", TIDListe[k]);
                        cmd.Parameters.AddWithValue("@hafta", hafta);
                        cmd.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * (hafta - 1)).AddHours(saat));
                        cmd.Parameters.AddWithValue("@puan", 1);
                        cmd.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                        cmd.ExecuteNonQuery();
                        if (AnaSayfa.MacTur == 1)
                        {
                            SqlCommand cmdr = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());
                            cmdr.Parameters.AddWithValue("@t1ID", TIDListe[k]);
                            cmdr.Parameters.AddWithValue("@t2ID", TIDListe[i]);
                            cmdr.Parameters.AddWithValue("@hafta", 2 * TIDListe.Count - hafta - 1);
                            cmdr.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * (2 * TIDListe.Count - hafta - 1)).AddHours(saat));
                            cmdr.Parameters.AddWithValue("@puan", 1);
                            cmdr.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                            cmdr.ExecuteNonQuery();
                        }
                        if (i % 2 == 0) { hafta++; } else { hafta--; }
                    }
                    saat += 2;
                }
                MessageBox.Show("Fikstür Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fikstür oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void FTurnuva(DateTime tarih)
        {
            Sayi();
            saat = 12;
            for (int k = 0; k < TIDListe.Count-1; k++)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@t1ID", TIDListe[k]);
                cmd.Parameters.AddWithValue("@t2ID", TIDListe[k+1]);
                cmd.Parameters.AddWithValue("@hafta", Thafta);
                cmd.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * (Thafta - 1)).AddHours(saat));
                cmd.Parameters.AddWithValue("@puan", 0);
                cmd.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                cmd.ExecuteNonQuery();
                if (AnaSayfa.MacTur == 1)
                {
                    SqlCommand cmdr = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());
                    cmdr.Parameters.AddWithValue("@t1ID", TIDListe[k+1]);
                    cmdr.Parameters.AddWithValue("@t2ID", TIDListe[k]);
                    cmdr.Parameters.AddWithValue("@hafta", Thafta + 1);
                    cmdr.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * Thafta).AddHours(saat));
                    cmdr.Parameters.AddWithValue("@puan", 0);
                    cmdr.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                    cmdr.ExecuteNonQuery();
                }
                k++;
                saat += 2;
            }
            Thafta++;
            MessageBox.Show("Maçlar Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void FKarma(DateTime tarih)
        {
            saat = 12;
            hafta = 1;
            DateTime baslangicTarihi = dateTimePicker1.Value;

            try
    {
                SqlCommand cmdG = new SqlCommand("SELECT DISTINCT GrupID FROM TabloTakım WHERE TurnuvaID = @p1", bglF.baglanti());
                cmdG.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                SqlDataAdapter daGrup = new SqlDataAdapter(cmdG);
                DataTable dtGruplar = new DataTable();
                daGrup.Fill(dtGruplar);

                foreach (DataRow grup in dtGruplar.Rows)
                {
                    int grupID = Convert.ToInt32(grup["GrupID"]);

                    TIDListe.Clear();
                    SqlCommand cmdt = new SqlCommand("SELECT TakımID FROM TabloTakım WHERE TurnuvaID = @turnuvaID AND Devam = 1 AND GrupID = @grupID", bglF.baglanti());
                    cmdt.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                    cmdt.Parameters.AddWithValue("@grupID", grupID);

                    SqlDataReader dr = cmdt.ExecuteReader();

                    while (dr.Read())
                    {
                        TIDListe.Add(Convert.ToInt32(dr["TakımID"]));
                    }
                    dr.Close();
                    bglF.baglanti().Close();

                    for (int i = 0; i < TIDListe.Count; i++)
                    {
                        if (i % 2 == 0) { hafta = 1; } else { hafta = TIDListe.Count - 1; }
                        for (int k = i + 1; k < TIDListe.Count; k++)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@t1ID", TIDListe[i]);
                            cmd.Parameters.AddWithValue("@t2ID", TIDListe[k]);
                            cmd.Parameters.AddWithValue("@hafta", hafta);
                            cmd.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * (hafta - 1)).AddHours(saat));
                            cmd.Parameters.AddWithValue("@puan", 1);
                            cmd.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                            cmd.ExecuteNonQuery();
                            if (AnaSayfa.MacTur == 1)
                            {
                                SqlCommand cmdr = new SqlCommand("INSERT INTO TabloMaclar (Takim1ID, Takim2ID, Hafta, TurnuvaID, Puan, Tarih) VALUES (@t1ID, @t2ID, @hafta, @turnuvaID, @puan, @tarih)", bglF.baglanti());
                                cmdr.Parameters.AddWithValue("@t1ID", TIDListe[k]);
                                cmdr.Parameters.AddWithValue("@t2ID", TIDListe[i]);
                                cmdr.Parameters.AddWithValue("@hafta", 2 * TIDListe.Count - hafta - 1);
                                cmdr.Parameters.AddWithValue("@tarih", tarih.AddDays(7 * (2 * TIDListe.Count - hafta - 1)).AddHours(saat));
                                cmdr.Parameters.AddWithValue("@puan", 1);
                                cmdr.Parameters.AddWithValue("@turnuvaID", AnaSayfa.TID);
                                cmdr.ExecuteNonQuery();
                            }
                            if (i % 2 == 0) { hafta++; } else { hafta--; }
                        }
                        saat += 2;
                    }
                }
                MessageBox.Show("Fikstür Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fikstür oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Karma1 = true;
        }

        private void BtnOEkle_Click(object sender, EventArgs e)
        {
            switch (AnaSayfa.TTur)
            {
                // Lig Türü 0-Lig  1-Turnuva  2-Karma
                case 0:
                    FLig(dateTimePicker1.Value);
                    break;
                case 1:
                    FTurnuva(dateTimePicker1.Value);
                    break;
                case 2:
                    if (Karma1)
                    {
                        FTurnuva(dateTimePicker1.Value);
                    }
                    else
                    {
                        FKarma(dateTimePicker1.Value);
                    }
                    break;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand komut = new SqlCommand("Delete from TabloMaclar where TurnuvaID=@p1", bglF.baglanti());
                komut.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                komut.ExecuteNonQuery();
                bglF.baglanti().Close();
                MessageBox.Show("Fikstür Temizlendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e) // Fikstür Güncelle
        {
            try
            {
                if (tkmsay.Text == "")
                {
                    MessageBox.Show("Lütfen bir sayı giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlCommand cmdG = new SqlCommand("SELECT DISTINCT GrupID FROM TabloTakım WHERE TurnuvaID = @p1", bglF.baglanti());
                    cmdG.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                    SqlDataAdapter daGrup = new SqlDataAdapter(cmdG);
                    DataTable dtGruplar = new DataTable();
                    daGrup.Fill(dtGruplar);

                    foreach (DataRow grup in dtGruplar.Rows)
                    {
                        int grupID = Convert.ToInt32(grup["GrupID"]);

                        SqlCommand takimKomut = new SqlCommand(@"SELECT TakımID FROM TabloTakım WHERE GrupID = @p1 AND TurnuvaID = @p2 ORDER BY Puan DESC, [Atılan Gol] - [Yenilen Gol] DESC", bglF.baglanti());
                        takimKomut.Parameters.AddWithValue("@p1", grupID);
                        takimKomut.Parameters.AddWithValue("@p2", AnaSayfa.TID);
                        SqlDataAdapter daTakim = new SqlDataAdapter(takimKomut);
                        DataTable dtTakimlar = new DataTable();
                        daTakim.Fill(dtTakimlar);

                        for (int i = Convert.ToInt32(tkmsay.Text); i < dtTakimlar.Rows.Count; i++)
                        {
                            int takimID = Convert.ToInt32(dtTakimlar.Rows[i]["TakımID"]);

                            SqlCommand guncelleKomut = new SqlCommand("UPDATE TabloTakım SET Devam = 0 WHERE TakımID = @p1", bglF.baglanti());
                            guncelleKomut.Parameters.AddWithValue("@p1", takimID);
                            guncelleKomut.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Fikstür Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fikstür oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand komut = new SqlCommand("Update TabloTakım set Galibiyet = 0, Beraberlik = 0, Yenilgi = 0, Puan = 0, [Atılan Gol] = 0, [Yenilen Gol] = 0, Devam = true where TurnuvaID=@p1", bglF.baglanti());
                komut.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                komut.ExecuteNonQuery();
                bglF.baglanti().Close();
                MessageBox.Show("Takımlar Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {

            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand kmtO = new SqlCommand("Update TabloOyuncular set Skor = 0, Maç = 0, Asist = 0 where TurnuvaID=@p1", bglF.baglanti());
                kmtO.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                kmtO.ExecuteNonQuery();
                bglF.baglanti().Close();
                MessageBox.Show("Oyuncular Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {

            }
        }
    }
}
