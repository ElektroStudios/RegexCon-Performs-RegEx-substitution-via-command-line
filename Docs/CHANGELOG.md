# RegexCon Change Log 📋

## v1.2 (2026) *(current)* 🆕
#### 🐞 Bug-Fixes:
 - The default value of the "/MatchCase" switch (True) was not applied when the switch was omitted, so the substitution was performed case-insensitive by default, contradicting the documentation. Now, when "/MatchCase" is omitted, the substitution is case-sensitive.
 - Unknown arguments (e.g. "/Inputs=abc", "/I:abc", or any typo) were silently ignored. Now they are reported as an error.
 - A parameter specified more than once (e.g. "/I=a /Input=b") silently overwrote the previous value. Now it is reported as an error.
 - A parameter specified without separator and value (e.g. "/I") was silently ignored. Now it is reported as a syntax error.
 - An empty regular expression ("/E=") was accepted, inserting the substitution string between every character of the input. Now it is reported as an error.
 - The "/?" switch was only processed when reached during the parsing, so a previous invalid argument could prevent the help from being shown. Now "/?" has priority over any other argument.
 - Error messages were written to the standard output, mixing them with the substitution result. Now they are written to the standard error (exit code 1).
 - Parameter values were converted using the current culture. Now they use the invariant culture.
 - Converting a command-line parameter without short name threw a NullReferenceException.
 - Adding command-line parameters without short name to the parameter collection threw a NullReferenceException.
 - A command-line parameter without short name matched any argument starting with the separator character ("=").
 - The parameter separator was not validated against the parameter short name.
 - ConsoleColor.Black could not be used in colored console output, because it was treated as "keep the current color".

#### 🌟 Improvements:
 - Added a unit test project (MSTest) that covers the argument parser, the parameter types, the RegEx substitution, the console output, the help section and the end-to-end execution of the application.
 - The help and the Readme now document the "/MatchCase" default value and the exit codes.
 - Defensive argument validation (fail-fast exceptions) in the parameter types, the argument parser and the utilities, and exception handlers narrowed to the expected exception types.

## v1.1 (2024) 🔄
#### 🌟 Improvements:
 - Source-code upgraded to .NET Framework 4.8
 - Syntax documentation has been improved a bit.

## v1.0 (2016) 🔄
Initial Release.