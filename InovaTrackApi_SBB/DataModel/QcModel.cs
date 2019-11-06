using System;
using System.Collections.Generic;
using System.Linq;
using InovaTrackApi_SBB.Helper;
using InovaTrackApi_SBB.Models;

namespace InovaTrackApi_SBB.DataModel
{
    public class QcModel
    {
        #region contructor
        private ApplicationDbContext _db;
        private AppSettings _config;
        private ProductModel _product;

        public QcModel(ApplicationDbContext db, AppSettings config)
        {
            _db = db;
            _config = config;
            _product = new ProductModel(db, config);
        }

        #endregion

        public IQueryable<QcResponseModel> get(string qcNik = null)
        {
            IQueryable<QcHeader> qQcHeader = _db.qcHeaders;

            if (string.IsNullOrEmpty(qcNik)) qQcHeader = qQcHeader.Where(x => x.nikQc == qcNik);

            var data = (from qcHeader in qQcHeader
                        join qcLab in _db.QcLabs on qcHeader.labCode equals qcLab.lab_code
                        join qcMaster in _db.QcMasters on qcHeader.nikQc equals qcMaster.nik
                        join project in _db.Projects on qcHeader.projectId equals project.id
                        join customer in _db.Customers on project.customer_id equals (int)customer.CustomerId
                        join product in _db.SAPProductMaterials on project.product_material_id equals product.MaterialId
                        join batchingPlant in _db.BatchingPlants on project.batching_plant_id equals batchingPlant.BatchingPlantId
                        let qcDetails = _db.qcDetails.Where(x => x.qcheaderId == qcHeader.qcheaderId).ToList()
                        select new QcResponseModel
                        {
                            projectName = project.project_name,
                            batchingPlantName = batchingPlant.BatchingPlantName,
                            customerName = customer.CustomerName,
                            tanggalCor = qcHeader.tglCor,
                            labName = qcLab.lab_name,
                            gradeName = product.GradeCode,
                            kodeBendaUji = qcHeader.kodeBendaUji,
                            qcDetails = qcDetails,
                            qcHeader = qcHeader
                        }
                );

            if(data != null)
            {
                data.ToList().ForEach(x => x.projectName = "Coba aja deh");
            }

            return data;
        }


        public QcDetail updateDetail(QcDetail qcDetail)
        {
            try
            {
                var data = _db.qcDetails.Where(x => x.qcdetailId == qcDetail.qcdetailId).FirstOrDefault();

                if (data != null)
                {
                    data.qcdetailId = qcDetail.qcdetailId;
                    data.qcdetailGuid = qcDetail.qcdetailGuid;
                    data.qcheaderGuid = qcDetail.qcheaderGuid;
                    data.qcheaderId = qcDetail.qcheaderId;
                    data.tglTes = qcDetail.tglTes;
                    data.suhu = qcDetail.suhu;
                    data.totalSample = qcDetail.totalSample;
                    data.tambahObat = qcDetail.tambahObat;
                    //data.fotoHasil1 = qcDetail.fotoHasil1;
                    //data.fotoHasil2 = qcDetail.fotoHasil2;
                    //data.fotoHasil3 = qcDetail.fotoHasil3;
                    data.fotoHasil1 = UploadHelper.saveImage(qcDetail.fotoHasil1, _config, "qcFoto1");
                    data.fotoHasil2 = UploadHelper.saveImage(qcDetail.fotoHasil1, _config, "qcFoto2");
                    data.fotoHasil3 = UploadHelper.saveImage(qcDetail.fotoHasil1, _config, "qcFoto3");
                    data.umur = qcDetail.umur;
                    data.luas = qcDetail.luas;
                    data.berat = qcDetail.berat;
                    data.gayaTekan = qcDetail.gayaTekan;
                    data.teganganTekananKgcm = qcDetail.teganganTekananKgcm;
                    data.teganganTekananNmm = qcDetail.teganganTekananNmm;
                    data.keterangan = qcDetail.keterangan;
                    data.createdBy = qcDetail.createdBy;
                    data.createdDate = qcDetail.createdDate;
                    data.modifiedBy = qcDetail.modifiedBy;
                    data.modifiedDate = qcDetail.modifiedDate;
                }

                _db.SaveChanges();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QcHeader UpdateHeader(QcHeader qcHeader)
        {
            try
            {
                var data = _db.qcHeaders.Where(x => x.qcheaderId == qcHeader.qcheaderId).FirstOrDefault();

                if (data != null)
                {
                    data.qcheaderId = qcHeader.qcheaderId;
                    data.qcheaderGuid = qcHeader.qcheaderGuid;
                    data.projectId = qcHeader.projectId;
                    data.sapShipmentNo = qcHeader.sapShipmentNo;
                    data.nomorBom = qcHeader.nomorBom;
                    data.nikQc = qcHeader.nikQc;
                    data.labCode = qcHeader.labCode;
                    data.kodeBendaUji = qcHeader.kodeBendaUji;
                    data.kodeMutu = qcHeader.kodeMutu;
                    data.tglCor = qcHeader.tglCor;
                    data.tglTes = qcHeader.tglTes;
                    data.batchingPlantId = qcHeader.batchingPlantId;
                    data.isExternal = qcHeader.isExternal;
                    data.volume = qcHeader.volume;
                    data.jumlahBesar = qcHeader.jumlahBesar;
                    data.slumpActual = qcHeader.slumpActual;
                    data.status = qcHeader.status;
                    data.createdBy = qcHeader.createdBy;
                    data.createdDate = qcHeader.createdDate;
                    data.modifiedBy = qcHeader.modifiedBy;
                    data.modifiedDate = qcHeader.modifiedDate;
                }

                _db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region classModel

        public class QcResponseModel
        {
            public string projectName { get; set; }
            public string customerName { get; set; }
            public DateTime? tanggalCor { get; set; }
            public string batchingPlantName { get; set; }
            public string labName { get; set; }
            public string gradeName { get; set; }
            public string kodeBendaUji { get; set; }
            public QcHeader qcHeader { get; set; }
            public List<QcDetail> qcDetails { get; set; }
        }

        #endregion
    }
}
