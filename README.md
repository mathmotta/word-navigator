# Word Navigator
Navigate through words and find your target!

# Running

* Download the latest release in the [release page](https://github.com/mathmotta/word-navigator/releases/tag/1.0.0)
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

The application __do__ handle cancellation. __To safely stop the execution, press *CTRL+C*.__

# Settings

The [appsettings.json](/src/BluePrism.WordNavigator.Bootstrap/appsettings.json) file can be edited for further customization:
```json
{
	"FileManagementService":"OnDemandFileService",
	"NavigationService":"WordNavigationService",
	"WordLength":4,
	"Serilog": {
		"MinimumLevel":{
			"Default":"Information",
			"Override":{
				"Microsoft": "Information",
				"System": "Warning"
			}
		}
	}
}
```

## Configurable Implementations

Both __FileManagementService__ and __NavigationService__ implementations are configurable.

For __FileManagementService__, the [FileService](/src/BluePrism.WordNavigator.Common/Services/IO/FileService.cs) and the [OnDemandFileService](/src/BluePrism.WordNavigator.Common/Services/IO/OnDemandFileService.cs) can be used. __FileService__ is a quicker service, more memory costly approach, better when dealing with small dictionaries as it loads all lines in memory. However it is not recommended for files that are too big (e.g. 100mb+), in such a case, the __OnDemandFileService__ is a better and much safer apporach as lines are lazily loaded.

For __NavigationService__, only one word navigation was implemented, [WordNavigatioNService](/src/BluePrism.WordNavigator.Core/Navigation/WordNavigationService.cs). The setting is still optional for future extensions.

## Optional WordLength

The WordLength setting is there because of the requirement restriction, but the application runs with any two words of same length!

## Log Settings

The Information is the recommended default log level, you can change it to Debug if you want to see the paths being constructed step-by-step.

# The Challenge

Write a program to solve a word puzzle
* Choose a start word;
* Choose a target word; 
* Move from start word to target in a list, one letter at a time with only valid words;

# The Requirements

* Command-line app in .NET (in this case, .NET Core 3.1)
* Arguments input in command line
* Arguments include: start word, target word, dictionary file and output file
* Both words should be 4 characters long
* The result should be an output file with the list of shortest paths
# Design Principles, Architecture Decisions and Methodologies


## Latest features

The application uses latest C# 8.0 features such as AsyncEnumerables and default interface implementations

## Asynchronous Scalability

Most methods were made with async/await and multithreading. It may not be a huge gain at this level of workloads, but the decision was made to increase the application's scalability.

As a bonus, workloads can be safely cancelled by pressing __CTRL+C__! The CancellationToken is triggered and verified between each path iteration. Cancelling will dispose the loaded lines and return an empty result.

## Test-Driven Development

TDD is great to establish functional specs for your methods so you know what to expect from them. Although not used for every single method, TDD was plently used

## Future proof

Although the requirement is to make it work with words with 4 character, the solution design is generic enough to work with any length. 

Try removing the WordLength setting and trying for maria -> deuce to see how big paths can become!

## Custom extensions

AsyncEnumerable is all nice and shiny, but it doesn't come with the same interfaces as the normal Enumerable, so I had to make my own! See the [AsyncEnumerableExtensions](/src/BluePrism.WordNavigator.Common/Extensions/AsyncEnumerableExtensions.cs) to see what kind of methods I made.

A special [ConcurrentHashMap(T)](/src/BluePrism.WordNavigator.Common/Concurrent/ConcurrentHashSet.cs) and extensions for [Collections](/src/BluePrism.WordNavigator.Common/Extensions/CollectionExtensions.cs), [Files](/src/BluePrism.WordNavigator.Common/Extensions/FileExtensions.cs), [Enumerables](/src/BluePrism.WordNavigator.Common/Extensions/EnumerableExtensions.cs) and [ConcurrentDictionaries](/src/BluePrism.WordNavigator.Common/Extensions/ConcurrentDictionaryExtensions.cs) were also created

## Design Patterns

Factory is indirectly used with Microsoft's dependency injection framework as well as Observer to enable a cancel hability
## SOLID

* Methods, Classes and services has a single purpose to encapsulate functionality; 
* Classes are extensible and modifyable where it makes sense;
* Compatible subclasses were used, no class was forced to implement interface methods they don't have use for;
* Classes in the whole application depend on abstraction, making heavy use of dependency injection;

## Interface dependencies, not implementation

* All dependencies are setup as interfaces. Implementations are optional and configurable in runtime with a configuration file.

# How the Magic Happens

* Get the dictionary content and store in a ConcurrentHashSet
     * The order doesn't matter so we might as well get a bit of performance out of this, right?
* Then create groups of similarities, that is represented by a ConcurrentDictionary with a string key for a group and a List of strings for the similarities
* Create a list of paths and add the start word to it, this will be our entrypoint
* Iterate over the paths, getting the first element for each element of the paths list and then removing it from the list as we'll create a group for that word.
* Iterate over each index of that word, for each index, iterate for each letter in the alphabet, making a new similarity for each new letter
* If the similarity is not even a word, ignore it
* If, however, it is a word, we have a new similarity! Add that similarity to that group
* Add the similarity to a group of already known words, we don't want to re-add them in the future, and also to the list of paths, we will have to iterate over each similarity as well so we find similarities of similarities
* Although this is all logical, this tree-shaped solution is well used on any problem that requires this pattern of repetition. The implemented solution was based on the Breadth First Search algorithm, although the implementation itself is quite different as it doesn't create an intermediate state between two groups - That is very time consuming to process!
* The last step is to run a recursive method to create the shortest paths between the groups.
* Yes, plural! The shortest path is about its lenght, but there are many valid results with the same shortest lenght.

# References


https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/default-interface-methods-versions
https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/
https://en.wikipedia.org/wiki/Breadth-first_search
https://github.com/dotnet/runtime/issues/2214
https://stackoverflow.com/questions/13167934/how-to-async-files-readalllines-and-await-for-results/52829926
https://darchuk.net/2017/11/03/concurrenthashsett/
