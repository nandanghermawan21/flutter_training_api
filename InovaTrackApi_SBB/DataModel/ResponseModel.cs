using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    /// <summary>
    /// basic response untuk transaction
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string phoneNumber { get; set; }

        public string email { get; set; }
        /// <summary>
        /// merupakan kode status dari transaksi biasanya 0 untuk gagal 1 untuk sukses 
        /// </summary>
        public int statusCode { get; set;}

        /// <summary>
        /// mendeskripsikan maksud dari kode status
        /// </summary>
        public string statusString { get; set; }

        /// <summary>
        /// mengembalikan nilai data yang dicari atau diperlukan
        /// </summary>
        public object data { get; set; }
    }
}
