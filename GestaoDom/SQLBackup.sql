USE OrcDom;
GO
BACKUP DATABASE OrcDom
TO DISK = 'D:\X_backup\SQL20230930.bak'
   WITH FORMAT,
      MEDIANAME = 'SQLServerBackups',
      NAME = 'Full Backup of SQLTestDB';
GO