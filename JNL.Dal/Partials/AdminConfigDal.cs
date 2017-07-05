using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JNL.Model;

namespace JNL.Dal
{
    public partial class AdminConfigDal: BaseDal<AdminConfig>
    {
        protected override string TableName => nameof(AdminConfig);
        protected override string PrimaryKeyName => nameof(AdminConfig.Id);
    }
}
