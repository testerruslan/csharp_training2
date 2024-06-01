using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace addressbook_tests_white
{
    [TestFixture]
    public class GroupRemovalTests : TestBase
    {
            [Test]
            public void TestGroupRemoval()
            {
                app.Groups.EmptyGroupList();
                List<GroupData> oldGroups = app.Groups.GetGroupList();
                GroupData toBeRemoved = oldGroups[0];

                app.Groups.Remove(toBeRemoved);

                List<GroupData> newGroups = app.Groups.GetGroupList();

                oldGroups.RemoveAt(0);
                Assert.AreEqual(oldGroups.Count, newGroups.Count);
            }
    }
    }

