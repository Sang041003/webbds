using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webbds.Models
{
    public class NhanXetViewModel
    {
            public List<NhanXet> Comments { get; set; } // List of existing comments
            public NhanXet NewComment { get; set; } // New comment being added      
    }
}