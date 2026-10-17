| Priority | Description | Status |
|---|---|
| Normal | TODO: rename all the xrbl/xml properties so they dont bleed up and the rest of the codebase doesnt have to use then. Map them here to something more usable for the rest of the codebase. | In Flight
| Normal | TODO: Clean up the statement tree json so that all the xrbl technical things are mapped away | In Flight
| Normal | TODO: Trim the statement tree down |
| Normal | TODO: Constituent facts: tangible book, capex, free cashflow, ebitda, adjusted ebitda, operating margin, ebitda margin, net margin |
| Normal | TODO: pie of contituents foreach year, multi line timeseries of constituents |
| Normal | TODO: for total facts show time series of their constituents. pie foreach year of their constituents |
| Normal | TODO: data quality check. Check if their are duplicates data points, same fact, year, period, and ascension, for constituents. We assume ONE of each fact per report |
| Normal | TODO: data quality check. Gaps |
| Normal | TODO: data quality check. check against some other data |
| Normal | TODO: change yoy and sequential |
| Normal | TODO: check for consecutive same number. at least one cashflow fact looked like it had same number consecutively. PaymentsOfDividends is on cashflow is one such |
| Normal | TODO: is the filtering correct? it seems like 10-K data points are returned even though we are filtering for one or the other |
| Normal | TODO: for any data point in the UI you should be able to see all the meta data, or the actual fact and/or link to that filing on edgar |
| Normal | TODO: make sure to escape anything passed into json query |
| Normal | TODO: UI ticker look ahead |
| Normal | TODO: UI default view when ticker selected |
| Normal | TODO: Select 10-K or 10-Q |
| Normal | TODO: single exe packaged with UI app |
| Normal | TODO: lock down CORS |
| Normal | TODO: For display in the UI, we need to include human readable label for the factName. Also, for troubleshooting we need to include the meta data about the datapoint so that any particular datapoint can be found with a single click, ideally, or at least |
| Normal | TODO: need a way to check this is in fact correct. Integration tests |
| Normal | TODO: need a way to show time series with all constituents |
| Normal | TODO: need to cache fasb statement json files |
| Normal | Document features
| Low | TODO: Make query builder fluent |
| Low | TODO: Make addition of new expressions extensible |
| Low | TODO: Make addition of new expressions extensible |
| Low | TODO: Make addition of new expressions extensible |
| Normal | TODO: Implement caching | DONE |

