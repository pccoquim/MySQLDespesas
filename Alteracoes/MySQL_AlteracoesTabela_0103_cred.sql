ALTER TABLE tbl_0103_contasdeb ADD cntdeb_datecreate VARCHAR(8) DEFAULT '0' AFTER cntdeb_userreg, ADD cntdeb_datelastchg VARCHAR(8) DEFAULT '0', ADD cntdeb_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0103_contasdeb ADD cntdeb_timecreate VARCHAR(6) DEFAULT '0' AFTER cntdeb_datecreate;

UPDATE tbl_0103_contasdeb
SET cntdeb_datecreate = CONCAT(YEAR(cntdeb_datareg), LPAD(MONTH(cntdeb_datareg), 2, '0'), LPAD(DAY(cntdeb_datareg), 2, '0')),
    cntdeb_timecreate = CONCAT(LPAD(HOUR(cntdeb_datareg), 2, '0'), LPAD(MINUTE(cntdeb_datareg), 2, '0'), LPAD(SECOND(cntdeb_datareg), 2, '0')),
	cntdeb_datelastchg = CONCAT(YEAR(cntdeb_dataregalt), LPAD(MONTH(cntdeb_dataregalt), 2, '0'), LPAD(DAY(cntdeb_dataregalt), 2, '0')),
    cntdeb_timelastchg = CONCAT(LPAD(HOUR(cntdeb_dataregalt), 2, '0'), LPAD(MINUTE(cntdeb_dataregalt), 2, '0'), LPAD(SECOND(cntdeb_dataregalt), 2, '0'));

UPDATE tbl_0103_contasdeb
SET cntdeb_timelastchg = COALESCE(cntdeb_timelastchg, '0')
WHERE cntdeb_timelastchg IS NULL;

UPDATE tbl_0103_contasdeb
SET cntdeb_datelastchg = COALESCE(cntdeb_datelastchg, '0')
WHERE cntdeb_datelastchg IS NULL;

ALTER TABLE tbl_0103_contasdeb
CHANGE cntdeb_userreg cntdeb_usercreate VARCHAR(100);

ALTER TABLE tbl_0103_contasdeb
CHANGE cntdeb_userregalt cntdeb_userlastchg VARCHAR(100);

UPDATE tbl_0103_contasdeb
SET cntdeb_userlastchg = COALESCE(cntdeb_userlastchg, '0')
WHERE cntdeb_userlastchg IS NULL;

UPDATE tbl_0103_contasdeb
SET cntdeb_status = '1'
WHERE cntdeb_status = "A";

UPDATE tbl_0103_contasdeb
SET cntdeb_status = '2'
WHERE cntdeb_status = "I";

ALTER TABLE tbl_0103_contasdeb
DROP COLUMN cntdeb_datareg, 
DROP COLUMN cntdeb_dataregalt;