using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks
{
    public class IncrementBuildNumber : Task
    {
        private string _assemblyFileLocation;

        [Required]
        public string AssemblyFileLocation
        {
            get { return _assemblyFileLocation; }
            set { _assemblyFileLocation = value; }
        }

        public override bool Execute()
        {
            try
            {
                return IncrementVersion();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                return false;
            }
        }

        private bool IncrementVersion()
        {
            if (System.IO.File.Exists(_assemblyFileLocation))
            {
                try
                {
                    var fileData = System.IO.File.ReadAllLines(_assemblyFileLocation);
                    if (fileData.Length == 0) return false;

                    for (var i = 0; i <= fileData.Length - 1; i++)
                    {
                        var line = fileData[i];
                        if (line.Length <= 2) continue;
                        if (!(!(line.Substring(0, 1) == "'") & !(line.Substring(0, 2) == "//"))) continue;
                        if (!(line.Contains("AssemblyVersion") | line.Contains("AssemblyFileVersion"))) continue;
                        
                        // Get the existing version.
                        var version = line.Substring(0, line.LastIndexOf('"'));
                        version = version.Substring(version.IndexOf('"') + 1, version.Length - version.IndexOf('"') - 1);                        
                        var v = version.Split('.');

                        var major = 1;
                        var minor = 0;
                        var buildNumber = 0;
                        var revision = 0;
                        
                        if (v.Length >= 0) major = Int32.Parse(v[0]);
                        if (v.Length >= 1) minor = Int32.Parse(v[1]);
                        if (v.Length >= 2) buildNumber = Int32.Parse(v[2]);
                        if (v.Length >= 3) revision = Int32.Parse(v[3]) + 1;

                        // Replace the original version strings with the updated version number.
                        fileData[i] = fileData[i].Replace(version, major + "." + minor + "." + buildNumber + "." + revision);
                    }

                    // Re-write the assembly info to the file.
                    System.IO.File.WriteAllLines(_assemblyFileLocation, fileData);
                }
                catch (Exception ex)
                {
                    Log.LogError(ex.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
