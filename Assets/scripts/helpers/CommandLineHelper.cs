using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CommandLineHelper : MonoBehaviour
{
    static public void PrintPdf(string pdf)
    {
        ExecuteProcessTerminal("lpr -P Star_TSP847II__STR_T_001_ \"" + "/Users/felixhu/legov5/" + pdf + "\"");
    }

    static public string ExecuteProcessTerminal(string argument)
    {
        try
        {
            UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ===============");
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = " -c \"" + argument + " \""
            };
            Process myProcess = new Process
            {
                StartInfo = startInfo
            };
            myProcess.Start();
            string output = myProcess.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);
            myProcess.WaitForExit();
            UnityEngine.Debug.Log("============== End ===============");

            return output;
        }
        catch (System.Exception e)
        {
            print(e);
            return null;
        }
    }
}
