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
    [NUnit.Framework.DescriptionAttribute("Bookmark Lists Deletion")]
    public partial class BookmarkListsDeletionFeature
    {
        
        private Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
#line 1 "PG-175.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, NUnit.Framework.TestContext.CurrentContext.WorkerId);
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Bookmark Lists Deletion", null, ProgrammingLanguage.CSharp, featureTags);
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
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 3
#line hidden
            Reqnroll.Table table3 = new Reqnroll.Table(new string[] {
                        "UserName",
                        "Email",
                        "FirstName",
                        "LastName",
                        "Password"});
            table3.AddRow(new string[] {
                        "Joshua Weiss",
                        "knott@example.com",
                        "Joshua",
                        "Weiss",
                        "FAKE PW"});
#line 4
 await testRunner.GivenAsync("the following users exist", ((string)(null)), table3, "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I see a delete button on bookmark lists")]
        public async System.Threading.Tasks.Task ISeeADeleteButtonOnBookmarkLists()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I see a delete button on bookmark lists", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 8
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 9
 await testRunner.GivenAsync("I am a user with first name \'Joshua\'", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 10
  await testRunner.AndAsync("I login", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 11
  await testRunner.AndAsync("I am on the \"Favorites\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 12
 await testRunner.WhenAsync("I fill out and submit the new bookmark list form with a unique title", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 14
 await testRunner.ThenAsync("I should see the new bookmark list displayed", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
#line 15
    await testRunner.AndAsync("I should see a delete button for the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I see a confirmation dialog when I click the delete button")]
        public async System.Threading.Tasks.Task ISeeAConfirmationDialogWhenIClickTheDeleteButton()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I see a confirmation dialog when I click the delete button", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 17
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 18
  await testRunner.GivenAsync("I am a user with first name \'Joshua\'", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 19
   await testRunner.AndAsync("I login", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 20
   await testRunner.AndAsync("I am on the \"Favorites\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 21
   await testRunner.AndAsync("I have created a new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 22
  await testRunner.WhenAsync("I click the delete button for the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 23
  await testRunner.ThenAsync("I should see a delete bookmark list confirmation dialog", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can cancel the deletion of a bookmark list")]
        public async System.Threading.Tasks.Task ICanCancelTheDeletionOfABookmarkList()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I can cancel the deletion of a bookmark list", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 25
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 26
  await testRunner.GivenAsync("I am a user with first name \'Joshua\'", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 27
   await testRunner.AndAsync("I login", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 28
   await testRunner.AndAsync("I am on the \"Favorites\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 29
   await testRunner.AndAsync("I have created a new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 30
   await testRunner.AndAsync("I have clicked the delete button for the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 31
  await testRunner.WhenAsync("I click the cancel button in the delete bookmark list confirmation dialog", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 32
  await testRunner.ThenAsync("I should not see the bookmark list confirmation dialog", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
#line 33
   await testRunner.AndAsync("I should see the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I can delete a bookmark list")]
        public async System.Threading.Tasks.Task ICanDeleteABookmarkList()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("I can delete a bookmark list", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 35
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
await this.FeatureBackgroundAsync();
#line hidden
#line 36
  await testRunner.GivenAsync("I am a user with first name \'Joshua\'", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
#line 37
   await testRunner.AndAsync("I login", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 38
   await testRunner.AndAsync("I am on the \"Favorites\" page", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 39
   await testRunner.AndAsync("I have created a new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 40
   await testRunner.AndAsync("I have clicked the delete button for the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
#line 41
  await testRunner.WhenAsync("I click the confirm button in the delete bookmark list confirmation dialog", ((string)(null)), ((Reqnroll.Table)(null)), "When ");
#line hidden
#line 42
  await testRunner.ThenAsync("I should not see the new bookmark list", ((string)(null)), ((Reqnroll.Table)(null)), "Then ");
#line hidden
#line 43
   await testRunner.AndAsync("I should not see the delete bookmark list confirmation dialog", ((string)(null)), ((Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
