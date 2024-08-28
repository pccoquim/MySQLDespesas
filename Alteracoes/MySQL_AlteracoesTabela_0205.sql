ALTER TABLE tbl_0206_taxasiva ADD iva_datecreate VARCHAR(8) DEFAULT '0' AFTER iva_userreg, ADD iva_datelastchg VARCHAR(8) DEFAULT '0', ADD iva_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0206_taxasiva ADD iva_timecreate VARCHAR(6) DEFAULT '0' AFTER iva_datecreate;

UPDATE tbl_0206_taxasiva
SET iva_datecreate = CONCAT(YEAR(iva_datareg), LPAD(MONTH(iva_datareg), 2, '0'), LPAD(DAY(iva_datareg), 2, '0')),
    iva_timecreate = CONCAT(LPAD(HOUR(iva_datareg), 2, '0'), LPAD(MINUTE(iva_datareg), 2, '0'), LPAD(SECOND(iva_datareg), 2, '0')),
	iva_datelastchg = CONCAT(YEAR(iva_dataregalt), LPAD(MONTH(iva_dataregalt), 2, '0'), LPAD(DAY(iva_dataregalt), 2, '0')),
    iva_timelastchg = CONCAT(LPAD(HOUR(iva_dataregalt), 2, '0'), LPAD(MINUTE(iva_dataregalt), 2, '0'), LPAD(SECOND(iva_dataregalt), 2, '0'));

UPDATE tbl_0206_taxasiva
SET iva_timelastchg = COALESCE(iva_timelastchg, '0')
WHERE iva_timelastchg IS NULL;

UPDATE tbl_0206_taxasiva
SET iva_datelastchg = COALESCE(iva_datelastchg, '0')
WHERE iva_datelastchg IS NULL;

ALTER TABLE tbl_0206_taxasiva
CHANGE iva_userreg iva_usercreate VARCHAR(100);

ALTER TABLE tbl_0206_taxasiva
CHANGE iva_userregalt iva_userlastchg VARCHAR(100);

UPDATE tbl_0206_taxasiva
SET iva_userlastchg = COALESCE(iva_userlastchg, '0')
WHERE iva_userlastchg IS NULL;

UPDATE tbl_0206_taxasiva
SET iva_status = '1'
WHERE iva_status = "A";

UPDATE tbl_0206_taxasiva
SET iva_status = '2'
WHERE iva_status = "I";

ALTER TABLE tbl_0206_taxasiva
DROP COLUMN iva_datareg, 
DROP COLUMN iva_dataregalt;