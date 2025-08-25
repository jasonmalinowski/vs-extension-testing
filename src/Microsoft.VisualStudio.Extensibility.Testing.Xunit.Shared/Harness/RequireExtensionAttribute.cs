// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Harness
{
    using System;

    /// <summary>
    /// Specifies an .vsix extension that will be installed before running this assembly's tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class RequireExtensionAttribute : Attribute
    {
        public RequireExtensionAttribute(string extensionFile)
        {
            ExtensionFile = extensionFile;
        }

        public string ExtensionFile
        {
            get;
        }
    }
}
