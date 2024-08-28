ALTER TABLE tbl_0205_viaturas ADD vtr_datecreate VARCHAR(8) DEFAULT '0' AFTER vtr_userreg, ADD vtr_datelastchg VARCHAR(8) DEFAULT '0', ADD vtr_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0205_viaturas ADD vtr_timecreate VARCHAR(6) DEFAULT '0' AFTER vtr_datecreate;

UPDATE tbl_0205_viaturas
SET vtr_datecreate = CONCAT(YEAR(vtr_datareg), LPAD(MONTH(vtr_datareg), 2, '0'), LPAD(DAY(vtr_datareg), 2, '0')),
    vtr_timecreate = CONCAT(LPAD(HOUR(vtr_datareg), 2, '0'), LPAD(MINUTE(vtr_datareg), 2, '0'), LPAD(SECOND(vtr_datareg), 2, '0')),
	vtr_datelastchg = CONCAT(YEAR(vtr_dataregalt), LPAD(MONTH(vtr_dataregalt), 2, '0'), LPAD(DAY(vtr_dataregalt), 2, '0')),
    vtr_timelastchg = CONCAT(LPAD(HOUR(vtr_dataregalt), 2, '0'), LPAD(MINUTE(vtr_dataregalt), 2, '0'), LPAD(SECOND(vtr_dataregalt), 2, '0'));

UPDATE tbl_0205_viaturas
SET vtr_timelastchg = COALESCE(vtr_timelastchg, '0')
WHERE vtr_timelastchg IS NULL;

UPDATE tbl_0205_viaturas
SET vtr_datelastchg = COALESCE(vtr_datelastchg, '0')
WHERE vtr_datelastchg IS NULL;

ALTER TABLE tbl_0205_viaturas
CHANGE vtr_userreg vtr_usercreate VARCHAR(100);

ALTER TABLE tbl_0205_viaturas
CHANGE vtr_userregalt vtr_userlastchg VARCHAR(100);

UPDATE tbl_0205_viaturas
SET vtr_userlastchg = COALESCE(vtr_userlastchg, '0')
WHERE vtr_userlastchg IS NULL;

UPDATE tbl_0205_viaturas
SET vtr_status = '1'
WHERE vtr_status = "A";

UPDATE tbl_0205_viaturas
SET vtr_status = '2'
WHERE vtr_status = "I";

ALTER TABLE tbl_0205_viaturas
DROP COLUMN vtr_datareg, 
DROP COLUMN vtr_dataregalt;