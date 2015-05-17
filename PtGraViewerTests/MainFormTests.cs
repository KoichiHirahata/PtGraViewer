using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PtGraViewer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace PtGraViewer.Tests
{
    [TestClass()]
    public class MainFormTests
    {
        [TestMethod()]
        public void isWrongFolderNameTest()
        {
            Assert.IsTrue(MainForm.isWrongFolderName("/fet"));
            Assert.IsTrue(MainForm.isWrongFolderName("f/et"));
            Assert.IsTrue(MainForm.isWrongFolderName("te\\st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te:st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te*st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te?st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te\"st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te<st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te>st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te|st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te#st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te{st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te}st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te%st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te&st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te~st"));
            Assert.IsTrue(MainForm.isWrongFolderName("te..st"));
            Assert.IsTrue(MainForm.isWrongFolderName(".test"));
            Assert.IsTrue(MainForm.isWrongFolderName("test."));
            Assert.IsTrue(MainForm.isWrongFolderName("　"));
            Assert.IsTrue(MainForm.isWrongFolderName("　test"));
            Assert.IsTrue(MainForm.isWrongFolderName("test　"));
            Assert.IsTrue(MainForm.isWrongFolderName(""));
            Assert.IsTrue(MainForm.isWrongFolderName(null));
            Assert.IsFalse(MainForm.isWrongFolderName("test"));
            Assert.IsFalse(MainForm.isWrongFolderName("test1000"));
            Assert.IsFalse(MainForm.isWrongFolderName("1004488"));
            Assert.IsFalse(MainForm.isWrongFolderName("100-100-1"));
        }

        [TestMethod()]
        public void isWrongDateStrTest()
        {
            Assert.IsTrue(MainForm.isWrongDateStr("201145"));
            Assert.IsTrue(MainForm.isWrongDateStr("22110405"));
            Assert.IsTrue(MainForm.isWrongDateStr("02110405"));
            Assert.IsTrue(MainForm.isWrongDateStr(""));
            Assert.IsTrue(MainForm.isWrongDateStr(null));
            Assert.IsTrue(MainForm.isWrongDateStr("20110011"));
            Assert.IsTrue(MainForm.isWrongDateStr("20111311"));
            Assert.IsTrue(MainForm.isWrongDateStr("20111100"));
            Assert.IsTrue(MainForm.isWrongDateStr("20111232"));
            Assert.IsTrue(MainForm.isWrongDateStr(null));
            Assert.IsFalse(MainForm.isWrongDateStr("20140505"));
            Assert.IsFalse(MainForm.isWrongDateStr("21001211"));
        }
    }
}
