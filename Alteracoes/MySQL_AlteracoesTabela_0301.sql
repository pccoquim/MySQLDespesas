ALTER TABLE tbl_0301_movimentosdebito_det ADD md_datecreate VARCHAR(8) DEFAULT '0' NOT NULL AFTER md_userreg, ADD md_datelastchg VARCHAR(8) DEFAULT '0' NOT NULL, ADD md_timelastchg VARCHAR(6) DEFAULT '0' NOT NULL;

ALTER TABLE tbl_0301_movimentosdebito_det ADD md_timecreate VARCHAR(6) DEFAULT '0' NOT NULL AFTER md_datecreate;

ALTER TABLE tbl_0301_movimentosdebito_det ADD md_datamov VARCHAR(8) DEFAULT '0' NOT NULL AFTER md_codiva;

UPDATE tbl_0301_movimentosdebito_det
SET md_datecreate = CONCAT(YEAR(md_datareg), LPAD(MONTH(md_datareg), 2, '0'), LPAD(DAY(md_datareg), 2, '0')),
    md_timecreate = CONCAT(LPAD(HOUR(md_datareg), 2, '0'), LPAD(MINUTE(md_datareg), 2, '0'), LPAD(SECOND(md_datareg), 2, '0')),
	md_datelastchg = CONCAT(YEAR(md_dataregalt), LPAD(MONTH(md_dataregalt), 2, '0'), LPAD(DAY(md_dataregalt), 2, '0')),
    md_timelastchg = CONCAT(LPAD(HOUR(md_dataregalt), 2, '0'), LPAD(MINUTE(md_dataregalt), 2, '0'), LPAD(SECOND(md_dataregalt), 2, '0')),
	md_datamov = CONCAT(YEAR(md_dataentrada), LPAD(MONTH(md_dataentrada), 2, '0'), LPAD(DAY(md_dataentrada), 2, '0'));

UPDATE tbl_0301_movimentosdebito_det
SET md_timelastchg = COALESCE(md_timelastchg, '0')
WHERE md_timelastchg IS NULL;

UPDATE tbl_0301_movimentosdebito_det
SET md_datelastchg = COALESCE(md_datelastchg, '0')
WHERE md_datelastchg IS NULL;

ALTER TABLE tbl_0301_movimentosdebito_det
CHANGE md_userreg md_usercreate VARCHAR(100);

ALTER TABLE tbl_0301_movimentosdebito_det
CHANGE md_userregalt md_userlastchg VARCHAR(100);

UPDATE tbl_0301_movimentosdebito_det
SET md_userlastchg = COALESCE(md_userlastchg, '0')
WHERE md_userlastchg IS NULL;

UPDATE tbl_0301_movimentosdebito_det
SET md_status = '1'
WHERE md_status = "A";

UPDATE tbl_0301_movimentosdebito_det
SET md_status = '2'
WHERE md_status = "I";

ALTER TABLE tbl_0301_movimentosdebito_det
DROP COLUMN md_datareg, 
DROP COLUMN md_dataregalt;