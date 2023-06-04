# VulcanStocks-QTS

VulcanStocks-QTS is a quant trading system (QTS) that provides a robust and versatile infrastructure for algorithmic trading of virtual assets. It allows for the development, backtesting, and execution of trading strategies using a simulated brokerage service.

> **Note**: VulcanStocks-QTS currently does not support real money trading.

## Features

- **Trading Engine**: The trading engine is the heart of the system, designed to execute trades based on the provided trading strategy.
- **Backtesting**: This feature allows you to test your trading strategies against historical data to gauge their performance before deploying them in real-time trading simulations.
- **Simulated Broker**: The Simulated Broker service is used to imitate real-world trading conditions. You can initialize it with a certain balance and amount of assets to buy.

## Code Overview

The `TradingOrchestrator` class is the primary entry point to the system. It initializes and controls the trading engine and backtesting services, among other things.

### Methods

- **InitSimulatedBroker**: This method initializes the simulated broker with a specific balance and a number of assets to buy.
- **StartTrader**: This method starts the trading engine.
- **StopTrader**: This method stops the trading engine.
- **RunBacktest**: This method runs the backtest of a specific trading strategy.
- **SimulatedBrokerHasAsset**: This method checks if the simulated broker currently holds any assets.
- **IsTraderRunning**: This method checks if the trading engine is currently running.

## How to Use

To start using VulcanStocks-QTS, initialize the `TradingOrchestrator` with your chosen trading strategy function, the time frame for trading, and the relevant data and broker services. Then you can start the trading process, stop it as needed, or run backtests.

## Disclaimer

Remember that VulcanStocks-QTS is currently a simulation-only system. It does not support real money trading and should not be used for such. Always conduct your own due diligence before making trading decisions.

