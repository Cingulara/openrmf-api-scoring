using System;
using System.Collections.Generic;

namespace openrmf_scoring_api.Models
{

    public class STIGS {

        public STIGS (){
            iSTIG = new iSTIG();
        }

        public iSTIG iSTIG { get; set; }
    }
}