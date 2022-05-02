// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;
using MyApp.Properties;

namespace Application.Test
{
    public class Program
    {
        /// <summary>
        /// Managed entry point shim, which starts the actual program.
        /// </summary>
        public static int Main(string[] args)
        {
            Console.WriteLine(Resources.Program_Main_Start_Execution);
            var baseDir = Path.Combine(Environment.CurrentDirectory, "Scripts");
            var fileName = Path.Combine(baseDir, "Test.ps1");
            // CreateDefault2 is intentional.
            InitialSessionState iss = InitialSessionState.CreateDefault();
            iss.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            // NOTE: instantiate custom host myHost for the next line to capture stdout and stderr output
            //       in addition to just the PSObjects
            using (Runspace runspace = RunspaceFactory.CreateRunspace(/*myHost,*/iss))
            {
                runspace.Open();
                //runspace.SessionStateProxy.Path.SetLocation(baseDir);

                using (PowerShell powerShell = PowerShell.Create())
                {
                    powerShell.Runspace = runspace;
                    //powerShell.AddCommand(".\\Test.ps1");

                    powerShell.AddCommand(fileName);
                    
                    int objCount = 0;

                    var results = powerShell.Invoke();

                    foreach (var result in results)
                    {
                        Console.WriteLine(result.BaseObject.ToString());
                    }
                }

                runspace.Close();
            }

            return 0;
        }
    }
}
