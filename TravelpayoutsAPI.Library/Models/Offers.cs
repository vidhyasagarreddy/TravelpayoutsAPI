﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace TravelpayoutsAPI.Library.Models
{
    [XmlRoot("offers")]
    public class Offers
    {
        [XmlElement("offer")]
        public List<Offer> OfferItems { get; set; }
    }
}
