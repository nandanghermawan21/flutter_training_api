using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class ShipmentModel
    {
        #region contructor
        private ApplicationDbContext _db;
        private readonly AppSettings _config;
        private ProductModel _product;

        public ShipmentModel(ApplicationDbContext db, IOptions<AppSettings> config)
        {
            _db = db;
            _config = config.Value;
            _product = new ProductModel(db, config);
        }

        public ShipmentModel()
        {

        }
        #endregion

        #region parameter

        #endregion

        /// <summary>
        /// ambil informasi shipment bisa berdasarkan projectId, shipment ataupun driver id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="shipmentNo"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public Response get(string projectId = null, string shipmentNo = null, string driverId = null)
        {
            return new Response();
        }

        /// <summary>
        /// catat konfirmsi status dari driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResponseModel confirm(string driverId, int status)
        {
            return new ResponseModel();
        }

        /// <summary>
        /// catat laporan emergency dari driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="status"></param>
        /// <param name="photo">nanti terima base64 string immage</param>
        /// <returns></returns>
        public ResponseModel emergency(Emergency data)
        {
            return new ResponseModel();
        }

        public ResponseModel pod(Pod data)
        {
            return new ResponseModel();
        }

        public class Emergency
        {
            public string photo { get; set; }
            public string message { get; set; }
        }


        public class Pod
        {
            public string photo { get; set; }
            public string ttd { get; set; }
            public string receipient { get; set; }
        }

        public class Response
        {
            public string projectId { get; set; }
            public string sapShipmentNo { get; set; }
            public string shipmentNo { get; set; } /* no do pengiriman*/
            public string shipmentTimes { get; set; } /* pengiriman keberapa*/
            public DateTime shipmentDate { get; set; }
            public string vehicleNumber { get; set; }
            public string driverId { get; set; } /* dihubungkan dengan user driver */
            public string driverName { get; set; }
            public double volume { get; set; } /* jumlah kubik yang dibawa di shipment */
            public int status { get; set; } /* status loading, qc check, leaving plant, arriving, uloading, clompleted */
            public string ststusString { get; set; } /* disertakan untuk cross chek */
            public double lat { get; set; } /* posisi kendaraan saat ini */
            public double lon { get; set; }
            public string qcFile { get; set; } /*file string base64 hasil qc */

            /* mungkin kedepannya responnya akan disertakan juga tanggal pod nya dan jam setiap driver melakukan konfirmasi status */
        }
    }
}
