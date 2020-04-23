# Util

Utilty object library provided generic support types such as results, optionals, etc.. for use in other software packages

## Result Types

System.Util provides 3 result types: QueryResult<T>, CommandResult, and CommandAndQueryResult<T>; CommandAndQueryResult<T> violates CQS but is provided because while command query seperation should be strived for it's not always possible with given code.

