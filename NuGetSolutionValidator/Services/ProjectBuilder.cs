using System.Collections.Generic;
using System.IO;
using NugetSolutionValidator.DomainModels;

namespace NugetSolutionValidator.Services
{
    public class ProjectBuilder : IBuilder<Project, BuildProjectRequest>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IBuilder<ICollection<NuGetPackageDependency>, string> _packageDependencyBuilder;
        private readonly IDependencyFileResolver _dependencyFileResolver;

        public ProjectBuilder(IFileSystem fileSystem, 
            IBuilder<ICollection<NuGetPackageDependency>, string> packageDependencyBuilder,
            IDependencyFileResolver dependencyFileResolver)
        {
            _fileSystem = fileSystem;
            _packageDependencyBuilder = packageDependencyBuilder;
            _dependencyFileResolver = dependencyFileResolver;
        }

        public virtual Project Build(BuildProjectRequest request)
        {
            var projectDirectory = _fileSystem.GetDirectory(request.ProjectFilePath);
            var packageFilePath = _dependencyFileResolver.GetPath(projectDirectory);

            var packageDependencies = _packageDependencyBuilder.Build(packageFilePath);

            var project = new Project
                {
                    Name = request.Name,
                    Path = request.ProjectFilePath,
                    PackageFilePath = packageFilePath,
                    PackageDependencies = packageDependencies
                };

            return project;
        }
    }
}