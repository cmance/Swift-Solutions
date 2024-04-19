﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:1.0.0.0
//      Reqnroll Generator Version:1.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace PopNGo_BDD_Tests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "1.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Scheduled Notifications for Admin Control")]
    [NUnit.Framework.CategoryAttribute("Tristan")]
    public partial class ScheduledNotificationsForAdminControlFeature
    {
        
        private Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = new string[] {
                "Tristan"};
        
#line 1 "ScheduledNotifications.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, NUnit.Framework.TestContext.CurrentContext.WorkerId);
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Scheduled Notifications for Admin Control", @"As an admin, I'd like to be able to see and delete scheduled notifications

Sometimes the site may go down or be unresponsive at midnight when the notification emails are supposed to be prepared. Currently, this would delay the next batch until the following midnight. An adminstrator should be able to instead alter the time of the scheduled job, or even delete it if they so wish. This involves converting timed notifications for emails and texts into scheduled jobs to be processed at the appointed times.", ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find the Scheduled Notifications area")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task FindTheScheduledNotificationsArea()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("Find the Scheduled Notifications area", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 9
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 10
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 11
await testRunner.WhenAsync("I am in the Admin area", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 12
await testRunner.ThenAsync("I should see a way to navigate to Notifications", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("See the Scheduled Notifications area")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task SeeTheScheduledNotificationsArea()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("See the Scheduled Notifications area", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 15
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 16
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 17
await testRunner.WhenAsync("I am on the \"Notifications\" page", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 18
await testRunner.ThenAsync("I should see all of the scheduled notifications", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("See buttons for Editing and Deleting Scheduled Notifications")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task SeeButtonsForEditingAndDeletingScheduledNotifications()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("See buttons for Editing and Deleting Scheduled Notifications", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 21
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 22
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 23
await testRunner.AndAsync("I am on the \"Notifications\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 24
await testRunner.ThenAsync("I should be able to edit or delete a scheduled notification", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I have edited a Scheduled Notification")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task IHaveEditedAScheduledNotification()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I have edited a Scheduled Notification", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 27
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 28
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 29
 await testRunner.AndAsync("I am on the \"Notifications\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 30
await testRunner.WhenAsync("I have edited a scheduled notification", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 31
await testRunner.ThenAsync("I should see the new time on the page", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I have deleted a Scheduled Notification")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task IHaveDeletedAScheduledNotification()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I have deleted a Scheduled Notification", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 34
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 35
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 36
 await testRunner.AndAsync("I am on the \"Notifications\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 37
await testRunner.WhenAsync("I have deleted a scheduled notification", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 38
await testRunner.ThenAsync("I should not see it in the list", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I have deleted a Scheduled Notification and see it rescheduled")]
        [NUnit.Framework.CategoryAttribute("scheduledNotifications")]
        public async System.Threading.Tasks.Task IHaveDeletedAScheduledNotificationAndSeeItRescheduled()
        {
            string[] tagsOfScenario = new string[] {
                    "scheduledNotifications"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I have deleted a Scheduled Notification and see it rescheduled", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 41
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 42
await testRunner.GivenAsync("I am an admin", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 43
 await testRunner.AndAsync("I am on the \"Notifications\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 44
await testRunner.WhenAsync("I have deleted a scheduled notification", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 45
await testRunner.ThenAsync("I should see a new scheduled notification for the next time period after the prev" +
                        "iously-scheduled one", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
