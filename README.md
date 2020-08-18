Jhv.PutGetConnector
===

Aplikace obsahuje nejčastěji se opakující obecné třídy, které se ti mohou hodit.
* **PutGetVariable** - Proměná pro práci s komunikací PutGet
* **PutGetConnection** - Třída umožňuje připojení k PLC Siemens pocí PutGet, několik metod pro ukládání/vyčítání kolekce i samoststných prvků  

Potřebné součásti
==

Aplikace ke svému chodu potřebuje

* [Jhv.Core](https://gitlab.com/jhvengineering/CsharpProjects/jhv_core)

Použití ve tvém PC
=

* Aby jsi měl implementaci co nejjednodušší **ukládej všechny standartní Jhv aplikace v souborovém systému na stejnou úroveň**. Například D:\Projekty\_JhvDatabaseApp
* Aplikaci pojmenuj **Jhv.PutGetConnector**

![Image_023](/uploads/2a424215f8e28012f5bbcb646349ea0d/Image_023.png)

Nastavení PLC
==
* **V PLC je potřeba v HW nastavení povolit PUT/GET komunikaci
* **DB ke ketrému přistupujete musí být neoptimalizované