-- Criar base de dados se não existir
CREATE DATABASE IF NOT EXISTS OrcDom;

-- Criar utilizador
CREATE USER IF NOT EXISTS 'UserGestao12345678'@'localhost' IDENTIFIED BY '87654321oatseGresU';

-- Acesso à base de dados
GRANT ALL PRIVILEGES ON  OrcDom.* TO 'UserGestao12345678'@'localhost';

USE OrcDom;
-- Criar tabela de estados
CREATE TABLE tbl_0001_status(
	status_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	status_cod VARCHAR(1) NOT NULL,
	status_descr VARCHAR(30) NOT NULL,
	status_usercreate VARCHAR(30) NOT NULL,
	status_datecreate VARCHAR(8) NOT NULL,
	status_timecreate VARCHAR(6) NOT NULL,
	status_userlastchg VARCHAR(30) NOT NULL DEFAULT 0,
	status_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	status_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar a tabela users
CREATE TABLE tbl_0002_users(
	user_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, 
	user_userID VARCHAR(30) NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    user_password VARCHAR(255) NOT NULL,
    user_type VARCHAR(50) NOT NULL,
    user_status VARCHAR(1) NOT NULL,
    user_chgpw BIT NOT NULL,
    user_pwcount INT NOT NULL,
    user_usercreate VARCHAR(100) NOT NULL,
    user_datecreate VARCHAR(8) NOT NULL,
	user_timecreate VARCHAR(6) NOT NULL,
    user_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
    user_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	user_timelastchg VARCHAR(6) NOT NULL DEFAULT 0,
	user_userlastchgpw VARCHAR(100) NOT NULL DEFAULT 0,
    user_datelastchgpw VARCHAR(8) NOT NULL DEFAULT 0,
	user_timelastchgpw VARCHAR(6) NOT NULL DEFAULT 0
    );

-- Criar a tabela opecoesMenu
CREATE TABLE tbl_0003_opcoesAcesso(
	opm_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	opm_cod VARCHAR(30) NOT NULL,
	opm_descr VARCHAR(255) NOT NULL,
	opm_nivel INT NOT NULL,
	opm_status VARCHAR(1) NOT NULL,
	opm_usercreate VARCHAR(100)NOT NULL,
	opm_datecreate VARCHAR(8) NOT NULL,
	opm_timecreate VARCHAR(6) NOT NULL,
	opm_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	opm_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	opm_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela acessos de utilizadores	
CREATE TABLE tbl_0004_acessos(
	acs_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	acs_userid INT NOT NULL,
	acs_cod VARCHAR(30) NOT NULL,
	acs_acesso BIT NOT NULL DEFAULT 0,
	acs_usercreate VARCHAR(100)NOT NULL,
	acs_datecreate VARCHAR(8) NOT NULL,
	acs_timecreate VARCHAR(6) NOT NULL,
	acs_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	acs_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	acs_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar tabela paises
CREATE TABLE tbl_0005_paises(
	pai_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	pai_desig VARCHAR(50) NOT NULL,
	pai_iso3166_1_number INT NOT NULL,
	pai_iso3166_1_alpha2 VARCHAR(2) NOT NULL,
	pai_iso3166_1_alpha3 VARCHAR(3) NOT NULL,
	pai_usercreation VARCHAR(30)NOT NULL,
	pai_datecreation VARCHAR(8) NOT NULL,
	pai_timecreation VARCHAR(6) NOT NULL,
	pai_usrlstchg VARCHAR(30) NOT NULL,
	pai_dtlstchg VARCHAR(8) NOT NULL,
	pai_tmlstchg VARCHAR(6) NOT NULL
	);

-- Criar tabela tipos de terceiros
CREATE TABLE tbl_0101_tipoterceiro(
	tipoterc_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	tipoterc_cod VARCHAR(2) NOT NULL,
	tipoterc_descr VARCHAR(50) NOT NULL,
	tipoterc_status VARCHAR(1) NOT NULL,
	tipoterc_usercreate VARCHAR(100)NOT NULL,
	tipoterc_datecreate VARCHAR(8) NOT NULL,
	tipoterc_timecreate VARCHAR(6) NOT NULL,
	tipoterc_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	tipoterc_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	tipoterc_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela terceiros
CREATE TABLE tbl_0102_terceiros(
	terc_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	terc_cod VARCHAR(3) NOT NULL,
	terc_descr VARCHAR(110) NOT NULL,
	terc_codtipo VARCHAR(2) NOT NULL,
	terc_morada1 VARCHAR(50) NOT NULL,
	terc_morada2 VARCHAR(50),
	terc_cp VARCHAR(20) NOT NULL,
	terc_localidade VARCHAR(50) NOT NULL,
    terc_pais INT NOT NULL,
	terc_tlf VARCHAR(20) NOT NULL DEFAULT 0,
	terc_email VARCHAR(50) NOT NULL DEFAULT 0,
	terc_nif VARCHAR(20) NOT NULL,
	terc_status VARCHAR(1) NOT NULL,
	terc_usercreate VARCHAR(100)NOT NULL,
	terc_datecreate VARCHAR(8) NOT NULL,
	terc_timecreate VARCHAR(6) NOT NULL,
	terc_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	terc_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	terc_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar tabela contas crédito
CREATE TABLE tbl_0103_contascred(
	cntcred_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	cntcred_cod VARCHAR(3) NOT NULL,
	cntcred_descr VARCHAR(110) NOT NULL,
	cntcred_nr VARCHAR(50) NOT NULL DEFAULT 0,
	cntcred_saldo DECIMAL(18, 2) NOT NULL DEFAULT 0,
	cntcred_status VARCHAR(1) NOT NULL,
	cntcred_usercreate VARCHAR(100)NOT NULL,
	cntcred_datecreate VARCHAR(8) NOT NULL,
	cntcred_timecreate VARCHAR(6) NOT NULL,
	cntcred_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	cntcred_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	cntcred_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar tabela contas dédito
CREATE TABLE tbl_0103_contasdeb(
	cntdeb_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	cntdeb_cod VARCHAR(3) NOT NULL,
	cntdeb_descr VARCHAR(110) NOT NULL,
	cntdeb_nr VARCHAR(50) NOT NULL DEFAULT 0,
	cntdeb_status VARCHAR(1) NOT NULL,
	cntdeb_usercreate VARCHAR(100)NOT NULL,
	cntdeb_datecreate VARCHAR(8) NOT NULL,
	cntdeb_timecreate VARCHAR(6) NOT NULL,
	cntdeb_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	cntdeb_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	cntdeb_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela tipo de receita
CREATE TABLE tbl_0104_tiporeceita(
	tr_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	tr_cod VARCHAR(3) NOT NULL,
	tr_descr VARCHAR(110) NOT NULL,
	tr_status VARCHAR(1) NOT NULL,
	tr_usercreate VARCHAR(100)NOT NULL,
	tr_datecreate VARCHAR(8) NOT NULL,
	tr_timecreate VARCHAR(6) NOT NULL,
	tr_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	tr_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	tr_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela familias
CREATE TABLE tbl_0201_familias(
	fam_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	fam_codigo VARCHAR(2) NOT NULL,
	fam_descr VARCHAR(50) NOT NULL,
	fam_status VARCHAR(1) NOT NULL,
	fam_usercreate VARCHAR(100)NOT NULL,
	fam_datecreate VARCHAR(8) NOT NULL,
	fam_timecreate VARCHAR(6) NOT NULL,
	fam_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	fam_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	fam_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela sub-familias
CREATE TABLE tbl_0202_subfamilias(
	sfm_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	sfm_codfam VARCHAR(2) NOT NULL,
	sfm_cod VARCHAR(3) NOT NULL,
	sfm_codigo VARCHAR(5) NOT NULL,
	sfm_descr VARCHAR(50) NOT NULL,
	sfm_status VARCHAR(1) NOT NULL,
	sfm_usercreate VARCHAR(100)NOT NULL,
	sfm_datecreate VARCHAR(8) NOT NULL,
	sfm_timecreate VARCHAR(6) NOT NULL,
	sfm_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	sfm_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	sfm_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela grupos
CREATE TABLE tbl_0203_grupos(
	grp_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	grp_codsubfam VARCHAR(5) NOT NULL,
	grp_cod VARCHAR(3) NOT NULL,
	grp_codigo VARCHAR(8) NOT NULL,
	grp_descr VARCHAR(50) NOT NULL,
	grp_status VARCHAR(1) NOT NULL,
	grp_usercreate VARCHAR(100)NOT NULL,
	grp_datecreate VARCHAR(8) NOT NULL,
	grp_timecreate VARCHAR(6) NOT NULL,
	grp_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	grp_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	grp_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela unidades
CREATE TABLE tbl_0204_unidades(
	uni_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	uni_cod VARCHAR(3) NOT NULL,
	uni_descr VARCHAR(50) NOT NULL,
	uni_peso DECIMAL(9,3) NOT NULL DEFAULT 0,
	uni_volume DECIMAL(9,3) NOT NULL DEFAULT 0,
	uni_status VARCHAR(1) NOT NULL,
	uni_usercreate VARCHAR(100)NOT NULL,
	uni_datecreate VARCHAR(8) NOT NULL,
	uni_timecreate VARCHAR(6) NOT NULL,
	uni_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	uni_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	uni_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela viaturas
CREATE TABLE tbl_0205_viaturas(
	vtr_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	vtr_cod VARCHAR(2) NOT NULL,
	vtr_matricula VARCHAR(8) NOT NULL,
	vtr_descr VARCHAR(50) NOT NULL,
	vtr_status VARCHAR(1) NOT NULL,
	vtr_usercreate VARCHAR(100)NOT NULL,
	vtr_datecreate VARCHAR(8) NOT NULL,
	vtr_timecreate VARCHAR(6) NOT NULL,
	vtr_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	vtr_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	vtr_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela taxas IVA
CREATE TABLE tbl_0206_taxasiva(
	iva_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	iva_cod VARCHAR(2) NOT NULL,
	iva_taxa DECIMAL(5,2) NOT NULL,
	iva_status VARCHAR(1) NOT NULL,
	iva_usercreate VARCHAR(100)NOT NULL,
	iva_datecreate VARCHAR(8) NOT NULL,
	iva_timecreate VARCHAR(6) NOT NULL,
	iva_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	iva_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	iva_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela artigos
CREATE TABLE tbl_0207_artigos(
	art_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	art_codgrupo VARCHAR(8) NOT NULL,
	art_cod VARCHAR(6) NOT NULL,
	art_coduni VARCHAR(3) NOT NULL,
	art_codterc VARCHAR(3) NOT NULL,
	art_codigo VARCHAR(20) NOT NULL,
	art_descr VARCHAR(50) NOT NULL,
	art_status VARCHAR(1) NOT NULL,
	art_usercreate VARCHAR(100)NOT NULL,
	art_datecreate VARCHAR(8) NOT NULL,
	art_timecreate VARCHAR(6) NOT NULL,
	art_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	art_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	art_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar tabela movimentos a débito
CREATE TABLE tbl_0301_movimentosdebito(
	fd_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	fd_id_fatura INT NOT NULL,
	fd_codterc VARCHAR(3) NOT NULL,
	fd_numdoc VARCHAR(50) NOT NULL,
	fd_datadoc VARCHAR(8) NOT NULL DEFAULT 0,
	fd_datalimitepag VARCHAR(8) NOT NULL DEFAULT 0,
	fd_dataemissaodoc VARCHAR(8) NOT NULL DEFAULT 0,
	fd_conta VARCHAR(3) NOT NULL,
	fd_valor DECIMAL(15, 4) NOT NULL,
	fd_status VARCHAR(1) NOT NULL,
	fd_usercreate VARCHAR(100)NOT NULL,
	fd_datecreate VARCHAR(8) NOT NULL,
	fd_timecreate VARCHAR(6) NOT NULL,
	fd_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	fd_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	fd_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
-- Criar tabela de detalhes de movimentos a débito 
CREATE TABLE tbl_0302_movimentosdebito_det(
	md_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	md_id_fatura INT NOT NULL,
   md_id_linha INT NOT NULL,
	md_codartigo VARCHAR(20) NOT NULL,
	md_codiva VARCHAR(2) NOT NULL,
	md_datamov VARCHAR(8) NOT NULL,
	md_quantidade DECIMAL(15,4) NOT NULL,
	md_precobase DECIMAL(15,4) NOT NULL,
	md_desconto1 DECIMAL(7,4) NOT NULL DEFAULT 0,
	md_desconto2 DECIMAL(7,4) NOT NULL DEFAULT 0,
	md_precofinal DECIMAL(15,4) NOT NULL,
	md_valordesconto DECIMAL(15,4) NOT NULL,
	md_combustivel BIT NOT NULL DEFAULT 0,
	md_codviatura VARCHAR(2) NOT NULL DEFAULT 0,
	md_kmi INT NOT NULL DEFAULT 0,
	md_kmf INT NOT NULL DEFAULT 0,
	md_kmefetuados INT NOT NULL DEFAULT 0,
	md_status VARCHAR(1) NOT NULL,
	md_usercreate VARCHAR(100)NOT NULL,
	md_datecreate VARCHAR(8) NOT NULL,
	md_timecreate VARCHAR(6) NOT NULL,
	md_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	md_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	md_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela movimentos a crébito 
CREATE TABLE tbl_0402_movimentoscredito(
	mc_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	mc_cred_id INT NOT NULL,
	mc_codterc VARCHAR(3) NOT NULL,
	mc_numerodoc VARCHAR(50) NOT NULL,
	mc_datamov VARCHAR(8) NOT NULL,
	mc_codtiporeceita VARCHAR(3) NOT NULL,
	mc_contacredito VARCHAR(3) NOT NULL,
	mc_contadebito VARCHAR(3) NOT NULL,
	mc_valor DECIMAL(15,2) NOT NULL,
	mc_transf BIT NOT NULL DEFAULT 0,
	mc_status VARCHAR(1) NOT NULL,
	mc_usercreate VARCHAR(100)NOT NULL,
	mc_datecreate VARCHAR(8) NOT NULL,
	mc_timecreate VARCHAR(6) NOT NULL,
	mc_userlastchg VARCHAR(100) NOT NULL DEFAULT 0,
	mc_datelastchg VARCHAR(8) NOT NULL DEFAULT 0,
	mc_timelastchg VARCHAR(6) NOT NULL DEFAULT 0
	);

-- Criar tabela de manutenção de viaturas
CREATE TABLE tbl_0901_viaturasmanutencao(
	vtm_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	vtm_mid INT NOT NULL,
	vtm_codviatura VARCHAR(2) NOT NULL,
	vtm_descr VARCHAR(100) NOT NULL,
	vtm_datamanut VARCHAR(8) NOT NULL,
	vtm_km INT NOT NULL,
	vtm_oleo BIT NOT NULL DEFAULT 0,
	vtm_filtrooleo BIT NOT NULL DEFAULT 0,
	vtm_filtroar BIT NOT NULL DEFAULT 0,
	vtm_efetuado VARCHAR(100) NOT NULL,
	vtm_kmproxima INT NOT NULL,
	vtm_status VARCHAR(1) NOT NULL,
	vtm_usercreate VARCHAR(100)NOT NULL,
	vtm_datecreate VARCHAR(8) NOT NULL,
	vtm_timecreate VARCHAR(6) NOT NULL,
	vtm_userlstchg VARCHAR(100) NOT NULL DEFAULT 0,
	vtm_datelstchg VARCHAR(8) NOT NULL DEFAULT 0,
	vtm_timelstchg VARCHAR(6) NOT NULL DEFAULT 0
	);
	
CREATE TABLE tbl_0902_viaturasmatmanut(
	vmm_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	vmm_mid INT NOT NULL,
	vmm_refmanut INT NOT NULL,
	vmm_id_linha INT NOT NULL,
	vmm_id_debdet INT NOT NULL,
	vmm_quantidade DECIMAL(15,4) NOT NULL,
	vmm_status VARCHAR(1) NOT NULL,
	vmm_usercreate VARCHAR(100)NOT NULL,
	vmm_datecreate VARCHAR(8) NOT NULL,
	vmm_timecreate VARCHAR(6) NOT NULL,
	vmm_userlstchg VARCHAR(100) NOT NULL DEFAULT 0,
	vmm_datelstchg VARCHAR(8) NOT NULL DEFAULT 0,
	vmm_timelstchg VARCHAR(6) NOT NULL DEFAULT 0
	);
    
INSERT INTO tbl_0001_status (status_cod, status_descr, status_usercreate, status_datecreate, status_timecreate)
VALUES  ("1", "Ativo", "Master", "19000101", "000001"),
		("2", "Inativo", "Master", "19000101", "000001");

INSERT INTO tbl_0002_users (user_userID, user_name, user_password, user_type, user_status, user_chgpw, user_pwcount, user_usercreate, user_datecreate, user_timecreate)
VALUES ("Admin", "Administrador", "!@#$%^&*", "Administrador", "1", "0", "0", "Master", "19000101", "000001");

INSERT INTO tbl_0003_opcoesacesso (opm_cod, opm_descr, opm_nivel, opm_status, opm_usercreate, opm_datecreate, opm_timecreate)
VALUES  ("M01", "Ficheiro", "1", "1", "Master", "19000101", "000001"),
		  ("M01L01", "Ficheiro / Login", "2", "1", "Master", "19000101", "000001"),
        ("M01O01", "Ficheiro / Logout", "2", "1", "Master", "19000101", "000001"),
        ("M01S01", "Ficheiro / Sair", "2", "1", "Master", "19000101", "000001"),
		  ("M20", "Entidades", "1", "1", "Master", "19000101", "000001"),
		  ("M20C01", "Entidades / Contas", "2", "1", "Master", "19000101", "000001"),
        ("M20C0101", "Entidades / Contas / Manutenção de contas", "3", "1", "Master", "19000101", "000001"),
        ("M20C010101", "Adicionar contas", "20", "1", "Master", "19000101", "000001"),
        ("M20C010102", "Alterar contas", "20", "1", "Master", "19000101", "000001"),
        ("M20C010103", "Eliminar contas", "20", "1", "Master", "19000101", "000001"),
        ("M20C0102", "Entidades / Contas / Manutenção de tipos de receitas", "3", "1", "Master", "19000101", "000001"),
        ("M20C010201", "Adicionar tipos de receitas", "20", "1", "Master", "19000101", "000001"),
        ("M20C010202", "Alterar tipos de receitas", "20", "1", "Master", "19000101", "000001"),
        ("M20C010203", "Eliminar tipos de receitas", "20", "1", "Master", "19000101", "000001"),
        ("M20M01", "Entidades / Movimentação", "2", "1", "Master", "19000101", "000001"),
        ("M20M0101", "Entidades / Movimentação / Manutenção geral", "3", "1", "Master", "19000101", "000001"),
        ("M20M0102", "Entidades / Movimentação / Manutenção de familias", "3", "1", "Master", "19000101", "000001"),
        ("M20M010201", "Adicionar familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M010202", "Alterar familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M010203", "Eliminar familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M0103", "Entidades / Movimentação / Manutenção de sub-familias", "3", "1", "Master", "19000101", "000001"),
        ("M20M010301", "Adicionar sub-familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M010302", "Alterar sub-familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M010303", "Eliminar sub-familias", "20", "1", "Master", "19000101", "000001"),
        ("M20M0104", "Entidades / Movimentação / Manutenção de grupos", "3", "1", "Master", "19000101", "000001"),
        ("M20M010401", "Adicionar grupos", "20", "1", "Master", "19000101", "000001"),
        ("M20M010402", "Alterar grupos", "20", "1", "Master", "19000101", "000001"),
        ("M20M010403", "Eliminar grupos", "20", "1", "Master", "19000101", "000001"),
        ("M20M0105", "Entidades / Movimentação / Manutenção de artigos", "3", "1", "Master", "19000101", "000001"),
        ("M20M010501", "Adicionar artigos", "20", "1", "Master", "19000101", "000001"),
        ("M20M010502", "Alterar artigos", "20", "1", "Master", "19000101", "000001"),
        ("M20M010503", "Eliminar artigos", "20", "1", "Master", "19000101", "000001"),
        ("M20M0106", "Entidades / Movimentação / Manutenção de unidades", "3", "1", "Master", "19000101", "000001"),
        ("M20M010601", "Adicionar unidades", "20", "1", "Master", "19000101", "000001"),
        ("M20M010602", "Alterar unidades", "20", "1", "Master", "19000101", "000001"),
        ("M20M010603", "Eliminar unidades", "20", "1", "Master", "19000101", "000001"),
		  ("M20T01", "Entidades / Terceiros", "2", "1", "Master", "19000101", "000001"),
        ("M20T0101", "Entidades / Terceiros / Manutenção de tipos de terceiros", "3", "1", "Master", "19000101", "000001"),
        ("M20T010101", "Adicionar tipo de terceiro", "20", "1", "Master", "19000101", "000001"),
        ("M20T010102", "Alterar tipo de terceiro", "20", "1", "Master", "19000101", "000001"),
        ("M20T010103", "Eliminar tipo de terceiro", "20", "1", "Master", "19000101", "000001"),
        ("M20T0102", "Entidades / Terceiros / Manutenção de terceiros", "3", "1", "Master", "19000101", "000001"),
        ("M20T010201", "Adicionar terceiros", "20", "1", "Master", "19000101", "000001"),
        ("M20T010202", "Alterar terceiros", "20", "1", "Master", "19000101", "000001"),
        ("M20T010203", "Eliminar terceiros", "20", "1", "Master", "19000101", "000001"),
        ("M20U01", "Entidades / Utilizadores", "2", "1", "Master", "19000101", "000001"),
        ("M20U0101", "Entidades / Utilizadores / Manutenção de utilizadores", "3", "1", "Master", "19000101", "000001"),
        ("M20U010101", "Adicionar utilizadores", "20", "1", "Master", "19000101", "000001"),
        ("M20U010102", "Alterar utilizadores", "20", "1", "Master", "19000101", "000001"),
        ("M20U010103", "Eliminar utilizadores", "20", "1", "Master", "19000101", "000001"),
        ("M20U010104", "Alterar palavra-passe a utilizadores", "20", "1", "Master", "19000101", "000001"),
        ("M20U010105", "Adicionar acessos a utilizadores", "20", "1", "Master", "19000101", "000001"),
	     ("M20U0102", "Entidades / Utilizadores / Manutenção de acessos", "3", "1", "Master", "19000101", "000001"),
        ("M20U010201", "Adicionar acesso", "20", "1", "Master", "19000101", "000001"),
        ("M20U010202", "Alterar acesso", "20", "1", "Master", "19000101", "000001"),
        ("M20U010203", "Eliminar acesso", "20", "1", "Master", "19000101", "000001"),
        ("M20V0101", "Entidades / viaturas", "2", "1", "Master", "19000101", "000001"),
        ("M20V010101", "Adicionar viaturas", "20", "1", "Master", "19000101", "000001"),
        ("M20V010102", "Alterar viaturas", "20", "1", "Master", "19000101", "000001"),
        ("M20V010103", "Eliminar viaturas", "20", "1", "Master", "19000101", "000001"),
        ("M25", "Movimentação", "1", "1", "Master", "19000101", "000001"),
        ("M25C01", "Movimentação / Crédito", "2", "1", "Master", "19000101", "000001"),
        ("M25C0101", "Adicionar movimentos a crédito", "20", "1", "Master", "19000101", "000001"),
        ("M25C0102", "Alterar movimentos a crédito", "20", "1", "Master", "19000101", "000001"),
        ("M25C0103", "Eliminar movimentos a crédito", "20", "1", "Master", "19000101", "000001"),
        ("M25D01", "Movimentação / Débitos - cabeçalho", "2", "1", "Master", "19000101", "000001"),
        ("M25D0101", "Adicionar cabeçalhos de movimento a débito", "20", "1", "Master", "19000101", "000001"),
        ("M25D0102", "Alterar cabeçalhos de movimento a débito", "20", "1", "Master", "19000101", "000001"),
        ("M25D0103", "Eliminar cabeçalhos de movimento a débito", "20", "1", "Master", "19000101", "000001"),
        ("M25D02", "Movimentação / Débitos - detalhe", "2", "1", "Master", "19000101", "000001"),
        ("M25D0201", "Adicionar detalhe (linha) de movimento a débito", "20", "1", "Master", "19000101", "000001"),
        ("M25D0202", "Alterar detalhe (linha) de movimento a débito", "20", "1", "Master", "19000101", "000001"),
        ("M25D0203", "Eliminar detalhe (linha) de movimento a débito", "20", "1", "Master", "19000101", "000001"),
		  ("M25V02", "Movimentação / Registo de manutenções a viaruras", "2", "1", "Master", "19000101", "000001"),
		  ("M25V0201", "Adicionar registo de manutenções a viaturas", "20", "1", "Master", "19000101", "000001"),
		  ("M25V0202", "Alterar registo de manutenções a viaturas", "20", "1", "Master", "19000101", "000001"),
		  ("M25V0203", "Eliminar registo de manutenções a viaturas", "20", "1", "Master", "19000101", "000001"),
		  ("M28", "Relatórios", "1", "1", "Master", "19000101", "000001"),
		  ("M28A0101", "Relatórios / Por artigo, mês e seleção de ano", "2", "1", "Master", "19000101", "000001"),
		  ("M28A0102", "Relatórios / Por grupo, mês e seleção de ano", "2", "1", "Master", "19000101", "000001"),
		  ("M28A0103", "Relatórios / Por subfamilia, mês e seleção de ano", "2", "1", "Master", "19000101", "000001"),
		  ("M28A0104", "Relatórios / Por familia, mês e seleção de ano", "2", "1", "Master", "19000101", "000001"),
		  ("M28A0201", "Relatório de saldo por ano/mês com seleção de ano", "2", "1", "Master", "19000101", "000001"),
        ("M30", "Configurações", "1", "1", "Master", "19000101", "000001"),
        ("M30D01", "Configurações / Localização da base de dados", "2", "1", "Master", "19000101", "000001"),
        ("M30D02", "Configurações / Backup da base de dados", "2", "1", "Master", "19000101", "000001"),
        ("M30D03", "Configurações / Restaurar a base de dados", "2", "1", "Master", "19000101", "000001"),
        ("M40", "Utilitários", "1", "1", "Master", "19000101", "000001"),
        ("M90", "Ajuda", "1", "1", "Master", "19000101", "000001");