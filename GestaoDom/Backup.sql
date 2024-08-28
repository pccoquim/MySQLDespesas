DELIMITER //

CREATE PROCEDURE BackupDatabase(IN dbName VARCHAR(255))
BEGIN
    DECLARE backupPath VARCHAR(256);
    DECLARE fileName VARCHAR(256);
    DECLARE fileDate VARCHAR(20);

    SET backupPath = 'D:/X_backup/';

    SET fileDate = DATE_FORMAT(NOW(), '%Y%m%d');

    SET fileName = CONCAT(backupPath, dbName, '_', fileDate, '.sql');

    SET @sql = CONCAT('mysqldump --routines -u gastos -p #x123', dbName, ' > "', fileName, '"');
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END //

DELIMITER ;
