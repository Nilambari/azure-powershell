//
// Copyright (c) Microsoft and contributors.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

// Warning: This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using Microsoft.Azure.Commands.Compute.Automation.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Compute.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Compute.Automation
{
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ImageOsDisk", SupportsShouldProcess = true)]
    [OutputType(typeof(PSImage))]
    public partial class SetAzureRmImageOsDiskCommand : Microsoft.Azure.Commands.ResourceManager.Common.AzureRMCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSImage Image { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        public OperatingSystemTypes? OsType { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        public OperatingSystemStateTypes? OsState { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 3,
            ValueFromPipelineByPropertyName = true)]
        public string BlobUri { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        public CachingTypes? Caching { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        public int DiskSizeGB { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        [PSArgumentCompleter("Standard_LRS", "Premium_LRS", "StandardSSD_LRS", "UltraSSD_LRS")]
        public string StorageAccountType { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        public string SnapshotId { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        public string ManagedDiskId { get; set; }

        protected override void ProcessRecord()
        {
            if (ShouldProcess("Image", "Set"))
            {
                Run();
            }
        }

        private void Run()
        {
            if (this.MyInvocation.BoundParameters.ContainsKey("OsType"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }

                this.Image.StorageProfile.OsDisk.OsType = this.OsType.Value;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("OsState"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }

                this.Image.StorageProfile.OsDisk.OsState = this.OsState.Value;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("BlobUri"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                this.Image.StorageProfile.OsDisk.BlobUri = this.BlobUri;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("Caching"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                this.Image.StorageProfile.OsDisk.Caching = this.Caching;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("DiskSizeGB"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                this.Image.StorageProfile.OsDisk.DiskSizeGB = this.DiskSizeGB;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("StorageAccountType"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                this.Image.StorageProfile.OsDisk.StorageAccountType = this.StorageAccountType;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("SnapshotId"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                // Snapshot
                if (this.Image.StorageProfile.OsDisk.Snapshot == null)
                {
                    this.Image.StorageProfile.OsDisk.Snapshot = new SubResource();
                }
                this.Image.StorageProfile.OsDisk.Snapshot.Id = this.SnapshotId;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("ManagedDiskId"))
            {
                // StorageProfile
                if (this.Image.StorageProfile == null)
                {
                    this.Image.StorageProfile = new ImageStorageProfile();
                }
                // OsDisk
                if (this.Image.StorageProfile.OsDisk == null)
                {
                    this.Image.StorageProfile.OsDisk = new ImageOSDisk();
                }
                // ManagedDisk
                if (this.Image.StorageProfile.OsDisk.ManagedDisk == null)
                {
                    this.Image.StorageProfile.OsDisk.ManagedDisk = new SubResource();
                }
                this.Image.StorageProfile.OsDisk.ManagedDisk.Id = this.ManagedDiskId;
            }

            WriteObject(this.Image);
        }
    }
}
