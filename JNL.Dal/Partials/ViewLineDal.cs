using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JNL.Model;

namespace JNL.Dal
{
    public partial class ViewLineDal : BaseDal<ViewLine>
    {
        protected override string TableName => "ViewLine";
        protected override string PrimaryKeyName => "Id";
    }
}
