SQLServerRealTimeUpdates
-----------------------------------------------
This sample demonstrates how to use C1 FlexGrid with SqlTableDependency to update data in real time.

## Pre-requisites.

1. Create 'TableDependencyDB' Database.
2. Create 'Products' Table by running script in 'Data/Create Products Table.sql'.
3. Enable Service Broker: ALTER DATABASE [TableDependencyDB] SET ENABLE_BROKER;
4. Change the 'ConnectionString' in appsettings.json to your Database Connection String.
