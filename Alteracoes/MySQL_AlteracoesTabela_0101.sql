ALTER TABLE tbl_0102_terceiros ADD terc_datecreate VARCHAR(8) DEFAULT '0' AFTER terc_userreg, ADD terc_timecreate VARCHAR(6) DEFAULT '0' AFTER terc_userreg, ADD terc_datelastchg VARCHAR(8) DEFAULT '0', ADD terc_timelastchg VARCHAR(6) DEFAULT '0';

UPDATE tbl_0102_terceiros
SET terc_datecreate = CONCAT(YEAR(terc_datareg), LPAD(MONTH(terc_datareg), 2, '0'), LPAD(DAY(terc_datareg), 2, '0')),
    terc_timecreate = CONCAT(LPAD(HOUR(terc_datareg), 2, '0'), LPAD(MINUTE(terc_datareg), 2, '0'), LPAD(SECOND(terc_datareg), 2, '0')),
	terc_datelastchg = CONCAT(YEAR(terc_dataregalt), LPAD(MONTH(terc_dataregalt), 2, '0'), LPAD(DAY(terc_dataregalt), 2, '0')),
    terc_timelastchg = CONCAT(LPAD(HOUR(terc_dataregalt), 2, '0'), LPAD(MINUTE(terc_dataregalt), 2, '0'), LPAD(SECOND(terc_dataregalt), 2, '0'));

UPDATE tbl_0102_terceiros
SET terc_timelastchg = COALESCE(terc_timelastchg, '0')
WHERE terc_timelastchg IS NULL;

UPDATE tbl_0102_terceiros
SET terc_datelastchg = COALESCE(terc_datelastchg, '0')
WHERE terc_datelastchg IS NULL;

ALTER TABLE tbl_0102_terceiros
CHANGE terc_userreg terc_usercreate VARCHAR(100);

ALTER TABLE tbl_0102_terceiros
CHANGE terc_userregalt terc_userlastchg VARCHAR(100);

UPDATE tbl_0102_terceiros
SET terc_userlastchg = COALESCE(terc_userlastchg, '0')
WHERE terc_userlastchg IS NULL;

UPDATE tbl_0102_terceiros
SET terc_status = '1'
WHERE terc_status = "A";

UPDATE tbl_0102_terceiros
SET terc_status = '2'
WHERE terc_status = "I";

ALTER TABLE tbl_0102_terceiros
DROP COLUMN terc_datareg, 
DROP COLUMN terc_dataregalt;