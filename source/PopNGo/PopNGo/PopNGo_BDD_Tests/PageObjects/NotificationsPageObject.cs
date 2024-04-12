using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;
using FluentAssertions.Extensions;
using OpenQA.Selenium.Support.Extensions;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class NotificationsPageObject : PageObject
    {
        string _scheduleId = "";
        public NotificationsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Notifications";
        }
        
        public IWebElement SchedulesTable => _webDriver.FindElement(By.Name("admin-schedules-table"));
        public IWebElement ScheduleRow => SchedulesTable.FindElement(By.TagName("tr[data-userId=\"6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5\"]"));
        public IWebElement ScheduleRowButtons { get {
            SetScheduleId();
            return ScheduleRow.FindElement(By.XPath("following-sibling::*"));
        } }
        public IWebElement ScheduleRowButtonEdit => ScheduleRowButtons.FindElement(By.Id("edit-notification"));
        public IWebElement ScheduleRowButtonDelete => ScheduleRowButtons.FindElement(By.Id("delete-notification"));
        public IWebElement ScheduleModal => _webDriver.FindElement(By.Id("admin-schedule-details-modal"));
        public IWebElement ScheduleModalInputTime => ScheduleModal.FindElement(By.Id("schedule-details-time"));
        public IWebElement ScheduleModalButtonSave => ScheduleModal.FindElement(By.Id("schedule-details-save"));

        public void SetScheduleId()
        {
            _scheduleId = ScheduleRow.GetAttribute("data-scheduleId");
        }
        public int GetScheduleId()
        {
            return int.Parse(_scheduleId);
        }

        public DateTime EditTime()
        {
            DateTime now = DateTime.Now;
            now = now
                .AddSeconds(-now.Second)
                .AddMilliseconds(-now.Millisecond)
                .AddNanoseconds(-now.Nanosecond)
                .AddMicroseconds(-now.Microsecond);

            DateTime newTime = now.AddMinutes(5);

            ScheduleModalInputTime.Clear();
            ScheduleModalInputTime.SendKeys(newTime.ToString("M/d/yyyy, hh:mm tt"));

            return newTime;
        }

        public DateTime GetTime()
        {
            string time = ScheduleRow.FindElement(By.CssSelector("td:nth-child(4)")).Text;
            return DateTime.ParseExact(time, "M/d/yyyy, hh:mm tt", null);
        }
    }
}