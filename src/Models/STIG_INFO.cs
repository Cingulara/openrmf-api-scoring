// Copyright (c) Cingulara 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Xml.Serialization;

namespace openrmf_scoring_api.Models
{

    public class STIG_INFO {

        public STIG_INFO (){
            SI_DATA = new List<SI_DATA>();
        }

        [XmlElement("SI_DATA")]
        public List<SI_DATA> SI_DATA { get; set;}
    }
}