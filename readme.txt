0. Environment Specification
 * Windows 10 Pro
 * .NET 8.0 / C# 12

1. Folder Description
 * mas_csharp_assignment - local storage
  - Client reads asset / positions / orders / transaction records / order records from text files in a folder under the Client project 
  - Client saves asset / positions / orders / transaction records / order records on text files in a folder under the Client project 
  - Logger.cs is under Client project
  - asset / positions / orders: ~ \mas_csharp_assignment - local storage\Client\bin\Debug\net8.0-windows/active
  - transaction records / order records: ~ \mas_csharp_assignment - local storage\Client\bin\Debug\net8.0-windows/records
 * mas_csharp_assignment - remote storage [main]
  - Client retrieves asset / positions / orders / transaction records / order records from Server(who reads data from text files in a folder under the Server project) over network
  - Client updates asset / positions / orders / transaction records / order records on Server(who reads data from text files in a folder under the Server project) over network over network
  - Logger.cs is under Common project
  - asset / positions / orders: ~ \mas_csharp_assignment - remote storage\Server\bin\Debug\net8.0-windows/active
  - transaction records / order records: ~ \mas_csharp_assignment - remote storage\Server\bin\Debug\net8.0-windows/records

2. Client Structure(Design Pattern)
 * [Model/Controller] partial application of structures for multi-exchange arbitrage systems(5 layers: interface, integration, strategy, execution, and updating) 
  - Interface: Execution.cs for non-streaming data, Streams.cs for streaming data
  - Integration: Integration.cs
  - Strategy: no required
  - Execution: UI -> Integration.cs -> Execution.cs
  - Updating: Execution.cs -> Integration.cs -> UI
 * [View] data binding on UI
  - Each form has a background worker who updates data on UI periodically, safely, and asynchronously
  - In order to avoid threads accessing the same data at a time, positions and orders take the benefit of ConcurrentDictionary, whereas transaction records and order records rely on mutex lock.  
  - An orderbook form for each symbol has only one instance at a time in order to save computer memory. The close button just hides the window. 

3. Data Exchange Methodology(by SignalR)
 * Request-Response
  - common: symbol constraints
  - remote storage: positions / orders / transaction records / order records
 * Streaming
  - common: orderbooks

4. Orderbooks
 * Symbols
  - top 10 market capitalization of KOSPI
  - The opening price for each symbol forms the last price(a field in Orderbook structure) on April 23, 2024 (Reference: Naver Financial)
 * Dynamic orderbook
  - To create an effect for a live orderbook, price and quantity for each level of ask/bid for a symbol has random values for each call
  - To prevent the price from being lower than , each symbol's ask/bid has a value above its initial price(a field in SymbolConstraint). 
  - A symbol's final ask/bid is determined by the product of its price step and the deviation factor(ranging from 0 to 10) generated randomly for each call 

5. Order Execution
 - A virtual order is executed based on OrderApplication structure, which is the source for updating positions / orders / transaction records / order records
 - The send_order function filters out some special cases(lack of available balance, shorting, and closing a position over holding quantity) that are not suitable for processing an order execution. 
 - For the sake of simplicity, the price of an order does not consider quantities at each ask/bid price level. In other words, a buy(sell) order price cannot be higher(lower) than lowest ask(highest bid) no matter how much the order quantity is. 
 - For the sake of simplicity, an order is not partially filled. 

6. Possible Improvement/Optimization
 - Migrating from background worker to invocation for reaction speed and thread safety 
 - Handling the case of an order to be partially filled