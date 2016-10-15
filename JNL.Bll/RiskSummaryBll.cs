using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNL.Bll
{
    public partial class RiskSummaryBll
    {
        /// <summary>
        /// 判断以指定id为parentId的风险概述信息是否为最底层（没有子级）
        /// 判断依据是从这一层开始往上找parentId，直到最顶层
        /// 若最后结果是4，则返回true，否则返回false
        /// </summary>
        public bool IsChildrenBottom(int id)
        {
            var parentId = id;
            var counter = 0;

            while (parentId > 0)
            {
                var model = DalInstance.QuerySingle(parentId);
                parentId = model?.ParentId ?? 0;

                counter++;
            }

            return counter == 4;
        }
    }
}
