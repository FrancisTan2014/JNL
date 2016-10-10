using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JNL.Bll;
using JNL.Model;
using JNL.Utilities.FileUpload;
using JNL.Web.Models;
using JNL.Web.Utils;
using NPOI.HSSF.UserModel;
using JNL.Utilities.Extensions;
using NPOI.SS.UserModel;
using WebGrease.Css.Extensions;

namespace JNL.Web.Controllers
{
    public class ExamController : Controller
    {
        public ActionResult Score()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload(int fileType)
        {
            var file = Request.Files[0];
            if (file == null)
            {
                return Json(ErrorModel.InputError);
            }

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || (ext.ToLower() != ".xls" && ext.ToLower() != ".xlsx"))
            {
                return Json(ErrorModel.UnknownUploadFileType);
            }

            var pathInfo = UploadHelper.FileUpload() as FilePathInfo;
            if (pathInfo == null)
            {
                return Json(ErrorModel.FileUploadFailed);
            }

            List<string> msgList;
            var list = GetModelFromExcel(pathInfo.FileAbsolutePath, out msgList);
            if (!list.Any())
            {
                return Json(ErrorModel.FileUploadFailed);
            }

            var examBll = new ExamScoreBll();
            examBll.BulkInsert(list);

            return Json(msgList);
        }

        private List<ExamScore> GetModelFromExcel(string excelPath, out List<string> msgList)
        {
            msgList = new List<string>();
            var examList = new List<ExamScore>();
            try
            {
                using (var fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
                {
                    var workBook = new HSSFWorkbook(fileStream);

                    var staffList = new StaffBll().QueryAll();
                    var workNoStaffIdDic = new Dictionary<string, int>();
                    staffList.ForEach(staff =>
                    {
                        if (!workNoStaffIdDic.ContainsKey(staff.SalaryId))
                        {
                            workNoStaffIdDic.Add(staff.SalaryId, staff.Id);
                        }
                    });

                    foreach (ISheet sheet in workBook)
                    {
                        var rows = sheet.GetRowEnumerator();
                        while (rows.MoveNext())
                        {
                            var row = rows.Current as HSSFRow;

                            string msg;
                            if (IsLegalRow(row, out msg))
                            {
                                var model = GetModelFromHssfRow(row, workNoStaffIdDic);
                                examList.Add(model);
                            }
                            else
                            {
                                msgList.Add(msg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogBll.ExceptionPersistence("ExamController.cs", "ExamController", ex);
            }

            return examList;
        }

        private ExamScore GetModelFromHssfRow(HSSFRow row, Dictionary<string, int> workNoStaffIdDic)
        {
            var model = new ExamScore
            {
                StaffName = row.Cells[1].ToString(),
                WorkNo = row.Cells[2].ToString().Trim(),
                Position = row.Cells[3].ToString(),
                WorkPlace = row.Cells[4].ToString(),
                ExamTheme = row.Cells[5].ToString(),
                ExamSubject = row.Cells[6].ToString(),
                Score = row.Cells[7].ToString().ToInt32()
            };

            var examTime = row.Cells[8].DateCellValue;
            if (examTime == DateTime.MinValue)
            {
                DateTime tempTime;
                examTime = DateTime.TryParse(row.Cells[8].ToString(), out tempTime) ? tempTime : DateTime.Now;
            }
            model.ExamTime = examTime;

            if (workNoStaffIdDic.ContainsKey(model.WorkNo))
            {
                model.StaffId = workNoStaffIdDic[model.WorkNo];
            }

            return model;
        }

        /// <summary>
        /// 对excel的单行进行格式验证
        /// </summary>
        private static bool IsLegalRow(HSSFRow row, out string msg)
        {
            msg = string.Empty;

            if (row.Cells.Count < 8)
            {
                return false;
            }

            // 第一个单元格：序号号必须是数字
            var number = row.Cells[0].ToString().ToInt32();
            if (number == 0)
            {
                return false;
            }

            // 第二个单元格：姓名，不能为空
            var name = row.Cells[1].ToString();
            if (string.IsNullOrEmpty(name))
            {
                msg = $"序号为【{number}】这一行被排除，原因：姓名为空。";
                return false;
            }

            // 第三个单元格：工号必须28开头
            var workNo = row.Cells[2].ToString().ToInt32();
            if (workNo < 2800000)
            {
                msg = $"序号为【{number}】这一行被排除，原因：工号没有以28开头。";
                return false;
            }

            // 第四五六七单元格：不能为空
            for (int i = 3; i < 7; i++)
            {
                var temp = row.Cells[i].ToString();
                if (string.IsNullOrWhiteSpace(temp))
                {
                    msg = $"序号为【{number}】这一行被排除，原因：第{i + 1}个单元格为空";
                    return false;
                }
            }

            // 第八个单元格：答案，必须能转换为数字
            var score = row.Cells[7].ToString().ToInt32();
            if (score == 0)
            {
                msg = $"序号为【{number}】这一行被排除，原因：成绩为非法值";
                return false;
            }

            var time = row.Cells[8].ToString();
            if (string.IsNullOrWhiteSpace(time))
            {
                msg = $"序号为【{number}】这一行被排除，原因：考试时间为空";
                return false;
            }

            return true;
        }
    }
}
