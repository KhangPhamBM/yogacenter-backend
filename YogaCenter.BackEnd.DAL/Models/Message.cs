using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int ClassDetailId { get; set; }
        [ForeignKey(nameof(ClassDetailId))]
        public ClassDetail ClassDetail { get; set; }
        public string MessageContent { get; set; }

        public DateTime SendTime { get; set; }
        public bool isDeleted { get; set; } = false;

    }
}
