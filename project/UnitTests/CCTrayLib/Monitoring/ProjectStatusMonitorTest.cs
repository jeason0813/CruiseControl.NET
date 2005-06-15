using System;
using NMock;
using NUnit.Framework;
using ThoughtWorks.CruiseControl.CCTrayLib.Monitoring;
using ThoughtWorks.CruiseControl.Remote;

namespace ThoughtWorks.CruiseControl.UnitTests.CCTrayLib.Monitoring
{
	[TestFixture]
	public class ProjectStatusMonitorTest
	{
		private DynamicMock mockCruiseManager;
		private ICruiseProjectManager manager;
		const string PROJECT_NAME = "projectName";

		[SetUp]
		public void SetUp()
		{
			mockCruiseManager = new DynamicMock( typeof (ICruiseManager) );
			mockCruiseManager.Strict = true;

			manager = new CruiseProjectManager((ICruiseManager) mockCruiseManager.MockInstance, PROJECT_NAME);
		}

		[TearDown]
		public void TearDown()
		{
			mockCruiseManager.Verify();
		}

		[Test]
		public void CanRetriveProjectName()
		{
			Assert.AreEqual(PROJECT_NAME, manager.ProjectName);
		}

		[Test]
		public void ProjectStatusReturnsTheStatusForTheNominatedProject()
		{
			ProjectStatus[] result = new ProjectStatus[]
				{
					CreateProjectStatus("a name"),
					CreateProjectStatus(PROJECT_NAME),
				};

			mockCruiseManager.ExpectAndReturn("GetProjectStatus", result);

			Assert.AreSame(result[1], manager.ProjectStatus);
		}

		[Test]
		public void ProjectStatusReturnsNullIfProjectNotFound()
		{
			ProjectStatus[] result = new ProjectStatus[]
				{
					CreateProjectStatus("a name"),
					CreateProjectStatus("another name"),
			};

			mockCruiseManager.ExpectAndReturn("GetProjectStatus", result);

			Assert.IsNull(manager.ProjectStatus);
		}

		private static ProjectStatus CreateProjectStatus(string projectName)
		{
			return new ProjectStatus(ProjectIntegratorState.Running, IntegrationStatus.Failure, ProjectActivity.Sleeping, projectName, "url", DateTime.Now, "label", null, DateTime.Now );
		}

		[Test]
		public void CanForceABuild()
		{
			mockCruiseManager.Expect("ForceBuild", PROJECT_NAME);
			manager.ForceBuild();
		}
	}
}