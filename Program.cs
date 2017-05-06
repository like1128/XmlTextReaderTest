using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonLib.Suite s = new CommonLib.Suite();
            string path = Directory.GetCurrentDirectory();

            XmlTextReader xtr = new XmlTextReader("testCases.xml");
            xtr.WhitespaceHandling = WhitespaceHandling.None;
            xtr.Read(); // read the XML declaration node, advance to <suite> tag
            xtr.Read();
            xtr.Read();
            while (xtr.Name == "") xtr.Read();

            while (!xtr.EOF) //load loop
            {
                CommonLib.TestCase tc = new CommonLib.TestCase();
                tc.id = xtr.GetAttribute("id");
                tc.kind = xtr.GetAttribute("kind");

                while (xtr.Name != "arg1" || !xtr.IsStartElement())
                    xtr.Read(); // advance to <arg1> tag

                tc.arg1 = xtr.ReadElementString("arg1"); // consumes the </arg1> tag

                while (xtr.Name != "arg2" || !xtr.IsStartElement())
                    xtr.Read(); // advance to <arg2> tag
                tc.arg2 = xtr.ReadElementString("arg2"); // consumes the </arg2> tag

                while (xtr.Name != "expected" || !xtr.IsStartElement())
                    xtr.Read(); // advance to <arg2> tag
                tc.expected = xtr.ReadElementString("expected"); // consumes the </expected> tag
                                                                 // we are now at an </testcase> tag
                s.items.Add(tc);
                xtr.Read();
                while (xtr.Name == "") xtr.Read(); //</testcase>

                xtr.Read();
                while (xtr.Name == "") xtr.Read(); //<testcase> or </suite>

                if (!xtr.IsStartElement()) break;
            } // load loop

            xtr.Close();
            s.Display(); // show the suite of TestCases

        } // Main()
    }

    namespace CommonLib
    {
        public class TestCase
        {
            public string id;
            public string kind;
            public string arg1;
            public string arg2;
            public string expected;
        }

        public class Suite
        {
            public ArrayList items = new ArrayList();
            public void Display()
            {
                foreach (TestCase tc in items)
                {
                    Console.Write(tc.id + " " + tc.kind + " " + tc.arg1 + " ");
                    Console.WriteLine(tc.arg2 + " " + tc.expected);
                }
                Console.ReadLine();
            }
        } // class Suite
    } // ns 
}
