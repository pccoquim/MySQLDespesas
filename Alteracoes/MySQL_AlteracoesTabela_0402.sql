ALTER TABLE tbl_0402_movimentoscredito ADD mc_datecreate VARCHAR(8) DEFAULT '0' AFTER mc_userreg, ADD mc_datelastchg VARCHAR(8) DEFAULT '0', ADD mc_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0402_movimentoscredito ADD mc_timecreate VARCHAR(6) DEFAULT '0' NOT NULL AFTER mc_datecreate;

ALTER TABLE tbl_0402_movimentoscredito ADD mc_datamov VARCHAR(8) DEFAULT '0' NOT NULL AFTER mc_numerodoc;

UPDATE tbl_0402_movimentoscredito
SET mc_datecreate = CONCAT(YEAR(mc_datareg), LPAD(MONTH(mc_datareg), 2, '0'), LPAD(DAY(mc_datareg), 2, '0')),
    mc_timecreate = CONCAT(LPAD(HOUR(mc_datareg), 2, '0'), LPAD(MINUTE(mc_datareg), 2, '0'), LPAD(SECOND(mc_datareg), 2, '0')),
	mc_datelastchg = CONCAT(YEAR(mc_dataregalt), LPAD(MONTH(mc_dataregalt), 2, '0'), LPAD(DAY(mc_dataregalt), 2, '0')),
    mc_timelastchg = CONCAT(LPAD(HOUR(mc_dataregalt), 2, '0'), LPAD(MINUTE(mc_dataregalt), 2, '0'), LPAD(SECOND(mc_dataregalt), 2, '0')),
	mc_datamov = CONCAT(YEAR(mc_datadoc), LPAD(MONTH(mc_datadoc), 2, '0'), LPAD(DAY(mc_datadoc), 2, '0'));

UPDATE tbl_0402_movimentoscredito
SET mc_timelastchg = COALESCE(mc_timelastchg, '0')
WHERE mc_timelastchg IS NULL;

UPDATE tbl_0402_movimentoscredito
SET mc_datelastchg = COALESCE(mc_datelastchg, '0')
WHERE mc_datelastchg IS NULL;

ALTER TABLE tbl_0402_movimentoscredito
CHANGE mc_userreg mc_usercreate VARCHAR(100);

ALTER TABLE tbl_0402_movimentoscredito
CHANGE mc_userregalt mc_userlastchg VARCHAR(100);

UPDATE tbl_0402_movimentoscredito
SET mc_userlastchg = COALESCE(mc_userlastchg, '0')
WHERE mc_userlastchg IS NULL;

UPDATE tbl_0402_movimentoscredito
SET mc_status = '1'
WHERE mc_status = "A";

UPDATE tbl_0402_movimentoscredito
SET mc_status = '2'
WHERE mc_status = "I";

ALTER TABLE tbl_0402_movimentoscredito
DROP COLUMN mc_datadoc,
DROP COLUMN mc_datareg, 
DROP COLUMN mc_dataregalt;