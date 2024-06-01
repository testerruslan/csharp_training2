using System;
using System.Collections.Generic;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using System.Windows.Automation;
using TestStack.White.WindowsAPI;

namespace addressbook_tests_white
{
    public class GroupHelper : HelperBase
    {
        public static string GROUPWINTITLE = "Group editor";
        public static string DELETEGROUPWINTITLE = "Delete group";

        public GroupHelper(ApplicationManager manager) : base(manager)
        {

        }

        public void Add(GroupData newGroup)
        {
            Window dialogue = OpenGroupsDialog();
            dialogue.Get<Button>("uxNewAddressButton").Click();
            TextBox textBox = (TextBox) dialogue.Get(SearchCriteria.ByControlType(ControlType.Edit));
            textBox.Enter(newGroup.Name);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            CloseGroupsDialog(dialogue);
        }

        private void CloseGroupsDialog(Window dialogue)
        {
            dialogue.Get<Button>("uxCloseAddressButton").Click();
        }

        private Window OpenGroupsDialog()
        {
            manager.MainWindow.Get<Button>("groupButton").Click();
            return manager.MainWindow.ModalWindow(GROUPWINTITLE);
        }

        public List<GroupData> GetGroupList()
        {
            List<GroupData> list = new List<GroupData>();

            Window dialogue = OpenGroupsDialog();
            Tree tree = dialogue.Get<Tree>("uxAddressTreeView");
            TreeNode root = tree.Nodes[0];
            foreach (TreeNode item in root.Nodes)
            {
                list.Add(new GroupData()
                {
                    Name = item.Text
                }) ;
            }
            CloseGroupsDialog(dialogue);
            return list;
        }

        public void Remove(GroupData newGroup)
        {
            Window dialogue = OpenGroupsDialog();
            //aux.ControlTreeView(GROUPWINTITLE, newGroup.Name, "WindowsForms10.SysTreeView32.app.0.2c908d51", "Select", "#0|#0", "");
            //aux.ControlClick(GROUPWINTITLE, "", "WindowsForms10.BUTTON.app.0.2c908d51");
            //aux.WinWait(DELETEGROUPWINTITLE);
            //aux.ControlClick(DELETEGROUPWINTITLE, "", "WindowsForms10.BUTTON.app.0.2c908d53");
            CloseGroupsDialog(dialogue);
        }

        public void EmptyGroupList()
        {
            Window dialogue = OpenGroupsDialog();

            if (GetGroupList().Count <= 1)
            {
                GroupData group = new GroupData()
                {
                    Name = "test"
                };

                Add(group);
            }
        }
    }
}