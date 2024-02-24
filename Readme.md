<!-- Common Project Tags:
command-line 
console-applications 
dotnet 
dotnet-core 
netcore 
netframework 
netframework48 
tool 
tools 
vbnet 
visualstudio 
windows 
windows-app 
windows-application 
windows-applications 
windows-forms 
winforms 
 -->

# RegexCon

### Command-line app to perform Regular Expression (RegEx) string substitutions.

![logo](/Images/logo.png)

------------------

## 👋 Introduction

I decided to develop **RegexCon** as a solution for a third-party application that is written in a scripting language with limited Regex capabilities, but that third-party application supports running external apps, and this is where **RegexCon** comes into play for me, bridging the gap by enabling that third-party application to execute regular expression substitutions as required.

**RegexCon** could be used, for example, in a for-loop of any programming language to perform file renamings.

I'm convinced you would find additional scenarios where this tool proves invaluable to you!.

## ‼️ Limitations

**RegexCon** relies on the .NET regex engine and it utilizes its pattern syntax. While most common regex pattern syntax from other languages will work properly with **RegexCon**, it's important to note that each programming language has its own regex implementations, which may include minor syntax differences and/or limitations. For instance, a named group in PHP is written differently than in .NET. The responsibility for syntax adaptation lies with the end user.

## 📝 Requirements

- Microsoft Windows OS.

## 🤖 Getting Started

Download the latest release by clicking [here](https://github.com/ElektroStudios/RegexCon-Performs-RegEx-substitution-via-command-line/releases/latest),

#### Syntax
    RegexCon.exe /I=(input) /E=(expression) /S=(substitution)

#### Switches
    /Input        or /I | The input string.
    /Expression   or /E | The regular expression.
    /Substitution or /S | The substitution string.
    /MatchCase    or /M | Matches characters in a case-sensitive manner.
    /?                  | Shows this help.

#### Switch value types
---------------------------------------------------------
	/Input        or /I | Any string
	/Expression   or /E | Any string
	/Substitution or /S | Any string
	/MatchCase    or /M | True/False

#### Usage examples
---------------------------------------------------------
	( Regular string substitution. )
	RegexCon.exe /I="Hello World" /E="^(.+)\s(.+)$" /S="$2 $1" /M=True

	( Named-group string substitution. )
	RegexCon.exe /I="Hello World" /E="^(?<one>.+)\s(?<two>.+)$" /S="${two} ${one}"

## 🔄 Change Log

Explore the complete list of changes, bug fixes, and improvements across different releases by clicking [here](/Docs/CHANGELOG.md).

## ⚠️ Disclaimer:

This Work (the repository and the content provided in) is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the Work or the use or other dealings in the Work.

## 💪 Contributing

Your contribution is highly appreciated!. If you have any ideas, suggestions, or encounter issues, feel free to open an issue by clicking [here](https://github.com/ElektroStudios/RegexCon-Performs-RegEx-substitution-via-command-line/issues/new/choose). 

Your input helps make this Work better for everyone. Thank you for your support! 🚀

## 💰 Beyond Contribution 

This work is distributed for educational purposes and without any profit motive. However, if you find value in my efforts and wish to support and motivate my ongoing work, you may consider contributing financially through the following options:

 - ### Paypal:
    You can donate any amount you like via **Paypal** by clicking on this button:

    [![Donation Account](Images/Paypal_Donate.png)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=E4RQEV6YF5NZY)

 - ### Envato Market:
   If you are a .NET developer, you may want to explore '**DevCase Class Library for .NET**', a huge set of APIs that I have on sale.
   Almost all reusable code that you can find across my works is condensed, refined and provided through DevCase Class Library.

    Check out the product:
    
   [![DevCase Class Library for .NET](Images/DevCase_Banner.png)](https://codecanyon.net/item/elektrokit-class-library-for-net/19260282)

<u>**Your support means the world to me! Thank you for considering it!**</u> 👍
