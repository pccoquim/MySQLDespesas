ALTER TABLE tbl_0301_movimentosdebito ADD fd_datecreate VARCHAR(8) DEFAULT '0' AFTER fd_userreg, ADD fd_datelastchg VARCHAR(8) DEFAULT '0', ADD fd_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0301_movimentosdebito ADD fd_timecreate VARCHAR(6) DEFAULT '0' AFTER fd_datecreate;

ALTER TABLE tbl_0301_movimentosdebito ADD fd_datadoc1 VARCHAR(8) DEFAULT '0' NOT NULL AFTER fd_numdoc;

ALTER TABLE tbl_0301_movimentosdebito ADD fd_datalimitepag VARCHAR(8) DEFAULT '0' NOT NULL AFTER fd_datadoc1;

ALTER TABLE tbl_0301_movimentosdebito ADD fd_dataemissaodoc VARCHAR(8) DEFAULT '0' NOT NULL AFTER fd_datalimitepag;

UPDATE tbl_0301_movimentosdebito
SET fd_datecreate = CONCAT(YEAR(fd_datareg), LPAD(MONTH(fd_datareg), 2, '0'), LPAD(DAY(fd_datareg), 2, '0')),
    fd_timecreate = CONCAT(LPAD(HOUR(fd_datareg), 2, '0'), LPAD(MINUTE(fd_datareg), 2, '0'), LPAD(SECOND(fd_datareg), 2, '0')),
	fd_datelastchg = CONCAT(YEAR(fd_dataregalt), LPAD(MONTH(fd_dataregalt), 2, '0'), LPAD(DAY(fd_dataregalt), 2, '0')),
    fd_timelastchg = CONCAT(LPAD(HOUR(fd_dataregalt), 2, '0'), LPAD(MINUTE(fd_dataregalt), 2, '0'), LPAD(SECOND(fd_dataregalt), 2, '0'));

UPDATE tbl_0301_movimentosdebito
SET fd_datadoc1 = CONCAT(YEAR(fd_datadoc), LPAD(MONTH(fd_datadoc), 2, '0'), LPAD(DAY(fd_datadoc), 2, '0'));

UPDATE tbl_0301_movimentosdebito
SET fd_timelastchg = COALESCE(fd_timelastchg, '0')
WHERE fd_timelastchg IS NULL;

UPDATE tbl_0301_movimentosdebito
SET fd_datelastchg = COALESCE(fd_datelastchg, '0')
WHERE fd_datelastchg IS NULL;

ALTER TABLE tbl_0301_movimentosdebito
CHANGE fd_userreg fd_usercreate VARCHAR(100);

ALTER TABLE tbl_0301_movimentosdebito
CHANGE fd_userregalt fd_userlastchg VARCHAR(100);

UPDATE tbl_0301_movimentosdebito
SET fd_userlastchg = COALESCE(fd_userlastchg, '0')
WHERE fd_userlastchg IS NULL;

UPDATE tbl_0301_movimentosdebito
SET fd_status = '1'
WHERE fd_status = "A";

UPDATE tbl_0301_movimentosdebito
SET fd_status = '2'
WHERE fd_status = "I";

ALTER TABLE tbl_0301_movimentosdebito
DROP COLUMN fd_datareg, 
DROP COLUMN fd_dataregalt;