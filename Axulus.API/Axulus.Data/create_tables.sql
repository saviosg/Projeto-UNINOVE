create table if not exists categoria_departamento
(
	id_categoria_departamento integer primary key,
	id_categoria integer,
	id_departamento integer,
	foreign key(id_categoria) references categoria(id_categoria),
	foreign key(id_departamento) references departamento(id_departamento)
);

create table if not exists categorias
(
	id_categoria integer primary key,
	nome_categoria text
);

create table if not exists departamento
(
	id_departamento integer primary key,
	id_empresa integer,
	nome_departamento text,
	foreign key(id_empresa) references empresa(id_empresa)
);

create table if not exists empresa
(
	id_empresa integer primary key,
	nome_empresa text,
	email text,
	cnpj text,
	data_liberacao integer,
	data_cadastro integer,
	data_alteracao integer
);

create table if not exists image
(
	id_image integer primary key,
	id_empresa integer,
	descricao text,
	image_base64 text,
	data_cadastro integer,
	data_alteracao integer,
	foreign key(id_empresa) references empresa(id_empresa)
);

create table if not exists usuario
(
	id_usuario integer primary key,
	id_empresa integer,
	id_departamento integer,
	nome_usuario text,
	cpf text,
	email text,
	sexo text,
	data_nas integer,
	id_image integer,
	us_ativo integer,
	foreign key(id_empresa) references empresa(id_empresa),
	foreign key(id_departamento) references departamento(id_departamento),
	foreign key(id_image) references image(id_image)
);
