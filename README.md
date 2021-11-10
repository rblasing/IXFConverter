# IXFConverter
This is a small utility I wrote to import a set of IXF files into SQL Server which had been exported from an old DB/2 application database.  The Command Reference HTML page contains a description of the IXF format.

It will create tables, keys and constraints equivalent to those originally present in the DB/2 schema, based on each IXF file's metadata.
