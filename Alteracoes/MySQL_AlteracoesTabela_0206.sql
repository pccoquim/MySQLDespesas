ALTER TABLE tbl_0207_artigos ADD art_datecreate VARCHAR(8) DEFAULT '0' AFTER art_userreg, ADD art_datelastchg VARCHAR(8) DEFAULT '0', ADD art_timelastchg VARCHAR(6) DEFAULT '0';

ALTER TABLE tbl_0207_artigos ADD art_timecreate VARCHAR(6) DEFAULT '0' AFTER art_datecreate;

UPDATE tbl_0207_artigos
SET art_datecreate = CONCAT(YEAR(art_datareg), LPAD(MONTH(art_datareg), 2, '0'), LPAD(DAY(art_datareg), 2, '0')),
    art_timecreate = CONCAT(LPAD(HOUR(art_datareg), 2, '0'), LPAD(MINUTE(art_datareg), 2, '0'), LPAD(SECOND(art_datareg), 2, '0')),
	art_datelastchg = CONCAT(YEAR(art_dataregalt), LPAD(MONTH(art_dataregalt), 2, '0'), LPAD(DAY(art_dataregalt), 2, '0')),
    art_timelastchg = CONCAT(LPAD(HOUR(art_dataregalt), 2, '0'), LPAD(MINUTE(art_dataregalt), 2, '0'), LPAD(SECOND(art_dataregalt), 2, '0'));

UPDATE tbl_0207_artigos
SET art_timelastchg = COALESCE(art_timelastchg, '0')
WHERE art_timelastchg IS NULL;

UPDATE tbl_0207_artigos
SET art_datelastchg = COALESCE(art_datelastchg, '0')
WHERE art_datelastchg IS NULL;

ALTER TABLE tbl_0207_artigos
CHANGE art_userreg art_usercreate VARCHAR(100);

ALTER TABLE tbl_0207_artigos
CHANGE art_userregalt art_userlastchg VARCHAR(100);

UPDATE tbl_0207_artigos
SET art_userlastchg = COALESCE(art_userlastchg, '0')
WHERE art_userlastchg IS NULL;

UPDATE tbl_0207_artigos
SET art_status = '1'
WHERE art_status = "A";

UPDATE tbl_0207_artigos
SET art_status = '2'
WHERE art_status = "I";

ALTER TABLE tbl_0207_artigos
DROP COLUMN art_datareg, 
DROP COLUMN art_dataregalt;