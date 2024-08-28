ALTER TABLE tbl_0104_tiporeceita ADD tr_datecreate VARCHAR(8) DEFAULT '0' AFTER tr_userreg, ADD tr_datelastchg VARCHAR(8) DEFAULT '0', ADD tr_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0104_tiporeceita ADD tr_timecreate VARCHAR(6) DEFAULT '0' AFTER tr_datecreate;

UPDATE tbl_0104_tiporeceita
SET tr_datecreate = CONCAT(YEAR(tr_datareg), LPAD(MONTH(tr_datareg), 2, '0'), LPAD(DAY(tr_datareg), 2, '0')),
    tr_timecreate = CONCAT(LPAD(HOUR(tr_datareg), 2, '0'), LPAD(MINUTE(tr_datareg), 2, '0'), LPAD(SECOND(tr_datareg), 2, '0')),
	tr_datelastchg = CONCAT(YEAR(tr_dataregalt), LPAD(MONTH(tr_dataregalt), 2, '0'), LPAD(DAY(tr_dataregalt), 2, '0')),
    tr_timelastchg = CONCAT(LPAD(HOUR(tr_dataregalt), 2, '0'), LPAD(MINUTE(tr_dataregalt), 2, '0'), LPAD(SECOND(tr_dataregalt), 2, '0'));

UPDATE tbl_0104_tiporeceita
SET tr_timelastchg = COALESCE(tr_timelastchg, '0')
WHERE tr_timelastchg IS NULL;

UPDATE tbl_0104_tiporeceita
SET tr_datelastchg = COALESCE(tr_datelastchg, '0')
WHERE tr_datelastchg IS NULL;

ALTER TABLE tbl_0104_tiporeceita
CHANGE tr_userreg tr_usercreate VARCHAR(100);

ALTER TABLE tbl_0104_tiporeceita
CHANGE tr_userregalt tr_userlastchg VARCHAR(100);

UPDATE tbl_0104_tiporeceita
SET tr_userlastchg = COALESCE(tr_userlastchg, '0')
WHERE tr_userlastchg IS NULL;

UPDATE tbl_0104_tiporeceita
SET tr_status = '1'
WHERE tr_status = "A";

UPDATE tbl_0104_tiporeceita
SET tr_status = '2'
WHERE tr_status = "I";

ALTER TABLE tbl_0104_tiporeceita
DROP COLUMN tr_datareg, 
DROP COLUMN tr_dataregalt;