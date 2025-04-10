using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace otomasyon
{
    class SqlBagla
    {
        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection(@"Data Source=DESKTOP-0DDS0PS;Initial Catalog=dbo.TurnuvaOtomasyon;Integrated Security=True");
            baglan.Open();
            return baglan;
        }
    }
}
