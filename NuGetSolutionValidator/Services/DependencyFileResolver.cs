using System.IO;

namespace NugetSolutionValidator.Services
{
    public interface IDependencyFileResolver
    {
        string GetPath(string directory);
    }

    public class FullFrameworkDependencyFileResolver : IDependencyFileResolver
    {
        public string GetPath(string directory)
        {
            var filePath = Path.Combine(directory, "packages.config");
            return filePath;
        }
    }
}