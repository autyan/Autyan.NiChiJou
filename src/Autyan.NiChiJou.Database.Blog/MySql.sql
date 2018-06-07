create table ActivityLogs
(
  Id               bigint                               not null
    primary key,
  OperateUserId    bigint                               not null,
  ActivityType     tinyint default '5'                  not null,
  Content          varchar(1000) charset utf8           not null,
  OperateIpAddress varchar(50) charset utf8 default '*' null,
  ClientType       tinyint                              not null,
  CreatedAt        datetime                             not null,
  CreatedBy        bigint                               not null
);

create index IX_ActivityLogs_OperateUserId_ActivityType_ClientType
  on ActivityLogs (OperateUserId, ActivityType, ClientType);

create index IX_ActivityLogs_OperateUserId_ClientType_ActivityType
  on ActivityLogs (OperateUserId, ClientType, ActivityType);

create table ArticleComments
(
  Id          bigint                     not null
    primary key,
  Content     varchar(4000) charset utf8 not null,
  CommentedBy bigint                     null,
  PostId      bigint                     not null,
  ToComment   bigint                     null,
  CreatedAt   datetime                   not null,
  CreatedBy   bigint                     not null,
  ModifiedAt  datetime                   null,
  ModifiedBy  bigint                     null
);

create index IX_ArticleComments_PostId
  on ArticleComments (PostId);

create table ArticleContents
(
  Id         bigint   not null
    primary key,
  ArticleId  bigint   not null,
  Content    longtext not null,
  CreatedAt  datetime not null,
  CreatedBy  bigint   not null,
  ModifiedAt datetime null,
  ModifiedBy bigint   null
);

create index IX_ArticleContents_ArticleId
  on ArticleContents (ArticleId);

create table Articles
(
  Id         bigint                    not null
    primary key,
  Title      varchar(200) charset utf8 not null,
  Extract    varchar(500) charset utf8 null,
  BlogId     bigint                    not null,
  `Reads`    int                       not null,
  Comments   int                       not null,
  LastReadAt datetime                  null,
  CreatedAt  datetime                  not null,
  CreatedBy  bigint                    not null,
  ModifiedAt datetime                  null,
  ModifiedBy bigint                    null
);

create index IX_Articles_BlogId
  on Articles (BlogId);

create index IX_Articles_Title
  on Articles (Title);

create table BlogUsers
(
  Id         bigint                    not null
    primary key,
  NickName   varchar(50) charset utf8  not null,
  AvatorUrl  varchar(200) charset utf8 null,
  MemberCode varchar(50) charset utf8  not null,
  Gender     tinyint                   null,
  CreatedAt  datetime                  not null,
  CreatedBy  bigint                    not null,
  ModifiedAt datetime                  null,
  ModifiedBy bigint                    null
);

create index IX_BlogUsers_MemberCode
  on BlogUsers (MemberCode);

create table Blogs
(
  Id          bigint                    not null
    primary key,
  BlogName    varchar(200) charset utf8 not null,
  Description varchar(200) charset utf8 null,
  BlogUserId  bigint                    not null,
  CreatedAt   datetime                  not null,
  CreatedBy   bigint                    not null,
  ModifiedAt  datetime                  null,
  ModifiedBy  bigint                    null
);

create index IX_Blogs_BlogUser
  on Blogs (BlogUserId);

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

