using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using PopNGo_BDD_Tests.PageObjects;
using PopNGo_BDD_Tests.Drivers;
using OpenQA.Selenium.Chrome; // Add this line
using OpenQA.Selenium.Edge; // Add this line
using OpenQA.Selenium.Firefox;
using SpecFlow.Actions.Selenium; // Add this line


namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class ScheduledNotificationsStepDefinitions
    {
        private readonly AdminPageObject _adminPage;
        private readonly NotificationsPageObject _notificationsPage;
        private readonly Drivers.BrowserDriver _webDriver;
        private readonly ScenarioContext _scenarioContext;
        private IConfigurationRoot Configuration { get; set; }

        public ScheduledNotificationsStepDefinitions(ScenarioContext context, Drivers.BrowserDriver browserDriver)
        {
            _scenarioContext = context;
            _webDriver = browserDriver;
            _adminPage = new AdminPageObject(_webDriver.Current);
            _notificationsPage = new NotificationsPageObject(_webDriver.Current);

            // Get the configuration
            // we need to keep the admin password secret
            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets<ScheduledNotificationsStepDefinitions>();
            Configuration = builder.Build();
        }

        [When("I am in the Admin area")]
        public void WhenIAmInTheAdminArea()
        {
            _adminPage.GoTo();
        }

        [Then("I should see a way to navigate to Notifications")]
        public void ThenTheUserShouldSeeTheNavigationToNotifications()
        {
            _adminPage.scheduledNotificationsNavItem.Displayed.Should().BeTrue();
        }

        [Then("I should see all of the scheduled notifications")]
        public void ThenTheUserShouldSeeTheScheduledNotifications()
        {
            // Check that the scheduled notifications are displayed
            _notificationsPage.SchedulesTable.Displayed.Should().BeTrue();
        }

        [Then("I should be able to edit or delete a scheduled notification")]
        public void ThenTheUserShouldBeAbleToEditOrDeleteAScheduledNotification()
        {
            // Check that the edit and delete buttons are displayed
            _notificationsPage.ScheduleRowButtonEdit.Displayed.Should().BeTrue();
            _notificationsPage.ScheduleRowButtonDelete.Displayed.Should().BeTrue();
        }

        [When("I have edited a scheduled notification")]
        public void WhenIHaveEditedANotification()
        {
            _webDriver.ScrollToElement(_notificationsPage.ScheduleRowButtonEdit);
            _notificationsPage.ScheduleRowButtonEdit.Click();
            _scenarioContext["EditedTime"] = _notificationsPage.EditTime();
            _notificationsPage.ScheduleModalButtonSave.Click();
        }

        [Then("I should see the new time on the page")]
        public void ThenIShouldSeeTheNewTimeOnThePage()
        {
            DateTime newTime = (DateTime)_scenarioContext["EditedTime"];
            DateTime time = _notificationsPage.GetTime();
            time.Should().Be(newTime);
            Console.WriteLine("Time = " + time);
        }

        [When("I have deleted a scheduled notification")]
        public void WhenIHaveDeletedANotification()
        {
            _notificationsPage.SetScheduleId();
            _scenarioContext["DeletedId"] = _notificationsPage.GetScheduleId();
            _scenarioContext["DeletedTime"] = _notificationsPage.GetTime();
            _webDriver.ScrollToElement(_notificationsPage.ScheduleRowButtonDelete);
            _notificationsPage.ScheduleRowButtonDelete.Click();
        }

        [Then("I should not see it in the list")]
        public void ThenIShouldNotSeeItInTheList()
        {
            int id = (int)_scenarioContext["DeletedId"];
            _notificationsPage.SchedulesTable.FindElements(By.TagName("tr[data-scheduleId=\"" + id + "\"]")).Count.Should().Be(0);
        }

        [Then("I should see a new scheduled notification for the next time period after the previously-scheduled one")]
        public void ThenIShouldSeeANewScheduledNotificationForTheNextTimePeriodAfterThePreviouslyScheduledOne()
        {
            DateTime time = (DateTime)_scenarioContext["DeletedTime"];
            DateTime newTime = _notificationsPage.GetTime();
            newTime.Should().BeAfter(time);

            int id = (int)_scenarioContext["DeletedId"];
            _notificationsPage.SetScheduleId();
            int newId = _notificationsPage.GetScheduleId();
            newId.Should().NotBe(id);
        }
    }
}
