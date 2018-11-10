using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webapp.DB.Models
{
    [Table("TestTableRecords")]
    public class TestTableRecord
    {
        [Key]
        public int TestTableRecordId { get; set; }
        public string Information { get; set; }
    }
}
