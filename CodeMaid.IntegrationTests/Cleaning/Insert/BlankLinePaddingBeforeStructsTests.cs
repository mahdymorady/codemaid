﻿#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Linq;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBeforeStructs.cs", "Data")]
    [DeploymentItem(@"Cleaning\Insert\Data\BlankLinePaddingBeforeStructs_Cleaned.cs", "Data")]
    public class BlankLinePaddingBeforeStructsTests
    {
        #region Setup

        private static InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_insertBlankLinePaddingLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\BlankLinePaddingBeforeStructs.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBeforeStructs_CleansAsExpected()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunInsertBlankLinePaddingBeforeStructs, _projectItem, @"Data\BlankLinePaddingBeforeStructs_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBeforeStructs_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunInsertBlankLinePaddingBeforeStructs, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningInsertBlankLinePaddingBeforeStructs_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunInsertBlankLinePaddingBeforeStructs, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunInsertBlankLinePaddingBeforeStructs(Document document)
        {
            var codeItems = TestOperations.CodeModelManager.RetrieveAllCodeItems(document);
            var structs = codeItems.OfType<CodeItemStruct>().ToList();

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(structs);
        }

        #endregion Helpers
    }
}