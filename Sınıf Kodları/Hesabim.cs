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
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace otomasyon
{
    public partial class Hesabim : Form
    {
        SqlBagla bglH = new SqlBagla();
        Giris giris;
        string Sifre;
        public Hesabim()
        {
            InitializeComponent();
        }
        private void Hesabim_Load(object sender, EventArgs e)
        {
            HBilgi();
        }

        private void BtnOEkle_Click(object sender, EventArgs e)
        {
            giris = new Giris();
            
            Form form1 = Application.OpenForms["Form1"]; // Form1'i bul
            if (form1 != null)
            {
                form1.Close(); // Form1'i kapat
            }
            giris.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void HBilgi()
        {
            KID.Text = Convert.ToString(Giris.KID);
            SqlCommand komut = new SqlCommand("SELECT [Kullanıcı Adı], Ad, Soyad, [E posta], Telefon, Şifre FROM TabloKullanıcı WHERE KullanıcıID = @KID", bglH.baglanti());
            komut.Parameters.AddWithValue("@KID", Giris.KID);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                Sifre = dr["Şifre"].ToString();
                txtKAd.Text = dr["Kullanıcı Adı"].ToString();
                txtAd.Text = dr["Ad"].ToString();
                txtSoyad.Text = dr["Soyad"].ToString();
                txtEposta.Text = dr["E posta"].ToString();
                txtTel.Text = dr["Telefon"].ToString();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (txtSifre.Text == Sifre)
            {
                SqlCommand komut = new SqlCommand("Update TabloKullanıcı set [Kullanıcı Adı]=@p1, Ad=@p2, Soyad=@p3, [E posta]=@p4, Telefon=@p5 where KullanıcıID=@p6 ", bglH.baglanti());
                komut.Parameters.AddWithValue("@p1", txtKAd.Text);
                komut.Parameters.AddWithValue("@p2", txtAd.Text);
                komut.Parameters.AddWithValue("@p3", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p4", txtEposta.Text);
                komut.Parameters.AddWithValue("@p5", txtTel.Text);
                komut.Parameters.AddWithValue("@p6", Giris.KID);
                komut.ExecuteNonQuery();
                MessageBox.Show("Bilgiler Güncellendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Şifre Yanlış", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (txtS1.Text == txtS2.Text)
            {
                if (txtSSifre.Text == Sifre)
                {
                    SqlCommand komut = new SqlCommand("Update TabloKullanıcı set Şifre=@p1 where KullanıcıID=@p2 ", bglH.baglanti());
                    komut.Parameters.AddWithValue("@p1", txtS1.Text);
                    komut.Parameters.AddWithValue("@p2", Giris.KID);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Şifre Güncellendi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Şifre Yanlış", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Yeni şifre eşleşmiyor", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            PuanDurumuPDF();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            FikstürPDF();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            OyuncularPDF();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            HepsiPDF();
        }
        public void OyuncularPDF()
        {
            try
            {
                using (SqlConnection conn = bglH.baglanti())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmnd = new SqlCommand("SELECT * FROM TabloOyuncular WHERE TurnuvaID = @p1", conn);
                    cmnd.Parameters.AddWithValue("@p1", AnaSayfa.TID);

                    using (SqlDataReader rdr = cmnd.ExecuteReader())
                    {
                        using (PdfDocument document = new PdfDocument())
                        {
                            document.Info.Title = "Oyuncular";
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XFont font = new XFont("Verdana", 12);

                            double yPoint = 40;

                            gfx.DrawString("Ad", font, XBrushes.Black, new XRect(20, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Soyad", font, XBrushes.Black, new XRect(130, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Ülke", font, XBrushes.Black, new XRect(240, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Yaş", font, XBrushes.Black, new XRect(310, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Takım", font, XBrushes.Black, new XRect(340, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Maç", font, XBrushes.Black, new XRect(460, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Skor", font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Asist", font, XBrushes.Black, new XRect(540, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;

                            while (rdr.Read())
                            {
                                gfx.DrawString(rdr["Ad"].ToString(), font, XBrushes.Black, new XRect(20, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Soyad"].ToString(), font, XBrushes.Black, new XRect(130, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Ülke"].ToString(), font, XBrushes.Black, new XRect(240, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Yaş"].ToString(), font, XBrushes.Black, new XRect(310, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Takım"].ToString(), font, XBrushes.Black, new XRect(340, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Maç"].ToString(), font, XBrushes.Black, new XRect(460, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Skor"].ToString(), font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Asist"].ToString(), font, XBrushes.Black, new XRect(540, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                yPoint += 20;
                            }

                            string yol = Environment.CurrentDirectory;
                            document.Save("Oyuncular.pdf");
                            MessageBox.Show("Oyuncular PDF olarak kaydedildi: " + yol, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FikstürPDF()
        {
            using (SqlConnection conn = bglH.baglanti())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmnd = new SqlCommand(@"
            SELECT 
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
                m.Hafta", conn);
                cmnd.Parameters.AddWithValue("@p1", AnaSayfa.TID);

                using (SqlDataReader rdr = cmnd.ExecuteReader())
                {
                    using (PdfDocument document = new PdfDocument())
                    {
                        document.Info.Title = "Fikstür";
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont font = new XFont("Verdana", 12);

                        double yPoint = 40;

                        gfx.DrawString("Hafta", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("EvSahibi", font, XBrushes.Black, new XRect(90, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("SkorEv", font, XBrushes.Black, new XRect(220, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("SkorDp", font, XBrushes.Black, new XRect(270, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Deplasman", font, XBrushes.Black, new XRect(320, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Tarih", font, XBrushes.Black, new XRect(430, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                        yPoint += 20;

                        while (rdr.Read())
                        {
                            gfx.DrawString(rdr["Hafta"].ToString(), font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["EvSahibi"].ToString(), font, XBrushes.Black, new XRect(90, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["SkorEv"].ToString(), font, XBrushes.Black, new XRect(220, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["SkorDp"].ToString(), font, XBrushes.Black, new XRect(270, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Deplasman"].ToString(), font, XBrushes.Black, new XRect(320, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Tarih"].ToString(), font, XBrushes.Black, new XRect(430, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;
                        }
                        string yol = Environment.CurrentDirectory;
                        document.Save("Fikstur.pdf");
                        MessageBox.Show("Fikstür PDF olarak kaydedildi: " + yol, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public void PuanDurumuPDF()
        {
            using (SqlConnection conn = bglH.baglanti())
            {
                // Bağlantıyı kontrol et
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand command = new SqlCommand("SELECT [Takım Adı], Puan, Galibiyet, Beraberlik, Yenilgi, [Atılan Gol], [Yenilen Gol] FROM TabloTakım WHERE TurnuvaID = @p1 ORDER BY Puan DESC", conn);
                command.Parameters.AddWithValue("@p1", AnaSayfa.TID);

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    using (PdfDocument document = new PdfDocument())
                    {
                        document.Info.Title = "Puan Durumu";
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont font = new XFont("Verdana", 12); // Stil belirtilmeden font oluşturuldu

                        // Y ekseninde başlangıç noktası
                        double yPoint = 40;

                        // Başlıkları yaz
                        gfx.DrawString("Takım Adı", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Puan", font, XBrushes.Black, new XRect(200, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Galibiyet", font, XBrushes.Black, new XRect(300, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Beraberlik", font, XBrushes.Black, new XRect(400, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Mağlubiyet", font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Atılan Gol", font, XBrushes.Black, new XRect(600, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        gfx.DrawString("Yenilen Gol", font, XBrushes.Black, new XRect(700, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                        yPoint += 20;

                        while (rdr.Read())
                        {
                            gfx.DrawString(rdr["Takım Adı"].ToString(), font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Puan"].ToString(), font, XBrushes.Black, new XRect(200, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Galibiyet"].ToString(), font, XBrushes.Black, new XRect(300, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Beraberlik"].ToString(), font, XBrushes.Black, new XRect(400, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Yenilgi"].ToString(), font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Atılan Gol"].ToString(), font, XBrushes.Black, new XRect(600, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString(rdr["Yenilen Gol"].ToString(), font, XBrushes.Black, new XRect(700, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;
                        }
                        string yol = Environment.CurrentDirectory;
                        document.Save("PuanDurumu.pdf");
                        MessageBox.Show("Puan durumu PDF olarak kaydedildi: "+ yol, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
        }

        public void HepsiPDF()
        {
            try
            {
                using (SqlConnection conn = bglH.baglanti())
                {
                    double yPoint = 40;
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (PdfDocument document = new PdfDocument())
                    {
                        // Oyuncular
                        SqlCommand cmnd = new SqlCommand("SELECT * FROM TabloOyuncular WHERE TurnuvaID = @p1", conn);
                        cmnd.Parameters.AddWithValue("@p1", AnaSayfa.TID);
                        
                        using (SqlDataReader rdr = cmnd.ExecuteReader())
                        {
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XFont font = new XFont("Verdana", 12);
                            

                            gfx.DrawString("Oyuncular", new XFont("Verdana", 16), XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            yPoint += 30;

                            gfx.DrawString("Ad", font, XBrushes.Black, new XRect(20, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Soyad", font, XBrushes.Black, new XRect(130, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Ülke", font, XBrushes.Black, new XRect(240, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Yaş", font, XBrushes.Black, new XRect(310, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Takım", font, XBrushes.Black, new XRect(340, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Maç", font, XBrushes.Black, new XRect(460, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Skor", font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Asist", font, XBrushes.Black, new XRect(540, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;

                            while (rdr.Read())
                            {
                                gfx.DrawString(rdr["Ad"].ToString(), font, XBrushes.Black, new XRect(20, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Soyad"].ToString(), font, XBrushes.Black, new XRect(130, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Ülke"].ToString(), font, XBrushes.Black, new XRect(240, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Yaş"].ToString(), font, XBrushes.Black, new XRect(310, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Takım"].ToString(), font, XBrushes.Black, new XRect(340, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Maç"].ToString(), font, XBrushes.Black, new XRect(460, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Skor"].ToString(), font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Asist"].ToString(), font, XBrushes.Black, new XRect(540, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                yPoint += 20;
                            }
                        }

                        yPoint += 20; // Boşluk bırak

                        // Fikstür
                        SqlCommand fikstürCmd = new SqlCommand(@"
                    SELECT 
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
                        m.Hafta", conn);
                        fikstürCmd.Parameters.AddWithValue("@p1", AnaSayfa.TID);

                        using (SqlDataReader rdr = fikstürCmd.ExecuteReader())
                        {
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XFont font = new XFont("Verdana", 12);
                            yPoint = 40;

                            gfx.DrawString("Fikstür", new XFont("Verdana", 16), XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            yPoint += 30;

                            gfx.DrawString("Hafta", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("EvSahibi", font, XBrushes.Black, new XRect(90, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("SkorEv", font, XBrushes.Black, new XRect(220, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("SkorDp", font, XBrushes.Black, new XRect(270, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Deplasman", font, XBrushes.Black, new XRect(320, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Tarih", font, XBrushes.Black, new XRect(430, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;

                            while (rdr.Read())
                            {
                                gfx.DrawString(rdr["Hafta"].ToString(), font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["EvSahibi"].ToString(), font, XBrushes.Black, new XRect(90, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["SkorEv"].ToString(), font, XBrushes.Black, new XRect(220, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["SkorDp"].ToString(), font, XBrushes.Black, new XRect(270, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Deplasman"].ToString(), font, XBrushes.Black, new XRect(320, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Tarih"].ToString(), font, XBrushes.Black, new XRect(430, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                yPoint += 20;
                            }
                        }

                        yPoint += 20; // Boşluk bırak

                        // Puan Durumu
                        SqlCommand puanDurumuCmd = new SqlCommand("SELECT [Takım Adı], Puan, Galibiyet, Beraberlik, Yenilgi, [Atılan Gol], [Yenilen Gol] FROM TabloTakım WHERE TurnuvaID = @p1 ORDER BY Puan DESC", conn);
                        puanDurumuCmd.Parameters.AddWithValue("@p1", AnaSayfa.TID);

                        using (SqlDataReader rdr = puanDurumuCmd.ExecuteReader())
                        {
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XFont font = new XFont("Verdana", 12);
                            yPoint = 40;

                            gfx.DrawString("Puan Durumu", new XFont("Verdana", 16), XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            yPoint += 30;

                            gfx.DrawString("Takım Adı", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Puan", font, XBrushes.Black, new XRect(200, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Galibiyet", font, XBrushes.Black, new XRect(300, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Beraberlik", font, XBrushes.Black, new XRect(400, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Yenilgi", font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Atılan Gol", font, XBrushes.Black, new XRect(600, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                            gfx.DrawString("Yenilen Gol", font, XBrushes.Black, new XRect(700, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

                            yPoint += 20;

                            while (rdr.Read())
                            {
                                gfx.DrawString(rdr["Takım Adı"].ToString(), font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Puan"].ToString(), font, XBrushes.Black, new XRect(200, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Galibiyet"].ToString(), font, XBrushes.Black, new XRect(300, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Beraberlik"].ToString(), font, XBrushes.Black, new XRect(400, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Yenilgi"].ToString(), font, XBrushes.Black, new XRect(500, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Atılan Gol"].ToString(), font, XBrushes.Black, new XRect(600, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                gfx.DrawString(rdr["Yenilen Gol"].ToString(), font, XBrushes.Black, new XRect(700, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                                yPoint += 20;
                            }
                        }

                        // PDF dosyasını kaydet
                        string yol = Environment.CurrentDirectory;
                        document.Save("Hepsi.pdf");
                        MessageBox.Show("Tüm veriler PDF olarak kaydedildi: " + yol, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

