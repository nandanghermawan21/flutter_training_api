using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("qc_header")]
    public class QcHeader
    {
        [Key]

        [Column("qcheader_id")]
        public int qcheaderId { get; set; }

        [Column("qcheader_guid")]
        public string qcheaderGuid { get; set; }

        [Column("project_id")]
        public string projectId { get; set; }

        [Column("sap_shipment_no")]
        public string sapShipmentNo { get; set; }

        [Column("nomor_bom")]
        public string nomorBom { get; set; }

        [Column("nik_qc")]
        public string nikQc { get; set; }

        [Column("lab_code")]
        public string labCode { get; set; }

        [Column("kode_benda_uji")]
        public string kodeBendaUji { get; set; }

        [Column("kode_mutu")]
        public string kodeMutu { get; set; }

        [Column("tgl_cor")]
        public DateTime? tglCor { get; set; }

        [Column("tgl_tes")]
        public DateTime? tglTes { get; set; }

        [Column("batching_plant_id")]
        public int? batchingPlantId { get; set; }

        [Column("is_external")]
        public bool? isExternal { get; set; }

        [Column("volume")]
        public int? volume { get; set; }

        [Column("jumlah_besar")]
        public int? jumlahBesar { get; set; }

        [Column("slump_actual")]
        public string slumpActual { get; set; }

        [Column("status")]
        public int? status { get; set; }

        [Column("created_by")]
        public string createdBy { get; set; }

        [Column("created_date")]
        public DateTime? createdDate { get; set; }

        [Column("modified_by")]
        public string modifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime? modifiedDate { get; set; }
    }
}
