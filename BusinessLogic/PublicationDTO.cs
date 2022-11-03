using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class PublicationDTO
    {
        public long productContentId { get; set; }
        public string productContentName { get; set; }
        public string LastModifiedDate { get; set; }
        public string ChangeFreqency { get; set; }
        public string Priority { get; set; }
    }
}
