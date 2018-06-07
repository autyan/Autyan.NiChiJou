create table IdentityUsers
(
  Id                   bigint                    not null
    primary key,
  LoginName            varchar(50) charset utf8  not null,
  NickName             varchar(50) charset utf8  not null,
  MemberCode           varchar(50) charset utf8  not null,
  Email                varchar(50) charset utf8  null,
  EmailConfirmed       bit                       not null,
  PhoneNumber          varchar(50) charset utf8  null,
  PhoneNumberConfirmed bit                       not null,
  PasswordHash         varchar(200) charset utf8 not null,
  SecuritySalt         varchar(200) charset utf8 not null,
  CreatedAt            datetime                  not null,
  CreatedBy            bigint                    not null,
  ModifiedAt           datetime                  null,
  ModifiedBy           bigint                    null,
  constraint IX_IdentityUsers_LoginName
  unique (LoginName)
);

create index IX_IdentityUsers_Email
  on IdentityUsers (Email);

create index IX_IdentityUsers_PhoneNumber
  on IdentityUsers (PhoneNumber);

create table ServiceTokens
(
  Id          bigint                   not null
    primary key,
  ServiceName varchar(50) charset utf8 not null,
  AppId       varchar(50) charset utf8 not null,
  ApiKey      varchar(50) charset utf8 not null,
  IsEnabled   bit                      not null,
  CreatedAt   datetime                 not null,
  CreatedBy   bigint                   not null,
  ModifiedAt  datetime                 null,
  ModifiedBy  bigint                   null
);

create table sequence
(
  name          varchar(50)     not null
  comment '序列的名字'
    primary key,
  current_value bigint          not null
  comment '序列的当前值',
  increment     int default '1' not null
  comment '序列的自增值'
)
  collate = utf8_bin;

create function current_value_for(seq_name varchar(50))
  returns bigint
  BEGIN
    DECLARE value BIGINT;
    SET value = 0;
    SELECT current_value
    INTO value
    FROM sequence
    WHERE name = seq_name;
    RETURN value;
  END;

create function next_value_for(seq_name varchar(50))
  returns bigint
  BEGIN
    UPDATE sequence
    SET current_value = current_value + increment
    WHERE name = seq_name;
    RETURN current_value_for(seq_name);
  END;

create function set_value_for(seq_name varchar(50), set_value bigint)
  returns bigint
  BEGIN
    UPDATE sequence
    SET current_value = set_value
    WHERE name = seq_name;
    RETURN current_value_for(seq_name);
  END;

