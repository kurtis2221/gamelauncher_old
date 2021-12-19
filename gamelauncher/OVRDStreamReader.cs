using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace gamelauncher
{
    public class OVRDStreamReader : StreamReader
    {
        int line = 0;

        public OVRDStreamReader(string fname, Encoding enc)
            : base(fname, enc)
        {

        }

        public int LineNumber
        {
            get
            {
                return line;
            }
        }

        public override string ReadLine()
        {
            line++;
            return base.ReadLine();
        }

        public override void Close()
        {
            line = 0;
            base.Close();
        }
    }
}
