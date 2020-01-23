using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceWeb.Util
{
    public class DateDifference
    {
        private DateTime _FromDate;

        private DateTime _ToDate;

        private int _Year = 0;

        private int _Month = 0;

        private int _Day = 0;

        public int Year
        {
            get { return _Year; }

            set { _Year = value; }
        }
        public int Month
        {
            get { return _Month; }

            set { _Month = value; }
        }
        public int Day
        {
            get { return _Day; }

            set { _Day = value; }
        }

        public DateTime FromDate
        {
            get { return _FromDate; }

            set { _FromDate = value; }
        }

        public DateTime ToDate
        {
            get { return _ToDate; }

            set { _ToDate = value; }
        }

        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public DateDifference(DateTime argFromDate)
        {
            _ToDate = DateTime.Now;

            _FromDate = argFromDate;

            CalcDifferenceDate();
        }
        public DateDifference(DateTime argFromDate, DateTime argToDate)
        {
            _ToDate = argToDate;

            _FromDate = argFromDate;

            CalcDifferenceDate();
        }
        private void SwapDate(ref DateTime LeftDate, ref DateTime RightDate)
        {
            DateTime tempDate;

            if (LeftDate > RightDate)
            {
                tempDate = LeftDate;

                LeftDate = RightDate;

                RightDate = tempDate;
            }
            else
            {
                LeftDate = LeftDate;

                RightDate = RightDate;
            }

        }
        private void CalcDifferenceDate()
        {
            SwapDate(ref _FromDate, ref _ToDate);

            int carryFlag = 0;

            // ***************************

            // Day calculation

            if (this.FromDate.Day > this.ToDate.Day)

                carryFlag = this.monthDay[this.FromDate.Month - 1];

            // febuary detect

            if (carryFlag == -1)
            {

                if (CheckedLeapYear(this.FromDate))
                {
                    // leap year february contain 29 days
                    carryFlag = 29;
                }
                else
                {
                    carryFlag = 28;
                }

            }
            if (carryFlag != 0)
            {

                this.Day = (this.ToDate.Day + carryFlag) - this.FromDate.Day;

                carryFlag = 1;

            }
            else
            {

                this.Day = this.ToDate.Day - this.FromDate.Day;
            }
            // ***************************

            // Month calculation

            if ((this.FromDate.Month + carryFlag) > this.ToDate.Month)
            {
                this.Month = (this.ToDate.Month + 12) - (this.FromDate.Month + carryFlag);

                carryFlag = 1;
            }
            else
            {
                this.Month = this.ToDate.Month - (this.FromDate.Month + carryFlag);

                carryFlag = 0;
            }
            this.Year = this.ToDate.Year - (this.FromDate.Year + carryFlag);
        }

        private bool CheckedLeapYear(DateTime checkedDate)
        {
            int myYear = checkedDate.Year;

            return (((myYear % 4) == 0) && ((myYear % 100) != 0) || ((myYear % 400) == 0));
        }

        public string ToString(string argYearUnit, string argMonthUnit, string argDayUnit)
        {
            string retStr = string.Empty;

            if (this.Year > 0)

                retStr = retStr + string.Format("{0} {1} ", this.Year.ToString("#,##0"), argYearUnit);

            if (this.Month > 0)

                retStr = retStr + string.Format("{0} {1} ", this.Month.ToString("#,##0"), argMonthUnit);

            if (this.Day > 0)

                retStr = retStr + string.Format("{0} {1} ", this.Day.ToString("#,##0"), argDayUnit);

            return retStr.Trim();
        }

        public override string ToString()
        {
            return this.ToString("Year(s)", "Month(s)", "Day(s)");
        }

    }
}