![alt tag](https://cdn.quantconnect.com/web/i/20180601-1615-lean-logo-small.png)

# Архитектура [Lean] algorithmic trading engine & автоматизации торговли на бирже

https://drive.google.com/drive/folders/1aFGbbeDtwOMK2snN_ok24I33_VmQlw5p

Индикатор канала регрессии RegressionChannelAlgorithm.cs

https://github.com/gridgentoo/Lean/blob/master/Algorithm.CSharp/RegressionChannelAlgorithm.cs

Индикатор пересечение Машек EMA  EmaCrossUniverseSelectionAlgorithm.cs

https://github.com/gridgentoo/Lean/blob/master/Algorithm.CSharp/EmaCrossUniverseSelectionAlgorithm.cs

Индикатор MACD MACDTrendAlgorithm.cs

https://github.com/gridgentoo/Lean/blob/master/Algorithm.CSharp/MACDTrendAlgorithm.cs

Step by Step Algorithmic Trading Guide with QuantConnect

https://www.youtube.com/watch?v=9wm8e7w6qN4

Lean Engine: Lesson 1 - Using a Desktop User Interface

https://www.youtube.com/watch?time_continue=2&v=m6llfznP4d4

Lean Engine: Lesson 2 - How do I use the QuantConnect API File Provider?

https://www.youtube.com/watch?v=uyHp_jyeSpA

Quick Start Lesson 5: Triggering Scheduled Events

https://www.youtube.com/watch?v=SUu4MKlm93I&index=6&list=PLD7-B3LE6mz7cqCrLa3Sy8I8Gb62CJwX9&t=0s

# Архитектура [Voltdb]  хранение данных в памяти с концепцией распределенной 

https://drive.google.com/drive/folders/1YqYZIwWFvy1lP1NMOxrhZrMX7xZj-628

СУБД VoltDB 3.0, развиваемой под руководством Майкла Стоунбрейкера (Mike Stonebraker), одного из основателей проектов Ingres и PostgreSQL. СУБД VoltDB поддерживает горизонтальное масштабирование и ориентирована на обработку транзакций в реальном времени (OLTP). На недорогом кластере, собранном своими силами из обычных серверов, СУБД способна обрабатывать миллионы транзакций в секунду.

# Apache Hadoop Big Data with VoltDB
![Image alt](https://www.voltdb.com/wp-content/uploads/2017/03/Apache-Hadoop-Big-Data-VoltDB.gif)

Идея VoltDB заключается в том, что все транзакции выполняются с помощью предварительно скомпилированных хранимых процедур, реализованных на Java, и все хранимые процедуры сериализуются, что позволяет VoltDB достичь самого высокого уровня изоляции и устранения блокировок. Использование устройства очень хорошее. В официальных результатах испытаний VoltDB может легко масштабироваться до 39 серверов (300 ядер), 120 разделов, обрабатывающих 1,6 миллиона сложных транзакций в секунду.

# Архитектура [Geode] Реверс инжениринг движка обработки финансовых транзакций в торговых платформах различных компаний на Уолл-стрит.
Geode был создан компанией Gemstone Systems в 2002 году и применяется в качестве высокопроизводительного движка обработки финансовых транзакций в торговых платформах различных компаний на Уолл-стрит.

В качестве примера внедрения Geode это Национальная железная дорога Китая, в которой кластер из 20 узлов (10 основных и 10 запасных) обеспечивает хранение 2 Тб оперативной информации о билетах. 
![Image alt](http://chinalogist.ru/sites/default/files/speed-railwas-of-china4.png)

Архитектура [Geode] & Реверс инжениринг движка обработки финансовых транзакций
https://drive.google.com/drive/folders/1tetUejh8WzscoCbCHPsdILM6desm5GzX

# Apache Nutch — модульный фреймворк для построения поисковых систем, написанный на языке Java таких как yandex или google

На базе Nutch 2.0, запущенного поверх 34-узлового Hadoop-кластера, построен поисковый сервис компании Kalooga, в индексе которого находится более миллиарда страниц. 

# Архитектура [nutch] & Реверс инжениринг

https://drive.google.com/drive/folders/13V9bx41gtxNxtYkfGE3p01UO0sqMKQ5G

![alt text](https://sites.google.com/site/nutch1936/_/rsrc/1427176500763/home/introduction/Nutch_Overview.png)

Apache Nutch — модульный фреймворк для построения поисковых систем, написанный на языке Java и основанный на технологиях Lucene, Solr, Tika, Hadoop и Gora, адаптированных для специфики поиска в интернете (например, поддерживается crawler, база ссылочной связи, парсинг HTML и других форматов). Архитектура Nutch позволяет разработчикам создавать плагины для обработки нового медиа-контента, получение данных через нестандартные каналы для формирования типовых запросов или организации поискового кластера.
Система предоставляет средства для построения высокомасштабных систем индексации, отвлеченных от типа хранилища, что позволяет использовать такие хранилища для больших объемов данных, как Apache Accumulo, Apache Avro, Apache Cassandra, Apache HBase и HDFS, так и SQL-базы и размещение в памяти NoSQL БД.
На базе Nutch 2.0, запущенного поверх 34-узлового Hadoop-кластера, построен поисковый сервис компании Kalooga, в индексе которого находится более миллиарда страниц[1].

# Архитектура [mahout] & Реверс инжениринг
https://drive.google.com/drive/folders/1dGfKjaP6qNcI3E98OkwWBAFMXxYsA4t6

![Image alt](https://shawnennis.com/wp-content/uploads/2018/06/ml-history.png)

Apache Mahout -- это проект Apache Software Foundation для создания свободных реализаций распределенных или, иначе, масштабируемых алгоритмов машинного обучения, ориентированных прежде всего на область совместной фильтрации, кластеризации и классификации. Многие из реализаций используют платформу Apache Hadoop. Mahout также предоставляет библиотеки Java для общих математических операций (ориентированных на линейную алгебру и статистику) и примитивные коллекции Java. Mahout находится в процессе разработки; количество реализованных алгоритмов сильно выросло за последнее время, но многие по-прежнему отсутствуют
В то время как основные алгоритмы Mahout рассчитаны на кластерный анализ, классификация и совместная фильтрация на основе пакетов реализованы поверх Apache Hadoop с использованием парадигмы map / reduce, она не ограничивает вклады в реализации на основе Hadoop. Также приветствуются вклады, которые выполняются на одном узле или в кластере, отличном от Hadoop. Например, рекомендатор по совместной фильтрации Taste, разработанный Mahout, первоначально был отдельным проектом и может работать автономно без Hadoop.

Начиная с версии 0.10.0, проект переключает свое внимание на создание независимой от программного обеспечения среды программирования под названием «Самсара». Среда состоит из алгебраического, независимого от программного обеспечения оптимизатора и алгебраического Scala DSL, объединяющего находящихся в памяти и распределенных алгебраических операторов. Ранее также поддерживались алгебраические платформы Apache Spark, H2O и Apache Flink.

![Image alt](https://github.com/renepickhardt/metalcon/wiki/mahout_architecture.png)

# Особенности
Ниже перечислены основные особенности и возможности Apache Mahout.
Алгоритмы Mahout написаны поверх Hadoop, поэтому он хорошо работает в распределенной среде. Использует библиотеку Apache Hadoop, Mahout способен эффективно выполнять масштабирование в облаке.
Mahout предлагает инженеру готовую к использованию инфраструктуру для выполнения задач интеллектуального анализа данных на больших объемах данных.
Mahout позволяет приложениям быстро и эффективно анализировать большие наборы данных.
Включает несколько реализаций кластеризации с поддержкой MapReduce, таких как k-средства, нечеткие k-средства, Canopy, Dirichlet и Mean-Shift.
Поставляется с возможностями распределенной функциональности для эволюционного программирования.
Включает матричные и векторные библиотеки.

# Клиенты

Mahout используют такие компании, как Adobe, Facebook, LinkedIn, Foursquare, Twitter и Yahoo.
Foursquare использует механизм рекомендаций Mahout для определения адресов.
Twitter использует Mahout для моделирования интересов пользователей.
Yahoo! использует Mahout для разработки шаблонов.

=========

[![Build Status](https://travis-ci.org/QuantConnect/Lean.svg?branch=feature%2Fremove-web-socket-4-net)](https://travis-ci.org/QuantConnect/Lean) &nbsp;&nbsp;&nbsp; [![Google Group](https://img.shields.io/badge/debug-Google%20Group-53c82b.svg)](https://groups.google.com/forum/#!forum/lean-engine) &nbsp;&nbsp;&nbsp; [![Slack Chat](https://img.shields.io/badge/chat-Slack-53c82b.svg)](https://www.quantconnect.com/slack)


[Lean Home - https://www.quantconnect.com/lean][1] | [Documentation][2] | [Download Zip][3]

----------

## Introduction Lean Engine  ##

Lean Engine is an open-source algorithmic trading engine built for easy strategy research, backtesting and live trading. We integrate with common data providers and brokerages so you can quickly deploy algorithmic trading strategies.

The core of the LEAN Engine is written in C#; but it operates seamlessly on Linux, Mac and Windows operating systems. It supports algorithms written in Python 3.6, C# or F#. Lean drives the web based algorithmic trading platform [QuantConnect][4].

## System Overview ##

![alt tag](Documentation/2-Overview-Detailed-New.png)

The Engine is broken into many modular pieces which can be extended without touching other files. The modules are configured in config.json as set "environments". Through these environments you can control LEAN to operate in the mode required. 

The most important plugins are:

 - **Result Processing** (IResultHandler)
   > Handle all messages from the algorithmic trading engine. Decide what should be sent, and where the messages should go. The result processing system can send messages to a local GUI, or the web interface.

 - **Datafeed Sourcing** (IDataFeed)
   > Connect and download data required for the algorithmic trading engine. For backtesting this sources files from the disk, for live trading it connects to a stream and generates the data objects.

 - **Transaction Processing** (ITransactionHandler)
   > Process new order requests; either using the fill models provided by the algorithm, or with an actual brokerage. Send the processed orders back to the algorithm's portfolio to be filled.

 - **Realtime Event Management** (IRealtimeHandler)
   > Generate real time events - such as end of day events. Trigger callbacks to real time event handlers. For backtesting this is mocked-up an works on simulated time. 
 
 - **Algorithm State Setup** (ISetupHandler)
   > Configure the algorithm cash, portfolio and data requested. Initialize all state parameters required.

For more information on the system design and contributing please see the Lean Website Documentation.

## Installation Instructions ##

Download the zip file with the [lastest master](https://github.com/QuantConnect/Lean/archive/master.zip) and unzip it to your favorite location.

Alternatively, install [Git](https://git-scm.com/downloads) and clone the repo:
```
git clone https://github.com/QuantConnect/Lean.git
cd Lean
```

### macOS 

- Install [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/)
- Open `QuantConnect.Lean.sln` in Visual Studio

Visual Studio will automatically start to restore the Nuget packages. If not, in the menu bar, click `Project > Restore NuGet Packages`.

- In the menu bar, click `Run > Start Debugging`.

Alternatively, run the compiled `exe` file. First, in the menu bar, click `Build > Build All`, then:
```
cd Lean/Launcher/bin/Debug
mono QuantConnect.Lean.Launcher.exe
```

### Linux (Debian, Ubuntu)

- Install [Mono](http://www.mono-project.com/download/#download-lin):
```
sudo apt-get update && sudo rm -rf /var/lib/apt/lists/*
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy/snapshots/4.6.1.5 main" > sudo /etc/apt/sources.list.d/mono-xamarin.list
```
If you get this error on the last command:
 
**Unable to locate package referenceassemblies-pcl**,
 
run the following command (it works on current version of Ubuntu - 17.10):
```
echo "deb http://download.mono-project.com/repo/ubuntu xenial main" | sudo tee /etc/apt/sources.list.d/mono-official.list
```

```
sudo apt-get update
sudo apt-get install -y binutils mono-complete ca-certificates-mono referenceassemblies-pcl fsharp
```
- Install Nuget
```
sudo apt-get update && sudo apt-get install -y nuget
```
- Restore NuGet packages then compile:
```
nuget restore QuantConnect.Lean.sln
xbuild QuantConnect.Lean.sln
```
If you get: "Error initializing task Fsc: Not registered task Fsc." -> `sudo apt-get upgrade mono-complete`

If you get: "XX not found" -> Make sure Nuget ran successfully, and re-run if neccessary.

If you get other errors that lead to the failure of your building, please refer to the commands in "DockerfileLeanFoundation" file for help.

- Run the compiled `exe` file:
```
cd Lean/Launcher/bin/Debug
mono ./QuantConnect.Lean.Launcher.exe
```
- Interactive Brokers set up details

Make sure you fix the `ib-tws-dir` and `ib-controller-dir` fields in the `config.json` file with the actual paths to the TWS and the IBController folders respectively.

If after all you still receive connection refuse error, try changing the `ib-port` field in the `config.json` file from 4002 to 4001 to match the settings in your IBGateway/TWS.

### Windows

- Install [Visual Studio](https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx)
- Open `QuantConnect.Lean.sln` in Visual Studio
- Build the solution by clicking Build Menu -> Build Solution (this should trigger the Nuget package restore)
- Press `F5` to run

Nuget packages not being restored is the most common build issue. By default Visual Studio includes NuGet, if your installation of Visual Studio (or your IDE) cannot find DLL references, install [Nuget](https://www.nuget.org/), run nuget on the solution and re-build the Solution again. 

### Python Support

A full explanation of the Python installation process can be found in the [Algorithm.Python](https://github.com/QuantConnect/Lean/tree/master/Algorithm.Python#quantconnect-python-algorithm-project) project.

### R Support

- Install R-base if you need to call R in your algorithm.
For Linux users:
```
sudo apt-get update && apt-get install -y r-base && apt-get install -y pandoc && apt-get install -y libcurl4-openssl-dev
```
For Windows and macOs users:
Please visit the official [R website](https://www.r-project.org/) to download R. 

### QuantConnect Visual Studio Plugin

For more information please see the QuantConnect Visual Studio Plugin [Documentation][8]

## Issues and Feature Requests ##

Please submit bugs and feature requests as an issue to the [Lean Repository][5]. Before submitting an issue please read others to ensure it is not a duplicate.

## Mailing List ## 

The mailing list for the project can be found on [Google Groups][6]. Please use this to request assistance with your installations and setup questions.

## Contributors and Pull Requests ##

Contributions are warmly very welcomed but we ask you read the existing code to see how it is formatted, commented and ensure contributions match the existing style. All code submissions must include accompanying tests. Please see the [contributor guide lines][7].

All accepted pull requests will get a 2mo free Prime subscription on QuantConnect. Once your pull-request has been merged write to us at support@quantconnect.com with a link to your PR to claim your free live trading. QC <3 Open Source.

## Acknowledgements ##

The open sourcing of QuantConnect would not have been possible without the support of the Pioneers. The Pioneers formed the core 100 early adopters of QuantConnect who subscribed and allowed us to launch the project into open source.

Ryan H, Pravin B, Jimmie B, Nick C, Sam C, Mattias S, Michael H, Mark M, Madhan, Paul R, Nik M, Scott Y, BinaryExecutor.com, Tadas T, Matt B, Binumon P, Zyron, Mike O, TC, Luigi, Lester Z, Andreas H, Eugene K, Hugo P, Robert N, Christofer O, Ramesh L, Nicholas S, Jonathan E, Marc R, Raghav N, Marcus, Hakan D, Sergey M, Peter McE, Jim M, INTJCapital.com, Richard E, Dominik, John L, H. Orlandella, Stephen L, Risto K, E.Subasi, Peter W, Hui Z, Ross F, Archibald112, MooMooForex.com, Jae S, Eric S, Marco D, Jerome B, James B. Crocker, David Lypka, Edward T, Charlie Guse, Thomas D, Jordan I, Mark S, Bengt K, Marc D, Al C, Jan W, Ero C, Eranmn, Mitchell S, Helmuth V, Michael M, Jeremy P, PVS78, Ross D, Sergey K, John Grover, Fahiz Y, George L.Z., Craig E, Sean S, Brad G, Dennis H, Camila C, Egor U, David T, Cameron W, Napoleon Hernandez, Keeshen A, Daniel E, Daniel H, M.Patterson, Asen K, Virgil J, Balazs Trader, Stan L, Con L, Will D, Scott K, Barry K, Pawel D, S Ray, Richard C, Peter L, Thomas L., Wang H, Oliver Lee, Christian L.


  [1]: https://www.quantconnect.com/lean "Lean Open Source Home Page"
  [2]: https://www.quantconnect.com/lean/docs "Lean Documentation"
  [3]: https://github.com/QuantConnect/Lean/archive/master.zip
  [4]: https://www.quantconnect.com "QuantConnect"
  [5]: https://github.com/QuantConnect/Lean/issues
  [6]: https://groups.google.com/forum/#!forum/lean-engine
  [7]: https://github.com/QuantConnect/Lean/blob/master/CONTRIBUTING.md
  [8]: https://github.com/QuantConnect/Lean/blob/master/VisualStudioPlugin/readme.md
