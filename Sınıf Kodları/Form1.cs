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
    public partial class Form1 : Form
    {
        PuanDurumu puanD;
        Oyuncular oyuncuAyar;
        Takimlar takimAyar;
        Bilgiler bilgiSayfasi;
        AnaSayfa anaSayfa;
        Hesabim hesabim;
        TurnuvaAyar trnvAyar;
        Fikstur fikstur;
        FiksturAyar fAyar;
        Sonuc snc;
        Istatistik ist;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            anaSayfa = new AnaSayfa();
            anaSayfa.MdiParent = this;
            anaSayfa.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AnaSayfa.KHD == 0)
            {
                Application.Exit();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AnaSayfa.KHD == 0)
            {
                    DialogResult result = MessageBox.Show("Uygulamayı kapatmak istiyor musunuz? ", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;  // Formun kapanmasını engelle
                }
            }

        }

        private void btnPuanD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (puanD == null || puanD.IsDisposed)
            { 
                puanD = new PuanDurumu();
                puanD.MdiParent = this;
                puanD.Show();
            }
        }

        private void btnOyuncu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (oyuncuAyar == null || oyuncuAyar.IsDisposed)
            {
                oyuncuAyar = new Oyuncular();
                oyuncuAyar.MdiParent = this;
                oyuncuAyar.Show();
            }
        }

        private void btnTakım_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (takimAyar == null || takimAyar.IsDisposed)
            {
                takimAyar = new Takimlar();
                takimAyar.MdiParent = this;
                takimAyar.Show();
            }
        }

        private void btnBilgi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bilgiSayfasi == null || bilgiSayfasi.IsDisposed)
            {
                bilgiSayfasi = new Bilgiler();
                bilgiSayfasi.MdiParent = this;
                bilgiSayfasi.Show();
            }
        }

        private void btnAnaS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (anaSayfa == null || anaSayfa.IsDisposed)
            {
                anaSayfa = new AnaSayfa();
                anaSayfa.MdiParent = this;
                anaSayfa.Show();
            }
        }

        private void btnTurnuva_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (trnvAyar == null || trnvAyar.IsDisposed)
            {
                trnvAyar = new TurnuvaAyar();
                trnvAyar.MdiParent = this;
                trnvAyar.Show();
            }
        }

        private void btnHesap_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (hesabim == null || hesabim.IsDisposed)
            {
                hesabim = new Hesabim();
                hesabim.MdiParent = this;
                hesabim.Show();
            }
        }

        private void btnFikstur_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (fikstur == null || fikstur.IsDisposed)
            {
                fikstur = new Fikstur();
                fikstur.MdiParent = this;
                fikstur.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (fAyar == null || fAyar.IsDisposed)
            {
                fAyar = new FiksturAyar();
                fAyar.MdiParent = this;
                fAyar.Show();
            }
        }

        private void btnSonuc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (snc == null || snc.IsDisposed)
            {
                snc = new Sonuc();
                snc.MdiParent = this;
                snc.Show();
            }
        }

        private void btnIst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ist == null || ist.IsDisposed)
            {
                ist = new Istatistik();
                ist.MdiParent = this;
                ist.Show();
            }
        }
    }
}
