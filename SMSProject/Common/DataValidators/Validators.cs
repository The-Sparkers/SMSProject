using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.Common.DataValidators
{
    public class CurrentMonthRange : RangeAttribute
    {
        public CurrentMonthRange() : base(typeof(DateTime), DateTime.MinValue.ToShortDateString(), new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).Day).ToShortDateString())
        {

        }
    }
    public class CurrentDateRange : RangeAttribute
    {
        public CurrentDateRange() : base(typeof(DateTime), DateTime.MinValue.ToShortDateString(), DateTime.Now.ToLongDateString())
        {

        }
    }
}