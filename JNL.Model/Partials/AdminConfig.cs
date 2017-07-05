using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNL.Model
{
    [Serializable]
    public partial class AdminConfig
    {
        public AdminConfig()
        {
            
        }


        public int Id { get; set; }
        public int ConfigType { get; set; }
        public int TargetId { get; set; }

    }
}
