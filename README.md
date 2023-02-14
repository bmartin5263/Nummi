# Nummi - Cryptocurrency Trading Bots
> Nummus (plural: nummi), a Latin term meaning "coin"

Nummi is a work-in-progress cryptocurrency trading bot framework built using ASP .NET Core. 

## Project Structure
| Directory      | Purpose |
| ----------- | ----------- |
| `/Api` | Defines REST Endpoints to interact with system |
| `/ClientApp` | React Frontend Application |
| `/Core` | Root Folder for Backend Implementation |
| `/Core/Database` | Database related functionality |
| `/Core/Domain` | Functionality related to the Trading domain |
| `/Core/External` | Integration clients for 3rd Party systems | 
| `/Core/Util` | Common utility functions not related to business domain |

## Domain Overview
This sections goes over the key domain entities involved in the application and what role they play

### Bots [`Core/Domain/Crypto/Bots`]
Bots represent autonomous traders within the system. They have a _name_, a fixed amount of _funds_ to spend, and, for the brains, a **Trading Strategy**.
Bots can only trade in a single _Trading Mode_ at a time, such as `Simulated`, `Paper`, or `Live`. They can be in _active_ or _inactive_ states.

While active, they spend most of their time sleeping, only to be woken up at fixed intervals defined by their Trading Strategy to check for trades. 
If an error happens while their Trading Strategy is executing, Bots enter an _error state_ where manual user intervention is required to clear the error.

### Trading Strategies [`Core/Domain/Crypto/Strategies`]
Trading Strategies are the brains of the bots, they define the algorithm that should run whenever a bot is woken up. The system defines an abstract `Strategy` class
that is meant to be overridden by concrete strategy implementations. 

Strategies start off _uninitialized_, but on first execution they call a user-overridden `Initialize()`
method that can be used to collect historical data and analyze trends. Once _initialized_, Strategies run at a fixed _Frequency_, such as every minute, each interval
calling the user-overriden `CheckForTrades()` method. This method is used for checking the current price against recent trends and making buy/sell decisions as a result.

### Trading Modes
| Value            | Meaning                                                          |
|------------------|------------------------------------------------------------------|
| `Simulated`      | Trading is simulated using historical data in a fixed time frame |
| `Paper`          | Trading is executed in real-time using fake paper money          |
| `Live`           | Trading is executed in real-time using no-nonsense real money    |