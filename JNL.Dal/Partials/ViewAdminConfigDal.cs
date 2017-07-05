using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JNL.Model;

namespace JNL.Dal
{
    public partial class ViewAdminConfigDal: BaseDal<ViewAdminConfig>
    {
        protected override string TableName => nameof(ViewAdminConfig);
        protected override string PrimaryKeyName => nameof(ViewAdminConfig.Id);
    }
}
