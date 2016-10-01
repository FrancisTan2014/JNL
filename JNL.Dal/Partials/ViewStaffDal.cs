using JNL.Model;

namespace JNL.Dal
{
    public partial class ViewStaffDal : BaseDal<ViewStaff>
    {
        protected override string TableName => "ViewStaff";
        protected override string PrimaryKeyName => "Id";
    }
}
