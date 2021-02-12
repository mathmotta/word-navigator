# Word Navigator
Navigate through words and find your target!

# Running

* Download the latest release in the release page;
* Unzip the file in any directory;
* Open a terminal an navigate to the directory the application was unzipped

* Usage:
```shell
WordNavigator 1.0.0
Copyright (C) 2021 WordNavigator

  -d, --dictionary        The path to the dictionary file
  -o, --output            The output file path
  --help                  Display this help screen.
  --version               Display version information.
  Start word (pos. 0)     The word to start the search
  Target word (pos. 1)    The target word to find
```

* Syntax:
```shell
WordNavigator START_WORD END_WORD [OPTION] [OPTION]
```

* Example
```shell
WordNavigator spin spot -d "words-english.txt" -o "paths.txt"
```

Note: The dictionary and output paths can be ommited, in which case the default dictionary will be used instead and the results will be output to "results.txt" in the same directory.

# Design Choices

## SOLID

Methods, Classes and services has a single purpose to encapsulate functionality, classes are extensible and modifyable where it makes sense, compatible subclasses were used, no class was forced to implement interface methods they don't have use for and the whole application classes depend on abstraction making heavy use of dependency injection

## Interface dependencies, not implementation

All dependencies are setup as interfaces. Implementations are optional and configurable in runtime with a configuration file.

# Design Patterns

Factory is indirectly used by Microsoft's dependency injection framework as well as Observer to enable a cancel hability