## BLAZOR FlightStatistics 
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-Blazor-Samples/tree/master/General/FlightStatistics)
____
#### A Statistics application written in BLAZOR that demonstrates flight statistics.
____
This application displays statistical data about flights. The statistics include Region-wise total flights,
Region-wise on-time performance, Region-wise average delay and city-wise statistics. The city-wise statistics
include AverageDelay, CompletionFactor, Percentage of delayed flights, Percentage of on-time flights,
Ranking, Delay trend, etc.

The application allows the users to drill-down to a specific region and a specific city within a region.

The controls used in this application are:

FlexGrid - The FlexGrid is used to display detailed statistics of major Cities grouped by Region. Features
of FlexGrid used - Grouping, Sorting, CellTemplate and CellFactory.

Treemap - The Treemap control displays the Total flights of cities grouped by Region. It allows users to
drill-down to a Region and to a specific city within a region. To go back a level, users can right click
or use the breadcrumbs above the On-Time performance Chart.

FlexChart - There are two FlexChart controls in this application. The first FlexChart displays the On time
performance of all regions. It can be drilled-down to show the On-time performance of cities within a Region
and further drill-down will show the on-time performance of the selected city over a period of time.
The second FlexChart control shows the cummulative Average Delay of Regions over the last year. Selecting a
series in the chart, expands the particular region group in FlexGrid and the Treemap and the on-time performance
FlexChart are drilled-down to the selected Region.
