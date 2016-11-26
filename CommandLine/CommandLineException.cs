using System;
using System.Collections.Generic;

namespace CommandLine {

    public class CommandLineException : Exception {
        public CommandLineException(string message) : base(message) { }
    }

}

