ALTER TABLE tbl_0101_tipoterceiro ADD tipoterc_datecreate VARCHAR(8) DEFAULT '0', ADD tipoterc_timecreate VARCHAR(6) DEFAULT '0', ADD tipoterc_datelastchg VARCHAR(8) DEFAULT '0', ADD tipoterc_timelastchg VARCHAR(6) DEFAULT '0';

UPDATE tbl_0101_tipoterceiro
SET tipoterc_datecreate = CONCAT(YEAR(tipoterc_datareg), LPAD(MONTH(tipoterc_datareg), 2, '0'), LPAD(DAY(tipoterc_datareg), 2, '0')),
    tipoterc_timecreate = CONCAT(LPAD(HOUR(tipoterc_datareg), 2, '0'), LPAD(MINUTE(tipoterc_datareg), 2, '0'), LPAD(SECOND(tipoterc_datareg), 2, '0')),
	tipoterc_datelastchg = CONCAT(YEAR(tipoterc_dataregalt), LPAD(MONTH(tipoterc_dataregalt), 2, '0'), LPAD(DAY(tipoterc_dataregalt), 2, '0')),
    tipoterc_timelastchg = CONCAT(LPAD(HOUR(tipoterc_dataregalt), 2, '0'), LPAD(MINUTE(tipoterc_dataregalt), 2, '0'), LPAD(SECOND(tipoterc_dataregalt), 2, '0'));

UPDATE tbl_0101_tipoterceiro
SET tipoterc_timelastchg = COALESCE(tipoterc_timelastchg, '0')
WHERE tipoterc_timelastchg IS NULL;

UPDATE tbl_0101_tipoterceiro
SET tipoterc_datelastchg = COALESCE(tipoterc_datelastchg, '0')
WHERE tipoterc_datelastchg IS NULL;

ALTER TABLE tbl_0101_tipoterceiro
CHANGE tipoterc_userreg tipoterc_usercreate VARCHAR(100);

ALTER TABLE tbl_0101_tipoterceiro
CHANGE tipoterc_userregalt tipoterc_userlastchg VARCHAR(100);

UPDATE tbl_0101_tipoterceiro
SET tipoterc_userlastchg = COALESCE(tipoterc_userlastchg, '0')
WHERE tipoterc_userlastchg IS NULL;

UPDATE tbl_0101_tipoterceiro
SET tipoterc_status = '1'
WHERE tipoterc_status = "A";

UPDATE tbl_0101_tipoterceiro
SET tipoterc_status = '2'
WHERE tipoterc_status = "I";