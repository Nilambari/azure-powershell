﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Utilities;
using System.Management.Automation;

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.Test.UnitTests.Cmdlets.IaaS.Extensions
{

    [TestClass]
    public class RemoveAzureVMChefExtensionCommandTests : TestBase
    {
        private MockCommandRuntime mockCommandRuntime;
        private MockIPersistentVMForChefExtension mockIPersistentVM;
        [TestInitialize]
        public void SetupTest()
        {
            mockCommandRuntime = new MockCommandRuntime();
            mockIPersistentVM = new MockIPersistentVMForChefExtension();
        }

        [TestCleanup]
        public void CleanupTest()
        {
        }
        public class RemoveAzureVMChefExtensionCommandStub : RemoveAzureVMChefExtensionCommand
        {
            public RemoveAzureVMChefExtensionCommandStub() { }
        }

        [TestMethod]
        public void RemoveAzureVMChefExtensionExecuteChefCommand()
        {

            var getChefExtension = new RemoveAzureVMChefExtensionCommandStub()
            {
                CommandRuntime = mockCommandRuntime,
                VM = mockIPersistentVM,
            };

            getChefExtension.ExecuteCommand();
            Assert.AreEqual(1, mockCommandRuntime.OutputPipeline.Count, "One item should be in the output pipeline");
        }
    }
}
