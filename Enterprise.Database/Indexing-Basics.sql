-- blog.sqlauthority.com
-- blog.extremeexperts.com
-- The fundamental unit of storage or IO in SQL SERVER is the PAGE (8k) which means 128 pages per 1MB
-- Each page begins with 96-byte header,maximum amount of data contained in a single row on a page is 8060 bytes, with 36 byte slot rate that will be used. 
-- Extents are collection of eight physically contiguous pages.

-- A Heap is a table without a clustered index.The data rows are not stored in any particular order.Data pages are not linked in a linked list.
-- A Clustered Index : 
---- the data rows are stored in order based on the clustered index key.
---- It is implemented as B+ tree index structure.
---- data pages in the leaf level are linked in a doubly linked list.
---- have one row in sys.partitions with index_id=1
-- A Non-Clustered Index:
---- the same structure B+ tree index structure.
---- do not affect the order of the data rows.
---- each index row contains the nonclustered key value , a row locator and any included or non key columns.
-- Fill factor
---- the method to pre-allocate some space for future expansions.To avoid PAGESPLITS and degrade performance.
USE tempdb
GO

CREATE TABLE Indexing(
ID INT IDENTITY(1,1),
Name CHAR(4000),
Company CHAR(4000),
Pay INT
)

SELECT 
      OBJECT_NAME(OBJECT_ID) TableName,
	  ISNULL(name,OBJECT_NAME(OBJECT_ID)) IndexName,
	  Index_Id,
	  type_desc
FROM sys.indexs
WHERE OBJECT_NAME(OBJECT_ID)='Indexing'
GO

SET NOCOUNT ON -- suppress the end rows affected

INSERT INTO Indexing VALUES('Name','company name',10000)

-- Status Check
SELECT 
     OBJECT_NAME(OBJECT_ID) Name,
	 index_type_desc as Index_Type,
	 alloc_unit_type_desc as Data_Type,
	 index_id as Index_Id,
	 index_depth as Depth,
	 index_level as IND_LEVEL,
	 record_count as RecordCount,
	 page_count as PageCount,
	 fragment_count as Fragmentation
From sys.dm_db_index_physical_stats (DB_ID(),OBJECT_ID('Indexing'))
GO

INSERT INTO Indexing VALUES('Name','company name',10000),('Name','company name',10000)
GO

INSERT INTO Indexing VALUES('Name','company name',10000)
GO 100


CREATE CLUSTERED INDEX CI_IndexingID ON Indexing(ID)
GO

INSERT INTO Indexing VALUES('Name','company name',10000)
GO 700

DBCC SHOW_STATISTICS ('Indexing',CI_IndexingID)
GO

CREATE NONCLUSTERED INDEX NCI_Pay ON Indexing(Pay)
GO

DBCC SHOW_STATISTICS ('Indexing',NCI_Pay)
GO

-- Additional Information
-- Indexed Views have the same storage structure as clustered tables 
-- Non-clustered indexes per table -999
-- Columns key per index -16
-- Statistics on non-Indexed cloumns - 30000
-- XML indexes -249
-- maximum bytes in any index key - 900 bytes

-- Primary Key : automatically creates clustered index except when non-clustered index is specified or when clustered index already exists.
--               automatically creates unique index on clumn 
--Non unique clustered index adds 4-byte unique identifier column

-- create new empty table
SELECT * 
INTO TEMPTable
FROM db_datawriter
WHERE 1=2
GO

-- Create PK Clustered index not specified
ALTER TABLE TEMPTable ADD CONSTRAINT [PK_TABLENAME_COLUMNNAME] PRIMARY KEY([COLUMNNAME] ASC)
GO

ALTER TABLE TEMPTable DROP CONSTRAINT [PK_TABLENAME_COLUMNNAME] 
GO

-- Create PK Non Clustered index  specified
ALTER TABLE TEMPTable ADD CONSTRAINT [PK_TABLENAME_COLUMNNAME] PRIMARY KEY NONCLUSTERED([COLUMNNAME] ASC)
GO

-- Create Clustered index (no primary key)
CREATE CLUSTERED INDEX [CL_PKTABLENAME_Index] ON TABLENAME ([CLOUMNNAME] ASC,[CLOUMNNAME] ASC)
GO

-- Create PK and do not specify kind of index
ALTER TABLE TEMPTable ADD CONSTRAINT [PK_TABLENAME_COLUMNNAME] PRIMARY KEY ([COLUMNNAME] ASC)
GO

DROP TABLE [TEMPTable]

-- Over Indexing : consumes unnecessary disk space,queries may use less efficient index,less efficient execution plan,
--                 reduction in overall server performance, confusion amony developer and dpa,
-- best practice : drop unused indexes


