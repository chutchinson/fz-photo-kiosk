using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FeyenZylstra.PhotoKiosk.Models
{
    [XmlRoot("rsp")]
    public class Response
    {
        [XmlAttribute("status")]
        public string Status { get; set; }
    }
}
