ALTER TABLE nova_tabela ADD terc_datereg VARCHAR(8) DEFAULT '', ADD terc_timereg VARCHAR(6) DEFAULT '', ADD terc_dateregalt VARCHAR(8) DEFAULT '', ADD terc_timeregalt VARCHAR(6) DEFAULT '';

UPDATE nova_tabela
SET terc_datereg = CONCAT(YEAR(terc_datareg), LPAD(MONTH(terc_datareg), 2, '0'), LPAD(DAY(terc_datareg), 2, '0')),
    terc_timereg = CONCAT(LPAD(HOUR(terc_datareg), 2, '0'), LPAD(MINUTE(terc_datareg), 2, '0'), LPAD(SECOND(terc_datareg), 2, '0')),
	terc_dateregalt = CONCAT(YEAR(terc_dataregalt), LPAD(MONTH(terc_dataregalt), 2, '0'), LPAD(DAY(terc_dataregalt), 2, '0')),
    terc_timeregalt = CONCAT(LPAD(HOUR(terc_dataregalt), 2, '0'), LPAD(MINUTE(terc_dataregalt), 2, '0'), LPAD(SECOND(terc_dataregalt), 2, '0'));

UPDATE nova_tabela
SET terc_timeregalt = COALESCE(terc_timeregalt, '')
WHERE terc_timeregalt IS NULL;