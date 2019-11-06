using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InovaTrackApi_SBB.Models
{
    [Table("qc_detail")]
    public class QcDetail
    {
        [Key]

        [Column("qcdetail_id")]
        public int qcdetailId { get; set; }

        [Column("qcdetail_guid")]
        public string qcdetailGuid { get; set; }

        [Column("qcheader_guid")]
        public string qcheaderGuid { get; set; }

        [Column("qcheader_id")]
        public int qcheaderId { get; set; }

        [Column("tgl_tes")]
        public DateTime? tglTes { get; set; }

        [Column("suhu")]
        public decimal? suhu { get; set; }

        [Column("total_sample")]
        public byte? totalSample { get; set; }

        [Column("tambah_obat")]
        public decimal? tambahObat { get; set; }

        [Column("foto_hasil1")]
        public string fotoHasil1 { get; set; }

        [Column("foto_hasil2")]
        public string fotoHasil2 { get; set; }

        [Column("foto_hasil3")]
        public string fotoHasil3 { get; set; }

        [Column("umur")]
        public byte? umur { get; set; }

        [Column("luas")]
        public decimal? luas { get; set; }

        [Column("berat")]
        public decimal? berat { get; set; }

        [Column("gaya_tekan")]
        public decimal? gayaTekan { get; set; }

        [Column("tegangan_tekanan_nmm")]
        public decimal? teganganTekananNmm { get; set; }

        [Column("tegangan_tekanan_kgcm")]
        public decimal? teganganTekananKgcm { get; set; }

        [Column("keterangan")]
        public string keterangan { get; set; }

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
