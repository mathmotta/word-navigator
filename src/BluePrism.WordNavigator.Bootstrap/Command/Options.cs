using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BluePrism.WordNavigator.Bootstrap.Command
{
    class Options
    {
        [Value(0, MetaName = "Start word", HelpText = "The word to start the search")]
        public string Start { get; set; }
        [Value(1, MetaName = "Target word", HelpText = "The target word to find")]
        public string Target { get; set; }

        [Option('d', "dictionary", HelpText = "The path to the dictionary file")]
        public string Dictionary { get; set; }
        [Option('o', "output", HelpText = "The output file path")]
        public string Output { get; set; }
    }
}
