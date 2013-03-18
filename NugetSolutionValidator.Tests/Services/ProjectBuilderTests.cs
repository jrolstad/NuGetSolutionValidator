﻿using System.Collections.Generic;
using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using NugetSolutionValidator.DomainModels;
using NugetSolutionValidator.Services;

namespace NugetSolutionValidator.Tests.Services
{
    [TestFixture]
    public class ProjectBuilderTests
    {
        private string _projectFilePath;
        private IList<NuGetPackageDependency> _dependencies;
        private Project _project;
        private string _expectedPackageFilePath;
        private string _projectDirectory;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _projectFilePath = @"C:\myproject\myproject.csproj";
            _projectDirectory = @"C:\myproject";

            _dependencies = Builder<NuGetPackageDependency>.CreateListOfSize(2).Build();
            _expectedPackageFilePath = @"C:\myproject\packages.config";

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(fs => fs.GetDirectory(_projectFilePath)).Returns(_projectDirectory);

            var packageBuilder = new Mock<IBuilder<ICollection<NuGetPackageDependency>>>();
            packageBuilder.Setup(b => b.Build(_expectedPackageFilePath)).Returns(_dependencies);

            var builder = new ProjectBuilder(fileSystem.Object, packageBuilder.Object);

            _project = builder.Build(_projectFilePath);
        }


        [Test]
        public void Then_the_project_path_is_set()
        {
            // Assert
            Assert.That(_project.Path,Is.EqualTo(_projectFilePath));
        }

        [Test]
        public void Then_the_package_file_path_is_set()
        {
            // Assert
            Assert.That(_project.PackageFilePath, Is.EqualTo(_expectedPackageFilePath));
        }


        [Test]
        public void Then_the_nuget_dependencies_are_read_from_the_package_file_path()
        {
            // Assert
            Assert.That(_project.PackageDependencies,Is.EquivalentTo(_dependencies));
        }

    }
}